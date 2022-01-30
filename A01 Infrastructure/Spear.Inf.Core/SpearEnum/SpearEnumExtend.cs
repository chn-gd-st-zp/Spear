using Autofac;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.CusEnum
{
    public static class SpearEnumExtend
    {
        public static SpearEnumItem NewEnum(this SpearEnumFactory enumParams, string name)
        {
            return NewEnum(enumParams, name, ++enumParams.Counter);
        }

        public static SpearEnumItem NewEnum(this SpearEnumFactory enumParams, string name, int value)
        {
            enumParams.Counter = value;

            SpearEnumItem cusEnum = new SpearEnumItem();
            cusEnum.Name = name;
            cusEnum.Value = value;

            enumParams.Members.Add(cusEnum);

            if (!enumParams.Keys.ContainsKey(cusEnum.Name))
                enumParams.Keys.Add(cusEnum.Name, cusEnum);

            if (!enumParams.Values.ContainsKey(cusEnum.Value))
                enumParams.Values.Add(cusEnum.Value, cusEnum);

            return cusEnum;
        }

        public static SpearEnumItem Restore<TSpearEnum>(this object value, bool fromName = true)
        where TSpearEnum : ISpearEnum, new()
        {
            var result = default(SpearEnumItem);

            var spearEnum = ISpearEnum.Restore<TSpearEnum>();
            spearEnum = spearEnum == null ? new TSpearEnum() : spearEnum;
            foreach (var property in spearEnum.GetType().GetProperties())
            {
                if (!property.PropertyType.IsExtendType(typeof(SpearEnumItem)))
                    continue;

                var prop = property.GetValue(spearEnum);
                if (prop == null)
                    continue;

                var spearEnumItem = prop as SpearEnumItem;
                if (spearEnumItem == null)
                    continue;

                if (fromName)
                {
                    if (spearEnumItem.Name != value.ToString())
                        continue;
                }
                else
                {
                    if (spearEnumItem.Value != (int)value)
                        continue;
                }

                result = spearEnumItem;

                break;
            }

            return result;
        }

        public static ContainerBuilder RegisSpearEnumNameConverter<TSpearEnum>(this ContainerBuilder containerBuilder)
            where TSpearEnum : ISpearEnum, new()
        {
            containerBuilder.RegisterGeneric(typeof(SpearEnumNameConverter<>)).As(typeof(ISpearEnumConverter<>)).InstancePerDependency();

            return containerBuilder;
        }

        public static ContainerBuilder RegisSpearEnumValueConverter<TSpearEnum>(this ContainerBuilder containerBuilder)
            where TSpearEnum : ISpearEnum, new()
        {
            containerBuilder.RegisterGeneric(typeof(SpearEnumValueConverter<>)).As(typeof(ISpearEnumConverter<>)).InstancePerDependency();

            return containerBuilder;
        }

        public static ContainerBuilder RegisStateCodeNameConverter<TStateCode>(this ContainerBuilder containerBuilder)
            where TStateCode : IStateCode, new()
        {
            return containerBuilder.RegisSpearEnumNameConverter<TStateCode>();
        }

        public static ContainerBuilder RegisStateCodeValueConverter<TStateCode>(this ContainerBuilder containerBuilder)
            where TStateCode : IStateCode, new()
        {
            return containerBuilder.RegisSpearEnumValueConverter<TStateCode>();
        }
    }
}
