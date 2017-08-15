using Work.BLL.Interfaces;
using Work.DAL.Interfaces;
using System.Collections.Generic;
using System;
using rosette_api;
using System.Configuration;
using System.Linq;

namespace Work.BLL.Services
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
            Dictionary<string, string> languages = new Dictionary<string, string> { { "rus", "Russian" }, { "eng", "English" }, { "spa", "Spanish" }, { "bul", "Bulgarian" }, { "por", "Portuguese" } };
            Dictionary<string, string> dictionaryResult;
            Dictionary<string, decimal?> matchLanguages = new Dictionary<string, decimal?>();
            dictionaryResult = Database.UserTable.GetSavedRequest(word);
            if (dictionaryResult.Count == 0)
            {
                string apikey = ConfigurationManager.AppSettings["apikey"];
                CAPI LanguageCAPI = new CAPI(apikey);
                LanguageCAPI.SetCustomHeaders("X-RosetteAPI-App", "csharp-app");
                LanguageIdentificationResponse response = LanguageCAPI.Language(word);
                decimal? allLanguageValue = 0;
                foreach (var language in response.LanguageDetections)
                {
                    if (languages.ContainsKey(language.Language))
                    {
                        allLanguageValue += language.Confidence;
                        matchLanguages.Add(language.Language, language.Confidence);
                    }
                }
                foreach (var language in languages)
                {
                    if (matchLanguages.Keys.Contains(language.Key))
                        dictionaryResult.Add(language.Value, String.Format("{0:0.##}%", matchLanguages[language.Key].Value * 100 / allLanguageValue));
                    else
                        dictionaryResult.Add(language.Value, "0%");
                }
                Database.UserTable.SaveRequest(dictionaryResult, word);
            }
            return dictionaryResult;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
