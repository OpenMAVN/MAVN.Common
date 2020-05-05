using System;
using System.Security.Cryptography;
using System.Text;

namespace MAVN.Common
{
    /// <summary>
    /// Sha256 Hashing utils 
    /// </summary>
    public class Sha256HashingUtil
    {
        static Sha256HashingUtil()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Hashes the input with SHA256 algorithm using encoding with codepage 1252
        /// </summary>
        /// <param name="input">The input you want to hash</param>
        /// <returns></returns>
        public string Sha256HashEncoding1252(string input)
        {
            return Sha256Hash(input, Encoding.GetEncoding(1252));
        }

        /// <summary>
        /// Hashes the input with SHA256 algorithm using the provided encoding
        /// </summary>
        /// <param name="input">The input you want to hash</param>
        /// <param name="encoding">Encoding which should be used</param>
        /// <returns></returns>
        public string Sha256Hash(string input, Encoding encoding)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var encodedInput = encoding.GetBytes(input);
            
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(encodedInput);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}