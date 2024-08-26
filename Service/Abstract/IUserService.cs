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
        Task UpdateUser(UserDto userDto);
        Task AssignRoleToUser(string userEmail, string roleName);
        Task AddRole(string[] roles);
        Task<string> GenerateJwtToken(UserDto userDto);
        Task<bool> UpdateUserRoleAsync(string userId, string roleName);
    }
}
