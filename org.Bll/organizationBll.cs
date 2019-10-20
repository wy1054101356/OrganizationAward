using org.Data;
using org.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Bll
{
   public class organizationBll : BaseData<organization>
    {
        public static bool Delete(long id)
        {
            var info = SingleOrDefault(id);
            info.status = (int)Enums.StatusEnum.Deleted;
            return Update(info);
        }
    }
}
