using System;
using System.IO;

namespace Spear.Inf.Core.Tool
{
    public static  class FileConverter
    {
        public static byte[] ToBytes(this string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }

        /// <summary>
        /// 文件流 转换成 Base64字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToBase64(this Stream stream, bool closeStream = false)
        {
            string result = null;

            try
            {
                if (stream == null)
                    return null;

                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, byteArray.Length);

                result = Convert.ToBase64String(byteArray);

                if (closeStream)
                    stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Base64字符串 转换成 文件流
        /// 记得关闭、释放流
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static Stream ToStream(this string base64)
        {
            Stream result = null;

            try
            {
                byte[] byteArray = Convert.FromBase64String(base64);
                result = new MemoryStream(byteArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// stream 保存成文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        public static void ToFile(this Stream stream, string fileName)
        {
            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            stream.Seek(0, SeekOrigin.Begin);

            FileStream fs = new FileStream(fileName, FileMode.Create);

            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(bytes);

            bw.Close();

            fs.Close();
        }
    }
}
