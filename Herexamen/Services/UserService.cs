using Herexamen.Helpers;
using Herexamen.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Herexamen.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings; 
        private readonly DatabaseContext _databaseContext; 
        public UserService(IOptions<AppSettings> appSettings, DatabaseContext databaseContext) 
        { 
            _appSettings = appSettings.Value; 
            _databaseContext = databaseContext; 
        }
        public User Authenticate(string username, string password)
        {
            var user = _databaseContext.Users.SingleOrDefault(x => x.UserName == username && x.Password == password);
            // return null if user not found
            if(user == null)
                return null;
            // authentication successful so generate jwttoken
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret); 
            var tokenDescriptor = new SecurityTokenDescriptor 
            { 
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID", user.UserID.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("UserName", user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)};
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            // remove password before returning
            user.Password = null;
            return user;
        }
        }
}
