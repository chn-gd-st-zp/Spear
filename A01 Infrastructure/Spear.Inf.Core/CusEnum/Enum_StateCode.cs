using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Spear.Inf.Core.CusEnum
{
    [Serializable]
    public static class Enum_StateCode
    {
        public static EnumParams EnumParams = new EnumParams();

        public static EnumInfo None = EnumParams.NewEnum("None", 0);
        public static EnumInfo SysError = EnumParams.NewEnum("SysError", -500);
        public static EnumInfo ValidError = EnumParams.NewEnum("ValidError", -400);
        public static EnumInfo EmptyToken = EnumParams.NewEnum("EmptyToken", -300);

        public static EnumInfo Success = EnumParams.NewEnum("Success", 1);
        public static EnumInfo Fail = EnumParams.NewEnum("Fail", 2);
        public static EnumInfo IncorrectPassword = EnumParams.NewEnum("IncorrectPassword", 3);
        public static EnumInfo NoLogin = EnumParams.NewEnum("NoLogin", 4);
        public static EnumInfo NoAuth = EnumParams.NewEnum("NoAuth", 5);
        public static EnumInfo Exist = EnumParams.NewEnum("Exist", 6);
        public static EnumInfo NotExist = EnumParams.NewEnum("NotExist", 7);
        public static EnumInfo CaptchaFailed = EnumParams.NewEnum("CaptchaFailed", 8);
        public static EnumInfo Error = EnumParams.NewEnum("Error", 9);
    }

    [Serializable]
    public class EnumInfo
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public string ToStr() { return Name; }

        public int ToInt() { return Value; }

        public string ToIntString() { return Value.ToString(); }
    }

    public class EnumParams
    {
        public List<EnumInfo> Members = new List<EnumInfo>();
        public Dictionary<string, EnumInfo> Keys = new Dictionary<string, EnumInfo>();
        public Dictionary<int, EnumInfo> Values = new Dictionary<int, EnumInfo>();
        public int Counter = -1;
    }

    public static class EnumFunc
    {
        public static EnumInfo NewEnum(this EnumParams enumParams, string name)
        {
            return NewEnum(enumParams, name, ++enumParams.Counter);
        }

        public static EnumInfo NewEnum(this EnumParams enumParams, string name, int value)
        {
            enumParams.Counter = value;

            EnumInfo cusEnum = new EnumInfo();
            cusEnum.Name = name;
            cusEnum.Value = value;

            enumParams.Members.Add(cusEnum);

            if (!enumParams.Keys.ContainsKey(cusEnum.Name))
                enumParams.Keys.Add(cusEnum.Name, cusEnum);

            if (!enumParams.Values.ContainsKey(cusEnum.Value))
                enumParams.Values.Add(cusEnum.Value, cusEnum);

            return cusEnum;
        }

        public static EnumInfo ParseString(this EnumParams enumParams, string name)
        {
            return enumParams.Keys.ContainsKey(name) ? enumParams.Keys[name] : null;
        }

        public static List<EnumInfo> ParseString(this EnumParams enumParams, string[] nameArray)
        {
            List<EnumInfo> result = new List<EnumInfo>();

            foreach (string name in nameArray)
            {
                EnumInfo enumInfo = enumParams.ParseString(name);
                if (enumInfo == null)
                    continue;

                result.Add(enumInfo);
            }

            return result;
        }

        public static EnumInfo ParseInt(this EnumParams enumParams, int value)
        {
            return enumParams.Values.ContainsKey(value) ? enumParams.Values[value] : null;
        }

        public static List<EnumInfo> ParseInt(this EnumParams enumParams, int[] valueArray)
        {
            List<EnumInfo> result = new List<EnumInfo>();

            foreach (int value in valueArray)
            {
                EnumInfo enumInfo = enumParams.ParseInt(value);
                if (enumInfo == null)
                    continue;

                result.Add(enumInfo);
            }

            return result;
        }
    }

    public class EnumJsonConverter : JsonConverter
    {
        /// <summary>
        /// 是否开启自定义反序列化，值为true时，反序列化时会走ReadJson方法，值为false时，不走ReadJson方法，而是默认的反序列化
        /// </summary>
        public override bool CanRead => false;

        /// <summary>
        /// 是否开启自定义序列化，值为true时，序列化时会走WriteJson方法，值为false时，不走WriteJson方法，而是默认的序列化
        /// </summary>
        public override bool CanWrite => true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType != typeof(EnumInfo))
                return "";

            return reader.Value.ToString();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            EnumInfo result = value as EnumInfo;
            writer.WriteValue(result.ToStr());
        }
    }
}
