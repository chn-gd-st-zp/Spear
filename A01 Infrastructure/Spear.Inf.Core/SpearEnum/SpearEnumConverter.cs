using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Injection;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.Interface
{
    internal interface ISpearEnumConverter<TSpearEnum> : ISingleton
        where TSpearEnum : ISpearEnum
    {
        SpearEnumItem Read(object value);

        object Write(SpearEnumItem spearEnumItem);
    }
}

namespace Spear.Inf.Core.CusEnum
{
    internal class SpearEnumNameConverter<TSpearEnum> : ISpearEnumConverter<TSpearEnum>
        where TSpearEnum : SpearEnum, new()
    {
        public SpearEnumItem Read(object value)
        {
            var result = default(SpearEnumItem);

            var target = new TSpearEnum();
            var type = typeof(TSpearEnum);

            foreach (var property in type.GetProperties())
            {
                if (!property.PropertyType.IsExtendType(typeof(SpearEnumItem)))
                    continue;

                var prop = property.GetValue(target);
                if (prop == null)
                    continue;

                var spearEnumItem = prop as SpearEnumItem;
                if (spearEnumItem == null)
                    continue;

                if (spearEnumItem.Name != value.ToString())
                    continue;

                result = spearEnumItem;

                break;
            }

            return result;
        }

        public object Write(SpearEnumItem spearEnumItem)
        {
            if (spearEnumItem == null)
                return null;

            return spearEnumItem.Name;
        }
    }

    internal class SpearEnumValueConverter<TSpearEnum> : ISpearEnumConverter<TSpearEnum>
        where TSpearEnum : SpearEnum, new()
    {
        public SpearEnumItem Read(object value)
        {
            var result = default(SpearEnumItem);

            var target = new TSpearEnum();
            var type = typeof(TSpearEnum);

            foreach (var property in type.GetProperties())
            {
                if (!property.PropertyType.IsExtendType(typeof(SpearEnumItem)))
                    continue;

                var prop = property.GetValue(target);
                if (prop == null)
                    continue;

                var spearEnumItem = prop as SpearEnumItem;
                if (spearEnumItem == null)
                    continue;

                if (spearEnumItem.Value != (int)value)
                    continue;

                result = spearEnumItem;

                break;
            }

            return result;
        }

        public object Write(SpearEnumItem spearEnumItem)
        {
            if (spearEnumItem == null)
                return null;

            return spearEnumItem.Value;
        }
    }
}
