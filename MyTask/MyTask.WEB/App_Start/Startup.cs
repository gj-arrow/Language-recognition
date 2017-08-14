using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using MyTask.BLL.Services;
using Microsoft.AspNet.Identity;
using MyTask.BLL.Interfaces;


[assembly: OwinStartup(typeof(MyTask.App_Start.Startup))]

namespace MyTask.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.CreatePerOwinContext<ILanguageService>(CreateLanguageService);
            app.CreatePerOwinContext<IGenerateData>(CreateGenerateDataService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService("DefaultConnection");
        }

        private ILanguageService CreateLanguageService()
        {
            return serviceCreator.CreateLanguageService("DefaultConnection");
        }

        private IGenerateData CreateGenerateDataService()
        {
            return serviceCreator.CreateGenerateDataService("DefaultConnection");
        }
    }
}
