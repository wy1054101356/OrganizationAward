using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Common
{
    public class MoneyConvert
    {
        /// <summary> 
        /// 输入Float格式数字，将其转换为货币表达方式 
        /// </summary> 
        /// <param name="ftype">货币表达类型：0=带￥的货币表达方式；1=不带￥的货币表达方式；其它=带￥的货币表达方式</param> 
        /// <param name="fmoney">传入的int数字</param> 
        /// <returns>返回转换的货币表达形式</returns> 
        public static string Rmoney(int ftype, double fmoney)
        {
            string _rmoney;
            try
            {
                switch (ftype)
                {
                    case 0:
                        _rmoney = string.Format("{0:C2}", fmoney);
                        break;
                    case 1:
                        _rmoney = string.Format("{0:N2}", fmoney);
                        break;
                    default:
                        _rmoney = string.Format("{0:C2}", fmoney);
                        break;
                }
            }
            catch
            {
                _rmoney = "0";
            }
            if (_rmoney == "0")
                return _rmoney;
            return _rmoney.Substring(0, _rmoney.Length - 3);
        }
    }
}
