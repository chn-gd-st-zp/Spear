using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.Interface
{
    public interface IStateCode : ISpearEnum
    {
        SpearEnumItem None { get; }

        SpearEnumItem Success { get; }

        SpearEnumItem Fail { get; }

        SpearEnumItem SysError { get; }

        SpearEnumItem ParamsValidationFailed { get; }

        SpearEnumItem CaptchaValidationFailed { get; }

        SpearEnumItem EmptyToken { get; }

        SpearEnumItem NoLogin { get; }

        SpearEnumItem NoPermission { get; }
    }
}

namespace Spear.Inf.Core.CusEnum
{
    public sealed class Enum_StateCode : SpearEnum, IStateCode
    {
        public Enum_StateCode()
        {
            _none = Factory.NewEnum("None", 0);
            _success = Factory.NewEnum("Success", 200);
            _fail = Factory.NewEnum("Fail", 400);
            _sysError = Factory.NewEnum("SysError", 500);

            _paramsValidationFailed = Factory.NewEnum("ParamsValidationFailed", 1);
            _captchaValidationFailed = Factory.NewEnum("CaptchaValidationFailed", 2);
            _emptyToken = Factory.NewEnum("EmptyToken", 3);
            _noLogin = Factory.NewEnum("NoLogin", 4);
            _noPermission = Factory.NewEnum("NoPermission", 5);
        }

        public SpearEnumItem None { get { return _none; } }
        private SpearEnumItem _none;

        public SpearEnumItem Success { get { return _success; } }
        private SpearEnumItem _success;

        public SpearEnumItem Fail { get { return _fail; } }
        private SpearEnumItem _fail;

        public SpearEnumItem SysError { get { return _sysError; } }
        private SpearEnumItem _sysError;

        public SpearEnumItem ParamsValidationFailed { get { return _paramsValidationFailed; } }
        private SpearEnumItem _paramsValidationFailed;

        public SpearEnumItem CaptchaValidationFailed { get { return _captchaValidationFailed; } }
        private SpearEnumItem _captchaValidationFailed;

        public SpearEnumItem EmptyToken { get { return _emptyToken; } }
        private SpearEnumItem _emptyToken;

        public SpearEnumItem NoLogin { get { return _noLogin; } }
        private SpearEnumItem _noLogin;

        public SpearEnumItem NoPermission { get { return _noPermission; } }
        private SpearEnumItem _noPermission;
    }
}
