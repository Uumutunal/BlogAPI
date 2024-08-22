using Domain.Core.Repositories;
using Domain.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Abstract;
using Service.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Concrete
{
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly IUserRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public UserService(IUserRepository<User> userRepository, IMapper mapper, UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }


        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = _userRepository.GetAllAsync();
            var mappedUsers = users.Adapt<List<UserDto>>();

            return mappedUsers;
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var user = _userRepository.GetByIdAsync(id);
            var mappedUser = user.Adapt<UserDto>();

            return mappedUser;

        }

        public async Task<bool> Login(string email, string password)
        {

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Generate JWT token here if using JWT authentication
                return true;
            }
            return false;
        }

        public async Task<UserDto> Register(UserDto userDto)
        {
            var user = new User { UserName = userDto.Email, Email = userDto.Email };
            var result = await _userManager.CreateAsync(user, userDto.PasswordHash);

            if (result.Succeeded)
            {
                // You can add more claims or roles here
                var mappedUser = user.Adapt<UserDto>();
                return mappedUser;
            }

            return null;
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var mappedUser = userDto.Adapt<User>();

            await _userRepository.Update(mappedUser);

        }

        public async Task AssignRoleToUser(string userEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleExist = await roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                return;
            }

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    Console.WriteLine($"{roleName} role assigned to {userEmail}");
                }
                else
                {
                    Console.WriteLine($"Error assigning role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

        }

        public async Task AddRole(string[] roles)
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            IdentityResult roleResult;

            foreach (var roleName in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

    }
}
