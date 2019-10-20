using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NsqSharp;
using org.Common;
using System.Threading;

namespace org.Bll.nsq
{
    /// <summary>
    /// NSQ MQ的Topic定义
    /// </summary>
    public class MQTopic
    {
        /// <summary>
        /// 帖子被创建时
        /// </summary>
        public const string TopicCreate = "TopicCreate";

        /// <summary>
        /// 帖子被下沉时
        /// </summary>
        public const string TopicSink = "TopicSink";

        /// <summary>
        /// 帖子内容被更新时
        /// 如更改期数、内容等
        /// </summary>
        public const string TopicUpdate = "TopicUpdate";

        /// <summary>
        /// 帖子被删除时
        /// </summary>
        public const string TopicDelete = "TopicDelete";

        /// <summary>
        /// 帖子中奖时
        /// </summary>
        public const string TopicWinning = "TopicWinning";

        /// <summary>
        /// 帖子被推荐时
        /// </summary>
        public const string TopicRecommend = "TopicRecommend";

        /// <summary>
        /// 修改配置cron
        /// </summary>
        public const string CronUpdate = "CronUpdate";

    }

    /// <summary>
    /// 消息操作
    /// </summary>
    public class MQ
    {
        /// <summary>
        /// 连接字串
        /// IP:Port
        /// </summary>
        static string Host {
            get {
                var server  = ConfigurationManager.AppSettings["NSQServer"]?.ToString().Trim() ?? "";
                if (string.IsNullOrEmpty(server))
                    server = "192.168.0.197:4150";
                return server;
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="msg"></param>
        //public static void Publish(string topic, object msg)
        //{
        //    if (string.IsNullOrEmpty(topic) || msg == null)
        //        return;
        //    Producer pro = null;
        //    try
        //    {
        //        pro = new Producer(Host);
               
        //        pro.Publish(topic, JsonConvert.SerializeObject(msg));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog("发布消息错误：", ex);
        //    }
        //    finally
        //    {
        //        if (pro != null)
        //            pro.Stop();
        //    }

        //}
        public static void Publish(string topic, object msg)
        {
            DoMQ DM = new DoMQ(topic,msg,Host);
            Thread t = new Thread(new ThreadStart(DM.Publish));
            t.Start();
        }

        public static void MultiPublish(string topic, IEnumerable<byte[]> msg)
        {
            DoMQ DM = new DoMQ(topic, null, Host,msg);
            Thread t = new Thread(new ThreadStart(DM.MultiPublish));
            t.Start();
        }

    }

    public class DoMQ
    {
        private string topic;
        private object msg;
        private string Host;
        private IEnumerable<byte[]> mutimsg;
        public DoMQ(string Topic, object Msg,string host, IEnumerable<byte[]> MutiMsg=null)
        {
            topic = Topic;
            msg = Msg;
            Host = host;
            mutimsg = MutiMsg;
        }

        public  void Publish()
        {
            if (string.IsNullOrEmpty(topic) || msg == null)
                return;
            Producer pro = null;
            try
            {
                pro = new Producer(Host);

                pro.Publish(topic, JsonConvert.SerializeObject(msg));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("发布消息错误：", ex);
            }
            finally
            {
                if (pro != null)
                    pro.Stop();
            }

        }
        public void MultiPublish() {
            if (string.IsNullOrEmpty(topic) || mutimsg == null)
                return;
            Producer pro = null;
            try
            {
                pro = new Producer(Host);
                pro.MultiPublish(topic, mutimsg);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("发布消息错误：", ex);
            }
            finally
            {
                if (pro != null)
                    pro.Stop();
            }
        }
    }
}
