using Newtonsoft.Json;
using org.Bll;
using org.Common;
using org.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace org.Admin.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 换行符号 
        /// </summary>
		public static string _n = "@#$@";
        /// <summary>
        /// 
        /// </summary>
        static string _QiListMoreServer = ConfigurationManager.AppSettings["QiListMoreServer"]?.ToString().Trim() ?? "";
        /// <summary>
        /// 分页条数
        /// </summary>
        public int _PageSize = 15;

        /// <summary>
        /// 当前登录用户属性
        /// </summary>
        public LoginUser _UserInfo { get; set; }

        /// <summary>
        /// 重新基类在Action执行之前的事情
        /// </summary>
        /// <param name="filterContext">重写方法的参数</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }


            var str = CookieHelper.Get(Keys.AdminAuth);
            if (!string.IsNullOrEmpty(str))
            {
                var json = XXTEA.Decode(str);
                var user = JsonConvert.DeserializeObject<LoginUser>(json);
                if (user != null && user.uid > 0)
                {
                    _UserInfo = user;
                }
                else
                {
                    Login(filterContext);
                }
            }
            else
            {
                Login(filterContext);
            }
        }

        /// <summary>
        /// 跳转到登录界面
        /// </summary>
        void Login(ActionExecutingContext filterContext)
        {
            CookieHelper.Delete(Keys.AdminAuth);
            filterContext.Result = new RedirectResult(Url.Action("Index", "Login"));
        }


        #region 错误提示信息
        /// <summary>
        /// 错误提示信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected void Error(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>\r\n");
            sb.Append("<html>\r\n");
            sb.Append("<head>\r\n");
            sb.Append("    <title>错误页面</title>\r\n");
            sb.Append("    <meta charset=\"utf-8\" />\r\n");
            sb.Append("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\" />\r\n");
            sb.Append("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1\" />\r\n");
            sb.Append("    <meta name=\"apple-mobile-web-app-capable\" content=\"yes\" />\r\n");
            sb.Append("    <meta name=\"apple-mobile-web-app-status-bar-style\" content=\"black\" />\r\n");
            sb.Append("    <meta content=\"telephone=no\" name=\"format-detection\" />\r\n");
            sb.Append("    <link rel=\"stylesheet\" type=\"text/css\" class=\"ui\" href=\"/static/css/semantic.min.css\" />\r\n");
            sb.Append("    <link rel=\"stylesheet\" type=\"text/css\" href=\"/static/css/docs.css?v=1\" />\r\n");
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("setTimeout(\"window.location.href = 'javascript:history.back(-1);'\", 3000);");
            sb.Append("</script>");
            sb.Append("</head>\r\n");
            sb.Append("<body id=\"example\" class=\"wide\" ontouchstart=\"\">\r\n");
            sb.Append("    <div class=\"pusher\">\r\n");
            sb.Append("        <div class=\"full height\">\r\n");
            sb.Append("            <div class=\"article\">\r\n");
            sb.Append("                <div class=\"ui masthead vertical segment\">\r\n");
            sb.Append("                    <div class=\"ui container\">\r\n");
            sb.Append("                        <div class=\"introduction\">\r\n");
            sb.Append("                            <h1 class=\"ui header\">\r\n");
            sb.Append("                                错误页面\r\n");
            sb.Append("                            </h1>\r\n");
            sb.Append("                        </div>\r\n");
            sb.Append("                    </div>\r\n");
            sb.Append("                </div>\r\n");
            sb.Append("                <div class=\"main container\">\r\n");
            sb.Append("                    <div class=\"ui active tab\" data-tab=\"overview\">\r\n");
            sb.Append("                        <div class=\"ui error message\" style=\"margin-top:8em\">\r\n");
            sb.Append("                            <div class=\"header\">\r\n");
            sb.Append("                                错误信息\r\n");
            sb.Append("                            </div>\r\n");
            sb.Append("                            <ul class=\"list\">\r\n");
            sb.Append("                                <li>" + message + "</li>\r\n");
            sb.Append("                                <li><a href='javascript:history.back(-1);'>5秒后跳转到上一页</a></li>\r\n");
            sb.Append("                            </ul>\r\n");
            sb.Append("                        </div>\r\n");
            sb.Append("                    </div>\r\n");
            sb.Append("                </div>\r\n");
            sb.Append("            </div>\r\n");
            sb.Append("        </div>\r\n");
            sb.Append("    </div>\r\n");
            sb.Append("</body>\r\n");
            sb.Append("</html>");

            HttpContext.Response.BufferOutput = true;
            HttpContext.Response.Write(sb);
            HttpContext.Response.End();
        }
        #endregion

        #region  操作成功
        /// <summary>
        /// 错误提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="url">返回地址</param>
        /// <returns></returns>
        protected void Success(string message, string url = null)
        {
            string redirect = string.Empty;
            if (string.IsNullOrEmpty(url))
                redirect = "history.back(-1);";
            else
                redirect = url;

            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>\r\n");
            sb.Append("<html>\r\n");
            sb.Append("<head>\r\n");
            sb.Append("    <title>操作成功</title>\r\n");
            sb.Append("    <meta charset=\"utf-8\" />\r\n");
            sb.Append("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\" />\r\n");
            sb.Append("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1\" />\r\n");
            sb.Append("    <meta name=\"apple-mobile-web-app-capable\" content=\"yes\" />\r\n");
            sb.Append("    <meta name=\"apple-mobile-web-app-status-bar-style\" content=\"black\" />\r\n");
            sb.Append("    <meta content=\"telephone=no\" name=\"format-detection\" />\r\n");
            sb.Append("    <link rel=\"stylesheet\" type=\"text/css\" class=\"ui\" href=\"/static/css/semantic.min.css\" />\r\n");
            sb.Append("    <link rel=\"stylesheet\" type=\"text/css\" href=\"/static/css/docs.css?v=1\" />\r\n");
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("setTimeout(\"top.location.href='" + redirect + "'\", 3000);");
            sb.Append("</script>");
            sb.Append("</head>\r\n");
            sb.Append("<body id=\"example\" class=\"wide\" ontouchstart=\"\">\r\n");
            sb.Append("    <div class=\"pusher\">\r\n");
            sb.Append("        <div class=\"full height\">\r\n");
            sb.Append("            <div class=\"article\">\r\n");
            sb.Append("                <div class=\"ui masthead vertical segment\">\r\n");
            sb.Append("                    <div class=\"ui container\">\r\n");
            sb.Append("                        <div class=\"introduction\">\r\n");
            sb.Append("                            <h1 class=\"ui header\">\r\n");
            sb.Append("                                操作成功\r\n");
            sb.Append("                            </h1>\r\n");
            sb.Append("                        </div>\r\n");
            sb.Append("                    </div>\r\n");
            sb.Append("                </div>\r\n");
            sb.Append("                <div class=\"main container\">\r\n");
            sb.Append("                    <div class=\"ui active tab\" data-tab=\"overview\">\r\n");
            sb.Append("                        <div class=\"ui success  message\" style=\"margin-top:8em\">\r\n");
            sb.Append("                            <div class=\"header\">\r\n");
            sb.Append("                                操作成功\r\n");
            sb.Append("                            </div>\r\n");
            sb.Append("                            <ul class=\"list\">\r\n");
            sb.Append("                                <li>" + message + "</li>\r\n");
            if (string.IsNullOrEmpty(url))
                sb.Append("                                <li><a href='javascript:history.back(-1);'>5秒后跳转到上一页</a></li>\r\n");
            else
                sb.Append("                                <li><a href='" + url + "'>5秒后跳转到上一页</a></li>\r\n");
            sb.Append("                            </ul>\r\n");
            sb.Append("                        </div>\r\n");
            sb.Append("                    </div>\r\n");
            sb.Append("                </div>\r\n");
            sb.Append("            </div>\r\n");
            sb.Append("        </div>\r\n");
            sb.Append("    </div>\r\n");
            sb.Append("</body>\r\n");
            sb.Append("</html>");

            Response.Write(sb);
            Response.End();
        }
        #endregion

        public static GoIntJson<T> Get<T>(string url, Dictionary<string, string> headers = null)
        {
            string recieve = string.Empty;
            try
            {
                recieve = PostHttp(url, "GET", headers, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                GoIntJson<T> ore = JsonConvert.DeserializeObject<GoIntJson<T>>(recieve);
                return ore;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string PostHttp(string url, string methodType, Dictionary<string, string> headers, string body = null)
        {
            HttpWebRequest request = null;
            StreamWriter sw = null;
            StreamReader s = null;
            string ret;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);//请求
                request.Method = methodType;
                if (headers != null && headers.Count > 0)
                    for (var i = 0; i < headers.Count; i++)
                        request.Headers.Add(headers.ElementAt(i).Key, headers.ElementAt(i).Value);

                request.KeepAlive = true;
                //request.CookieContainer = cook;
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.ConnectionLimit = int.MaxValue;

                if (!string.IsNullOrEmpty(body))
                {
                    sw = new StreamWriter(request.GetRequestStream());//获取写入流
                    sw.Write(body);
                    sw.Flush();                                     //强制写入
                }

                WebResponse response = request.GetResponse();   //获取响应
                s = new StreamReader(response.GetResponseStream());//获取响应流
                ret = s.ReadToEnd();                             //读取数据
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (s != null) s.Close();
                if (sw != null) sw.Close();
                if (request != null) request.Abort();
                s = null;
                sw = null;
                request = null;
            }
            return ret;

        }




        #region 返回Json ActionResult
        /// <summary>
        /// 返回json ActionResult
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ActionResult NetJSON(object obj)
        {
            return Content(JsonConvert.SerializeObject(obj), "application/json", Encoding.UTF8);
        }
        #endregion

        #region go语言接口返回的json数据结构
        public class GoIntJson<T>
        {
            public int code { get; set; }
            public string msg { get; set; }
            public string stime { get; set; }
            public Body<T> body { get; set; }
        }
        public class Body<T>
        {
            public int? count { set; get; }
            public T data { set; get; }
        }
        #endregion

        #region 期数返回模型
        public class QiMoreConfig
        {
            public QiConfig now { set; get; }

            public List<QiConfig> recents { set; get; }
        }
        public class QiConfig
        {
            public long qi { set; get; }
            public string bt { set; get; }
            public string et { set; get; }
            public string at { set; get; }
        }
        #endregion
    }
}