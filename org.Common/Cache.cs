using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace org.Common
{
    /// <summary>
    /// 缓存操作类
    /// </summary>
	public class Cache
	{
		static string pc = ConfigurationManager.AppSettings["PC"]?.ToString().Trim() ?? "";
		static string wap = ConfigurationManager.AppSettings["WAP"]?.ToString().Trim() ?? "";

		static string _forum = "ForumKey";//（版面key） 
		static string _index = "IndexKey";//（推荐列表key）
		static string _LastHitKey = "LastHitKey_";//LastHitKey_{gameCode}  （最新中奖实票列表）


		static string pcurl = $"{pc}home/WebCacheClear";
		static string wapurl = $"{wap}home/WebCacheClear";

		/// <summary>
		/// 清理版面缓存
		/// </summary>
		public static void Forum()
		{
			var body = JsonConvert.SerializeObject(new { key = _forum });
			PostH(_forum);
		}
		/// <summary>
		/// 清理推荐列表缓存
		/// </summary>
		public static void Index()
		{
			var body = JsonConvert.SerializeObject(new { key = _index});
			PostH(_index);
		}
		/// <summary>
		/// 清理最新中奖实票列表缓存
		/// </summary>
		/// <param name="gameCode"></param>
		public static void LastHit(int gameCode)
		{
			var body = JsonConvert.SerializeObject(new { key = _LastHitKey });
			PostH(_LastHitKey);
		}
		static void PostH(string key)
		{
			Dictionary<string, string> dic = new Dictionary<string, string>();
			dic.Add("key", key);
			var p = Post(pcurl, dic);
			var m = Post(wapurl, dic);
			//LogHelper.WriteLog($"清理{key}成功,返回 pc:{p} wap:{m}", null);
		}
		public static string Post(string url, Dictionary<string, string> dic)
		{
			string result = "";
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			#region 添加Post 参数  
			StringBuilder builder = new StringBuilder();
			int i = 0;
			foreach (var item in dic)
			{
				if (i > 0)
					builder.Append("&");
				builder.AppendFormat("{0}={1}", item.Key, item.Value);
				i++;
			}
			byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
			req.ContentLength = data.Length;
			using (Stream reqStream = req.GetRequestStream())
			{
				reqStream.Write(data, 0, data.Length);
				reqStream.Close();
			}
			#endregion
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			Stream stream = resp.GetResponseStream();
			//获取响应内容  
			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
			{
				result = reader.ReadToEnd();
			}
			return result;
		}
	}
}