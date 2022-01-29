using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusResult
{
    [DIModeForService(Enum_DIType.Exclusive, typeof(IStateCode))]
    public class StateCode : IStateCode
    {
        public EnumInfo None { get; } = Enum_StateCode.None;

        public EnumInfo SysError { get; } = Enum_StateCode.SysError;

        public EnumInfo Success { get; } = Enum_StateCode.Success;

        public EnumInfo Fail { get; } = Enum_StateCode.Fail;

        public EnumInfo ValidError { get; } = Enum_StateCode.ValidError;

        public EnumInfo CaptchaFailed { get; } = Enum_StateCode.CaptchaFailed;

        public EnumInfo EmptyToken { get; } = Enum_StateCode.EmptyToken;

        public EnumInfo NoLogin { get; } = Enum_StateCode.NoLogin;

        public EnumInfo NoAuth { get; } = Enum_StateCode.NoAuth;
    }

    internal static class Enum_StateCode
    {
        private static EnumParams _enumParams = new EnumParams();

        internal static EnumInfo None = _enumParams.NewEnum("None", 0);
        internal static EnumInfo SysError = _enumParams.NewEnum("SysError", -500);
        internal static EnumInfo Success = _enumParams.NewEnum("Success", 200);
        internal static EnumInfo Fail = _enumParams.NewEnum("Fail", 400);

        internal static EnumInfo ValidError = _enumParams.NewEnum("ValidError", 1);
        internal static EnumInfo CaptchaFailed = _enumParams.NewEnum("CaptchaFailed", 2);
        internal static EnumInfo EmptyToken = _enumParams.NewEnum("EmptyToken", 3);
        internal static EnumInfo NoLogin = _enumParams.NewEnum("NoLogin", 4);
        internal static EnumInfo NoAuth = _enumParams.NewEnum("NoAuth", 5);
    }
}
