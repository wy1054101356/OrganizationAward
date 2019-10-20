using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Model
{

    /// <summary>
    /// 后台登录中使用的模型
    /// </summary>
    public class LoginUser
    {

        public long oid { get; set; }

        public string oname { get; set; }

        public long uid { get; set; }

        public string uname { get; set; }

        public int type { get; set; }
    }

}
