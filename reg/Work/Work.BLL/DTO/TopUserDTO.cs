using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.BLL.DTO
{
   public class TopUserDTO
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int CountRequests { get; set; }

        public string AverageIntervalBetweenRequest { get; set; }

        public DateTime? DateLastLogin { get; set; }
    }
}
