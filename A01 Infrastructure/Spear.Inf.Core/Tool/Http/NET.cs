using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Http;

namespace Spear.Inf.Core.Tool
{
    /// <summary>
    /// 网络操作
    /// </summary>
    public class NET
    {
        public static string GetSimplifyOrigin => Origin?.Replace("http://", "").Replace("https://", "");

        /// <summary>
        /// 获取客户UserAgent
        /// </summary>
        public static string UserAgnet
        {
            get
            {
                HttpContext httpContext = ServiceContext.Resolve<IHttpContextAccessor>().HttpContext;
                return httpContext.Request.Headers["User-Agent"].FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取客户请求地址
        /// </summary>
        public static string UserRequestDomain
        {
            get
            {
                HttpContext httpContext = ServiceContext.Resolve<IHttpContextAccessor>().HttpContext;

                return new StringBuilder()
                 .Append(httpContext.Request.Scheme)
                 .Append("://")
                 .Append(httpContext.Request.Host)
                 .Append(httpContext.Request.PathBase).ToString();
            }
        }

        /// <summary>
        /// 获取浏览器信息
        /// </summary>
        public static string Browser
        {
            get
            {
                var httpContext = ServiceContext.Resolve<IHttpContextAccessor>();
                if (httpContext == null)
                    return string.Empty;
                var browser = httpContext.HttpContext.Request.Headers["User-Agent"];
                return browser;
            }
        }

        /// <summary>
        /// 获取 请求域名
        /// </summary>
        public static string Host
        {
            get
            {
                var httpContext = ServiceContext.Resolve<IHttpContextAccessor>();
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
        public static string Origin
        {
            get
            {
                var httpContext = ServiceContext.Resolve<IHttpContextAccessor>();
                if (httpContext != null)
                {
                    return httpContext.HttpContext.Request.Headers["Origin"];
                }

                return string.Empty;
            }
        }

        /// <summary>  
        /// 获取客户端IP地址  
        /// </summary>  
        /// <returns></returns>  
        public static string GetIP()
        {
            var httpContext = ServiceContext.Resolve<IHttpContextAccessor>().HttpContext;
            var request = httpContext.Request;

            if (request.Headers.ContainsKey("$http_X-Real-IP"))
            {
                return request.Headers["$http_X-Real-IP"].FirstOrDefault();
            }
            else if (request.Headers.ContainsKey("X-Real-IP"))
            {
                return request.Headers["X-Real-IP"].FirstOrDefault();
            }
            else if (request.Headers.ContainsKey("CF-Connecting-IP"))
            {
                return request.Headers["CF-Connecting-IP"].FirstOrDefault();
            }
            else if (request.Headers.ContainsKey("HTTP_X_FORWARDED_FOR"))
            {
                return request.Headers["HTTP_X_FORWARDED_FOR"].FirstOrDefault();
            }
            else if (request.Headers.ContainsKey("REMOTE_ADDR"))
            {
                return request.Headers["REMOTE_ADDR"].FirstOrDefault();
            }
            else
            {
                return "0.0.0.0";
            }
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
