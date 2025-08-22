using System.Security.Cryptography;
using System.Text;

namespace StreamableNet.Utils
{
    internal static class AwsSigV4Utils
    {
        public static string CalculateSignature(
    string secretKey,
    string date,
    string region,
    string service,
    string stringToSign)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes("AWS4" + secretKey);
            byte[] kDate = HmacSha256(date, kSecret);
            byte[] kRegion = HmacSha256(region, kDate);
            byte[] kService = HmacSha256(service, kRegion);
            byte[] kSigning = HmacSha256("aws4_request", kService);

            return BitConverter.ToString(HmacSha256(stringToSign, kSigning))
                              .Replace("-", "")
                              .ToLower();
        }

        private static byte[] HmacSha256(string data, byte[] key)
        {
            using var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public static string ComputeSha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
