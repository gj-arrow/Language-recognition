using MyTask.DAL.Identity;
using MyTask.DAL.Entities;
using System;

namespace MyTask.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        UserTable<ApplicationUser> UserTable { get; }
        // IClientManager ClientManager { get; }
        // ApplicationRoleManager RoleManager { get; }
        // System.Threading.Tasks.Task SaveAsync();
    }
}
