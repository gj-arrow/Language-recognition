using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Work.BLL.DTO;
using Work.BLL.Infrastructure;

namespace Work.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);

        bool FindUserByName(string userName);
        List<TopUserDTO> TopUsers();
    } 
}
