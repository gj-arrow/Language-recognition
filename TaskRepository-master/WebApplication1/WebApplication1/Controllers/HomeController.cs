using System.Collections.Generic;
using System.Web.Mvc;
using AspNet.Identity.SQLite;
using WebApplication1.Models.ViewModel;
using rosette_api;
using System;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public UserTable<IdentityUser> userTable = new UserTable<IdentityUser>(new SQLiteDatabase());

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetTopUsers()
        {
            IEnumerable<IdentityUser> users = userTable.GetTopUsers();
            List<TopUser> topUsers = new List<TopUser>();
            foreach (var user in users)
            {
                TopUser topUser = new TopUser();
                topUser.UserName = user.UserName;
                topUser.AverageIntervalBetweenRequest = string.Format("{0:hh\\:mm\\:ss}", userTable.GetAverageTimeBetweenRequests(user.Id));
                topUser.CountRequests = user.CountRequests;
                topUser.DateLastLogin = user.DateLastLogin;
                topUsers.Add(topUser);
            }
            return PartialView("_TopUsers",topUsers);
        }


        [HttpGet]
        public JsonResult Translator(string text)
        {
            userTable.IncreaseCountRequest(text, User.Identity.GetUserId());
            Dictionary<string, string> lang = new Dictionary<string, string> { { "rus", "Russian" }, { "eng", "English" }, { "spa", "Spanish" }, { "bul", "Bulgarian" }, { "por", "Portuguese" } };
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string apikey = "f9bf8b17393cda27b043da452c0d002e";
            CAPI LanguageCAPI = new CAPI(apikey);
            LanguageCAPI.SetCustomHeaders("X-RosetteAPI-App", "csharp-app");
            LanguageIdentificationResponse response = LanguageCAPI.Language(text);
            decimal? temp= 0;
            foreach (var langItem in response.LanguageDetections)
            {
                if (lang.ContainsKey(langItem.Language))
                temp += langItem.Confidence;
            }
            foreach (var langItem in response.LanguageDetections)
            {
                foreach (var item in lang)
                {
                    if (langItem.Language.Equals(item.Key))
                    {
                        if(!dict.ContainsKey(item.Value))
                        dict.Add(item.Value, String.Format("{0:0.##}%", langItem.Confidence*100/temp));
                        else dict[item.Value] = String.Format("{0:0.##}%", langItem.Confidence * 100 / temp);
                    }
                    else if (!dict.ContainsKey(item.Value))
                    {
                        dict.Add(item.Value, "0%");
                    }
                }
            }
            return Json(dict, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult Example(string text)
        {
            string apikey = "f9bf8b17393cda27b043da452c0d002e";
            CAPI LanguageCAPI = new CAPI(apikey);
            LanguageCAPI.SetCustomHeaders("X-RosetteAPI-App", "csharp-app");
            LanguageIdentificationResponse response = LanguageCAPI.Language(text);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}

