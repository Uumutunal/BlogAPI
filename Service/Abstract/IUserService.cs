﻿using Microsoft.AspNetCore.Identity;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IUserService
    {
        Task<UserDto> Register(UserDto userDto);
        Task<UserDto> Login(string email, string password);
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(string id);
        Task<IdentityUser> UpdateUser(UserDto userDto);
        Task AssignRoleToUser(string userEmail, string roleName);
        Task<bool> RemoveUserRole(string userId, string roleName);
        Task AddRole(string[] roles);
        Task<string> GenerateJwtToken(UserDto userDto);
        Task<bool> UpdateUserRoleAsync(string userId, string roleName);
        Task<List<string>> GetAllRoles();
        Task<IdentityResult> DeleteUser(string id);
        Task<List<string>> GetUserRoleById(string id);
        Task FollowUser(FollowerDto followerDto);
        Task UnFollowUser(Guid id);
        Task<List<FollowerDto>> GetAllFollowerUser();
    }
}
