using MyTask.BLL.DTO;
using MyTask.BLL.Infrastructure;
using MyTask.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MyTask.BLL.Interfaces;
using MyTask.DAL.Interfaces;
using System.Collections.Generic;
using System;
using rosette_api;


namespace MyTask.BLL.Services
{
  public  class LanguageService : ILanguageService
    {
        IUnitOfWork Database { get; set; }

        public LanguageService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public Dictionary<string, string> DetectedLanguage(string word,  string idUser) {
            Database.UserTable.IncreaseCountRequest(word, idUser);
            Dictionary<string, string> lang = new Dictionary<string, string> { { "rus", "Russian" }, { "eng", "English" }, { "spa", "Spanish" }, { "bul", "Bulgarian" }, { "por", "Portuguese" } };
            Dictionary<string, string> dict;
            dict = Database.UserTable.GetSavedRequest(word);
            if (dict.Count == 0)
            {
                string apikey = "f9bf8b17393cda27b043da452c0d002e";
                CAPI LanguageCAPI = new CAPI(apikey);
                LanguageCAPI.SetCustomHeaders("X-RosetteAPI-App", "csharp-app");
                LanguageIdentificationResponse response = LanguageCAPI.Language(word);
                decimal? temp = 0;
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
                            if (!dict.ContainsKey(item.Value))
                                dict.Add(item.Value, String.Format("{0:0.##}%", langItem.Confidence * 100 / temp));
                            else dict[item.Value] = String.Format("{0:0.##}%", langItem.Confidence * 100 / temp);
                        }
                        else if (!dict.ContainsKey(item.Value))
                        {
                            dict.Add(item.Value, "0%");
                        }
                    }
                }
                Database.UserTable.SaveRequest(dict, word);
            }
            return dict;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
