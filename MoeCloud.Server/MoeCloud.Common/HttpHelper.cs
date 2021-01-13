using System;
using System.IO;
using System.Net;
using System.Text;

namespace MoeCloud.Common
{
    public class HttpHelper
    {
        /// <summary>
        /// Get请求获取数据
        /// </summary>
        /// <param name="url">Url:*://*/*?a={1}&b={2}</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            try
            {
                string result = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);              
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                    reader.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Post方法获取数据
        /// </summary>
        /// <param name="url">请求的Url地址</param>
        /// <param name="data">需要发送的数据{a={1}&b={2}}或{\"a\":1,\"b\":2}</param>
        /// <param name="contentType">内容类型{application/json}</param>
        /// <returns></returns>
        public static string HttpPost(string url, string data,string token, string contentType = "application/x-www-form-urlencoded")
        {
            try
            {
                string result = string.Empty;
                byte[] bytes = Encoding.Default.GetBytes(data);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);                
                request.Method = "POST";
                request.Headers.Add("Authorized", token);
                request.ContentType = contentType;
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                    reader.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 去除一段字符串中的HTML标签
        /// </summary>
        /// <param name="html">源HTML的字符串</param>
        /// <param name="length">截取摘要长度[0为不截取]</param>
        /// <param name="placeholder">占位符</param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html, int length = 0, string placeholder = "...")
        {
            // 匹配左右尖括号
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            // 匹配html编码符号
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            // 替换所有空格
            strText = strText.Replace(" ", "");
            // 替换所有换行
            strText = strText.Replace("\r\n", "");
            // 判断字符串长度
            if (length > 0 && strText.Length > length)
            {
                // 截取字符串并添加占位符
                return strText.Substring(0, length) + placeholder;
            }
            // 返回字符串
            return strText;
        }
    }
}
