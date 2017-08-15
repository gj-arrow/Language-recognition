using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.BLL.Interfaces;
using Work.DAL.Interfaces;
using Work.BLL.Infrastructure;
using Work.BLL.DTO;

namespace Work.BLL.Services
{
    public class GeneratedDataService : IGenerateData
    {
        IUnitOfWork Database { get; set; }
        Random rnd = new Random();
        UserService userService;
        int countLetter, random = 0;
        char lettter = ' ';
        int all_lang , rus, eng, spa, bul, por = 0;
        public GeneratedDataService(IUnitOfWork uow)
        {
            Database = uow;
            userService = new UserService(Database);
        }

        public async Task GeneratingDataDatabase()
        {
            for (int i = 0; i < 13; i++)
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
            countLetter = random = 0;
            lettter = ' ';
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
            Dictionary<string, string> dict = new Dictionary<string, string>();
            int countRequest = rnd.Next(1500, 6000);
            for (int i = 0; i < countRequest; i++)
            { 
                word = GenerateWord();
                dict = DetectedLanguageFake(word, id);
             }
        }


        string GenerateWord()
        {
            rnd = new Random();
            countLetter = random = 0;
            lettter = ' ';
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
            all_lang = rus= eng= spa= bul=por = 0;
            Database.UserTable.IncreaseCountRequest(word, idUser);
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
