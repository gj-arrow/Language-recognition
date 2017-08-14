﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MyTask.BLL.DTO;
using MyTask.BLL.Infrastructure;

namespace MyTask.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        List<TopUserDTO> TopUsers();
    } 
}
