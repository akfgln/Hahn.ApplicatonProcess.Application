using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace Hahn.ApplicatonProcess.February2021.Domain.Common
{
    public class TokenAuthOption
    {
        public static string Audience { get; } = "TokenAuthOptionAudience";
        public static string Issuer { get; } = "TokenAuthOptionAudience";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(30);
        public static string TokenType { get; } = "Bearer";
    }
    public class RSAKeyHelper
    {
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }
    }
}
