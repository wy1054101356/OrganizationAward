using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServiceStack.CacheAccess;
using ServiceStack.Redis;

namespace org.Common
{

    /// <summary>
    /// redis连接配置
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string host { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int port { get; set; }
        /// <summary>
        /// 库
        /// </summary>
        public int db { get; set; }
    }

    public class RedisHelper
    {
        private static PooledRedisClientManager prcm;

        #region Init
        /// <summary>
        /// 入口方法
        /// </summary>
        static RedisHelper()
        {
            CreateManager();
        }

        private static void CreateManager()
        {
            string str = ConfigurationManager.AppSettings["ReidsDataConfig"]?.ToString().Trim() ?? "";
            if (string.IsNullOrEmpty(str))
                throw new Exception("初始化redis失败:未找到配置节点");

            RedisConfig config = new RedisConfig();
            try
            {
                config = JsonConvert.DeserializeObject<RedisConfig>(str);
            }
            catch (Exception ex)
            {
                throw new Exception("初始化redis失败:配置节点json数据格式错误。" + ex.Message);
            }

            CreateManager(config);
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        public static void CreateManager(RedisConfig config)
        {
            string readWriteHosts = $"{config.password}@{config.host}:{config.port}";
            string readOnlyHosts = $"{config.password}@{config.host}:{config.port}";
            RedisClientManagerConfig redisConfig = new RedisClientManagerConfig
            {
                AutoStart = true,
                MaxReadPoolSize = 60,
                MaxWritePoolSize = 60,
                DefaultDb = config.db
            };
            prcm = new PooledRedisClientManager(new List<string>() { readWriteHosts }, new List<string>() { readOnlyHosts }, redisConfig);
        }

        #endregion

        #region Client
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static RedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();
            return (RedisClient)prcm.GetClient();
        }

        public static IRedisClient GetReadOnlyClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetReadOnlyClient();
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static ICacheClient GetCacheClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetCacheClient();
        }
        #endregion

        #region Key

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveKey(string key)
        {
            using (var redis = GetClient())
            {
                if (redis.ContainsKey(key))
                    return redis.Remove(key);
                return false;
            }
        }

