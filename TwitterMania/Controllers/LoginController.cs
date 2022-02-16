using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterMania.DataAccess;
using TwitterMania.Model;

namespace TwitterMania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private List<UserModel> users = new List<UserModel>();
        static HttpClient client = new HttpClient();
        private ApplicationDbContext _context;
        public LoginController(IConfiguration config, ApplicationDbContext context) 
        {
            _config = config;
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(UserModel login) 
        {
            IActionResult response = Unauthorized();

            /*client.BaseAddress = new Uri("https://localhost:44323/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage getResponse = await client.GetAsync("api/twitter/user/3");
            if (getResponse.IsSuccessStatusCode)
            {
                users = await JsonSerializer.DeserializeAsync<List<UserModel>> (await getResponse.Content.ReadAsStreamAsync());
            }

            var user = users.SingleOrDefault(user1 => user1.Username == login.Username && user1.Password == login.Password);
            */
            users =  _context.User
                .OrderByDescending(t => t.ID)
                .Take(100).ToList();

            var user = users.SingleOrDefault(user1 => user1.Username == login.Username && user1.Password == login.Password);


            if (user != null) 
            {
                var tokenString = GenerateJWT(user);

                var responseUser = new ResponseUser 
                {
                    Username = user.Username,
                    Name = user.Name,
                    Surname = user.Surname,
                    UserType = user.UserType
                };

                response = Ok(
                    new
                    {
                        token = tokenString,
                        userDetails = responseUser
                    }
                    );
            }



            return response;
        }

        private object GenerateJWT(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("surname", user.Surname),
                new Claim("Name", user.Name),
                new Claim("Role", user.UserType),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credential

            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
