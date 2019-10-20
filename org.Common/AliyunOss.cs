using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace org.Common
{
    /// <summary>
    /// aliyunOSS操作类
    /// </summary>
	public class AliyunOss
	{
		const string accessKeyId = "LTAItY72L8NrvG2g";
		const string accessKeySecret = "7io5beY1pnJ5Towtsz8UTIe9MCMjUw";
		const string endpoint = "oss-cn-shenzhen.aliyuncs.com";
		const string bucketName = "niucai-img003";
		static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);

		/// <summary>
		/// 获取文件地址头
		/// </summary>
		/// <returns></returns>
		public static string GetHead()
		{
			var url = ConfigurationManager.AppSettings["ImgServer"]?.ToString().Trim() ?? $"http://{bucketName}.{endpoint}/";
			return url;
		}

		/// 上传文件
		/// </summary>
		/// <param name="fs">文件流</param>
		/// <param name="key">全名</param>
		/// <returns>0 上传成功</returns>
		public static int PutObject(string key, Stream fs, string contenttype = "image/jpeg")
		{
			try
			{
				using (var content = fs)
				{
					var metadata = new ObjectMetadata();
					metadata.ContentLength = content.Length;
					metadata.ContentType = contenttype;
					var state = client.PutObject(bucketName, key, content, metadata);
					if (state.HttpStatusCode == System.Net.HttpStatusCode.OK)
					{
						return 0;
					}
					return 1;
				}
			}
			catch (OssException ex)
			{
				//throw ex;
				LogHelper.WriteLog($"oss error", ex);
				return -1;
			}
			catch (Exception ex)
			{
				//throw ex;
				LogHelper.WriteLog($"error", ex);
				return -2;
			}
		}
	}
}