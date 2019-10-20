using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Model
{
    [TableName("tbl_record"), PrimaryKey("rid", AutoIncrement = true)]
    public class Record
    {
        public long rid { get; set; }
        public long uid { get; set; }
        public string uname { get; set; }
        public long oid { get; set; }
        public string oname { get; set; }
        public long created { get; set; }
        public long level { get; set; }
        public long powernum { get; set; }
        public long tongling { get; set; }
        public long fighttime { get; set; }
        public long lastweekdeduction { get; set; }
        public long otherbonuspoints { get; set; }
        public long otherdeduction { get; set; }
        public long sumpoints { get; set; }
        public int bagtype { get; set; }
    }
}
