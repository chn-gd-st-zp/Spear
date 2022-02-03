using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AspectInjector.Broker;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Permission
{

    [Aspect(Scope.PerInstance)]
    public class PermissionAspect : AOPAspectBase
    {
        private ISpearLogger<PermissionAspect> _logger;
        private IPermissionEnum _permissionEnum;
        private IPermissionRepository _repo_Permission;
        private IAccessLoggerRepository _repo_AccessLogger;
        private AccessRecord _accessRecord;

        public PermissionAspect()
        {
            _logger = ServiceContext.Resolve<ISpearLogger<PermissionAspect>>();
            _permissionEnum = ServiceContext.Resolve<IPermissionEnum>();
            _repo_Permission = ServiceContext.Resolve<IPermissionRepository>();
            _repo_AccessLogger = ServiceContext.Resolve<IAccessLoggerRepository>();
        }

        [Advice(Kind.Around)]
        public new object HandleMethod(
           [Argument(Source.Instance)] object source,
           [Argument(Source.Target)] Func<object[], object> method,
           [Argument(Source.Triggers)] Attribute[] triggers,
           [Argument(Source.Name)] string actionName,
           [Argument(Source.Arguments)] object[] actionParams
        )
        {
            return base.HandleMethod(source, method, triggers, actionName, actionParams);
        }

        protected override void Before(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams)
        {
            try
            {
                if (AppInitHelper.IsTestMode)
                    return;

                if (_permissionEnum == null || _repo_Permission == null || _repo_AccessLogger == null)
                    return;

                var attr = triggers.Where(o => o.GetType().IsExtendOf<MethodPermissionBaseAttribute>())
                    .Select(o => o as MethodPermissionBaseAttribute).Where(o => o.EType == Enum_PermissionType.Action)
                    .FirstOrDefault();
                if (attr == null)
                    return;

                var type = source.GetType();
                if (!type.IsImplementedOf(typeof(IServiceWithTokenProvider<>)))
                    return;

                var tokenProvider = ServiceContext.Resolve(type.GetGenericArguments()[0]) as ITokenProvider;
                if (tokenProvider == null)
                    return;

                var session = ServiceContext.ResolveByKeyed<ISpearSession>(tokenProvider.Protocol);
                if (session == null)
                    return;

                var permission = _repo_Permission.Permission(attr.Code);
                if (permission == null || permission.EStatus != Enum_Status.Normal)
                    return;

                session.VerifyPermission(attr.Code);

                if (!permission.AccessLogger)
                    return;

                var inputObj = actionParams.Where(o => o.GetType().IsExtendOf<IDTO_Input>()).FirstOrDefault();
                if (inputObj == null)
                    return;

                if (!attr.MappingType.DBDestinationType.IsExtendOf<DBEntity_Base>() || !attr.MappingType.DBDestinationType.IsGenericOf(typeof(IDBField_PrimeryKey<>)))
                    return;

                var otAttr = _permissionEnum.EnumType.GetEnumAttr<OperationTypeAttribute>(attr.Code);
                if (otAttr == null || otAttr.EOperationType == Enum_OperationType.None)
                    return;

                var primeryKey = inputObj.GetPrimeryKey();

                _accessRecord = new AccessRecord();
                _accessRecord.ERoleType = session.CurrentAccount.AccountInfo.ERoleType;
                _accessRecord.AccountID = session.CurrentAccount.AccountInfo.AccountID;
                _accessRecord.UserName = session.CurrentAccount.UserName;
                _accessRecord.EOperationType = otAttr.EOperationType;
                _accessRecord.TBName = _repo_AccessLogger.DBContext.GetTBName(attr.MappingType.DBDestinationType);
                _accessRecord.TBValue = attr.MappingType.DBDestinationType.GetRemark();
                _accessRecord.PKName = _repo_AccessLogger.DBContext.GetPKName(attr.MappingType.DBDestinationType);
                _accessRecord.PKValue = primeryKey == null ? string.Empty : primeryKey.ToString();
                _accessRecord.Descriptions = new List<AccessRecordDescription>();

                var dbObj = _accessRecord.PKValue.IsEmptyString() ? null : _repo_AccessLogger.GetDataObj(attr.MappingType.DBDestinationType, _accessRecord.TBName, _accessRecord.PKName, _accessRecord.PKValue);
                foreach (var inputProperty in attr.MappingType.InputType.GetProperties())
                {
                    if (inputProperty.GetCustomAttribute<LogIgnoreAttribute>() != null)
                        continue;

                    var record = new AccessRecordDescription
                    {
                        FieldName = inputProperty.Name,
                        FieldRemark = inputProperty.GetRemark(),
                        InputValue = inputProperty.GetValue(inputObj),
                        DBValue = dbObj.GetFieldValue(inputProperty.Name),
                    };

                    _accessRecord.Descriptions.Add(record);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        protected override void Error(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, Exception error, out bool throwException)
        {
            throwException = true;
        }

        protected override object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult)
        {
            try
            {
                if (_accessRecord == null)
                    return actionResult;

                _accessRecord.ExecResult = actionResult;

                _repo_AccessLogger.Create(_accessRecord);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return actionResult;
        }
    }
}
