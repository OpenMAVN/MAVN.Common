using System;

namespace MAVN.Common.Encryption
{
    /// <summary>
    /// Properties, marked with this attribute, will be stored encrypted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedPropertyAttribute : Attribute
    {
    }
}
