using System.Security.Cryptography;

namespace HS.Zf.Api.Common
{
    public static class Cryptography
    {
        public static byte[] Hash(this byte[] data, byte[] salt, HashAlgorithm algorithm)
        {
            var buffer = new byte[data.Length + salt.Length];

            salt.CopyTo(buffer, 0);
            data.CopyTo(buffer, salt.Length);

            return algorithm.ComputeHash(buffer);
        }
    }
}