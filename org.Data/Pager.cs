using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Data
{
    /// <summary>
    /// 自定义的翻页数据模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pager<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public long CurrentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalItems { get; set; }

        /// <summary>
        /// 每页数
        /// </summary>
        public long PageSize { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<T> Items { get; set; }
    }
}
