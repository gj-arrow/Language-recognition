using MyTask.BLL.DTO;
using MyTask.BLL.Infrastructure;
using MyTask.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using MyTask.BLL.Interfaces;
using MyTask.DAL.Interfaces;
using System.Collections.Generic;
using System;

namespace MyTask.BLL.Services
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
                return new OperationDetails(true, "Регистрация успешно пройдена", "");

            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Login");
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


        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
