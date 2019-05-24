using Jwt;
using Newtonsoft.Json;
using System;

namespace POSM.Core.Helpers
{
    public static class EncryptHelper
    {
        private const string SecretKey = "ThisKeyUsingForWeb";
        private const JwtHashAlgorithm JwtHashAlgorithm = Jwt.JwtHashAlgorithm.HS256;

        public static string Encrypt<T>(T item)
        {
            return JsonWebToken.Encode(item, SecretKey, JwtHashAlgorithm);
        }

        public static T Decrypt<T>(string str)
        {
            try
            {
                if (str.Contains(" "))
                    str = str.Split(' ')[1];
                var dataStr = JsonWebToken.Decode(str, SecretKey);
                return JsonConvert.DeserializeObject<T>(dataStr);
            }
            catch (SignatureVerificationException)
            {
                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
