using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    [LogIgnore]
    public class Exception_NoPermission : Exception_Base
    {
        public Exception_NoPermission() :base("没有权限")
        {
            ECode = ISpearEnum.Restore<IStateCode>().NoPermission;
        }
    }
}
