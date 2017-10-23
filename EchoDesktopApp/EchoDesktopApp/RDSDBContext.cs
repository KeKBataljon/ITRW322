using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoDesktopApp
{
    public class RDSDBContext : DbContext
    {
        public RDSDBContext()
            : base(RDSdb.GetRDSConnectionString())
        {
        }

        public static RDSDBContext Create()
        {
            return new RDSDBContext();
        }
    }
}
