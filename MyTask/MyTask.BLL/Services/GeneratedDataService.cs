using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTask.BLL.Interfaces;
using MyTask.DAL.Interfaces;
using MyTask.BLL.Infrastructure;
using MyTask.BLL.DTO;

namespace MyTask.BLL.Services
{
    public class GeneratedDataService : IGenerateData
    {
        IUnitOfWork Database { get; set; }
        Random rnd = new Random();
        UserService userService;
       // LanguageService languageService;
        public GeneratedDataService(IUnitOfWork uow)
        {
            Database = uow;
            userService = new UserService(Database);
           // languageService = new LanguageService(Database);
        }

        public async Task GeneratingDataDatabase()
        {
            for (int i = 0; i < 35; i++)
            await GenerateUsers();
        }

        async Task GenerateUsers()
        {
            string email,password = "";      
            password = GeneratePassword();
            email = password + "@mail.ru";
            UserDTO userDto = new UserDTO { Email = email, Password = password };
            OperationDetails operationDetails = await userService.Create(userDto);
            if (operationDetails.Succedeed)
            {
                GeneratRequests(Database.UserTable.GetUserId(email));
            }
        }

        string GeneratePassword()
        {
            int countLetter, random = 0;
            char lettter = ' ';
            string password = "";
            countLetter = rnd.Next(6,10);
            for (int j = 0; j < countLetter; j++)
            {
                random = rnd.Next(0, 26);
                lettter = (char)('a' + random);
                password += lettter;
            }
            return password;
        }


        void GeneratRequests(string id)
        {
            string word = "";
            int countRequest = rnd.Next(1000, 10000);
            for (int i = 0; i < countRequest; i++)
            { 
                word = GenerateWord();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict = DetectedLanguageFake(word, id);
             }
        }


        string GenerateWord()
        {
            rnd = new Random();
            int countLetter, random = 0;
            char lettter = ' ';
            string word = "";
            countLetter = rnd.Next(4, 16);
            for (int j = 0; j < countLetter; j++)
            {
                random = rnd.Next(0, 26);
                lettter = (char)('a' + random);
                word += lettter;
            }
            return word;
        }

        public Dictionary<string, string> DetectedLanguageFake(string word, string idUser)
        {
            int all_lang,rus, eng, spa, bul, por = 0;
            Database.UserTable.IncreaseCountRequest(word, idUser);
           // Dictionary<string, string> lang = new Dictionary<string, string> { { "rus", "Russian" }, { "eng", "English" }, { "spa", "Spanish" }, { "bul", "Bulgarian" }, { "por", "Portuguese" } };
            Dictionary<string, string> dict;
            dict = Database.UserTable.GetSavedRequest(word);
            if (dict.Count == 0)
            {
                rus = rnd.Next(1, 10);
                eng = rnd.Next(1, 10);
                spa = rnd.Next(1, 10);
                bul = rnd.Next(1, 10);
                por = rnd.Next(1, 10);
                all_lang = rus + eng + spa + bul + por;
         
                dict.Add("Russian", String.Format("{0:0.##}%", rus * 100 / all_lang));
                dict.Add("English", String.Format("{0:0.##}%", eng * 100 / all_lang));
                dict.Add("Spanish", String.Format("{0:0.##}%", spa * 100 / all_lang));
                dict.Add("Bulgarian", String.Format("{0:0.##}%", bul * 100 / all_lang));
                dict.Add("Portuguese", String.Format("{0:0.##}%", por * 100 / all_lang));
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
