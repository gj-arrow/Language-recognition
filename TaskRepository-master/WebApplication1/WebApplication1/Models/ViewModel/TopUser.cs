using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModel
{
    public class TopUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int CountRequest { get; set; }

        public string AverageIntervalBetweenRequest { get; set; }

        public DateTime? DateLastLogin { get; set; }
    }
}