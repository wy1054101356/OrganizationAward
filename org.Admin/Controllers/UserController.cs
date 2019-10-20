using org.Bll;
using org.Common;
using org.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace org.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index(int p = 1,string name="")
        {
            string sql = " where 1=1 and user_type !=1";
            if (!string.IsNullOrEmpty(name))
            {
                sql += " and uname like '%"+name.TrimEnd()+"%'";
            }
            string order = " order by uid asc";

            var plist = UserBll.Page(p, _PageSize, sql, order);
            var pager = PagerUtils.PageList(_PageSize, p, (int)plist.TotalItems);
            ViewBag.Pager = pager;
            ViewBag.Items = plist.Items;
            ViewBag.UserInfo = _UserInfo;
            ViewBag.LoginUser = _UserInfo;
            return View();
        }

        public ActionResult Edit(long id = 0)
        {
            if (_UserInfo.type == 3)
            {
                return Content("<script>alert('无权限访问！');location.href='/'</script>");
            }
            if (id != 0)
            {
                var model = UserBll.SingleOrDefault(id);
                return View(model);
            }
            ViewBag.LoginUser = _UserInfo;
            return View(new User());
        }

        public ActionResult EditUser(User model)
        {
            if (_UserInfo.type == 2)
            {
                return Content("<script>alert('无权限访问！');location.href='/'</script>");
            }
            bool flag = true;
            if (string.IsNullOrEmpty(model.uname))
            {
                flag = false;
                Error("请填写前台用户名");
            }
            if (string.IsNullOrEmpty(model.QQnumber.ToString()))
            {
                flag = false;
                Error("请填写QQ账号");
            }
            if (string.IsNullOrEmpty(model.password.ToString()))
            {
                flag = false;
                Error("请填写密码");
            }
            if (model.oid<=0)
            {
                flag = false;
                Error("请选择组织");
            }
            if (model.user_type <= 0)
            {
                flag = false;
                Error("请选择角色");
            }
            if (flag)
            {
                var info = UserBll.SingleOrDefault($"where QQnumber = '{model.QQnumber}' and oid={model.oid}  limit 1");
                if (info != null && info.uid !=model.uid)
                {
                    Error("此成员已注册：" + info.uname);

                }
                else
                {
                    if (model.uid == 0)
                    {
                        model.created = TypeConvert.DateTimeToInt(DateTime.Now);
                        model.status = (int)Enums.StatusEnum.Normal;
                        model.password = Utils.MD5(model.password);
                        var u = UserBll.AddBackId(model);
                        Success("添加成功!", Url.Action("Index"));
                    }
                    else
                    {
                        var u = UserBll.Update(model);
                        Success("修改成功!", Url.Action("Index"));
                    }

                }
            }
            return Content("");
        }

        public ActionResult Delete(long id)
        {
            if (_UserInfo.type == 3)
            {
                return Content("<script>alert('无权限访问！');location.href='/'</script>");
            }
            var s = UserBll.Delete(id);
            if (s)
            {
                return NetJSON(1);
            }
            return NetJSON(0);
        }

        public ActionResult GetUserList(string name)
        {
            List<User> list = new List<User>();
            if (!string.IsNullOrEmpty(name))
            {
                list = UserBll.Fetch($"where uname like '%{name}%'");
            }
            return NetJSON(list);

        }
    }
}