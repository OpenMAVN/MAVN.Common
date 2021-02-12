using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MAVN.Common.Encryption
{
    /// <summary>
    /// IEncryptionService implementation. Used to encrypt entities or single properties
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        private readonly IAesSerializer _serializer;
        private readonly ConcurrentDictionary<Type, List<PropertyInfo>> _encryptedPropertiesCache;

        public EncryptionService(IAesSerializer serializer)
        {
            _serializer = serializer;
            _encryptedPropertiesCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        }

        /// <summary>
        /// Decrypts an entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">The entity object</param>
        /// <returns>Decrypted entity</returns>
        public T Decrypt<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!_serializer.HasKey)
            {
                throw new ArgumentException("Encryption key is not configured.");
            }

            var encryptedProperties = GetEncryptedProperties(typeof(T));

            foreach (var property in encryptedProperties)
            {
                var encrypted = property.GetValue(entity) as string;

                if (string.IsNullOrEmpty(encrypted) || !_serializer.IsEncrypted(encrypted))
                {
                    continue;
                }

                var value = _serializer.Deserialize(encrypted);
                property.SetValue(entity, value);
            }

            return entity;
        }

        /// <summary>
        /// Encrypts an entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">The entity object</param>
        /// <returns>Encrypted entity</returns>
        public T Encrypt<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!_serializer.HasKey)
            {
                throw new ArgumentException("Encryption key is not configured.");
            }

            var encryptedProperties = GetEncryptedProperties(typeof(T));

            foreach (var property in encryptedProperties)
            {
                var value = property.GetValue(entity) as string;

                if (string.IsNullOrEmpty(value) || _serializer.IsEncrypted(value))
                {
                    continue;
                }

                var encrypted = _serializer.Serialize(value);
                property.SetValue(entity, encrypted);
            }

            return entity;
        }

        /// <summary>
        /// Decrypts a value
        /// </summary>
        /// <param name="value">Value to decrypt</param>
        /// <returns>Decrypted value</returns>
        public string DecryptValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!_serializer.HasKey)
            {
                throw new ArgumentException("Encryption key is not configured.");
            }

            if (!_serializer.IsEncrypted(value))
            {
                return value;
            }

            return _serializer.Deserialize(value);
        }

        /// <summary>
        /// Encrypts a value
        /// </summary>
        /// <param name="value">Value to encrypt</param>
        /// <returns>Encrypted value</returns>
        public string EncryptValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!_serializer.HasKey)
            {
                throw new ArgumentException("Encryption key is not configured.");
            }

            if (_serializer.IsEncrypted(value))
            {
                return value;
            }

            return _serializer.Serialize(value);
        }

        /// <inheritdoc cref="IEncryptionService">
        public bool IsEncrypted(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return _serializer.IsEncrypted(value);
        }

        /// <summary>
        /// Gets a list of encrypted properties for a type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>List of properties</returns>
        private List<PropertyInfo> GetEncryptedProperties(Type type)
        {
            if (_encryptedPropertiesCache.TryGetValue(type, out var properties))
            {
                return properties;
            }

            var encryptAttribute = typeof(EncryptedPropertyAttribute);
            var encryptedProperties = type.GetProperties().Where(x => Attribute.IsDefined((MemberInfo) x, encryptAttribute)).ToList();

            _encryptedPropertiesCache.TryAdd(type, encryptedProperties);

            return encryptedProperties;
        }
    }
}
