using Microsoft.Extensions.Primitives;

namespace Spear.Inf.Core.Tool
{
    public static class StringExtension
    {
        public static bool IsEmptyString(this string text)
        {
            if (text == null)
                return true;

            return string.IsNullOrWhiteSpace(text);
            //return string.IsNullOrEmpty(text);
        }

        public static bool IsEmptyString(this StringValues text)
        {
            return text.ToString().IsEmptyString();
        }

        public static bool IsEqual(this string value1, string value2, bool ignoreCase = true)
        {
            return string.Compare(value1, value2, ignoreCase) == 0;
        }
    }
}
