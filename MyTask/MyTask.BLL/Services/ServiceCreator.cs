using MyTask.BLL.Interfaces;
using MyTask.DAL.Repositories;

namespace MyTask.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }

        public ILanguageService CreateLanguageService(string connection)
        {
            return new LanguageService(new IdentityUnitOfWork(connection));
        }

        public IGenerateData CreateGenerateDataService(string connection)
        {
            return new GeneratedDataService(new IdentityUnitOfWork(connection));
        }
    }
}
