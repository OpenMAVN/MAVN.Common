namespace MAVN.Common.Encryption
{
    /// <summary>
    /// Used to encrypt entities or single properties
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Decrypts an entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">The entity object</param>
        /// <returns>Decrypted entity</returns>
        T Decrypt<T>(T entity) where T : class;

        /// <summary>
        /// Encrypts an entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">The entity object</param>
        /// <returns>Encrypted entity</returns>
        T Encrypt<T>(T entity) where T : class;

        /// <summary>
        /// Decrypts a value
        /// </summary>
        /// <param name="value">Value to decrypt</param>
        /// <returns>Decrypted value</returns>
        string DecryptValue(string value);

        /// <summary>
        /// Encrypts a value
        /// </summary>
        /// <param name="value">Value to encrypt</param>
        /// <returns>Encrypted value</returns>
        string EncryptValue(string value);

        /// <summary>
        /// Checks if a value was encrypted.
        /// </summary>
        /// <param name="value">Value to check for encryption</param>
        /// <returns>True if the value was encrypted, false otherwise</returns>
        bool IsEncrypted(string value);
    }
}
