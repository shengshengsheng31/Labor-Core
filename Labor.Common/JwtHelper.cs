using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Labor.Common
{
    public class JwtHelper
    {
        private static IConfiguration _configuration;
        /// <summary>
        /// 在startup中读取配置信息
        /// </summary>
        /// <param name="configuration"></param>
        public static void GetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Jwt加密
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string JwtEncrypt(TokenModelJwt tokenModel)
        {
            //获取配置信息
            var issuer = _configuration["JwtToken:issuer"];
            var audience = _configuration["JwtToken:audience"];
            var secret = _configuration["JwtToken:secret"];

            //claim信息
            var claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Iat,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),//令牌签发时间
                //new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),//不早于的时间
                //new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddHours(24)).ToUnixTimeSeconds()}"),//令牌过期时间
                //new Claim(ClaimTypes.Expiration,DateTime.Now.AddHours(24).ToString(CultureInfo.CurrentCulture)),//令牌截止时间
                //new Claim(JwtRegisteredClaimNames.Iss,issuer),//发行人
                //new Claim(JwtRegisteredClaimNames.Aud,audience),//订阅人
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.UserId.ToString()),//Jwt唯一标识
                new Claim("UserRole",tokenModel.Level),//权限
                new Claim("UserName",tokenModel.Account),//用户名
            };

            //秘钥处理
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //封装Jwt对象
            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: cred);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// Jwt解密
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenModelJwt JwtDecrypt(string tokenStr)
        {
            if (string.IsNullOrEmpty(tokenStr) || string.IsNullOrWhiteSpace(tokenStr))
            {
                return new TokenModelJwt();
            }
            if (tokenStr.Substring(0,6).ToLower()=="bearer")
            {
                tokenStr = tokenStr.Substring(7);//截取前面的bearer和空格
            }
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = jwtHandler.ReadJwtToken(tokenStr);
            token.Payload.TryGetValue(ClaimTypes.Role, out object level);
            var model = new TokenModelJwt
            {
                UserId = Guid.Parse(token.Id),
                Level = level == null ? "" : level.ToString()
            };
            return model;
        }
    }

    public class TokenModelJwt
    {
        public Guid UserId { get; set; }

        public string Account { get; set; }

        public string Level { get; set; }
    }
}
