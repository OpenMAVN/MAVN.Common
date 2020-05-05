using System;
using System.IO;
using System.Security.Cryptography;

namespace MAVN.Common.Encryption
{
    /// <summary>
    /// Implementation of IAesSerializer. Used to serialize data
    /// </summary>
    public class AesSerializer : IAesSerializer
    {
        private const string Prefix = "Enc|\n";
        private byte[] _key;
        private byte[] _iv;

        /// <summary>
        /// Checks if serializer has key set
        /// </summary>
        public bool HasKey => _key != null;
        /// <summary>
        /// Checks if serializer iv key set
        /// </summary>
        public bool HasIV => _iv != null;

        public AesSerializer(string key, string iv)
        {
            if (key != null)
            {
                SetKey(Convert.FromBase64String(key));
            }

            if (iv != null)
            {
                SetIV(Convert.FromBase64String(iv));
            }
        }

        /// <summary>
        /// Sets the key to use
        /// </summary>
        /// <param name="key">The key</param>
        public void SetKey(byte[] key)
        {
            if (HasKey)
            {
                throw new InvalidOperationException("Key is already set.");
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length != 32)
            {
                throw new ArgumentException($"Incorrect key size {key.Length}. Expected: 32");
            }

            _key = key;
        }

        /// <summary>
        /// Sets the IV to use
        /// </summary>
        /// <param name="iv">The IV</param>
        public void SetIV(byte[] iv)
        {
            if (HasIV)
            {
                throw new InvalidOperationException("IV has already been set");
            }

            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            if (iv.Length != 16)
            {
                throw new ArgumentException($"Incorrect iv size {iv.Length}. Expected: 16");
            }

            _iv = iv;
        }

        /// <summary>
        /// Serializes a value
        /// </summary>
        /// <param name="value">Value to serialize</param>
        /// <returns>Serialized value</returns>
        public string Serialize(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Data cannot be empty", nameof(value));
            }

            var buf = Encrypt(value, _key, _iv);

            var bufText = Convert.ToBase64String(buf);

            return $"{Prefix}{bufText}";
        }

        /// <summary>
        /// Deserializes a value
        /// </summary>
        /// <param name="value">Value to deserialize</param>
        /// <returns>Deserialized value as string</returns>
        public string Deserialize(string value)
        {
            if (!IsEncrypted(value))
            {
                throw new ArgumentException("Data is not encrypted or not supported format", nameof(value));
            }

            var cipherText = Convert.FromBase64String(value.Substring(Prefix.Length));

            return Decrypt(cipherText, _key, _iv);
        }

        /// <summary>
        /// Checks if a value is encrypted
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>If value is encrypted</returns>
        public bool IsEncrypted(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Data cannot be empty", nameof(value));
            }

            return value.StartsWith(Prefix);
        }

        private static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            byte[] encrypted;

            using (var algo = Aes.Create())
            {
                algo.KeySize = 256;
                algo.Key = key;
                algo.IV = iv;

                using (var encryptor = algo.CreateEncryptor(algo.Key, algo.IV))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            encrypted = memoryStream.ToArray();
                        }
                    }
                }
            }

            return encrypted;
        }

        private static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length == 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (iv == null || iv.Length == 0)
                throw new ArgumentNullException(nameof(iv));

            string plaintext;

            using (var algo = Aes.Create())
            {
                algo.KeySize = 256;
                algo.Key = key;
                algo.IV = iv;

                using (var decryptor = algo.CreateDecryptor(algo.Key, algo.IV))
                {
                    using (var memoryStream = new MemoryStream(cipherText))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (var streamReader = new StreamReader(cryptoStream))
                            {
                                plaintext = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
