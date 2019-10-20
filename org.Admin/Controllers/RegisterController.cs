using org.Model;
using org.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using org.Common;

namespace org.Admin.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            ViewBag.LoginUser = new LoginUser();
            return View(new User());
        }

        public ActionResult EditUser(User model)
        {
            bool flag = true;
            if (string.IsNullOrEmpty(model.uname))
            {
                flag = false;
                return Content("<script>alert('请填写前台用户名！');location.href='/Register'</script>");
            }
            if (string.IsNullOrEmpty(model.QQnumber.ToString()))
            {
                flag = false;
                return Content("<script>alert('请填写QQ账号！');location.href='/Register'</script>");

            }
            if (string.IsNullOrEmpty(model.password.ToString()))
            {
                flag = false;
                return Content("<script>alert('请填写密码！');location.href='/Register'</script>");

            }
            if (model.oid <= 0)
            {
                flag = false;
                return Content("<script>alert('请选择组织！');location.href='/Register'</script>");

            }
            if (model.user_type <= 0)
            {
                flag = false;
                return Content("<script>alert('请选择角色！');location.href='/Register'</script>");

            }
            if (flag)
            {
                var info = UserBll.SingleOrDefault($"where QQnumber = '{model.QQnumber}' and oid={model.oid}  limit 1");
                if (info != null && info.uid != model.uid)
                {
                    return Content("<script>alert('此成员已注册！');location.href='/Register'</script>");

                }
                else
                {
                    if (model.uid == 0)
                    {
                        model.created = TypeConvert.DateTimeToInt(DateTime.Now);
                        model.status = (int)Enums.StatusEnum.Normal;
                        model.password = Utils.MD5(model.password);
                        var u = UserBll.AddBackId(model);
                        return Content("<script>alert('添加成功！');location.href='/'</script>");

                    }
                    else
                    {
                        var u = UserBll.Update(model);
                        return Content("<script>alert('添加成功！');location.href='/'</script>");
                    }

                }
            }
            return Content("");
        }

    }
}