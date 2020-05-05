using System;
using System.Text;
using Xunit;

namespace MAVN.Common.Tests
{
    public class Sha256HashingUtilTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("asd", null)]
        public void When_CallSha256HashWithNullParameters_Expect_ArgumentNullException(string input, Encoding encoding)
        {
            Assert.Throws<ArgumentNullException>(() => new Sha256HashingUtil().Sha256Hash(input, encoding));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void When_CallSha256HashEncoding1251WithNullParameters_Expect_ArgumentNullException(string input)
        {
            Assert.Throws<ArgumentNullException>(() => new Sha256HashingUtil().Sha256HashEncoding1252(input));
        }

        [Fact]
        public void When_CallSha256HashEncoding1251WithNonNullParameters_Expect_HashedInput()
        {
            var hash = "2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824";
            var input = "hello";

            var hashedInput = new Sha256HashingUtil().Sha256HashEncoding1252(input);

            Assert.Equal(hash, hashedInput);
        }

        [Fact]
        public void When_CallSha256HashAndPassEncoding1252_Expect_HashedInput()
        {
            var hash = "2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824";
            var input = "hello";

            var hashedInput = new Sha256HashingUtil().Sha256Hash(input, Encoding.GetEncoding(1252));

            Assert.Equal(hash, hashedInput);
        }
    }
}