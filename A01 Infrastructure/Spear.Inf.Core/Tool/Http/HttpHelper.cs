using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Spear.Inf.Core.Tool
{
    public static class HttpHelper
    {
        public static async Task<HttpResponseMessage> PostAsync(string url, string contentType = null, Dictionary<string, string> headers = null, string postData = null, CookieContainer cookieContainer = null)
        {
            postData = postData ?? string.Empty;

            HttpClientHandler handler = null;
            if (cookieContainer != null)
            {
                handler = new HttpClientHandler();
                handler.UseCookies = true;
                handler.CookieContainer = cookieContainer;
            }

            using (HttpClient client = handler == null ? new HttpClient() : new HttpClient(handler))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
                {
                    if (contentType != null)
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                    return await client.PostAsync(url, httpContent);
                }
            }
        }

        public static async Task<string> PostAsync_String(string url, string contentType = null, Dictionary<string, string> headers = null, string postData = null, CookieContainer cookieContainer = null)
        {
            var obj = await PostAsync(url, contentType, headers, postData, cookieContainer);
            return obj.Content.ReadAsStringAsync().Result;
        }

        public static async Task<T> PostAsync_Object<T>(string url, string contentType = null, Dictionary<string, string> headers = null, string postData = null, CookieContainer cookieContainer = null)
        {
            var obj = await PostAsync_String(url, contentType, headers, postData, cookieContainer);
            return obj.ToObject<T>();
        }

        public static async Task<HttpResponseMessage> GetAsync(string url, string contentType = null, Dictionary<string, string> headers = null, CookieContainer cookieContainer = null)
        {
            HttpClientHandler handler = null;
            if (cookieContainer != null)
            {
                handler = new HttpClientHandler();
                handler.UseCookies = true;
                handler.CookieContainer = cookieContainer;
            }

            using (HttpClient client = handler == null ? new HttpClient() : new HttpClient(handler))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                if (!contentType.IsEmptyString())
                    client.DefaultRequestHeaders.Add("ContentType", contentType);

                return await client.GetAsync(url);
            }
        }

        public static async Task<string> GetAsync_String(string url, string contentType = null, Dictionary<string, string> headers = null, CookieContainer cookieContainer = null)
        {
            var obj = await GetAsync(url, contentType, headers, cookieContainer);
            return obj.Content.ReadAsStringAsync().Result;
        }

        public static async Task<T> GetAsync_Object<T>(string url, string contentType = null, Dictionary<string, string> headers = null, CookieContainer cookieContainer = null)
        {
            var obj = await GetAsync_String(url, contentType, headers, cookieContainer);
            return obj.ToObject<T>();
        }

        public static object GetQueryString(this HttpRequest request)
        {
            QueryString queryString = request.QueryString;
            NameValueCollection Collection = HttpUtility.ParseQueryString(queryString.ToString());
            return Collection.AllKeys.ToDictionary(k => k, v => Collection[v]).ToJson();
        }

        public static object GetQueryString(this HttpRequest request, string name)
        {
            QueryString queryString = request.QueryString;
            NameValueCollection Collection = HttpUtility.ParseQueryString(queryString.ToString());
            return Collection[name];
        }

        public static async Task<object> GetRequestValue(this HttpRequest request)
        {
            try
            {
                if (request.Method == "GET")
                {
                    QueryString queryString = request.QueryString;
                    NameValueCollection Collection = HttpUtility.ParseQueryString(queryString.ToString());
                    return Collection.AllKeys.ToDictionary(k => k, v => Collection[v]).ToJson();
                }
                else
                {
                    if (request.HasFormContentType && request.Form != null && request.Form.Keys.Count > 0)
                    {
                        return request.Form.Keys.ToDictionary(k => k, v => request.Form[v].First()).ToJson();
                    }
                    else
                    {
                        string body = await request.ReadBodyAsync();
                        return body.GetCallBackStr();
                    }
                }
            }
            catch
            {
                string body = await request.ReadBodyAsync();
                return body.GetCallBackStr();
            }
        }

        public static async Task<string> ReadBodyAsync(this HttpRequest request)
        {
            if (request.ContentLength > 0)
            {
                await EnableRewindAsync(request).ConfigureAwait(false);
                var encoding = GetRequestEncoding(request);
                return await ReadStreamAsync(request.Body, encoding).ConfigureAwait(false);
            }
            return null;
        }

        private static string GetCallBackStr(this string str)
        {
            string json = string.Empty;
            try
            {
                if (str.FirstOrDefault().ToString() == "{" && str.LastOrDefault().ToString() == "}")
                {
                    json = str;
                }
                else if (str.Contains('&') && str.Contains('='))
                {
                    NameValueCollection Collection = HttpUtility.ParseQueryString(str);
                    json = Collection.AllKeys.ToDictionary(k => k, v => Collection[v]).ToJson();
                }
                else if (str.Contains("?xml"))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(str);
                    json = JsonConvert.SerializeXmlNode(xml);
                }
            }
            catch
            {
                json = str;
            }
            return str;
        }

        private static async Task EnableRewindAsync(HttpRequest request)
        {
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();

                await request.Body.DrainAsync(CancellationToken.None);
                request.Body.Seek(0L, SeekOrigin.Begin);
            }
        }

        private static Encoding GetRequestEncoding(HttpRequest request)
        {
            var requestContentType = request.ContentType;
            var requestMediaType = requestContentType == null ? default(MediaType) : new MediaType(requestContentType);
            var requestEncoding = requestMediaType.Encoding;
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.UTF8;
            }
            return requestEncoding;
        }

        private static async Task<string> ReadStreamAsync(Stream stream, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(stream, encoding, true, 1024, true))//这里注意Body部分不能随StreamReader一起释放
            {
                stream.Seek(0, SeekOrigin.Begin);//内容读取完成后需要将当前位置初始化，否则后面的InputFormatter会无法读取
                var str = await sr.ReadToEndAsync();
                return str;
            }
        }
    }
}
