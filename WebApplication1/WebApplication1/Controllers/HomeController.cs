using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AspNet.Identity.SQLite;
using WebApplication1.Models.ViewModel;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            UserTable<IdentityUser> userTable = new UserTable<IdentityUser>(new SQLiteDatabase());
            IEnumerable<IdentityUser> users = userTable.GetTopUsers();
            List<TopUser> topUsers = new List<TopUser>();
            foreach (var user in users)
            {
                TopUser topUser = new TopUser();
                topUser.UserName = user.UserName;
                topUser.AverageIntervalBetweenRequest = user.AverageIntervalBetweenRequest;
                topUser.CountRequest = user.CountRequest;
                topUser.DateLastLogin = user.DateLastLogin;
                topUsers.Add(topUser);
            }
            return View(topUsers);
        }
    }
}