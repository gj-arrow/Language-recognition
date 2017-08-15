using Work.BLL.Interfaces;
using Work.DAL.Repositories;

namespace Work.BLL.Services
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
