using System.Collections.Generic;
using System.Web.Mvc;
using AspNet.Identity.SQLite;
using WebApplication1.Models.ViewModel;
using rosette_api;
using System;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public UserTable<IdentityUser> userTable = new UserTable<IdentityUser>(new SQLiteDatabase());

        [Authorize]
        public ActionResult Index()
        {
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

        [HttpGet]
        public JsonResult Translator(string text)
        {
            userTable.IncreaseCountRequest(HttpContext.User.Identity.Name);
            Dictionary<string, string> lang = new Dictionary<string, string> { { "rus", "Russian" }, { "eng", "English" }, { "spa", "Spanish" }, { "bul", "Bulgarian" }, { "por", "Portuguese" } };
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string apikey = "f9bf8b17393cda27b043da452c0d002e";
            CAPI LanguageCAPI = new CAPI(apikey);
            LanguageCAPI.SetCustomHeaders("X-RosetteAPI-App", "csharp-app");
            LanguageIdentificationResponse response = LanguageCAPI.Language(text);
            foreach(var langItem in response.LanguageDetections)
            {
                foreach (var item in lang)
                {
                    if (langItem.Language.Equals(item.Key))
                    {
                        dict.Add(item.Value, String.Format("{0:0.##}%", langItem.Confidence*100));
                    }
                }
            }
            foreach (var item in lang)
            {
                if (!dict.ContainsKey(item.Value))
                {
                    dict.Add(item.Value, "0%");
                }
            }
            return Json(dict, JsonRequestBehavior.AllowGet);
        }

    }
}

