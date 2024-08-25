using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationManager : IAuthenticationService
    {
        private readonly ILogerService logerService;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private User? _user;
        public AuthenticationManager(ILogerService logerService,
            IMapper mapper,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            this.logerService = logerService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            var signinCredentials = GetSiginCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }



        public async Task<IdentityResult> RegisterUser(UserForRegistrationsDto userForRegistrationsDto)
        {
            var user = mapper.Map<User>(userForRegistrationsDto);
            var result = await userManager.CreateAsync(user, userForRegistrationsDto.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(user, userForRegistrationsDto.Roles);

            }
            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto)
        {
            _user = await userManager.FindByNameAsync(userForAuthenticationDto.UserName);
            var result = (_user is not null && await userManager.CheckPasswordAsync(_user, userForAuthenticationDto.Password));
            if (!result)
            {
                logerService.LogWarning($"{nameof(ValidateUser)} : Authentication failed. Wrong username or password");
            }
            return result;
        }
        private SigningCredentials GetSiginCredentials()
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };
            var roles = await userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["validAudience"])),
                signingCredentials:signinCredentials
                );
               
            return tokenOptions;
        }
    }
}
