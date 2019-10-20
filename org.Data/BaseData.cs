using PetaPoco;
using MySql.Data.MySqlClient;
using NIU.Forum.Common;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace org.Data
{
	public class BaseData<T>
	{
		/// <summary>
		/// 最大页
		/// </summary>
		public static int _MaxPage = TypeConvert.ObjectToInt(ConfigurationManager.AppSettings["MaxPage"]?.ToString(), 1000);
		/// <summary>
		/// mysql连接字串
		/// </summary>
		public static string mysql
		{
			get
			{
				if (ConfigurationManager.ConnectionStrings["mysql"] != null)
				{
					return ConfigurationManager.ConnectionStrings["mysql"].ToString().Trim();
				}
				else
				{
					return "";
				}
			}
		}

        #region 方法


        /// <summary>
        /// 是否存在
        /// </summary>
        public static bool Exists(long id)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                var info = db.SingleOrDefault<T>(id);
                if (info != null)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        public static bool Exists(string sql, params object[] args)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                var info = db.SingleOrDefault<T>(sql, args);
                if (info != null)
                    return true;
                return false;

            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        public static bool Add(T t)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                long flag = TypeConvert.ObjectToLong(db.Insert(t));
                return flag > 0 ? true : false;
            }
        }



        /// <summary>
		/// 添加-返回id
		/// </summary>
		public static long AddBackId(T t)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                long flag = TypeConvert.ObjectToLong(db.Insert(t));
                return flag;
            }
        }

        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static bool Update(T t, IEnumerable<string> fields)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                int flag = TypeConvert.ObjectToInt(db.Update(t, fields));
                return flag > 0 ? true : false;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql"></param>
        public static void Execute(string sql)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                db.Execute(sql);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public static bool Update(T t)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                int flag = TypeConvert.ObjectToInt(db.Update(t));
                return flag > 0 ? true : false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static bool Del(long id)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                int flag = TypeConvert.ObjectToInt(db.Delete<T>(id));
                return flag > 0 ? true : false;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public static bool Del(string sql, params object[] args)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                int flag = TypeConvert.ObjectToInt(db.Delete<T>(sql, args));
                return flag > 0 ? true : false;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public static bool Del(Sql sql)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                int flag = TypeConvert.ObjectToInt(db.Delete<T>(sql));
                return flag > 0 ? true : false;
            }
        }
        /// <summary>
        /// id查数据
        /// </summary>
        public static T SingleOrDefault(long id)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                return db.SingleOrDefault<T>(id);
            }
        }
        /// <summary>
        /// sql查数据 不带表名称
        /// </summary>
        public static T SingleOrDefault(string sql, params object[] args)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                return db.SingleOrDefault<T>(sql, args);
            }
        }
        /// <summary>
        /// sql查数据
        /// </summary>
        public static T SingleOrDefault(Sql sql)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                return db.SingleOrDefault<T>(sql);
            }
        }
        /// <summary>
        /// sql查数据获取列表
        /// </summary>
        public static List<T> Fetch(Sql sql)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                return db.Fetch<T>(sql);
            }
        }

        /// <summary>
        /// sql查数据获取列表 不带表名称
        /// </summary>
        public static List<T> Fetch(string sql, params object[] args)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                return db.Fetch<T>(sql, args);
            }
        }
        /// <summary>
        /// 通用分页 存在count性能问题 数据量大的表 请勿使用
        /// </summary>
        public static Page<T> Page(int p, int pagesize, string sql, params object[] args)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                return db.Page<T>(p, pagesize, sql, args);
            }
        }
        /// <summary>
        /// 通用分页 存在count性能问题 数据量大的表 请勿使用
        /// </summary>
        public static Page<T> Page(int p, int pagesize, Sql sql)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                return db.Page<T>(p, pagesize, sql);
            }
        }
        /// <summary>
        /// 通用分页
        /// </summary>
        public static Pager<T> Page(int p, int pagesize, string where, string order)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                if (string.IsNullOrWhiteSpace(where))
                    where = "where 1=1";
                string sql = string.Format("{0} {1}", where, order);
                //HttpContext.Current.Response.Write(sql);
                //HttpContext.Current.Response.End();
                var list = db.Page<T>(p, pagesize, sql);
                return new Pager<T>()
                {
                    Items = list.Items,
                    CurrentPage = p,
                    PageSize = pagesize,
                    TotalItems = list.TotalItems,
                    TotalPages = _MaxPage
                };
            }

        }
        /// <summary>
        /// 通用分页,无排序
        /// </summary>
        public static Pager<T> PageNoOrder(int p, int pagesize, string where)
        {
            using (Database db = new Database(mysql, MySqlClientFactory.Instance))
            {
                if (string.IsNullOrWhiteSpace(where))
                    where = "where 1=1";
                string sql = string.Format("{0}", where);
                //HttpContext.Current.Response.Write(sql);
                //HttpContext.Current.Response.End();
                var list = db.Page<T>(p, pagesize, sql);
                return new Pager<T>()
                {
                    Items = list.Items,
                    CurrentPage = p,
                    PageSize = pagesize,
                    TotalItems = list.TotalItems,
                    TotalPages = _MaxPage
                };
            }

        }

        #endregion
    }
}
