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
    public class RecordController : BaseController
    {
        // GET: Record
        public ActionResult Index(DateTime? date = null, int p = 1, string uname = "")
        {
            if (date == null)
                date = DateTime.Now;
            int week = (int)date.Value.DayOfWeek;
            if (week == 0)
                week = 7;
            DateTime timebegin = Convert.ToDateTime(date.Value.AddDays(-(week - 1)).ToString("yyyy-MM-dd 00:00"));
            DateTime timeend = Convert.ToDateTime(date.Value.AddDays(8 - week).ToString("yyyy-MM-dd 00:00"));
            long timebeginnumber = TypeConvert.DateTimeToLong(timebegin);
            long timeendnumber = TypeConvert.DateTimeToLong(timeend);
            string sql = " where 1=1 ";
            if (!string.IsNullOrEmpty(uname))
            {
                sql += " and uname like '%" + uname.TrimEnd() + "%'";
            }
            sql += string.Format(" and created between {0} and {1}", timebeginnumber, timeendnumber);
            sql += string.Format(" and oid = {0}", _UserInfo.oid);
            string order = " order by sumpoints asc";

            var plist = RecordBll.Page(p, _PageSize, sql, order);
            var pager = PagerUtils.PageList(_PageSize, p, (int)plist.TotalItems);
            ViewBag.Pager = pager;
            ViewBag.Items = plist.Items;
            ViewBag.LoginUser = _UserInfo;
            ViewBag.CurrentTime = date.Value.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult Edit()
        {
            ViewBag.LoginUser = _UserInfo;
            DateTime date = DateTime.Now;
            int week = (int)date.DayOfWeek;
            if (week == 0)
                week = 7;
            DateTime timebegin = Convert.ToDateTime(date.AddDays(-(week - 1)).ToString("yyyy-MM-dd 00:00"));
            DateTime timeend = Convert.ToDateTime(date.AddDays(8 - week).ToString("yyyy-MM-dd 00:00"));
            long timebeginnumber = TypeConvert.DateTimeToLong(timebegin);
            long timeendnumber = TypeConvert.DateTimeToLong(timeend);
            string sql = string.Format(" where created between {0} and {1}", timebeginnumber, timeendnumber);
            sql += string.Format(" and uid = {0}", _UserInfo.uid);
            var model = RecordBll.SingleOrDefault(sql);
            if (model == null)
            {
                return View(new Record());
            }
            if (model.bagtype != (int)Enums.BagType.Normal)
            {
                return Content("<script>alert('该周礼包已分配，不能再修改！')</script>");
            }
            return View(model);

        }
        public ActionResult EditRecord(Record model)
        {
 
            long powernum = 0;
            if (model.powernum < 100000)
            {
                powernum = 1000;
            }
            else {
                powernum =Convert.ToInt64(1000 + Math.Floor((Convert.ToDecimal(model.powernum - 100000) / 20000)) * 200);
            }
            long tongling = model.tongling;
            long fighttimepoints = model.fighttime * 500;
            long lastweekdeduction = 0;
            {
                DateTime date = DateTime.Now.AddDays(-7);
                int week = (int)date.DayOfWeek;
                if (week == 0)
                    week = 7;
                DateTime timebegin = Convert.ToDateTime(date.AddDays(-(week - 1)).ToString("yyyy-MM-dd 00:00"));
                DateTime timeend = Convert.ToDateTime(date.AddDays(8 - week).ToString("yyyy-MM-dd 00:00"));
                long timebeginnumber = TypeConvert.DateTimeToLong(timebegin);
                long timeendnumber = TypeConvert.DateTimeToLong(timeend);
                string sql = string.Format(" where created between {0} and {1}", timebeginnumber, timeendnumber);
                sql += string.Format(" and uid = {0}", model.uid);
                var lastweekmodel = RecordBll.SingleOrDefault(sql);
                if (lastweekmodel != null)
                {
                    switch (lastweekmodel.bagtype)
                    {
                        case (int)Enums.BagType.CS: lastweekdeduction = -800; break;
                        case (int)Enums.BagType.YX: lastweekdeduction = -500; break;
                        case (int)Enums.BagType.JY: lastweekdeduction = -300; break;
                    }
                }
            }
            List<Reward> rewardlst = RewardBll.Fetch("where uid=" + _UserInfo.uid);
            long otherbonuspoints = 0;
            {
                otherbonuspoints = rewardlst.Where(x => x.type == (int)Enums.RewardType.reward).Sum(x => x.points);
            }
            long otherdeduction = 0;
            {
                otherdeduction = rewardlst.Where(x => x.type == (int)Enums.RewardType.punishment).Sum(x => x.points);

            }
            model.otherbonuspoints = otherbonuspoints;
            model.otherdeduction = otherdeduction;
            model.sumpoints = powernum + tongling + fighttimepoints + lastweekdeduction + otherbonuspoints + otherdeduction;
            model.bagtype = (int)Enums.BagType.Normal;
            if (model.rid == 0)
            {
                model.uid = _UserInfo.uid;
                model.uname = _UserInfo.uname;
                model.oid = _UserInfo.oid;
                model.oname = _UserInfo.oname;
                model.created = TypeConvert.DateTimeToLong(DateTime.Now);
                RecordBll.Add(model);

            }
            else {
                RecordBll.Update(model);
            }
            Success("保存成功!", Url.Action("Index"));
            return Content("");
        }

        public ActionResult Distribution(int cscount=0, int yxcount=0, int jycount=0)
        {
            DateTime date = DateTime.Now;
            int week = (int)date.DayOfWeek;
            if (week == 0)
                week = 7;
            DateTime timebegin = Convert.ToDateTime(date.AddDays(-(week - 1)).ToString("yyyy-MM-dd 00:00"));
            DateTime timeend = Convert.ToDateTime(date.AddDays(8 - week).ToString("yyyy-MM-dd 00:00"));
            long timebeginnumber = TypeConvert.DateTimeToLong(timebegin);
            long timeendnumber = TypeConvert.DateTimeToLong(timeend);
            string sql = string.Format(" where created between {0} and {1}", timebeginnumber, timeendnumber);
            sql += string.Format(" and oid = {0} order by sumpoints asc", _UserInfo.oid);
            List<Record> lst = RecordBll.Fetch(sql);
            if (lst.FirstOrDefault(x => x.bagtype > 0)!=null)
            {
                return Content("<script>alert('该周礼包已分配，不能再分配！')</script>");

            }
            foreach (var item in lst)
            {
                if (cscount > 0)
                {
                    item.bagtype = (int)Enums.BagType.CS;
                    RecordBll.Update(item);
                    cscount--;
                }
                else if (yxcount > 0)
                {
                    item.bagtype = (int)Enums.BagType.YX;
                    RecordBll.Update(item);
                    yxcount--;
                }
                else if (jycount > 0)
                {
                    item.bagtype = (int)Enums.BagType.JY;
                    RecordBll.Update(item);
                    jycount--;
                }
            }
            return NetJSON(new OutInfo() { code = 1, msg = "处理成功！" });
        }
    }
}