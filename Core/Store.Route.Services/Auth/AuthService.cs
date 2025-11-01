using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.Route.Domain.Entities.Identity;
using Store.Route.Domain.Exceptions;
using Store.Route.Services.Abstractions.Auth;
using Store.Route.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Store.Route.Shared.JWT;
using Microsoft.Extensions.Options;

namespace Store.Route.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager,IOptions<JWTOptions> options) : IAuthService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User is null) throw new UnAuthorizedException();

            var Flag=await _userManager.CheckPasswordAsync(User, loginDto.Password);
            if (!Flag) throw new UnAuthorizedException();

            var Result = new UserResultDto()
            {

                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await GenerateToken(User)
            };
            return Result;
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var User = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName,
                

            };

            var Result=await _userManager.CreateAsync(User,registerDto.Password);

            if (!Result.Succeeded)
            {
                var Errors = Result.Errors.Select(e => e.Description);
                throw new ValidationException(Errors);
            }
            return new UserResultDto()
            {
                Email = User.Email,
                DisplayName = User.DisplayName,
                Token = await GenerateToken(User)
            };
        }


        private async Task<string> GenerateToken(AppUser user)
        {

            var AuthClaims = new List<Claim>()
            {

                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var Roles = await _userManager.GetRolesAsync(user);

            var JWTOptions = options.Value;

            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptions.SecretKey));
            foreach (var role in Roles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(
             issuer: JWTOptions.Issuer,
             audience: JWTOptions.Audience,
             claims: AuthClaims,
             expires: DateTime.UtcNow.AddDays(JWTOptions.DurationInDays),
             signingCredentials:new SigningCredentials(SecretKey,SecurityAlgorithms.HmacSha256Signature)
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
