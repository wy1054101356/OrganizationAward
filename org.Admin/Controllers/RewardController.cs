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
    public class RewardController : BaseController
    {
        // GET: Reward
        public ActionResult Index(int p = 1, string name = "")
        {
            string sql = " where 1=1";
            if (!string.IsNullOrEmpty(name))
            {
                sql += " and uname like '%" + name.TrimEnd() + "%'";
            }
            string order = " order by created desc";

            var plist = RewardBll.Page(p, _PageSize, sql, order);
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
                var model = RewardBll.SingleOrDefault(id);
                return View(model);
            }
            ViewBag.LoginUser = _UserInfo;
            return View(new Reward());
        }

        public ActionResult EditReward(Reward model)
        {
            if (_UserInfo.type == 3)
            {
                return Content("<script>alert('无权限访问！');location.href='/'</script>");
            }
            bool flag = true;
            if (model.uid <= 0)
            {
                flag = false;
                Error("请选择用户");
            }
            if (model.type <= 0)
            {
                flag = false;
                Error("请选择奖惩类型");
            }
            if (flag)
            {

                if (model.id == 0)
                {
                    model.created = TypeConvert.DateTimeToInt(DateTime.Now);
                    var u = RewardBll.AddBackId(model);
                    Success("添加成功!", Url.Action("Index"));
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
            var s = RewardBll.Del(id);
            if (s)
            {
                return NetJSON(1);
            }
            return NetJSON(0);
        }
    }
}