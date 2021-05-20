using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.Api.Model;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
      
        public AuthController(
                              SignInManager<IdentityUser> signInManager, 
                              UserManager<IdentityUser> userManager, 
                              IOptions<AppSettings> appSettings
                              ) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }
        
        [HttpPost("token")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
          
            return Ok(await GerarJwt(loginUser.Email));

            //var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
                       
            //if (result.Succeeded)
            //{
            //    _logger.LogInformation("Usuario "+ loginUser.Email +" logado com sucesso");
            //    return CustomResponse(await GerarJwt(loginUser.Email));
            //}
            //if (result.IsLockedOut)
            //{
            //    NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
            //    return CustomResponse(loginUser);
            //}

            //NotificarErro("Usuário ou Senha incorretos");
            //return CustomResponse(loginUser);
        }

        private async Task<LoginResponseViewModel> GerarJwt(string email)
        {
            var user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Luiz Fernando",
                Email = email
            };

            //var user = new UserManager<IdentityUser>(iu);// await _userManager.FindByEmailAsync(email);
            //var claims = await _userManager.GetClaimsAsync(user);
            //var userRoles = await _userManager.GetRolesAsync(user);

            //claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            //foreach (var userRole in userRoles)
            //{
            //    claims.Add(new Claim("role", userRole));
            //}

            //var identityClaims = new ClaimsIdentity();
            //identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                //Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                //UserToken = new UserTokenViewModel
                //{
                //    Id = user.Id,
                //    Email = user.Email,
                //    //Claims = claims.Select(c=> new ClaimViewModel{ Type = c.Type, Value = c.Value})
                //}
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}