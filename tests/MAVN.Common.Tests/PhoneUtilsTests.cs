using System.Linq;
using Common;
using Xunit;

namespace MAVN.Common.Tests
{
    public class PhoneUtilsTests
    {
        [Fact]
        public void When_PhoneNumberPassed_Expect_FormatedPhoneNumber()
        {
            //Arange
            var phoneNumber = "00459 886 660 177";

            //Act
            var formatedPhoneNumber = PhoneUtils.PreparePhoneNumber(phoneNumber);

            //Assert
            var expectedPhoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());
            expectedPhoneNumber = expectedPhoneNumber.TrimStart('0');
            expectedPhoneNumber = $"+{expectedPhoneNumber}";

            Assert.Equal(expectedPhoneNumber, formatedPhoneNumber);
        }

        [Fact]
        public void When_PhoneAndIsoCodePassed_Expect_E164FormattedNumber()
        {
            //Arange
            var shortPhoneNumber = "886 660 177";
            var isoCode = "459";

            //Act
            var formatedPhoneNumber = PhoneUtils.GetE164FormattedNumber(shortPhoneNumber, isoCode);

            //Assert
            var fullPhoneNumber = $"{isoCode} {shortPhoneNumber}";

            var preparedNumber = new string(fullPhoneNumber.Where(char.IsDigit).ToArray());
            preparedNumber = preparedNumber.TrimStart('0');
            preparedNumber = $"+{preparedNumber}";

            var e164FormattedNumber = preparedNumber.ToE164Number();
            Assert.Equal(e164FormattedNumber, formatedPhoneNumber);
        }

        [Fact]
        public void When_FullPhonePassed_Expect_E164FormattedNumber()
        {
            //Arange
            var fullPhoneNumber = "459 886 660 177";

            //Act
            var formatedPhoneNumber = PhoneUtils.GetE164FormattedNumber(fullPhoneNumber);

            //Assert
            var preparedNumber = new string(fullPhoneNumber.Where(char.IsDigit).ToArray());
            preparedNumber = preparedNumber.TrimStart('0');
            preparedNumber = $"+{preparedNumber}";

            var e164FormattedNumber = preparedNumber.ToE164Number();
            Assert.Equal(e164FormattedNumber, formatedPhoneNumber);
        }

        [Fact]
        public void When_FullPhonePassedWith0s_Expect_E164FormattedNumber()
        {
            //Arange
            var fullPhoneNumber = "00459 886 660 177";

            //Act
            var formatedPhoneNumber = PhoneUtils.GetE164FormattedNumber(fullPhoneNumber);

            //Assert
            var preparedNumber = new string(fullPhoneNumber.Where(char.IsDigit).ToArray());
            preparedNumber = preparedNumber.TrimStart('0');
            preparedNumber = $"+{preparedNumber}";

            var e164FormattedNumber = preparedNumber.ToE164Number();
            Assert.Equal(e164FormattedNumber, formatedPhoneNumber);
        }

        [Fact]
        public void When_FullPhonePassedWithPlus_Expect_E164FormattedNumber()
        {
            //Arange
            var fullPhoneNumber = "+459 886 660 177";

            //Act
            var formatedPhoneNumber = PhoneUtils.GetE164FormattedNumber(fullPhoneNumber);

            //Assert
            var preparedNumber = new string(fullPhoneNumber.Where(char.IsDigit).ToArray());
            preparedNumber = preparedNumber.TrimStart('0');
            preparedNumber = $"+{preparedNumber}";

            var e164FormattedNumber = preparedNumber.ToE164Number();
            Assert.Equal(e164FormattedNumber, formatedPhoneNumber);
        }

        [Fact]
        public void When_PhoneWithLetters_Expect_E164FormatOfOnlyNumbers()
        {
            //Arange
            var numberWithLetters = "+459test 886 660 177";

            //Act
            var formatedPhoneNumber = PhoneUtils.GetE164FormattedNumber(numberWithLetters);

            //Assert
            var preparedNumber = new string(numberWithLetters.Where(char.IsDigit).ToArray());
            preparedNumber = preparedNumber.TrimStart('0');
            preparedNumber = $"+{preparedNumber}";

            var e164FormattedNumber = preparedNumber.ToE164Number();
            Assert.Equal(e164FormattedNumber, formatedPhoneNumber);
        }

        [Fact]
        public void When_PhoneWithExtraSpaces_Expect_E164FormatOfOnlyNumbers()
        {
            //Arange
            var extraSpacesNumber = "  +45  9     8  8    6     6 6  0      1 77";

            //Act
            var formatedPhoneNumber = PhoneUtils.GetE164FormattedNumber(extraSpacesNumber);

            //Assert
            var preparedNumber = new string(extraSpacesNumber.Where(char.IsDigit).ToArray());
            preparedNumber = preparedNumber.TrimStart('0');
            preparedNumber = $"+{preparedNumber}";

            var e164FormattedNumber = preparedNumber.ToE164Number();
            Assert.Equal(e164FormattedNumber, formatedPhoneNumber);
        }

        [Fact]
        public void When_Only0sPassed_Expect_PlusSign()
        {
            //Arange
            var zeroes = "0000";

            //Act
            var formatedPhoneNumber = PhoneUtils.GetE164FormattedNumber(zeroes);

            //Assert
            var preparedNumber = new string(zeroes.Where(char.IsDigit).ToArray());
            preparedNumber = preparedNumber.TrimStart('0');
            preparedNumber = $"+{preparedNumber}";

            var e164FormattedNumber = preparedNumber.ToE164Number();
            Assert.Equal(e164FormattedNumber, formatedPhoneNumber);
        }

        [Fact]
        public void When_EmptyStringPassed_Expect_PlusSign()
        {
            //Arange
            var emptyNumber = " ";

            //Act
            var formatedPhoneNumber = PhoneUtils.GetE164FormattedNumber(emptyNumber);

            //Assert
            var preparedNumber = new string(emptyNumber.Where(char.IsDigit).ToArray());
            preparedNumber = preparedNumber.TrimStart('0');
            preparedNumber = $"+{preparedNumber}";

            var e164FormattedNumber = preparedNumber.ToE164Number();
            Assert.Equal(e164FormattedNumber, formatedPhoneNumber);
        }
    }
}
