using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTask.Models
{
    public class TopUserViewModel
    {
            public string Id { get; set; }

            public string UserName { get; set; }

            public int CountRequests { get; set; }

            public string AverageIntervalBetweenRequest { get; set; }

            public DateTime? DateLastLogin { get; set; }  
    }
}