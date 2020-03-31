using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Service.Jwt
{
    public class JwtSettings
    {
        public string Issure { get; set; }
        public string SecretKey { get; set; }
        public string EncryptKey { get; set; }
        public string Audience { get; set; }
        public int NotBeforMinute { get; set; }
        public int ExpirationMinute { get; set; }
    }
}
