using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace BlogWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public AccountController(UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration, IServiceProvider serviceProvider, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // You can add more claims or roles here
                return Ok(new { result = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {

                //var token = GenerateToken(model.Email);

                // Generate JWT token here if using JWT authentication
                return Ok(new { result = "Login successful" });
            }

            return Unauthorized();
        }

        [HttpPost("AddRole")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("AssignRoleToUser")]
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

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken(string userEmail)
        {

            //appsettings.json'daki tanımlamalarımız yoksa hata versin.
            if (!_configuration.GetSection("JwtTokenSettings").Exists())
                return BadRequest("JwtTokenSettings mevcut değildir!");

            if (!_configuration.GetSection("JwtTokenSettings").GetSection("Issuer").Exists())
                return BadRequest("Issuer mevcut değildir!");


            if (!_configuration.GetSection("JwtTokenSettings").GetSection("Audience").Exists())
                return BadRequest("Audience mevcut değildir!");

            if (!_configuration.GetSection("JwtTokenSettings").GetSection("Key").Exists())
                return BadRequest("Key mevcut değildir!");

            if (!_configuration.GetSection("JwtTokenSettings").GetSection("Lifetime").Exists())
                return BadRequest("Lifetime mevcut değildir!");

            //hepsi varsa istenen kullanıcıyı bul
            var user = await _userManager.FindByEmailAsync(userEmail);


            if (user != null)
            {
                //token'ı oluştur !

                string issuer = _configuration.GetSection("JwtTokenSettings").GetSection("Issuer").Value;
                //string issuer = _configuration["JwtTokenSettings:Issuer"];

                string audience = _configuration.GetSection("JwtTokenSettings").GetSection("Audience").Value;
                string key = _configuration.GetSection("JwtTokenSettings").GetSection("Key").Value;
                string lifetime = _configuration.GetSection("JwtTokenSettings").GetSection("Lifetime").Value;

                DateTime sonlanmaTarihi = DateTime.Now.AddMinutes(Convert.ToDouble(lifetime));

                SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                //kullanıcının özelliklerini hesaba katalım

                List<Claim> claims = new List<Claim>();

                var role = await _userManager.GetRolesAsync(user);

                //Claim claim1 = new Claim(ClaimTypes.Name, istenenKullanici.Ad);
                //Claim claim2 = new Claim(ClaimTypes.Surname, istenenKullanici.Soyad);
                //Claim claim3 = new Claim(ClaimTypes.NameIdentifier, istenenKullanici.KullaniciAd);
                Claim claim4 = new Claim(ClaimTypes.Role, role[0]);

                //claims.Add(claim1);
                //claims.Add(claim2);
                //claims.Add(claim3);
                claims.Add(claim4);

                //token'ı oluşturmaya başlayalım....
                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer, audience, claims, expires: sonlanmaTarihi, signingCredentials: signingCredentials);

                string token = _jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

                return Ok(token);

            }

            return NotFound("Kullanıcı adı veya şifre hatalıdır");

        }


    }
}
