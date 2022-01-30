using System;

namespace Spear.Inf.Core.Tool
{
    public static class DataConverter
    {
        /// <summary>
        /// 拼接url路径
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string CombineUrl(this string root, string path, params string[] paths)
        {
            if (root.IsEmptyString())
                return "";

            if (path.IsEmptyString())
                return root;

            root += !root.EndsWith("/") ? "/" : "";
            path += !path.EndsWith("/") ? "/" : "";

            Uri baseUri = new Uri(root);
            Uri combinedPaths = new Uri(baseUri, path);
            foreach (string extendedPath in paths)
            {
                combinedPaths = new Uri(combinedPaths, extendedPath);
            }

            return combinedPaths.AbsoluteUri;
        }

        public static byte[] ToByteArray(this string hexStr)
        {
            hexStr = hexStr.Replace(" ", "");
            if ((hexStr.Length % 2) != 0)
                hexStr += " ";

            byte[] returnBytes = new byte[hexStr.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexStr.Substring(i * 2, 2).Trim(), 16);

            return returnBytes;
        }

        public static string ToHexStr(this byte[] dataArray)
        {
            string returnStr = "";
            if (dataArray != null)
            {
                for (int i = 0; i < dataArray.Length; i++)
                {
                    returnStr += dataArray[i].ToString("X2");
                }
            }
            return returnStr;
        }
    }
}
