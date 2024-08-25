using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<IdentityResult> RegisterUser(UserForRegistrationsDto userForRegistrationsDto)
        {
            var user = mapper.Map<User>(userForRegistrationsDto);
            var result=await userManager.CreateAsync(user,userForRegistrationsDto.Password);
            if(result.Succeeded)
            {
                await userManager.AddToRolesAsync(user, userForRegistrationsDto.Roles);
               
            }
            return result;
        }
    }
}
