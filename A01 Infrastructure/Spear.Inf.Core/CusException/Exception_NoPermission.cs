using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    public class Exception_NoPermission : Exception_Base
    {
        public Exception_NoPermission() :base("没有权限")
        {
            ECode = ServiceContext.Resolve<IStateCode>().NoPermission;
        }
    }
}
