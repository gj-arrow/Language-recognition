using System;
using AspNet.Identity.SQLite;

namespace WebApplication1.Models.ViewModel
{
    public class TopUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int CountRequests { get; set; }

        public string AverageIntervalBetweenRequest { get; set; }

        public DateTime? DateLastLogin { get; set; }
    }
}