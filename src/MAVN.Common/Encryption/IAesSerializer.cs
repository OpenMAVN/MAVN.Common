namespace MAVN.Common.Encryption
{
    /// <summary>
    /// Used to serialize data
    /// </summary>
    public interface IAesSerializer
    {
        /// <summary>
        /// Serializes a value
        /// </summary>
        /// <param name="value">Value to serialize</param>
        /// <returns>Serialized value</returns>
        string Serialize(string value);

        /// <summary>
        /// Deserializes a value
        /// </summary>
        /// <param name="value">Value to deserialize</param>
        /// <returns>Deserialized value as string</returns>
        string Deserialize(string value);

        /// <summary>
        /// Checks if a value is encrypted
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>If value is encrypted</returns>
        bool IsEncrypted(string value);

        /// <summary>
        /// Sets the key to use
        /// </summary>
        /// <param name="key">The key</param>
        void SetKey(byte[] key);

        /// <summary>
        /// Sets the IV to use
        /// </summary>
        /// <param name="iv">The IV</param>
        void SetIV(byte[] key);

        /// <summary>
        /// Checks if serializer has key set
        /// </summary>
        bool HasKey { get; }

        /// <summary>
        /// Checks if serializer has iv set
        /// </summary>
        bool HasIV { get; }
    }
}