        /// <summary>
        /// key是否正了
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            using (var redis = GetClient())
            {
                return redis.ContainsKey(key);
            }
        }

        #endregion


        /// <summary>
        /// 获取T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            using (var redis = GetClient())
            {
                if (redis.ContainsKey(key))
                    return redis.Get<T>(key);
                return default(T);
            }
        }

        #region string
        /// <summary>
        /// 获取string类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            using (var redis = GetClient())
            {
                return redis.GetValue(key);
            }
        }

        public static bool AddValue<T>(string key, T value)
        {
            using (var redis = GetClient())
            {
                return redis.Add(key, value);
            }
        }
        #endregion

        #region Hash

        /// <summary>
        /// 从Hash到Model的映射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetItemFromHash<T>(string key)
        {
            using (var redis = GetClient())
            {
                if (!redis.ContainsKey(key))
                {
                    return default(T);
                }

                var dic = redis.GetAllEntriesFromHash(key);
                if (dic != null && dic.Count > 0)
                {
                    var str = JsonConvert.SerializeObject(dic);
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(str);
                    }
                    catch
                    {

                    }
                }
                return default(T);
            }
        }

        /// <summary>
        /// 把一个single t 丢到Hash里
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void SetItemToHash<T>(string key, T t)
        {
            if (t == null)
                return;

            using (var redis = GetClient())
            {
                var str = JsonConvert.SerializeObject(t);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
                if (dict != null && dict.Count > 0)
                {
                    foreach (var item in dict)
                    {
                        if (string.IsNullOrWhiteSpace(item.Value))
                            redis.SetEntryInHash(key, item.Key.ToLower(), "");
                        else
                            redis.SetEntryInHash(key, item.Key.ToLower(), item.Value);
                    }
                }
            }
        }


        public static bool SetEntryInHash(string hashID, string key, string value)
        {
            using (var redis = GetClient())
            {
                return redis.SetEntryInHash(hashID, key, value);
            }
        }

        public static bool RemoveEntryFromHash(string hashID, string key)
        {
            using (var redis = GetClient())
            {
                return redis.RemoveEntryFromHash(hashID, key);
            }
        }

        /// <summary>
        /// 某个hash下所有的字段
        /// </summary>
        /// <param name="hashID"></param>
        /// <returns></returns>
        public static List<string> GetHashFields(string hashID)
        {
            using (var redis = GetClient())
            {
                return redis.GetHashKeys(hashID); ;
            }
        }

        /// <summary>
        /// 某个hash下所有的值
        /// </summary>
        /// <param name="hashID"></param>
        /// <returns></returns>
        public static List<string> GetHashValues(string hashID)
        {
            using (var redis = GetClient())
            {
                return redis.GetHashValues(hashID); ;
            }
        }

        /// <summary>
        /// 获得hash型key某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetHashField(string key, string field)
        {
            using (var redis = GetClient())
            {
                return redis.GetValueFromHash(key, field);
            }
        }

        /// <summary>
        /// 设置hash型key某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public static void SetHashField(string key, string field, string value)
        {
            using (var redis = GetClient())
            {
                redis.SetEntryInHash(key, field, value);
            }
        }
        #endregion

        #region Set
        /// <summary>
        /// 向集合中添加数据
        /// </summary>
        /// <param name="setID"></param>
        /// <param name="item"></param>
        public static void AddItemToSet(string setID, string item)
        {
            using (var redis = GetClient())
            {
                redis.AddItemToSet(setID, item);
            }
        }

        public static void RemoveItemFromSet(string setID, string item)
        {
            using (var redis = GetClient())
            {
                redis.RemoveItemFromSet(setID, item);
            }
        }

        /// <summary>
        /// 获得集合中所有数据
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static HashSet<string> GetAllItemsFromSet(string setID)
        {
            using (var redis = GetClient())
            {
                return redis.GetAllItemsFromSet(setID);
            }
        }

        #endregion

        #region SortedSet

        /// <summary>
        /// 向有序集合中添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public static bool AddItemToSortedSet(string key, string value, long score)
        {
            using (var redis = GetClient())
            {
                return redis.AddItemToSortedSet(key, value, score);
            }
        }

        public static bool AddItemToSortedSet(string key, string value, double score)
        {
            using (var redis = GetClient())
            {
                return redis.AddItemToSortedSet(key, value, score);
            }
        }

        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的降序排列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long GetItemIndexInSortedSetDesc(string key, string value)
        {
            using (var redis = GetClient())
            {
                return redis.GetItemIndexInSortedSetDesc(key, value);
            }
        }


        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的升序排列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long GetItemIndexInSortedSet(string key, string value)
        {
            using (var redis = GetClient())
            {
                return redis.GetItemIndexInSortedSet(key, value);
            }
        }
        /// <summary>
        /// 获得有序集合中某个值得分数
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetItemScoreInSortedSet(string key, string value)
        {
            using (var redis = GetClient())
            {
                return redis.GetItemScoreInSortedSet(key, value);
            }
        }

        /// <summary>
        /// 获得有序集合中，某个排名范围的所有值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginRank"></param>
        /// <param name="endRank"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSet(string key, int beginRank, int endRank)
        {
            using (var redis = GetClient())
            {
                return redis.GetRangeFromSortedSet(key, beginRank, endRank);
            }
        }

        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，升序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSet(string key, double beginScore, double endScore)
        {
            using (var redis = GetClient())
            {
                return redis.GetRangeFromSortedSetByHighestScore(key, beginScore, endScore);
            }
        }

        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，降序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSetDesc(string key, double beginScore, double endScore)
        {
            using (var redis = GetClient())
            {
                return redis.GetRangeFromSortedSetByLowestScore(key, beginScore, endScore);
            }
        }

        /// <summary>
        /// 从ZSET中移出项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool RemoveItemFromScoredSet(string key, string value)
        {
            using (var redis = GetClient())
            {
                return redis.RemoveItemFromSortedSet(key, value);
            }
        }

        public static bool HasItemFromScoredSet(string key, string value)
        {
            using (var redis = GetClient())
            {
                return redis.GetItemIndexInSortedSet(key, value) > 0 ? true : false; ;
            }
        }

        #endregion

        #region List

        /// <summary>
        /// 向List底部添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public static void AddItemToListRight(string key, string value)
        {
            using (var redis = GetClient())
            {
                redis.AddItemToList(key, value);
            }
        }

        /// <summary>
        /// 向顶部添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddItemToListLeft(string key, string value)
        {
            using (var redis = GetClient())
            {
                redis.LPush(key, Encoding.Default.GetBytes(value));
            }
        }

        /// <summary>
        /// 获取key包含的所有数据集合
        /// </summary>  
        public static List<string> GetAllItemList(string key)
        {
            using (var redis = GetClient())
            {
                return redis.GetAllItemsFromList(key);
            }
        }

        /// <summary>
        /// 移除list中，key/value,与参数相同的值，并返回移除的数量
        /// </summary>  
        public static long RemoveItemFromList(string key, string value)
        {
            using (var redis = GetClient())
            {
                return redis.RemoveItemFromList(key, value);
            }
        }

        #endregion

        #region 有序集合Zset 
        /// <summary>
        /// 获取key的所有集合
        /// </summary>
        public List<string> GetAllItemsFromSortedSet(string key)
        {
            using (var redis = GetClient())
            {
                return redis.GetAllItemsFromSortedSet(key);
            }
        }
        /// <summary>
        /// 获取key的所有集合，倒叙输出
        /// </summary>
        public static List<string> GetAllItemsFromSortedSetDesc(string key)
        {
            using (var redis = GetClient())
            {
                return redis.GetAllItemsFromSortedSetDesc(key);
            }
        }

        /// <summary>
        /// 获取所有集合，带分数
        /// </summary>
        public static IDictionary<string, double> GetAllWithScoresFromSortedSet(string key)
        {
            using (var redis = GetClient())
            {
                return redis.GetAllWithScoresFromSortedSet(key);
            }
        }

        /// <summary>
        /// 删除key为value的数据
        /// </summary>
        public static bool RemoveItemFromSortedSet(string key, string value)
        {
            using (var redis = GetClient())
            {
                return redis.RemoveItemFromSortedSet(key, value);
            }
        }
        #endregion

        #region BITMAP
        public static long GetBitValue(string key, int offset)
        {
            using (var redis = GetClient())
            {
               return redis.GetBit(key,offset);
            }
        }
        #endregion
    }


}
