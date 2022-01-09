using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.MidM.SessionNAuth;

namespace Spear.GlobalSupport.Base.Filter
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IRequestFilterItems), Enum_FilterType.Ctrler)]
    public class CtrlerFilterItems : List<IRequestFilterItem>, IRequestFilterItems
    {
        public CtrlerFilterItems()
        {
            Add(new CtrlerFilterItem());
        }
    }

    public class CtrlerFilterItem : CtrlerFilterItemBase
    {
        private ISessionNAuth<HTTPTokenProvider> _sessionNAuth { get; set; }

        public CtrlerFilterItem()
        {
            _sessionNAuth = ServiceContext.Resolve<ISessionNAuth<HTTPTokenProvider>>();
        }

        public override void OnExecuting(object context)
        {
            ActionExecutingContext realContext = context as ActionExecutingContext;

            base.OnExecuting(realContext);

            //var permissionAttr = ((ControllerActionDescriptor)realContext.ActionDescriptor).MethodInfo.GetCustomAttribute<PermissionAttribute>();
            //if (permissionAttr == null)
            //    return;

            //_sessionNAuth.VerifyPermission(permissionAttr.Code);

            //if(permissionAttr.AccessLogger)
        }
    }
}
