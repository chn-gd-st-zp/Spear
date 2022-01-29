using Spear.Inf.Core.Base;
using Spear.Inf.Core.Injection;

namespace Spear.Inf.Core.Interface
{
    public interface IStateCode : ISingleton
    {
        EnumInfo None { get; }

        EnumInfo SysError { get; }

        EnumInfo Success { get; }

        EnumInfo Fail { get; }

        EnumInfo ValidError { get; }

        EnumInfo CaptchaFailed { get; }

        EnumInfo EmptyToken { get; }

        EnumInfo NoLogin { get; }

        EnumInfo NoAuth { get; }
    }
}
