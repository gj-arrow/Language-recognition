using Work.BLL.DTO;
using Work.BLL.Infrastructure;
using Work.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Work.BLL.Interfaces;
using Work.DAL.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Work.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                await Database.UserManager.CreateAsync(user, userDto.Password);
                Database.UserTable.UpdateDateLastLogin(userDto.Email, DateTime.Now);
                return new OperationDetails(true, "Registration was success", "");

            }
            else
            {
                return new OperationDetails(false, "Invalid Login or Password", "Login");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null) { 
                claim= await Database.UserManager.CreateIdentityAsync(user,DefaultAuthenticationTypes.ApplicationCookie);
                Database.UserTable.UpdateDateLastLogin(userDto.Email, DateTime.Now);
            }

            return claim;
        }

        public List<TopUserDTO> TopUsers()
        {
            IEnumerable<IdentityUser> users = Database.UserTable.GetTopUsers();
            List<TopUserDTO> topUsers = new List<TopUserDTO>();
            foreach (var user in users)
            {
                TopUserDTO topUser = new TopUserDTO();
                topUser.UserName = user.UserName;
                topUser.AverageIntervalBetweenRequest = string.Format("{0:hh\\:mm\\:ss}", Database.UserTable.GetAverageTimeBetweenRequests(user.Id));
                topUser.CountRequests = user.CountRequests;
                topUser.DateLastLogin = user.DateLastLogin;
                topUsers.Add(topUser);
            }
            return topUsers;
        }

        public bool FindUserByName(string userName)
        {
            List<ApplicationUser> user = Database.UserTable.GetUserByName(userName);
            if (user.Count == 0)
                return false;
            return true;
        }


        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
