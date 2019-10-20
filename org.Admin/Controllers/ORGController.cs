using org.Admin.Controllers;
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
    public class ORGController : BaseController
    {
        // GET: ORG
        public ActionResult Index(int p = 1)
        {
            if (_UserInfo.type != 1)
            {
                return Content("<script>alert('无权限访问！');location.href='/'</script>");
            }
            string sql = " where 1=1";
            string order = " order by oid asc";

            var plist = organizationBll.Page(p, _PageSize, sql, order);
            var pager = PagerUtils.PageList(_PageSize, p, (int)plist.TotalItems);
            ViewBag.Pager = pager;
            ViewBag.Items = plist.Items;
            ViewBag.LoginUser = _UserInfo;
            return View();
        }

        public ActionResult Edit(long id = 0)
        {
            if (id != 0)
            {
                var model = organizationBll.SingleOrDefault(id);
                return View(model);
            }
            ViewBag.LoginUser = _UserInfo;
            return View(new organization());
        }

        public ActionResult EditORG(organization model)
        {

            bool flag = true;
            if (string.IsNullOrEmpty(model.oname))
            {
                flag = false;
                Error("请填写组织名");
            }
            if (flag)
            {
                var info = organizationBll.SingleOrDefault($"where oname = '{model.oname}' limit 1");
                if (info != null && info.oid !=model.oid)
                {
                    Error("此组织名已注册：" + info.oname);

                }
                else
                {
                    if (model.oid == 0)
                    {
                        model.created = TypeConvert.DateTimeToInt(DateTime.Now);
                        model.status = (int)Enums.StatusEnum.Normal;
                        var u = organizationBll.AddBackId(model);
                        Success("添加成功!", Url.Action("Index"));
                    }
                    else {
                        var u = organizationBll.Update(model);
                        Success("修改成功!", Url.Action("Index"));
                    }

                }
            }
            return Content("");
        }

        public ActionResult Delete(long id)
        {
            if (_UserInfo.type != 1)
            {
                return Content("<script>alert('无权限访问！');location.href='/'</script>");
            }
            if (id == 1)
            {
                return Content("<script>ModelExt.alert('您删您妈拉个靶子呢删？！');location.href='/'</script>");
            }
            var s = organizationBll.Delete(id);
            if (s)
            {
                return NetJSON(1);
            }
            return NetJSON(0);
        }
    }
}