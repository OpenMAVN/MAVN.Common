using System.Linq;
using Common;

namespace MAVN.Common
{
    public static class PhoneUtils
    {
        /// <summary>
        /// Prepares the phone number by removing any extra spacing and beginning 0's and prefixes a +
        /// </summary>
        /// <param name="phoneNumber">The unformatted phone number</param>
        /// <returns>Formatted phone number</returns>
        public static string PreparePhoneNumber(string phoneNumber)
        {
            var phone = new string(phoneNumber.Where(char.IsDigit).ToArray());

            phone = phone.TrimStart('0');

            phone = $"+{phone}";

            return phone;
        }

        /// <summary>
        /// Prepares a phone number formatter by E164 standards 
        /// </summary>
        /// <param name="shortPhoneNumber">Phone number without Iso code or 00/+</param>
        /// <param name="isoCode">Country's Iso code (phone code)</param>
        /// <returns>E164 formatted phone number</returns>
        public static string GetE164FormattedNumber(string shortPhoneNumber, string isoCode)
        {
            var fullPhoneNumber = $"{isoCode} {shortPhoneNumber}";

            return GetE164FormattedNumber(fullPhoneNumber);
        }

        /// <summary>
        /// Prepares a phone number formatter by E164 standards 
        /// </summary>
        /// <param name="fullPhoneNumber">Full phone number with Iso code</param>
        /// <returns>E164 formatted phone number</returns>
        public static string GetE164FormattedNumber(string fullPhoneNumber)
        {
            var preparedNumber = PreparePhoneNumber(fullPhoneNumber);
            var e164FormattedNumber = preparedNumber.ToE164Number();
            return e164FormattedNumber;
        }
    }
}
