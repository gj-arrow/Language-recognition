using Work.DAL.Identity;
using Work.DAL.Entities;
using System;

namespace Work.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        UserTable<ApplicationUser> UserTable { get; }
    }
}
