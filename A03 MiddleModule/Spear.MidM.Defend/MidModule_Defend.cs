using Autofac;

using Spear.MidM.Defend.EncryptionNDecrypt;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.CusEnum;

namespace Spear.MidM.Defend
{
    public static class MidModule_Defend
    {
        public static ContainerBuilder RegisAES(this ContainerBuilder containerBuilder, string secretPrefix)
        {
            containerBuilder.RegisterType<AESHandle>().Keyed<IEncryptionNDecrypt>(Enum_EncryptionNDecrypt.AES).WithParameter("secretPrefix", secretPrefix).SingleInstance();

            return containerBuilder;
        }

        public static ContainerBuilder RegisDES(this ContainerBuilder containerBuilder, string secretPrefix)
        {
            containerBuilder.RegisterType<DESHandle>().Keyed<IEncryptionNDecrypt>(Enum_EncryptionNDecrypt.DES).WithParameter("secretPrefix", secretPrefix).SingleInstance();

            return containerBuilder;
        }

        public static ContainerBuilder RegisMD5(this ContainerBuilder containerBuilder, string secretPrefix = default)
        {
            containerBuilder.RegisterType<MD5Handle>().Keyed<IEncryptionNDecrypt>(Enum_EncryptionNDecrypt.MD5).WithParameter("secretPrefix", secretPrefix).SingleInstance();

            return containerBuilder;
        }
    }
}
