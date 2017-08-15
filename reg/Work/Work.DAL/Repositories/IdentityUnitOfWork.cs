using Work.DAL.Entities;
using Work.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using Work.DAL.Identity;

namespace Work.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private SQLiteDatabase db;

        private ApplicationUserManager userManager;
        private UserTable<ApplicationUser> userTable;

        public IdentityUnitOfWork(string connectionString)
        {
            db = new SQLiteDatabase(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser,ApplicationRole>(db));
            userTable = new UserTable<ApplicationUser>(db);
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public UserTable<ApplicationUser> UserTable
        {
            get { return userTable; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
