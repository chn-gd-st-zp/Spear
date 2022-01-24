using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

using AutoMapper;
using Mapster;

using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.Tool
{
    /// <summary>
    /// 数据转换器
    /// </summary>
    public static class DataConvert
    {
        #region 枚举

        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Convert2Enum(this Type type, string value)
        {
            try
            {
                return Enum.Parse(type, value, true);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 将数字转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Convert2Enum<T>(this int value) where T : Enum
        {
            try
            {
                return (T)Enum.ToObject(typeof(T), value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Convert2Enum<T>(this string value) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Convert2EnumList<T>() where T : Enum
        {
            List<T> result = new List<T>();

            try
            {
                string[] nameArray = Enum.GetNames(typeof(T));
                foreach (string name in nameArray)
                    result.Add(Convert2Enum<T>(name));
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataArray"></param>
        /// <returns></returns>
        public static List<T> Convert2EnumList<T>(this string[] dataArray) where T : Enum
        {
            List<T> result = new List<T>();

            try
            {
                foreach (string data in dataArray)
                    result.Add(Convert2Enum<T>(data));
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 将枚举转为字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string[]> Convert2Dictionary(this Type type)
        {
            var dic = new Dictionary<int, string[]>();

            var eValueArray = Enum.GetValues(type);
            foreach (var eValue in eValueArray)
            {
                var key = (int)eValue;
                var value = new string[] { eValue.ToString(), type.GetRemark(eValue.ToString()) };

                dic.Add(key, value);
            }

            return dic;
        }

        /// <summary>
        /// 将枚举转为字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetEnum"></param>
        /// <returns></returns>
        public static Dictionary<int, string[]> Convert2Dictionary<T>() where T : Enum
        {
            var dic = new Dictionary<int, string[]>();

            var eValueArray = Enum.GetValues(typeof(T));
            foreach (var eValue in eValueArray)
            {
                var key = (int)eValue;
                var value = new string[] { eValue.ToString(), eValue.ToString().Convert2Enum<T>().GetRemark() };

                dic.Add(key, value);
            }

            return dic;
        }

        #endregion

        #region 文件

        public static byte[] ToBytes(this string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }

        /// <summary>
        /// 文件流 转换成 Base64字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string Convert2Base64(this Stream stream, bool closeStream = false)
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
        public static Stream Convert2Stream(this string base64)
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
        public static void StreamToFile(this Stream stream, string fileName)
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

        #endregion

        #region 数学

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dividend">被除数</param>
        /// <param name="divisor">除数（不能为0）</param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static decimal Divider(decimal dividend, decimal divisor, int decimals = -1)
        {
            if (divisor == 0)
                return 0;

            decimal result = dividend / divisor;

            if (decimals == -1)
                return result;

            result = Math.Round(result, decimals);

            return result;
        }

        /// <summary>
        /// 转为正数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal ToPositive(this decimal num)
        {
            return num >= 0 ? num : Math.Abs(num);
        }

        /// <summary>
        /// 转为负数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal ToNegative(this decimal num)
        {
            return num >= 0 ? -num : num;
        }

        #endregion

        #region 映射

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static object TypeTo(this object obj, Type destinationType)
        {
            if (!destinationType.IsGenericType)
            {
                return Convert.ChangeType(obj, destinationType);
            }
            else
            {
                Type genericTypeDefinition = destinationType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                    return Convert.ChangeType(obj, Nullable.GetUnderlyingType(destinationType));
            }

            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", obj.GetType().FullName, destinationType.FullName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object TypeTo<TDestination>(this object obj)
        {
            return TypeTo(obj, typeof(TDestination));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="byProfile"></param>
        /// <returns></returns>
        public static TDestination MapTo<TDestination>(this object source, bool byProfile = false)
            where TDestination : class
        {
            if (source == null) return default(TDestination);

            Type tSource = source.GetType();
            Type tTarget = typeof(TDestination);

            IMapper mapper = byProfile ? ServiceContext.Resolve<IMapper>() : new MapperConfiguration(cfg => cfg.CreateMap(tSource, tTarget)).CreateMapper();

            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="byProfile"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> MapTo<TSource, TDestination>(this IEnumerable<TSource> source, bool byProfile = false)
            where TSource : class
            where TDestination : class
        {
            return source.Select(o => o.MapTo<TDestination>(byProfile));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TDestination>(this object source)
        {
            return source.AdaptTo<TDestination>(TypeAdapterConfig.GlobalSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TDestination>(this object source, TypeAdapterConfig config)
        {
            if (source == null) return default(TDestination);

            return config.GetDynamicMapFunction<TDestination>(source.GetType())(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TSource, TDestination>(this TSource source)
        {
            return TypeAdapter<TSource, TDestination>.Map(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TSource, TDestination>(this TSource source, TypeAdapterConfig config)
        {
            return config.GetMapFunction<TSource, TDestination>()(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return source.AdaptTo(destination, TypeAdapterConfig.GlobalSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TSource, TDestination>(this TSource source, TDestination destination, TypeAdapterConfig config)
        {
            return config.GetMapToTargetFunction<TSource, TDestination>()(source, destination);
        }

        #endregion

        #region 其他

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

        #endregion

        #region 对象、DT、List

        /// <summary>
        /// DataTable转成实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataTable dt)
        {
            T s = InstanceCreator.Create<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return default(T);
            }
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                if (info != null)
                {
                    try
                    {
                        if (!Convert.IsDBNull(dt.Rows[0][i]))
                        {
                            object v = null;
                            if (info.PropertyType.ToString().Contains("System.Nullable"))
                            {
                                v = Convert.ChangeType(dt.Rows[0][i], Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                v = Convert.ChangeType(dt.Rows[0][i], info.PropertyType);
                            }
                            info.SetValue(s, v, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// DataTable转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToDataList<T>(this DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow item in dt.Rows)
            {
                T s = InstanceCreator.Create<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                object v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    v = Convert.ChangeType(item[i], Nullable.GetUnderlyingType(info.PropertyType));
                                }
                                else
                                {
                                    v = Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        /// <summary>
        /// List转成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            var dt = new DataTable();

            var props = typeof(T).GetProperties();
            props.ToList().ForEach(o =>
            {
                DataColumn column = null;

                if (o.PropertyType.IsGenericType && o.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    column = new DataColumn(o.Name, o.PropertyType.GetGenericArguments()[0]);
                else
                    column = new DataColumn(o.Name, o.PropertyType);

                dt.Columns.Add(column);
            });

            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        obj = obj == null ? DBNull.Value : obj;
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }

            return dt;
        }

        /// <summary>
        /// List转成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static DataTable ToDataTable2<T>(this List<T> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            var result = CreateTable<T>();
            FillData(result, entities);
            return result;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static DataTable CreateTable<T>()
        {
            var result = new DataTable();
            var type = typeof(T);
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    propertyType = propertyType.GetGenericArguments()[0];
                result.Columns.Add(property.Name, propertyType);
            }
            return result;
        }

        /// <summary>
        /// 创建行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static DataRow CreateRow<T>(DataTable dt, T entity)
        {
            DataRow row = dt.NewRow();
            var type = typeof(T);
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                row[property.Name] = property.GetValue(entity) ?? DBNull.Value;
            }

            return row;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="entities"></param>
        private static void FillData<T>(DataTable dt, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                dt.Rows.Add(CreateRow(dt, entity));
            }
        }

        #endregion
    }
}
