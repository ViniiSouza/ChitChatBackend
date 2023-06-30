using System.Security.Cryptography;
using System.Text;

namespace Chat.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Transforms the <paramref name="str"/> into a SHA256 hash
        /// </summary>
        /// <param name="str"></param>
        public static string ToShaHash(this string str)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(str));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
