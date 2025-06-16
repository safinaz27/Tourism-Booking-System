using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TourismApi.Data.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<User> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            this.config = config;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto UserDto)
        {
            if (await _userManager.FindByEmailAsync(UserDto.Email) != null)
            {
                return BadRequest(new { Message = "Email is alerdy in use" });
            }
            else
            {
                User user = new User();
                user.UserName = UserDto.UserName;
                user.FullName = UserDto.FullName;
                user.Email = UserDto.Email;
                user.PhoneNumber = UserDto.PhoneNumber;
                user.UserRole = UserDto.UserRole;
                if (ModelState.IsValid)
                {
                    IdentityResult result = await _userManager.CreateAsync(user, UserDto.Password);
                    if (result.Succeeded)
                    {
                        string role = UserDto.UserRole.ToString();
                        await _userManager.AddToRoleAsync(user, role);
                        // claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        claims.Add(new Claim(ClaimTypes.Role, role));
                        //get role 
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var item in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, item));
                        }
                        //signingCredentials
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
                        SigningCredentials signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                        //create Token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:Issuer"], //URL web api
                            audience: config["JWT:Audience"], //URL for consumer
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signingCred
                            );
                        var StringToken = new JwtSecurityTokenHandler().WriteToken(mytoken);
                        Response.Cookies.Append("AuthToken", StringToken, new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.Now.AddHours(1)
                        });
                        return Ok(new
                        {

                            Message = "SignUP succesfull"
                        });
                    }
                    return BadRequest(result.Errors.FirstOrDefault());
                }

                else
                {
                    return BadRequest(ModelState);
                }
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto UserDto)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(UserDto.UserName);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, UserDto.Password);
                    if (found)
                    {
                        //create JWT

                        // claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        //get role 
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var item in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, item));
                        }
                        //signingCredentials
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
                        SigningCredentials signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                        //create Token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:Issuer"], //URL web api
                            audience: config["JWT:Audience"], //URL for consumer
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signingCred
                            );
                        var StringToken = new JwtSecurityTokenHandler().WriteToken(mytoken);
                        Response.Cookies.Append("AuthToken", StringToken, new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.Now.AddHours(1)
                        });
                        return Ok(new
                        {

                            Message = "Login succesfull"
                        });
                    }

                }
                return Unauthorized();
            }
            return Unauthorized();
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return Ok(new { Message = "Logout successful" });
        }



    }
}
