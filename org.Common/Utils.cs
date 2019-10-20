using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace org.Common
{
    /// <summary>
    /// 常用工具类
    /// </summary>
    public class Utils
    {

        /// <summary>
        /// cookie域
        /// </summary>
        private static string _Domain = ConfigurationManager.AppSettings["Domain"]?.ToString().Trim() ?? "niu.cn";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutString(string str, int len)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            if (str.Length > len)
            {
                return str.Substring(0, len)+"...";
            }
            return str;
        }
        

        /// <summary>
        /// 前台时间显示
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static string DateTimeConvert(long timestamp,bool all=false)
        {
            if (timestamp == 0)
                return "";
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timestamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime date = dtStart.Add(toNow);
            if (!all)
            {
                if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
                    return $"今天 {date.ToString("HH:mm")}";
                return date.ToString("MM-dd HH:mm");
            }
            else {
                if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
                    return $"今天  {date.ToString("HH:mm:ss")}";
                return date.ToString("yyyy-MM-dd HH:mm:ss");

            }
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static List<int> String2IntList(List<string> strs)
        {
            List<int> list = new List<int>();
            if (strs != null && strs.Count > 0)
            {
                foreach (string s in strs)
                {
                    int i = TypeConvert.StrToInt(s, 0);
                    if (i > 0 && !list.Contains(i))
                    {
                        list.Add(i);
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// 掩码显示手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string MarkPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return "";

            if (phone.Length != 11)
            {
                return "";
            }

            return phone.Substring(0, 3) + "****" + phone.Substring(7);

        }

        /// <summary>
        /// 保留n位小数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static decimal CutDecimalWithN(decimal d, int n)
        {
            string strDecimal = d.ToString();
            int index = strDecimal.IndexOf(".");
            if (index == -1 || strDecimal.Length < index + n + 1)
            {
                strDecimal = string.Format("{0:F" + n + "}", d);
            }
            else
            {
                int length = index;
                if (n != 0)
                {
                    length = index + n + 1;
                }
                strDecimal = strDecimal.Substring(0, length);
            }
            return Decimal.Parse(strDecimal);
        }


        /// <summary>
        /// 显示格式化的开奖号与方案号的对比
        /// </summary>
        /// <param name="number"></param>
        /// <param name="kjh"></param>
        /// <returns></returns>
        public static string FormatNumber(string number, string kjh)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(number))
                return "";


            if (string.IsNullOrEmpty(kjh))
            {
                string[] strs = number.Split('+');
                if (strs.Length == 2)
                {
                    var r = strs[0].Split(',');
                    var b = strs[1].Split(',');

                    foreach (string s in r)
                    {
                        sb.Append("<span>" + s + "</span>");
                    }

                    sb.Append("+");

                    foreach (string s in b)
                    {
                        sb.Append("<span>" + s + "</span>");
                    }
                }
                else
                {

                }
            }
            else
            {
                string[] strs = number.Split('+');
                string[] kjhs = kjh.Split('+');

                List<string> rk = new List<string>(kjhs[0].Split(','));
                List<string> bk = new List<string>(kjhs[1].Split(','));

                if (strs.Length == 2)
                {
                    List<string> r = new List<string>(strs[0].Split(','));

                    var b = strs[1].Split(',');

                    foreach (string s in r)
                    {
                        if (rk.Contains(s))
                            sb.Append("<span  class=\"text-red\">" + s + "</span>");
                        else
                            sb.Append("<span>" + s + "</span>");
                    }

                    sb.Append("+");

                    foreach (string s in b)
                    {
                        if (bk.Contains(s))
                            sb.Append("<span class=\"text-blue\">" + s + "</span>");
                        else
                            sb.Append("<span>" + s + "</span>");
                    }
                }
                else
                {

                }

            }

            return sb.ToString();
        }

        /// <summary>
        /// 显示带球的开奖号
        /// </summary>
        /// <param name="kjh"></param>
        /// <returns></returns>
        public static string FormatKjh(string kjh)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(kjh))
                return "";

            string[] strs = kjh.Split('+');
            if (strs.Length == 2)
            {
                var r = strs[0].Split(',');
                var b = strs[1].Split(',');

                foreach (string s in r)
                {
                    sb.Append("<span class=\"ball\">" + s + "</span>");
                }

                foreach (string s in b)
                {
                    sb.Append("<span class=\"ball blue\">" + s + "</span>");
                }
            }
            else
            {
                var r = strs[0].Split(',');
                foreach (string s in r)
                {
                    sb.Append("<span class=\"ball\">" + s + "</span>");
                }
            }

            return sb.ToString();

        }


        /// <summary>
        /// 绑定Enum到DropList
        /// </summary>
        /// <param name="type"></param>
        /// <param name="selectID">选中的ID</param>
        /// <returns></returns>
        public static string BindDropListByEnum(object obj, int selectID = 0)
        {
            Type t = obj.GetType();
            StringBuilder sb = new StringBuilder();
            var list = EnumUtils.ToList(t);
            if (list != null && list.Count > 0)
            {
                list = list.OrderBy(n => n.ID).ToList();
                foreach (var item in list)
                {
                    if (item.ID == selectID)
                        sb.Append("<option selected=\"selected\" value=\"" + item.ID + "\">" + item.Description + "</option>");
                    else
                        sb.Append("<option value=\"" + item.ID + "\">" + item.Description + "</option>");
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 格式化显示银行卡号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string FormatBankID(string id)
        {
            StringBuilder ret = new StringBuilder();
            if (string.IsNullOrEmpty(id))
                return "";
            int len = id.Length;
            for (int i = 0; i < len; i++)
            {
                ret.Append(id.Substring(i, 1));
                if ((i + 1) % 4 == 0)
                    ret.Append(" ");
            }
            return ret.ToString();
        }


        /// <summary>
        /// 格式化UID
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string FormatUID(long uid)
        {
            return uid.ToString("0000");
        }




        /// <summary>
        /// 格式化金额
        /// </summary>
        /// <param name="money">钱</param>
        /// <param name="n">小数位数</param>
        /// <returns></returns>
        public static string FormatMoney(decimal money, int n = 2)
        {
            var culture = (CultureInfo)CultureInfo.GetCultureInfo("zh-cn").Clone();
            culture.NumberFormat.NumberGroupSizes =
                culture.NumberFormat.CurrencyGroupSizes = new int[] { 3 };
            culture.NumberFormat.NumberGroupSeparator =
                culture.NumberFormat.CurrencyGroupSeparator = ",";
            string ret = string.Empty;
            if (n == 2)
                ret = money.ToString("N2", culture);
            if (n == 0)
                ret = money.ToString("N0", culture);

            return ret;

        }

        /// <summary>
        /// 格式化显示手机
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="isVery"></param>
        /// <returns></returns>
        public static string FormatMobile(string mobile, int isVery, bool isMark = true)
        {
            if (string.IsNullOrEmpty(mobile))
                return "";
            string ret = string.Empty;
            string temp = mobile;
            if (mobile.Length == 11 && isMark)
            {
                temp = mobile.Substring(0, 3) + "****" + mobile.Substring(7);
            }

            if (!string.IsNullOrEmpty(mobile) && isVery == 1)
            {
                ret = temp + " - 已认证";
            }
            else if (!string.IsNullOrEmpty(mobile) && isVery == 0)
            {
                ret = temp + " - 未认证";
            }

            return ret;
        }

        /// <summary>
        /// 格式化性别
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static string FormatSex(int sex)
        {
            switch (sex)
            {
                case 1:
                    return "男";
                case 2:
                    return "女";
                default:
                    return "未知";
            }
        }

        /// <summary>
        /// 后台显示的格式化时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime time, bool week = true)
        {
            if (time == null || time.Year == 1)
                return "";
            var t = time;
            var now = DateTime.Now;

            if (week)
                return t.ToString("yyyy-MM-dd HH:mm dddd");
            else
                return t.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 后台显示的格式化时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatDateTime(long time, bool week = true)
        {
            if (time == 0)
                return "";
            string style = string.Empty;
            var t = TypeConvert.LongToDateTime(time);
            var now = DateTime.Now;
            if (!(t.Year == now.Year && t.Month == now.Month && t.Day == now.Day))
                style = "class=\"grey\"";

            if (week)
                return "<span " + style + ">" + t.ToString("yyyy-MM-dd HH:mm (dddd)") + "</span>";
            else
                return "<span " + style + ">" + t.ToString("yyyy-MM-dd HH:mm") + "</span>";
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static byte[] GetCheckCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "验证码";

            byte[] retByte = null;
            Bitmap map = null;
            System.IO.MemoryStream ms = null;
            Graphics graph = null;
            try
            {
                int randAngle = 90; //随机转动角度
                int mapwidth = (int)(code.Length * 20);
                map = new Bitmap(mapwidth, 36);//创建图片背景
                graph = Graphics.FromImage(map);
                graph.Clear(Color.AliceBlue);//清除画面，填充背景
                graph.DrawRectangle(new Pen(Color.SkyBlue, 0), 0, 0, map.Width - 1, map.Height - 1);//画一个边框
                //graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//模式
                Random rand = new Random();
                //背景噪点生成
                Pen blackPen = new Pen(Color.LightGray, 0);
                for (int i = 0; i < 80; i++)
                {
                    int x = rand.Next(0, map.Width);
                    int y = rand.Next(0, map.Height);
                    graph.DrawRectangle(blackPen, x, y, 1, 1);
                }
                //验证码旋转，防止机器识别
                char[] chars = code.ToCharArray();//拆散字符串成单字符数组

                //文字距中
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                //定义颜色
                Color[] c = { Color.YellowGreen, Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

                //定义字体
                string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "tahoma", "Georgia" };
                for (int i = 0; i < chars.Length; i++)
                {
                    int cindex = rand.Next(9);
                    int findex = rand.Next(6);
                    Font f = new System.Drawing.Font(font[findex], 23, System.Drawing.FontStyle.Bold);//字体样式(参数2为字体大小)
                    Brush b = new System.Drawing.SolidBrush(c[cindex]);
                    Point dot = new Point(13, 13);
                    //graph.DrawString(dot.X.ToString(),fontstyle,new SolidBrush(Color.Black),10,150);//测试X坐标显示间距的
                    float angle = rand.Next(-randAngle, randAngle);//转动的度数
                    graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置
                    graph.RotateTransform(angle);
                    graph.DrawString(chars[i].ToString(), f, b, 5, 5, format);
                    //graph.DrawString(chars[i].ToString(),fontstyle,new SolidBrush(Color.Blue),1,1,format);
                    graph.RotateTransform(-angle);//转回去
                    graph.TranslateTransform(1, -dot.Y);//移动光标到指定位置
                }
                //graph.DrawString(randomcode,fontstyle,new SolidBrush(Color.Blue),2,2); //标准随机码

                //生成图片
                ms = new System.IO.MemoryStream();
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                retByte = ms.ToArray();

            }
            catch
            {
            }
            finally
            {
                if (ms != null)
                    ms.Close();
                graph.Dispose();
                map.Dispose();
            }

            return retByte;


        }

        /// <summary>
        /// 获取用户代理信息
        /// </summary>
        /// <returns></returns>
        public static string GetUserAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !IsIP(result))
                return "127.0.0.1";

            return result;
        }

        /// <summary>
        /// 生成GUID
        /// 去掉-的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetGUID()
        {
            string key = Guid.NewGuid().ToString().Replace("-", "");
            return key;
        }

        /// <summary>
        /// MD5函数
        /// 返回值为大写
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret.ToUpper();
        }


        /// <summary>
        /// 是否是合法的IP格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 生成随机因子
        /// </summary>
        /// <returns></returns>
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 生成min-max的随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomInt(int min, int max)
        {
            Random random = new Random(GetRandomSeed());
            return random.Next(min, max);
        }

        /// <summary>
        /// 生成4位数字随机数
        /// </summary>
        /// <returns></returns>
        public static string GetRandomInt()
        {
            Random random = new Random();
            return (random.Next(1000, 9999).ToString());
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest|管理员|彩宝网|彩宝|8200");
        }

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            return TypeConvert.StrToBool(expression, defValue);
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            return TypeConvert.StrToBool(expression, defValue);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object expression, int defValue)
        {
            return TypeConvert.ObjectToInt(expression, defValue);
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            return TypeConvert.StrToInt(expression, defValue);
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            return TypeConvert.StrToFloat(strValue, defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            return TypeConvert.StrToFloat(strValue, defValue);
        }

        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]*$");
        }

        #region Session操作

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void WriteSession(string strSessionName, string strValue, int iExpires = 20)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="strSessionName"></param>
        /// <returns></returns>
        public static string GetSession(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName].ToString();
            }
        }

        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Remove(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }

        #endregion

    }
}
