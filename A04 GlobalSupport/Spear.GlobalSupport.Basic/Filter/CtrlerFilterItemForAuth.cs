using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.ServGeneric;
using Spear.MidM.SessionNAuth;

namespace Spear.GlobalSupport.Basic.Filter
{
    public class CtrlerFilterItemForAuth : CtrlerFilterItemBasic
    {
        private SessionNAuth<HTTPTokenProvider> _sessionNAuth { get; set; }

        public CtrlerFilterItemForAuth()
        {
            _sessionNAuth = ServiceContext.Resolve<SessionNAuth<HTTPTokenProvider>>();
        }

        public override void OnExecuting(object context)
        {
            ActionExecutingContext realContext = context as ActionExecutingContext;

            base.OnExecuting(realContext);

            var paList = ((ControllerActionDescriptor)realContext.ActionDescriptor).MethodInfo.GetCustomAttributes<PermissionAuthAttribute>().ToList();
            if (paList != null && paList.Count() != 0)
                Auth(paList);
        }

        protected void Auth(List<PermissionAuthAttribute> paList)
        {
            foreach (var pa in paList)
                _sessionNAuth.PermissionAuth(pa.Code);
        }
    }
}
