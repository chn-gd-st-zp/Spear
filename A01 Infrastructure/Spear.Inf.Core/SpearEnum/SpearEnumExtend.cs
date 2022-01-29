using System.Collections.Generic;

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

        public static SpearEnumItem ParseString(this SpearEnumFactory enumParams, string name)
        {
            return enumParams.Keys.ContainsKey(name) ? enumParams.Keys[name] : null;
        }

        public static List<SpearEnumItem> ParseString(this SpearEnumFactory enumParams, string[] nameArray)
        {
            List<SpearEnumItem> result = new List<SpearEnumItem>();

            foreach (string name in nameArray)
            {
                SpearEnumItem enumInfo = enumParams.ParseString(name);
                if (enumInfo == null)
                    continue;

                result.Add(enumInfo);
            }

            return result;
        }

        public static SpearEnumItem ParseInt(this SpearEnumFactory enumParams, int value)
        {
            return enumParams.Values.ContainsKey(value) ? enumParams.Values[value] : null;
        }

        public static List<SpearEnumItem> ParseInt(this SpearEnumFactory enumParams, int[] valueArray)
        {
            List<SpearEnumItem> result = new List<SpearEnumItem>();

            foreach (int value in valueArray)
            {
                SpearEnumItem enumInfo = enumParams.ParseInt(value);
                if (enumInfo == null)
                    continue;

                result.Add(enumInfo);
            }

            return result;
        }
    }
}
