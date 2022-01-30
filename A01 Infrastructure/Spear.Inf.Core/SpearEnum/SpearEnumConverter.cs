using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Injection;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.Interface
{
    internal interface ISpearEnumConverter<TSpearEnum> : ISingleton
        where TSpearEnum : ISpearEnum, new()
    {
        SpearEnumItem Read(object value);

        object Write(SpearEnumItem spearEnumItem);
    }
}

namespace Spear.Inf.Core.CusEnum
{
    internal class SpearEnumNameConverter<TSpearEnum> : ISpearEnumConverter<TSpearEnum>
        where TSpearEnum : ISpearEnum, new()
    {
        public SpearEnumItem Read(object value)
        {
            return value.Restore<TSpearEnum>();
        }

        public object Write(SpearEnumItem spearEnumItem)
        {
            if (spearEnumItem == null) return null;
            return spearEnumItem.Name;
        }
    }

    internal class SpearEnumValueConverter<TSpearEnum> : ISpearEnumConverter<TSpearEnum>
        where TSpearEnum : ISpearEnum, new()
    {
        public SpearEnumItem Read(object value)
        {
            return value.Restore<TSpearEnum>(false);
        }

        public object Write(SpearEnumItem spearEnumItem)
        {
            if (spearEnumItem == null) return null;
            return spearEnumItem.Value;
        }
    }
}
