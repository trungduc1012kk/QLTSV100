
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MISA.HCSN2.Common.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MISA.HCSN2.API.NTier.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthsController : ControllerBase
    {

        public static WebApplicationBuilder? builder;

        public static IConfiguration configuration;


        private List<User> users = new()
        {
            new User("trunggermany", "anhduc00"),
            new User("trunggermany1", "anhduc001"),
            new User("trunggermany2", "anhduc002"),
            new User("string", "string"),
        };

        /// <summary>
        /// đăng nhập hệ thống
        /// </summary>
        /// <param name="user">Thông tin đăng nhập</param>
        /// <returns>token </returns>
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] User user)
        {
            var check = users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

            if (check == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Đăng nhập thất bại!");
            }

            var key = Encoding.ASCII.GetBytes
            (configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return StatusCode(StatusCodes.Status200OK, stringToken);

        }

        /// <summary>
        /// check đăng nhập
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            return Ok("check thành công!");
        }

        /// <summary>
        /// đăng xuất hệ thống
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("sign-out")]
        public  IActionResult SignOutAsync()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return StatusCode(StatusCodes.Status200OK, "đăng xuất thành công!");
        }
    }
}
