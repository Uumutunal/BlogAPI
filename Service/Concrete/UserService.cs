﻿using Domain.Core.Repositories;
using Domain.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.Abstract;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly IRepository<Follower> _followerRepository;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public UserService(IUserRepository<User> userRepository, IMapper mapper, UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration, IServiceProvider serviceProvider, IRepository<Follower> followerRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _followerRepository = followerRepository;
        }


        public async Task<List<UserDto>> GetAllUsers()
        {

            var users = _userManager.Users.Where(u => !u.IsDeleted).ToList();
            var mappedUsers = users.Adapt<List<UserDto>>();

            return mappedUsers;
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var mappedUser = user.Adapt<UserDto>();

            return mappedUser;

        }
        public async Task<IdentityResult> DeleteUser(string id)
        {


            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {

                var result = await _userManager.DeleteAsync(user);

                return result;
            }

            return null;

        }
        public async Task<UserDto> Login(string email, string password)
        {

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                return new UserDto
                {
                    Email = user.Email,
                    Id = user.Id
                };
            }
            return null;
        }

        public async Task<UserDto> Register(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname,
                Photo = userDto.Photo,
            };
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                // You can add more claims or roles here
                var mappedUser = user.Adapt<UserDto>();
                return mappedUser;
            }

            return null;
        }

        public async Task<IdentityUser> UpdateUser(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user != null)
            {
                user.Firstname = userDto.Firstname;
                user.Lastname = userDto.Lastname;
                user.Email = userDto.Email;
                user.UserName = userDto.Email;
                user.ModifiedDate = DateTime.Now;
                user.Photo = userDto.Photo;

                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, userDto.Password);

                if (!string.IsNullOrWhiteSpace(userDto.Password))
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userDto.Password);
                }

                await _userManager.UpdateAsync(user);
                return user;
            }
            return null;
        }

        public async Task<bool> UpdateUserRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleExist = await roleManager.RoleExistsAsync(roleName);

            if (roleExist == null)
            {
                return false;
            }

            var addResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addResult.Succeeded)
            {
                return false;
            }

            return true;

        }

        public async Task FollowUser(FollowerDto followerDto)
        {
            var mappedFollow = followerDto.Adapt<Follower>();
            var followerUser = await _followerRepository.AddAsync(mappedFollow);
        }
        public async Task UnFollowUser(Guid id)
        {
            await _followerRepository.Delete(id);
        }

        public async Task<List<FollowerDto>> GetAllFollowerUser()
        {
            var allFollowers = await _followerRepository.GetAllAsync();
            var mappedFollowers = allFollowers.Adapt<List<FollowerDto>>();
            return mappedFollowers;
        }

        public async Task<bool> RemoveUserRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleExist = await roleManager.RoleExistsAsync(roleName);

            if (roleExist == null)
            {
                return false;
            }


            var removeResult = await _userManager.RemoveFromRolesAsync(user, new List<string>() { roleName });
            if (!removeResult.Succeeded)
            {
                return false;
            }
            return true;
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

        public async Task<List<string>> GetAllRoles()
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            var roles = roleManager.Roles.Select(r => r.Name).ToList();

            return roles;
        }

        public async Task<List<string>> GetUserRoleById(string id)
        {
            var user = await _userManager.FindByEmailAsync(id);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }

            return null;

        }



        public async Task<string> GenerateJwtToken(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            // JWT ayarlarını appsettings'den al
            var issuer = _configuration["JwtTokenSettings:Issuer"];
            var audience = _configuration["JwtTokenSettings:Audience"];
            var key = _configuration["JwtTokenSettings:Key"];
            var lifetime = Convert.ToDouble(_configuration["JwtTokenSettings:Lifetime"]);

            var user = _userManager.Users.FirstOrDefault(x => x.Id == userDto.Id);
            var roles = await _userManager.GetRolesAsync(user);

            // JWT için claim'leri oluştur
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userDto.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userDto.Id ?? string.Empty),
                new Claim(ClaimTypes.Name, userDto.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Şifreleme anahtarını ve kimlik doğrulama bilgilerini ayarla
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            // JWT token oluştur
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(lifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
