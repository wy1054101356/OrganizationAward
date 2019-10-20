using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Model
{
    [TableName("tbl_organization"), PrimaryKey("oid", AutoIncrement = true)]
    public class organization
    {
        public long oid { get; set; }
        public string oname { get; set; }
        public long created { get; set; }
        public long status { get; set; }
    }
}
