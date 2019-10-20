using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace org.Common
{
    public class CookieHelper
    {

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireMin">过期时间(分钟)</param>
        /// <param name="isHttp">是否httpOnly</param>
        public static void Set(string key, string value, int expireMin = 600, bool isHttp = true)
        {
            var c = new HttpCookie(key)
            {
                Value = value,
                Expires = DateTime.Now.AddMinutes(expireMin),
                HttpOnly = isHttp,
            };
            HttpContext.Current.Response.Cookies.Add(c);
        }

        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            string value = string.Empty;

            HttpCookie c = HttpContext.Current.Request.Cookies[key];

            return c != null
                   ? HttpContext.Current.Server.HtmlEncode(c.Value).Trim()
                   : value;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return HttpContext.Current.Request.Cookies[key] != null;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        public static void Delete(string key)
        {
            if (Exists(key))
            {
                var c = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddDays(-10),
                    Value = null
                };

                HttpContext.Current.Response.Cookies.Add(c);
            }
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="DeleteAll"></param>
        public static void DeleteAll(bool deleteServerCookies = false)
        {
            for (int i = 0; i <= HttpContext.Current.Request.Cookies.Count - 1; i++)
            {
                if (HttpContext.Current.Request.Cookies[i] != null)
                {
                    Delete(HttpContext.Current.Request.Cookies[i].Name);
                }
            }
            if (deleteServerCookies)
            {
                HttpContext.Current.Request.Cookies.Clear();
            }
        }

    }
}
