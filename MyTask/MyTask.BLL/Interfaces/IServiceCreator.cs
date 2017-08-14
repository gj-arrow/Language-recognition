namespace MyTask.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
        ILanguageService CreateLanguageService(string connection);
        IGenerateData CreateGenerateDataService(string connection);
    }
}
