using org.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace org.Admin.Common
{
    public class UpLoad
    {

        /// <summary>
        /// 上传android包
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static RetInfo UpLoadAndroid(string version,string dir = "apk")
        {
            RetInfo info = new RetInfo();

            var files = HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                string ext = Path.GetExtension(file.FileName);
                string contentType = "application/octet-stream";
                switch (ext.ToLower())
                {
                    case ".patch":
                        contentType = "application/octet-stream";
                        break;
                    case ".apk":
                        contentType = "application/vnd.android.package-archive";
                        break;
                }

                if (!string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = string.Format("{0}{1}", Utils.GetGUID(), ext);
                    string filePath = string.Format("{0}/{1}/{2}/{3}", dir, DateTime.Now.ToString("yyyyMMdd"), version, fileName);
                    
                    int a = AliyunOss.PutObject(filePath, file.InputStream, contentType);
                    if (a == 0)
                    {
                        info.code = 0;
                        info.url = AliyunOss.GetHead() + filePath;
                    }
                }
            }

            return info;
        }



        /// <summary>
        /// 编辑器上传图片的方法  
        /// </summary>
        /// <returns></returns>
        public static UpLoadResult UpLoadFile()
        {

            //{"originalName":"demo.jpg","name":"demo.jpg","url":"upload\/demo.jpg","size":"99697","type":".jpg","state":"SUCCESS"}

            UpLoadResult ret = new UpLoadResult();

            if (HttpContext.Current.Request.Files.Count == 0)
            {
                return new UpLoadResult() { state = "没有选择图片文件" };
            }

            var file = HttpContext.Current.Request.Files[0];
            if (string.IsNullOrEmpty(file.FileName))
            {
                return new UpLoadResult() { state = "没有选择上传文件" };
            }

            string fileName = string.Format("{0}.jpg", Utils.GetGUID());

            string filePath = string.Format("topic/{0}/{1}", DateTime.Now.ToString("yyyyMMdd"), fileName);

            int i = AliyunOss.PutObject(filePath, file.InputStream);
            if (i == 0)
            {
                return new UpLoadResult()
                {
                    originalName = fileName,
                    name = fileName,
                    url = AliyunOss.GetHead() + filePath,
                    size = "1024",
                    type = ".jpg",
                    state = "SUCCESS",
                };
            }
            else
            {
                return new UpLoadResult()
                {
                    originalName = fileName,
                    name = fileName,
                    url = AliyunOss.GetHead() + filePath,
                    size = "1024",
                    type = ".jpg",
                    state = "上传失败，请重试",
                };
            }
        }

        /// <summary>
        /// 多个图片上传
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<RetInfo> PostMoreUpLoadFile(string dir = "topic")
        {
            List<RetInfo> list = new List<RetInfo>();

            var files = HttpContext.Current.Request.Files;
            List<string> names = new List<string>(files.AllKeys);
            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    string name = files.GetKey(i);
                    var file = files[i];
                    RetInfo info = new RetInfo();
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = string.Format("{0}.jpg", Utils.GetGUID());
                        string filePath = string.Format("{0}/{1}/{2}", dir, DateTime.Now.ToString("yyyyMMdd"), fileName);
                        int a = AliyunOss.PutObject(filePath, file.InputStream);
                        if (a == 0)
                        {
                            info.code = 0;
                            info.url = AliyunOss.GetHead() + filePath;
                            switch (name)
                            {
                                case "sp":
                                    info.uplaodType = UpLoadType.SP;
                                    break;
                                case "djd":
                                    info.uplaodType = UpLoadType.DJD;
                                    info.name = "兑奖单";
                                    break;
                                case "hb":
                                    info.uplaodType = UpLoadType.HB;
                                    info.name = "牛票票喜上加喜红包";
                                    break;

                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(info.url))
                        list.Add(info);
                }
            }

            return list;

        }

        /// <summary>
        /// Post请求时的文件上传
        /// </summary>
        /// <returns></returns>
        public static List<RetInfo> PostUpLoadFile(string dir = "topic")
        {
            List<RetInfo> list = new List<RetInfo>();

            var files = HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    RetInfo info = new RetInfo();
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = string.Format("{0}.jpg", Utils.GetGUID());
                        string filePath = string.Format("{0}/{1}/{2}", dir, DateTime.Now.ToString("yyyyMMdd"), fileName);
                        int a = AliyunOss.PutObject(filePath, file.InputStream);
                        if (a == 0)
                        {
                            info.code = 0;
                            info.url = AliyunOss.GetHead() + filePath;
                        }
                    }
                    if (!string.IsNullOrEmpty(info.url))
                        list.Add(info);
                }
            }

            return list;

        }
    }


    public enum UpLoadType : int
    {
         SP = 1,


         DJD = 2,

         HB = 3,

    }

    public class RetInfo
    {
        public int code { get; set; }

        /// <summary>
        /// 上传类型
        /// </summary>
        public UpLoadType uplaodType { get; set; }

        public string url { get; set; }

        public string name { get; set; }

    }


    public class UpLoadResult
    {
        //{"originalName":"demo.jpg","name":"demo.jpg","url":"upload\/demo.jpg","size":"99697","type":".jpg","state":"SUCCESS"}

        public string originalName { get; set; }

        public string name { get; set; }

        public string url { get; set; }

        public string size { get; set; }

        public string type { get; set; }

        public string state { get; set; }


    }
}
