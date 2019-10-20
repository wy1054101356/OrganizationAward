using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Model
{
    [TableName("tbl_reward"), PrimaryKey("id", AutoIncrement = true)]
   public class Reward
    {
        public long id { get; set; }
        public int type { get; set; }
        public long uid { get; set; }
        public string uname { get; set; }
        public string remark { get; set; }
        public long points { get; set; }
        public long created { get; set; }
    }
}
