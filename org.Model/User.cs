using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Model
{
    /// <summary>
    /// 后台帐号登录表
    /// </summary>
    [TableName("tbl_user"), PrimaryKey("uid", AutoIncrement = true)]
    public class User
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long uid { get; set; }
        /// <summary>
        /// 组织id
        /// </summary>
        public long oid { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string oname { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string uname { get; set; }
        /// <summary>
        /// qq账号
        /// </summary>
        public long QQnumber { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public long created { get; set; }

        /// <summary>
        /// 状态值
        /// </summary>
        public int status { get; set; }

        public int user_type { get; set; }
    }
}
