using Newtonsoft.Json;
using org.Bll;
using org.Model;
using org.Common;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace org.Admin.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CookieHelper.Delete(Keys.AdminAuth);
            ModelState.AddModelError("adminPassword", "请填写登录信息");

            return View();
        }

        /// 图形验证码
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Code()
        {
            string page = "login";
            string code = Utils.GetRandomInt();
            CookieHelper.Set(page, XXTEA.Encode(code), 10);
            var b = Utils.GetCheckCode(code);
            if (b.Length > 0)
            {
                return File(b, "image/png");
            }
            else
            {
                return Content("");
            }

        }


        /// <summary>
        /// 返回json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ActionResult NetJSON(object obj)
        {
            return Content(JsonConvert.SerializeObject(obj), "application/json", Encoding.UTF8);
        }


        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Check(string qqnum, string password, string code)
        {
            OutInfo ret = new OutInfo() { code = 0, msg = "登录失败：数据填写不完整" };
            if (string.IsNullOrEmpty(qqnum) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(code))
            {
                return NetJSON(ret);
            }

            string ck = CookieHelper.Get("login");

            if (!string.IsNullOrEmpty(ck))
            {
                var chkCode = XXTEA.Decode(ck);
                if (!code.Equals(chkCode))
                {
                    return NetJSON(new OutInfo() { code = 0, msg = "验证码错误，请重试！" });
                }
            }
            else
            {
                return NetJSON(new OutInfo() { code = 0, msg = "验证码错误，请重试!"});
            }

            string sql = string.Format(" where QQnumber = '{0}' limit 1", qqnum);
            var info = UserBll.SingleOrDefault(sql);
            if (info != null && info.uid > 0)
            {
                if (info.password.Equals(Encryption.MD5(password)))
                {
                    var org = organizationBll.SingleOrDefault(info.oid);
                    if (org.status == (int)Enums.StatusEnum.Deleted || info.status == (int)Enums.StatusEnum.Deleted)
                    {
                        return NetJSON(new OutInfo() { code = 0, msg = "账号已被封禁或您所在组织已被封禁！" });
                    }
                    LoginUser login = new LoginUser()
                    {
                        oid = info.oid,
                        oname = info.oname,
                        uid = info.uid,
                        uname = info.uname,
                        type=info.user_type
                    };

                    string str = JsonConvert.SerializeObject(login);

                    CookieHelper.Set(Keys.AdminAuth, XXTEA.Encode(str));
                    return NetJSON(new OutInfo() { code =1, msg = "登录成功" });
                }
                else
                {
                    return NetJSON(new OutInfo() { code = 0, msg = "错误：帐号与密码不匹配" });
                }
            }
            else
            {
                return NetJSON(new OutInfo() { code = 0, msg = "错误：帐号不存在" });
            }

        }


        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
		public ActionResult Logout()
		{
            CookieHelper.Delete(Keys.AdminAuth);
            return Redirect(Url.Action("Index", "Login"));
		}
	}
}