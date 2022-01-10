using System;

using Spear.Inf.Core.Base;

namespace Spear.Inf.Core.CusEnum
{
    [Serializable]
    public static class Enum_StateCode
    {
        private static EnumParams _enumParams = new EnumParams();

        public static EnumInfo None = _enumParams.NewEnum("None", 0);
        public static EnumInfo SysError = _enumParams.NewEnum("SysError", -500);
        public static EnumInfo ValidError = _enumParams.NewEnum("ValidError", -400);
        public static EnumInfo EmptyToken = _enumParams.NewEnum("EmptyToken", -300);

        public static EnumInfo Success = _enumParams.NewEnum("Success", 1);
        public static EnumInfo Fail = _enumParams.NewEnum("Fail", 2);
        public static EnumInfo IncorrectPassword = _enumParams.NewEnum("IncorrectPassword", 3);
        public static EnumInfo NoLogin = _enumParams.NewEnum("NoLogin", 4);
        public static EnumInfo NoAuth = _enumParams.NewEnum("NoAuth", 5);
        public static EnumInfo Exist = _enumParams.NewEnum("Exist", 6);
        public static EnumInfo NotExist = _enumParams.NewEnum("NotExist", 7);
        public static EnumInfo CaptchaFailed = _enumParams.NewEnum("CaptchaFailed", 8);
        public static EnumInfo Error = _enumParams.NewEnum("Error", 9);
    }
}
