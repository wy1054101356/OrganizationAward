using org.Common;
using org.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace org.Bll
{
    /// <summary>
    /// dropdown list binder
    /// </summary>
    public class DDList
    {




        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetListItem(object obj, string first = "==请选择==")
        {
            List<SelectListItem> rlist = new List<SelectListItem>();
            Type t = obj.GetType();
            StringBuilder sb = new StringBuilder();
            var list = EnumUtils.ToList(t);
            if (list != null && list.Count > 0)
            {
                rlist.Add(new SelectListItem() { Text = first, Value = "-100" });
                foreach (var item in list)
                {
                    rlist.Add(new SelectListItem() { Text = item.Description, Value = item.ID.ToString() });
                }
            }

            return rlist;
        }

        /// <summary>
        /// 绑定用户组信息
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetORGListItem(string first = "==请选择==")
        {
            List<organization> list = organizationBll.Fetch("where status = 1");
            List<SelectListItem> rlist = new List<SelectListItem>();
            if (list != null && list.Count > 0)
            {
                rlist.Add(new SelectListItem() { Text = first, Value = "-100" });
                foreach (var item in list)
                {
                    rlist.Add(new SelectListItem() { Text = item.oname, Value = item.oid.ToString() });
                }
            }

            return rlist;
        }



        /// <summary>
        /// 返回枚举的列表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="first"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetEnumListItem(object obj, string first = "==请选择==")
        {
            Type t = obj.GetType();
            StringBuilder sb = new StringBuilder();
            var list = EnumUtils.ToList(t);
            List<SelectListItem> rlist = new List<SelectListItem>();
            if (list != null && list.Count > 0)
            {
                rlist.Add(new SelectListItem() { Text = first, Value = "-100" });
                foreach (var item in list)
                {
                    rlist.Add(new SelectListItem() { Text = item.Description, Value = item.ID.ToString() });
                }
            }

            return rlist;
        }

        /// <summary>
        /// 绑定Enum到DropList
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectID">选中的ID</param>
        /// <returns></returns>
        public static string BindDropListByEnum(object obj, string id, int selectID = 0, string first = "==请选择==")
        {
            Type t = obj.GetType();
            StringBuilder sb = new StringBuilder();
            var list = EnumUtils.ToList(t);
            if (list != null && list.Count > 0)
            {
                sb.Append("<select id=\"" + id + "\" name=\"" + id + "\">");
                sb.Append("<option value=\"-100\">" + first + "</option>");
                list = list.OrderBy(n => n.ID).ToList();
                foreach (var item in list)
                {
                    if (item.ID == selectID)
                        sb.Append("<option selected=\"selected\" value=\"" + item.ID + "\">" + item.Description + "</option>");
                    else
                        sb.Append("<option value=\"" + item.ID + "\">" + item.Description + "</option>");
                }
                sb.Append("</select>");
            }
            return sb.ToString();
        }

    }
}
