using AutoMapper;
using InterOP.Core.DTOs;
using InterOP.Core.Entities;
using InterOP.Core.InterfaceApplications;
using InterOP.Core.Structs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Auth.API.Controllers.v1
{
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration IPrvObConfig;
        private readonly ISecurityService IPrvSecurityService;
        private readonly IPasswordService IPrvPasswordService;
        private readonly IMapper IPrvMapper;

        public AuthController(IConfiguration pvConfig, ISecurityService pvSecurityService,
                                IMapper pvMapper, IPasswordService pvPasswordService)
        {
            IPrvObConfig = pvConfig;
            IPrvSecurityService = pvSecurityService;
            IPrvMapper = pvMapper;
            IPrvPasswordService = pvPasswordService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLogin login)
        {
            string vStrToken;
            string vFecExpirate;
            IActionResult vResponse;
            try
            {
                vResponse = Unauthorized();
                var vEntProveedor = await IPrvSecurityService.AuthenticateUserAsync(login);

                if (!vEntProveedor.IsSuccess)
                {
                    return NotFound(vEntProveedor.Result);
                }
                //Mapper
                var vProveedoresDto = IPrvMapper.Map<ProveedoresDto>(vEntProveedor.Result);

                //Validate User Password
                var isValid = IPrvPasswordService.PasswordCheck(vProveedoresDto.Pwd, login.p);
                (vStrToken, vFecExpirate) = GenerateJWT(vProveedoresDto);

                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    jwtToken = vStrToken,
                    passwordExpiration = vFecExpirate
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        // [Authorize]
        [HttpPost("TestLogin")]
        public string Post()
        {
            //var vIdentity = HttpContext.User.Identity as ClaimsIdentity;
            //IList<Claim> vLstClaim = vIdentity.Claims.ToList();
            string vUserName = "SIESA"; // vLstClaim[0].Value;
            return $"Welcome To: {vUserName}";
        }


        private (string vStrToken, string vFecExpirate) GenerateJWT(ProveedoresDto pvUserInfo)
        {

            string vStrToken;
            string vPasswordExpiration;
            DateTime vDateNow = DateTime.UtcNow;
            var vDtExpToken = vDateNow.AddMinutes(1440);
            long vDateGenToken = new DateTimeOffset(vDateNow).ToUnixTimeSeconds();
            long vDateExpureToken = new DateTimeOffset(vDtExpToken).ToUnixTimeSeconds();
            try
            {
                //Header
                SymmetricSecurityKey vStrSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IPrvObConfig["ServicesSecurity:Jwt:Key"]));
                SigningCredentials vCredentials = new SigningCredentials(vStrSecurityKey, SecurityAlgorithms.HmacSha256);
                JwtHeader vHeader = new JwtHeader(vCredentials);

                //Claims
                var vObListUser = new List<StrsUser>
                {
                    new StrsUser
                    {
                        username = pvUserInfo.Nit,
                        displayName = pvUserInfo.RazonSocial
                    }
                };
                var vObJsonUser = new
                {
                    user = vObListUser
                };

                Claim[] vClaims = {
                    new Claim(JwtRegisteredClaimNames.Iss,  IPrvObConfig["ServicesSecurity:Jwt:NitProveedor"]),
                    new Claim(JwtRegisteredClaimNames.Iat, vDateGenToken.ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, vDateExpureToken.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, pvUserInfo.Email),
                    new Claim("context", JsonConvert.SerializeObject(vObJsonUser)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                };

                //Payload
                JwtPayload vPayload = new JwtPayload(
                    issuer: IPrvObConfig["ServicesSecurity:Jwt:Issuer"],
                    audience: IPrvObConfig["ServicesSecurity:Jwt:Issuer"],
                    claims: vClaims,
                    notBefore: vDateNow,
                    expires: vDateNow.AddHours(24)
                    );

                //Generate Token
                JwtSecurityToken vSecToken = new JwtSecurityToken(vHeader, vPayload);
                vStrToken = new JwtSecurityTokenHandler().WriteToken(vSecToken);


                vPasswordExpiration = vDtExpToken.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return (vStrToken, vPasswordExpiration);
        }


    }
}
