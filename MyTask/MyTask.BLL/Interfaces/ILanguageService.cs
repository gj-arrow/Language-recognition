using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTask.BLL.Interfaces
{
   public interface ILanguageService : IDisposable
    {
        Dictionary<string, string> DetectedLanguage(string word, string idUser);
    }
}
