using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration, UserContext userContext, SignInManager<UserEntity> signInManager) : ControllerBase
    {
        private readonly SignInManager<UserEntity> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly UserContext _userContext = userContext;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _userContext.Users.AnyAsync(x => x.Email == model.Email))
                {
                    var userEntity = new UserEntity
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PasswordHash = model.Password,
                    };

                    _userContext.Users.Add(userEntity);
                    await _userContext.SaveChangesAsync();
                    return Created();
                }
                return Conflict();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(SignInModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userEntity = await _userContext.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                    if (userEntity != null)
                    {

                        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                        if (result.Succeeded)
                        {
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Sercret"]!);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                    new(ClaimTypes.Email, model.Email),
                                    new(ClaimTypes.Name, model.Email),
                                    new(ClaimTypes.Role, "User")
                                }),
                                Expires = DateTime.UtcNow.AddMinutes(60),
                                Issuer = _configuration["Jwt:Issuer"],
                                Audience = _configuration["Jwt:Audience"],
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };

                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            var tokenString = tokenHandler.WriteToken(token);

                            return Ok(new
                            {
                                Token = tokenString
                            });
                        }
                    }
                    return BadRequest();
                }
                return Unauthorized();
            }
            catch (Exception ex) { return  BadRequest(ex.Message); }
            
        }

        [HttpPost]
        [Route("token")]
        public IActionResult Token(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                                    new(ClaimTypes.Email, model.Email),
                                    new(ClaimTypes.Name, model.Email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    Token = tokenString
                });
            }
            return Unauthorized();
        }
    }
}
