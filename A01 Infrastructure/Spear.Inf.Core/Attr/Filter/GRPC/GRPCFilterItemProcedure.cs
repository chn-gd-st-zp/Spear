using System.Reflection;

using MS = MagicOnion.Server;

namespace Spear.Inf.Core.Attr
{
    public class GRPCFilterItemProcedure : GRPCFilterItemBasic
    {
        public override void OnExecuting(object context)
        {
            MS.ServiceContext realContext = context as MS.ServiceContext;

            base.OnExecuting(realContext);

            //是否标注了 鉴权忽略 的标签，无标注 则需进行 鉴权
            if (realContext.MethodInfo.GetCustomAttribute<AuthIgnoreAttribute>() == null)
                Auth();
        }

        protected virtual void Auth() { }
    }
}
