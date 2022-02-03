using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
    [Injection(typeof(AccessLoggerAspect))]
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class AccessLoggerAttribute : Attribute
    {
        public Enum_OperationType EOperationType { get; }

        public Type InputType { get; }

        public Type DBDestinationType { get; }

        public AccessLoggerAttribute(Enum_OperationType eOperationType, Type inputType, Type dbDestinationType)
        {
            EOperationType = eOperationType;
            InputType = inputType;
            DBDestinationType = dbDestinationType;
        }
    }

    [Aspect(Scope.PerInstance)]
    public class AccessLoggerAspect : AOPAspectBase
    {
        private ISpearLogger<AccessLoggerAspect> _logger;

        public AccessLoggerAspect()
        {
            _logger = ServiceContext.Resolve<ISpearLogger<AccessLoggerAspect>>();
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
            //
        }

        protected override void Error(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, Exception error, out bool throwException)
        {
            throwException = false;
        }

        protected override object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult)
        {
            Operation(source, triggers, actionParams, actionResult);
            return actionResult;
        }

        private async Task Operation(object source, Attribute[] triggers, object[] actionParams, object actionResult)
        {
            try
            {
                if (AppInitHelper.IsTestMode)
                    return;

                var permissionAttrs = triggers.Where(o => o.GetType().IsExtendOf<PermissionBaseAttribute>())
                    .Select(o => o as PermissionBaseAttribute)
                    .Where(o => o.EType == Enum_PermissionType.Action)
                    .ToList();
                if (permissionAttrs == null || permissionAttrs.Count() == 0)
                    return;

                var accessLoggerAttr = triggers.Where(o => o.GetType().IsExtendOf<AccessLoggerAttribute>())
                   .Select(o => o as AccessLoggerAttribute)
                   .SingleOrDefault();
                if (accessLoggerAttr == null)
                    return;

                if (accessLoggerAttr.EOperationType == Enum_OperationType.None)
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

                var inputObj = actionParams.Where(o => o.GetType().IsExtendOf<IDTO_Input>()).SingleOrDefault();
                if (inputObj == null)
                    return;

                if (!accessLoggerAttr.DBDestinationType.IsExtendOf<DBEntity_Base>() || !accessLoggerAttr.DBDestinationType.IsGenericOf(typeof(IDBField_PrimeryKey<>)))
                    return;

                var repository = ServiceContext.Resolve<IAccessLoggerRepository>();
                if (repository == null)
                    return;

                var primeryKey = inputObj.GetPrimeryKey();

                var accessRecord = new AccessRecord();
                accessRecord.ERoleType = session.CurrentAccount.AccountInfo.ERoleType;
                accessRecord.AccountID = session.CurrentAccount.AccountInfo.AccountID;
                accessRecord.UserName = session.CurrentAccount.UserName;
                accessRecord.EOperationType = accessLoggerAttr.EOperationType;
                accessRecord.TBName = repository.DBContext.GetTBName(accessLoggerAttr.DBDestinationType);
                accessRecord.TBValue = accessLoggerAttr.DBDestinationType.GetRemark();
                accessRecord.PKName = repository.DBContext.GetPKName(accessLoggerAttr.DBDestinationType);
                accessRecord.PKValue = primeryKey == null ? string.Empty : primeryKey.ToString();
                accessRecord.Descriptions = new List<AccessRecordDescription>();
                accessRecord.ExecResult = actionResult;

                var dbObj = accessRecord.PKValue.IsEmptyString() ? null : repository.GetDataObj(accessRecord.TBName, accessRecord.PKName, accessRecord.PKValue);
                foreach (var inputProperty in accessLoggerAttr.InputType.GetProperties())
                {
                    var record = new AccessRecordDescription
                    {
                        FieldName = inputProperty.Name,
                        FieldRemark = inputProperty.PropertyType.GetRemark(),
                        InputValue = inputProperty.GetValue(inputObj),
                        DBValue = dbObj.GetFieldValue(inputProperty.Name),
                    };

                    accessRecord.Descriptions.Add(record);
                }

                repository.Create(accessRecord);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
