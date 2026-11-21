using System.Security.Cryptography;
using System.Text;

namespace Business.Helpers
{
    public static class SecurityHelper
    {
        public static string? CalculateSecretHash(string secretKey, params string[] dataStringComponents)
        {
            string? secretHash = null;

            try
            {
                string dataString = string.Join(string.Empty, dataStringComponents);
                byte[] data = Encoding.UTF8.GetBytes(dataString);
                byte[] key = Encoding.UTF8.GetBytes(secretKey);
                byte[] computed = ApplyHmacSha256(data, key);
                secretHash = Convert.ToBase64String(computed);
            }
            catch (Exception ex)
            {
                // TODO: log the exception here somehow
            }

            return secretHash;
        }

        private static byte[] ApplyHmacSha256(byte[] data, byte[] key)
        {
            using HMACSHA256 shaAlgorithm = new(key);
            var result = shaAlgorithm.ComputeHash(data);
            return result;
        }
    }
}
