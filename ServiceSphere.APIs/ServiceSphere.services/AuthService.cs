using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Services.contract;
using ServiceSphere.repositery.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceSphereContext _context;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            //var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == user.Email);
            //var freelancer = await _context.Freelancers.FirstOrDefaultAsync(c => c.Email == user.Email);
            var AuthClaims = new List<Claim>()//claim>> properties of user like name,email,pass
          {

            //object of claim class
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.GivenName,user.DisplayName),
            new Claim(ClaimTypes.Email,user.Email),
            
          };
            //bzwed s3obet eltoken by adding roles
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            //key
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));//btreturn byte f h3ml encoding
            //object of token not token
            var token = new JwtSecurityToken(
                    //registered claim>>need some properties, i will put them in appsetting
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                    claims: AuthClaims,
                    signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)

                );
            //func that returns token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
