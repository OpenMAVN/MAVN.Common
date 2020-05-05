using System;
using MAVN.Common.Encryption;
using Moq;
using Xunit;

namespace MAVN.Common.Tests
{
    public class EncryptionServiceTests
    {
        class TestEntity
        {
            [EncryptedProperty]
            public string EncryptedValue { get; set; }

            public string NotEncryptedValue { get; set; }
        }

        [Fact]
        public void TryToEncryptObject_EverythingValid_PropertiesWithAttributeAreEncrypted()
        {
            var serializer = new Mock<IAesSerializer>();
            serializer.Setup(x => x.Serialize(It.IsAny<string>())).Returns("encrypted");
            serializer.Setup(x => x.HasKey).Returns(true);


            var encryptionService = new EncryptionService(serializer.Object);

            var expected = new TestEntity
            {
                EncryptedValue = "encrypted",
                NotEncryptedValue = "not encrypted"
            };

            var actual = new TestEntity
            {
                EncryptedValue = "not encrypted",
                NotEncryptedValue = "not encrypted"
            };

            actual = encryptionService.Encrypt(actual);

            Assert.Equal(expected.NotEncryptedValue, actual.NotEncryptedValue);
            Assert.Equal(expected.EncryptedValue, actual.EncryptedValue);
        }

        [Fact]
        public void TryToEncryptObject_PropertiesAlreadyEncrypted_TheEncryptedPropertiesAreSkipped()
        {
            var serializer = new Mock<IAesSerializer>();
            serializer.SetupSequence(x => x.Serialize(It.IsAny<string>()))
                .Returns("encrypted")
                .Returns("encrypted again");

            serializer.Setup(x => x.HasKey).Returns(true);
            serializer.Setup(x => x.IsEncrypted("encrypted")).Returns(true);

            var encryptionService = new EncryptionService(serializer.Object);

            var beforeEncryption = new TestEntity
            {
                EncryptedValue = "not encrypted",
                NotEncryptedValue = "not encrypted"
            };

            var expected = new TestEntity
            {
                EncryptedValue = "encrypted",
                NotEncryptedValue = "not encrypted"
            };


            var actual = encryptionService.Encrypt(beforeEncryption);
            actual = encryptionService.Encrypt(actual);

            Assert.Equal(expected.NotEncryptedValue, actual.NotEncryptedValue);
            Assert.Equal(expected.EncryptedValue, actual.EncryptedValue);
        }

        [Fact]
        public void TryToEncryptObject_ObjectIsNull_ArgumentNullExceptionIsThrown()
        {
            var serializer = Mock.Of<IAesSerializer>();

            var encryptionService = new EncryptionService(serializer);
            TestEntity nullEntity = null;

            Assert.Throws<ArgumentNullException>(() => encryptionService.Encrypt(nullEntity));
        }

        [Fact]
        public void TryToDecryptObject_ObjectIsNull_ArgumentNullExceptionIsThrown()
        {
            var serializer = Mock.Of<IAesSerializer>();

            var encryptionService = new EncryptionService(serializer);
            TestEntity nullEntity = null;

            Assert.Throws<ArgumentNullException>(() => encryptionService.Decrypt(nullEntity));
        }

        [Fact]
        public void TryToEncryptObject_EncryptionKeyIsNotSet_ArgumentExceptionIsThrown()
        {
            var serializer = Mock.Of<IAesSerializer>();

            var encryptionService = new EncryptionService(serializer);
            TestEntity entity = new TestEntity();

            Assert.Throws<ArgumentException>(() => encryptionService.Encrypt(entity));
        }

        [Fact]
        public void TryToDecryptObject_EncryptionKeyIsNotSet_ArgumentExceptionIsThrown()
        {
            var serializer = Mock.Of<IAesSerializer>();

            var encryptionService = new EncryptionService(serializer);
            TestEntity entity = new TestEntity();

            Assert.Throws<ArgumentException>(() => encryptionService.Decrypt(entity));
        }

        [Fact]
        public void TryToDecryptObject_EverythingValid_PropertiesWithAttributeAreDecrypted()
        {
            var serializer = new Mock<IAesSerializer>();
            serializer.Setup(x => x.Deserialize(It.IsAny<string>())).Returns("decrypted");
            serializer.Setup(x => x.HasKey).Returns(true);
            serializer.Setup(x => x.IsEncrypted("encrypted")).Returns(true);

            var encryptionService = new EncryptionService(serializer.Object);

            var expected = new TestEntity
            {
                EncryptedValue = "decrypted",
                NotEncryptedValue = "not encrypted"
            };

            var actual = new TestEntity
            {
                EncryptedValue = "encrypted",
                NotEncryptedValue = "not encrypted"
            };

            actual = encryptionService.Decrypt(actual);

            Assert.Equal(expected.NotEncryptedValue, actual.NotEncryptedValue);
            Assert.Equal(expected.EncryptedValue, actual.EncryptedValue);
        }
    }
}
