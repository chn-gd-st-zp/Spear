using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;

using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Tool
{
    /// <summary>
    /// 网络操作
    /// </summary>
    public class NET
    {
        public static string GetSimplifyOrigin => GetOrigin?.Replace("http://", "").Replace("https://", "");

        public static string IP
        {
            get
            {
                string result = String.Empty;
                var httpContext = ServiceContext.ResolveServ<IHttpContextAccessor>();
                if (httpContext != null)
                {
                    result = httpContext.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (result.IsEmptyString())
                    {
                        result = httpContext.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                    }

                    if (!result.IsEmptyString())
                    {
                        //可能有代理
                        if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式
                            result = null;
                        else
                        {
                            if (result.IndexOf(",") != -1)
                            {
                                //有“,”，估计多个代理。取第一个不是内网的IP。
                                result = result.Replace(" ", "").Replace("'", "");
                                string[] temparyip = result.Split(",;".ToCharArray());
                                for (int i = 0; i < temparyip.Length; i++)
                                {
                                    if (Verification.IsIP(temparyip[i])
                                        && temparyip[i].Substring(0, 3) != "10."
                                        && temparyip[i].Substring(0, 7) != "192.168"
                                        && temparyip[i].Substring(0, 7) != "172.16.")
                                    {
                                        return temparyip[i];    //找到不是内网的地址
                                    }
                                }
                            }
                            else if (Verification.IsIP(result)) //代理即是IP格式 ,IsIPAddress判断是否是IP的方法,
                                return result;
                            else
                                result = null;    //代理中的内容 非IP，取IP
                        }
                    }

                    if (result.IsEmptyString())
                        result = httpContext.HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();

                    if (result.IsEmptyString())
                        result = httpContext.HttpContext.Request.Host.Host;
                }
                return result;
            }
        }

        public static string IP_LW
        {
            get
            {
                return System.Net.NetworkInformation.NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Select(p => p.GetIPProperties())
                    .SelectMany(p => p.UnicastAddresses)
                    .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))
                    .FirstOrDefault()?.Address.ToString();
            }
        }

        /// <summary>  
        /// 获取客户端IP地址  
        /// </summary>  
        /// <returns></returns>  
        public static string GetIP()
        {
            HttpContext httpContext = ServiceContext.ResolveServ<IHttpContextAccessor>().HttpContext;

            if (httpContext.Request.Headers.ContainsKey("$http_X-Real-IP"))
            {
                return httpContext.Request.Headers["$http_X-Real-IP"].FirstOrDefault();
            }
            else if (httpContext.Request.Headers.ContainsKey("X-Real-IP"))
            {
                return httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            }
            else if (httpContext.Request.Headers.ContainsKey("CF-Connecting-IP"))
            {
                return httpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
            }
            else if (httpContext.Request.Headers.ContainsKey("HTTP_X_FORWARDED_FOR"))
            {
                return httpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].FirstOrDefault();
            }
            else if (httpContext.Request.Headers.ContainsKey("REMOTE_ADDR"))
            {
                return httpContext.Request.Headers["REMOTE_ADDR"].FirstOrDefault();
            }
            else
            {
                return "0.0.0.0";
            }
        }

        /// <summary>
        /// 获取客户UserAgent
        /// </summary>
        /// <returns></returns>
        public static string GetUserAgnet()
        {
            HttpContext httpContext = ServiceContext.ResolveServ<IHttpContextAccessor>().HttpContext;

            return httpContext.Request.Headers["User-Agent"].FirstOrDefault();
        }

        /// <summary>
        /// 获取客户请求地址
        /// </summary>
        /// <returns></returns>
        public static string GetUserRequestDomain()
        {
            HttpContext httpContext = ServiceContext.ResolveServ<IHttpContextAccessor>().HttpContext;

            return new StringBuilder()
             .Append(httpContext.Request.Scheme)
             .Append("://")
             .Append(httpContext.Request.Host)
             .Append(httpContext.Request.PathBase).ToString();
        }

        /// <summary>
        /// 获取IP地址信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetLocation(string ip)
        {
            if (ip.IsEmptyString() || ip.Equals("0.0.0.0") || ip.Equals("localhost") || ip.Equals("127.0.0.1"))
            {
                return string.Empty;
            }

            string res = "";

            try
            {
                string url = "http://apis.juhe.cn/ip/ip2addr?ip=" + ip + "&dtype=json&key=a86451534a6e72728b8cea430dabc633";
                res = HttpHelper.GetAsync_String(url).Result;
                var resjson = res.ToObject<objex>();
                res = resjson.result.area + " " + resjson.result.location;
            }
            catch
            {
                res = "";
            }

            if (!res.IsEmptyString())
            {
                return res;
            }

            try
            {
                string url = "https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?query=" + ip + "&resource_id=6006&ie=utf8&oe=gbk&format=json";
                res = HttpHelper.GetAsync_String(url).Result;
                var resjson = res.ToObject<obj>();
                res = resjson.data[0].location;
            }
            catch
            {
                res = "";
            }

            return res;
        }

        /// <summary>
        /// 获取浏览器信息
        /// </summary>
        public static string Browser
        {
            get
            {
                var httpContext = ServiceContext.ResolveServ<IHttpContextAccessor>();
                if (httpContext == null)
                    return string.Empty;
                var browser = httpContext.HttpContext.Request.Headers["User-Agent"];
                return browser;
            }
        }

        /// <summary>
        /// 获取 请求域名
        /// </summary>
        public static string GetWww
        {
            get
            {
                var httpContext = ServiceContext.ResolveServ<IHttpContextAccessor>();
                if (httpContext != null)
                {
                    return httpContext.HttpContext.Request.Host.Host;
                }

                return string.Empty;
            }

        }

        /// <summary>
        /// 获取 请求域名
        /// </summary>
        public static string GetOrigin
        {
            get
            {
                var httpContext = ServiceContext.ResolveServ<IHttpContextAccessor>();
                if (httpContext != null)
                {
                    return httpContext.HttpContext.Request.Headers["Origin"];
                }

                return string.Empty;
            }
        }
    }

    #region 对象实体

    /// <summary>
    /// 百度接口
    /// </summary>
    public class obj
    {
        public List<dataone> data { get; set; }
    }

    public class dataone
    {
        public string location { get; set; }
    }

    /// <summary>
    /// 聚合数据
    /// </summary>
    public class objex
    {
        public string resultcode { get; set; }
        public dataoneex result { get; set; }
        public string reason { get; set; }
        public string error_code { get; set; }
    }

    public class dataoneex
    {
        public string area { get; set; }
        public string location { get; set; }
    }

    #endregion
}
