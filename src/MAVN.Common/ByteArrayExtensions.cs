using System;
using System.Security.Cryptography;

namespace MAVN.Common
{
    /// <summary>
    /// Extensions for byte array
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Hashes the byte array with SHA256 algorithm
        /// </summary>
        /// <param name="src">The byte array you want to hash</param>
        /// <returns></returns>
        public static byte[] ComputeSha256Hash(this byte[] src)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(src);
            }
        }
    }
}