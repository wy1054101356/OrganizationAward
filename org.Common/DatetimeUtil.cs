using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Common
{
    public class DatetimeUtil
    {
        public static string GetTimeStr(long timestamp)
        {
            DateTime time = TypeConvert.LongToDateTime(timestamp);
            if (time.Date == DateTime.Now.Date)
                return $"今天 {(time.Hour > 9 ? $"{time.Hour}" : $"0{time.Hour}")}:{(time.Minute > 9 ? $"{time.Minute}" : $"0{time.Minute}")}:{(time.Second > 9 ? $"{time.Second}" : $"0{time.Second}")}";
            else
                return time.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
