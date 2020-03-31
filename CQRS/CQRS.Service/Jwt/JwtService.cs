using CQRS.Data.Configurtion.Contracts;
using CQRS.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Service.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IDateTime _dateTime;

        public JwtService(IOptions<JwtSettings> options, IDateTime dateTime)
        {
            _jwtSettings = options.Value;
            _dateTime = dateTime;
        }
        public AccessTokenDto CreateAccessToken(Users user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var signInCredential = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionKey = Encoding.UTF8.GetBytes(_jwtSettings.EncryptKey);
            var encryptionCredential = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
            var descriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtSettings.Audience,
                EncryptingCredentials = encryptionCredential,
                Expires = _dateTime.Now.AddMinutes(_jwtSettings.ExpirationMinute),
                IssuedAt = _dateTime.Now,
                Issuer = _jwtSettings.Issure,
                NotBefore = _dateTime.Now.AddMinutes(_jwtSettings.NotBeforMinute),
                SigningCredentials = signInCredential,
                Subject = new ClaimsIdentity(GetClaims(user.Id))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
            return new AccessTokenDto
            {
                AccessToken = tokenHandler.WriteToken(securityToken),
                TokenType = "Bearer",
                ExpiresIn = (int)(securityToken.ValidTo - _dateTime.UtcNow).TotalSeconds
            };
        }

        private IEnumerable<Claim> GetClaims(int userId)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
        }
    }
}
