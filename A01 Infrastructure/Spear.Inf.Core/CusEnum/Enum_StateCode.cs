using Spear.Inf.Core.Attr;
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

        SpearEnumItem ValidError { get; }

        SpearEnumItem CaptchaFailed { get; }

        SpearEnumItem EmptyToken { get; }

        SpearEnumItem NoLogin { get; }

        SpearEnumItem NoAuth { get; }
    }
}

namespace Spear.Inf.Core.CusEnum
{
    [DIModeForService(Enum_DIType.Exclusive, typeof(IStateCode))]
    public sealed class Enum_StateCode : SpearEnum, IStateCode
    {
        public Enum_StateCode()
        {
            _none = Factory.NewEnum("None", 0);
            _success = Factory.NewEnum("Success", 200);
            _fail = Factory.NewEnum("Fail", 400);
            _sysError = Factory.NewEnum("SysError", 500);

            _validError = Factory.NewEnum("ValidError", 1);
            _captchaFailed = Factory.NewEnum("CaptchaFailed", 2);
            _emptyToken = Factory.NewEnum("EmptyToken", 3);
            _noLogin = Factory.NewEnum("NoLogin", 4);
            _noAuth = Factory.NewEnum("NoAuth", 5);
        }

        public SpearEnumItem None { get { return _none; } }
        private SpearEnumItem _none;

        public SpearEnumItem Success { get { return _success; } }
        private SpearEnumItem _success;

        public SpearEnumItem Fail { get { return _fail; } }
        private SpearEnumItem _fail;

        public SpearEnumItem SysError { get { return _sysError; } }
        private SpearEnumItem _sysError;

        public SpearEnumItem ValidError { get { return _validError; } }
        private SpearEnumItem _validError;

        public SpearEnumItem CaptchaFailed { get { return _captchaFailed; } }
        private SpearEnumItem _captchaFailed;

        public SpearEnumItem EmptyToken { get { return _emptyToken; } }
        private SpearEnumItem _emptyToken;

        public SpearEnumItem NoLogin { get { return _noLogin; } }
        private SpearEnumItem _noLogin;

        public SpearEnumItem NoAuth { get { return _noAuth; } }
        private SpearEnumItem _noAuth;
    }
}
