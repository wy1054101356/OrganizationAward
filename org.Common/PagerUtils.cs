using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace org.Common
{
    /// <summary>
    /// 翻页显示类
    /// </summary>
    public class PagerUtils
    {

        /// <summary>
        /// 后台的翻页列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string PageList(int pageSize, int page, int count)
        {
            StringBuilder sb = new StringBuilder();
            if (count == 0)
                return "";

            int pageNum = 5;

            int pageCount = 0;

            if (count % pageSize == 0)
                pageCount = count / pageSize;
            else
                pageCount = count / pageSize + 1;

            if (page > pageCount)
                page = pageCount;

            sb.Append("<div class=\"ui right floated pagination menu\">");

            sb.Append("<label class=\"icon item\">共 " + count + " 条/ " + pageCount + " 页</label>");


            //第一页
            sb.Append("<a class=\"icon item\" href=\"" + GetLink(1) + "\"><i class=\"angle double left icon\"></i></a>");

            //上一页
            if (page > 1)
                sb.Append("<a class=\"icon item\" href=\"" + GetLink(page - 1) + "\"><i class=\"left chevron icon\"></i></a>");
            else
                sb.Append("<a class=\"icon item\" href=\"" + GetLink(1) + "\"><i class=\"left chevron icon\"></i></a>");

            if (page <= pageNum)
            {
                for (int i = 1; i <= pageCount && i <= pageNum; i++)
                {
                    if (i != page)
                        sb.Append("<a class=\"item\" href=\"" + GetLink(i) + "\">" + i + "</a>");
                    else
                        sb.Append("<a class=\"item\" style=\"background:#eee\">" + i + "</a>");
                }
            }
            else
            {
                for (int i = page - 2; i <= page + 2 && i <= pageCount; i++)
                {
                    if (i != page)
                        sb.Append("<a class=\"item\" href=\"" + GetLink(i) + "\">" + i + "</a>");
                    else
                        sb.Append("<a class=\"item\" style=\"background:#eee\">" + i + "</a>");
                }
            }


            if (page < pageCount - 2 && pageCount > 5)
            {
                sb.Append("<a class=\"icon item\" href=\"" + GetLink((page + 5) > pageCount ? pageCount : (page + 5)) + "\">...</a>");
            }

            //下一页
            if (page < pageCount)
                sb.Append("<a class=\"icon item\" href=\"" + GetLink(page + 1) + "\"><i class=\"right chevron icon\"></i></a>");
            else
                sb.Append("<a class=\"icon item\" href=\"" + GetLink(pageCount) + "\"><i class=\"right chevron icon\"></i></a>");

            //最后一页
            sb.Append("<a class=\"icon item\" href=\"" + GetLink(pageCount) + "\"><i class=\"angle double right icon\"></i></a>");


            sb.Append("</div>");

            return sb.ToString();
        }

        private static string GetLink(int page)
        {
            string link = string.Empty;
            string url = string.Empty;

            url = HttpContext.Current.Request.RawUrl;
            if (url.IndexOf('?') != -1)
            {
                if (page == 1)
                {
                    link = Regex.Replace(url, @"[\&,\?]p=\d+", "");
                }
                else
                {
                    if (url.IndexOf("p=") != -1)
                        link = Regex.Replace(url, @"p=\d+", "p=" + page);
                    else
                        link = url + "&p=" + page;
                }
            }
            else
            {
                if (page == 1)
                    link = url;
                else
                    link = $"{url}?p={page}";
            }
            link = "" + link;
            return link;
        }
    }
}
