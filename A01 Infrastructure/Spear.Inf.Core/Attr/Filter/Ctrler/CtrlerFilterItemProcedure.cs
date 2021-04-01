using System.Reflection;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Spear.Inf.Core.Attr
{
    public class CtrlerFilterItemProcedure : CtrlerFilterItemBasic
    {
        public override void OnExecuting(object context)
        {
            ActionExecutingContext realContext = context as ActionExecutingContext;

            base.OnExecuting(realContext);

            //是否标注了 鉴权忽略 的标签，无标注 则需进行 鉴权
            if (((ControllerActionDescriptor)realContext.ActionDescriptor).MethodInfo.GetCustomAttribute<AuthIgnoreAttribute>() == null)
                Auth();
        }

        protected virtual void Auth() { }
    }
}
