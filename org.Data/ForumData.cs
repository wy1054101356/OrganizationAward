using PetaPoco;
using MySql.Data.MySqlClient;
using NIU.Forum.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using PetaPoco.Core;

namespace org.Data
{
	public class ForumData<T>
	{
		/// <summary>
		/// 最大页
		/// </summary>
		public static int _MaxPage = TypeConvert.ObjectToInt(ConfigurationManager.AppSettings["MaxPage"]?.ToString(), 1000);
		/// <summary>
		/// forum连接字串
		/// </summary>
		public static string forum
		{
			get
			{
				if (ConfigurationManager.ConnectionStrings["forum"] != null)
				{
					return ConfigurationManager.ConnectionStrings["forum"].ToString().Trim();
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
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
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
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
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
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
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
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                long flag = TypeConvert.ObjectToLong(db.Insert(t));
                return flag;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public static bool Update(T t)
        {
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
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
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
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
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
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
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
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
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                return db.SingleOrDefault<T>(id);
            }
        }
        /// <summary>
        /// sql查数据 不带表名称
        /// </summary>
        public static T SingleOrDefault(string sql, params object[] args)
        {
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                return db.SingleOrDefault<T>(sql, args);
            }
        }
        /// <summary>
        /// sql查数据
        /// </summary>
        public static T SingleOrDefault(Sql sql)
        {
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                return db.SingleOrDefault<T>(sql);
            }
        }
        /// <summary>
        /// sql查数据获取列表
        /// </summary>
        public static List<T> Fetch(Sql sql)
        {
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                return db.Fetch<T>(sql);
            }
        }
        /// <summary>
        /// sql查数据获取列表 不带表名称
        /// </summary>
        public static List<T> Fetch(string sql, params object[] args)
        {
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                return db.Fetch<T>(sql, args);
            }
        }
        /// <summary>
        /// 通用分页 存在count性能问题 数据量大的表 请勿使用
        /// </summary>
        public static Page<T> Page(int p, int pagesize, string sql, params object[] args)
        {
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                return db.Page<T>(p, pagesize, sql, args);
            }
        }
        /// <summary>
        /// 通用分页 存在count性能问题 数据量大的表 请勿使用
        /// </summary>
        public static Page<T> Page(int p, int pagesize, Sql sql)
        {
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                return db.Page<T>(p, pagesize, sql);
            }
        }
        /// <summary>
        /// 通用分页
        /// </summary>
        public static Page<T> Page(int p, int pagesize, string where, string order)
        {
            using (Database db = new Database(forum, MySqlClientFactory.Instance))
            {
                string sql = string.Format("{0} {1}", where, order);
                //HttpContext.Current.Response.Write(sql);
                //HttpContext.Current.Response.End();
                var list = db.Page<T>(p, pagesize, sql);
                return new Page<T>()
                {
                    Items = list.Items,
                    CurrentPage = p,
                    TotalItems = list.TotalItems,
                    TotalPages = _MaxPage
                };
            }
            //Type t = typeof(T);
            //int startoff = p > _MaxPage ? _MaxPage : (p - 1) * pagesize;
            //if (string.IsNullOrEmpty(where))
            //    where = "where 1=1";
            //if (!string.IsNullOrEmpty(order))
            //    where += (" "+order);
            //where += $" LIMIT {startoff},{pagesize}";
            //var pd = PocoData.ForType(t.GetType(), new ConventionMapper());
            //string sql = $"SELECT * FROM {pd.TableInfo.TableName} AS t1 JOIN(SELECT {pd.TableInfo.PrimaryKey} FROM {pd.TableInfo.TableName} {where}) AS t2 WHERE t1.{pd.TableInfo.PrimaryKey} = t2.{pd.TableInfo.PrimaryKey} LIMIT {pagesize}";
            //var list = new List<T>();
            //using (Database db = new Database(forum, MySqlClientFactory.Instance))
            //{
            //    list = db.Fetch<T>(sql);
            //}
            //return new Page<T>()
            //{
            //    Items = list,
            //    CurrentPage = p,
            //    ItemsPerPage = pagesize,
            //    TotalPages = _MaxPage
            //};
        }

        #endregion
    }
}
