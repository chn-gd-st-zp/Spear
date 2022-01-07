using System;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Primitives;

namespace Spear.Inf.Core.Tool
{
    public static class Verification
    {
        #region 字符串判断

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

        #endregion

        #region 类型判断

        #region 接口

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="basicType"></param>
        /// <returns></returns>
        public static bool IsImplementedType(this Type type, Type basicType)
        {
            return basicType.IsAssignableFrom(type);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsImplementedType<T>(this object obj)
        {
            Type objType = obj.GetType();
            Type basicObjType = typeof(T);

            return objType.IsImplementedType(basicObjType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static bool IsImplementedType<T>(this Type objType)
        {
            Type basicObjType = typeof(T);

            return objType.IsImplementedType(basicObjType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="basicObj"></param>
        /// <returns></returns>
        public static bool IsImplementedType(this object obj, object basicObj)
        {
            Type objType = obj.GetType();
            Type basicObjType = basicObj.GetType();

            return objType.IsImplementedType(basicObjType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="basicObj"></param>
        /// <returns></returns>
        public static bool IsImplementedType(this Type objType, object basicObj)
        {
            Type basicObjType = basicObj.GetType();

            return objType.IsImplementedType(basicObjType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="basicObjType"></param>
        /// <returns></returns>
        public static bool IsImplementedType(this object obj, Type basicObjType)
        {
            Type objType = obj.GetType();

            return objType.IsImplementedType(basicObjType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="classInterfaceArray"></param>
        /// <returns></returns>
        public static bool IsImplementedType<T>(this object obj, Type[] classInterfaceArray)
        {
            Type objType = obj.GetType();
            Type basicObjType = typeof(T);

            if (objType.IsImplementedType(basicObjType))
                return true;

            if (classInterfaceArray.Contains(basicObjType))
                return true;

            return false;
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="classInterfaceArray"></param>
        /// <param name="basicObj"></param>
        /// <returns></returns>
        public static bool IsImplementedType(this object obj, Type[] classInterfaceArray, object basicObj)
        {
            Type objType = obj.GetType();
            Type basicObjType = basicObj.GetType();

            if (objType.IsImplementedType(basicObjType))
                return true;

            if (classInterfaceArray.Contains(basicObjType))
                return true;

            return false;
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="classInterfaceArray"></param>
        /// <param name="basicObjType"></param>
        /// <returns></returns>
        public static bool IsImplementedType(this Type objType, Type[] classInterfaceArray, Type basicObjType)
        {
            if (objType.IsImplementedType(basicObjType))
                return true;

            if (classInterfaceArray.Contains(basicObjType))
                return true;

            return false;
        }

        #endregion

        /// <summary>
        /// 判断是否 实现 或 继承 某个泛型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsImplementedNExtendGenericType(this Type type, Type genericType)
        {
            // 匹配接口。
            var isTheRawGenericType = type.GetInterfaces().Any(o => genericType == (o.IsGenericType ? o.GetGenericTypeDefinition() : o));
            if (isTheRawGenericType)
                return true;

            // 匹配类型。
            while (type != typeof(object))
            {
                isTheRawGenericType = genericType == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
                if (isTheRawGenericType)
                    return true;

                type = type.BaseType;
            }

            // 没有找到任何匹配的接口或类型。
            return false;
        }

        /// <summary>
        /// 判断是否 继承 某个类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="basicType"></param>
        /// <returns></returns>
        public static bool IsExtendType(this Type type, Type basicType)
        {
            return type.IsSubclassOf(basicType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="basicObj"></param>
        /// <returns></returns>
        public static bool IsExtendType(this object obj, object basicObj)
        {
            Type type = obj.GetType();
            Type basicType = basicObj.GetType();

            return type.IsExtendType(basicType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="basicType"></param>
        /// <returns></returns>
        public static bool IsExtendType(this object obj, Type basicType)
        {
            Type type = obj.GetType();

            return type.IsExtendType(basicType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsExtendType<T>(this object obj)
        {
            Type type = obj.GetType();
            Type basicType = typeof(T);

            return type.IsExtendType(basicType);
        }

        #endregion

        #region 正则判断

        /// <summary>
        /// 验证数字（包含整数和小数）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNum(this string data)
        {
            string expression = @"^[-]?\d+[.]?\d*$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证整数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsInt(this string data)
        {
            string expression = "^[0-9]*$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证传真
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsFax(this string data)
        {
            string expression = @"86-\d{2,3}-\d{7,8}";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsIP(this string data)
        {
            string expression = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证密码格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool CheckPassword(this string data)
        {
            string expression = @"^[A-Za-z_0-9]{6,18}$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证用户名(字母开头)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsUserName(this string data)
        {
            string expression = @"^[a-zA-Z]$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证邮件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsEmail(this string data)
        {
            string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证手机
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsMobile(this string data)
        {
            string expression = @"^[1]([3-9])[0-9]{9}$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证电话
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsPhone(this string data)
        {
            string expression = @"\d{3,4}-\d{7,8}";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证QQ格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsQQ(this string data)
        {
            string expression = "^[1-9][0-9]{4,12}$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证微信格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsWeChat(this string data)
        {
            string expression = "^[a-zA-Z0-9_-]{6,20}$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证用户真实姓名(是否含有2位以上中文字符)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsTrueName(this string data)
        {
            string expression = "^[\u4e00-\u9fa5]{2,20}$";

            return Regex.IsMatch(data, expression);
        }

        /// <summary>
        /// 验证昵称
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNickName(this string data)
        {
            string expression = "^[a-zA-Z0-9\u4e00-\u9fa5]{2,8}$";

            return Regex.IsMatch(data, expression);
        }

        #endregion
    }
}
