using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cashflow.Api.Utils;

namespace Cashflow.Tests
{
    [TestClass]
    public class UtilsTest
    {
        private string AES_128_BITS_KEY = "1234567890123456";

        [TestMethod]
        [TestCategory("CryptographyTest")]
        public void PasswordHash()
        {
            string password = "12345678";
            string passwordHash = CryptographyUtils.PasswordHash(password);
            var valid = CryptographyUtils.PasswordHashVarify(password, passwordHash);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        [TestCategory("CryptographyTest")]
        public void AesEncrypt()
        {
            string expected = "Szbv+xg3Z3TEXZYj0FBLWg==";
            string text = "123123123123";
            string encryptedBase64 = CryptographyUtils.AesEncrypt(text, AES_128_BITS_KEY);

            // Gerar o mesmo resultado com openssl:
            // echo -n "123123123123" | \
            // openssl enc -aes-128-cbc -K $(echo -n "1234567890123456" | xxd -p -c 256) \
            // -iv $(echo -n "1234567890123456" | xxd -p -c 256) -base64

            Assert.AreEqual(expected, encryptedBase64);
        }

        [TestMethod]
        [TestCategory("CryptographyTest")]
        public void AesDecrypt()
        {
            string encryptedBase64 = "Szbv+xg3Z3TEXZYj0FBLWg==";
            string expected = "123123123123";
            string decryptedText = CryptographyUtils.AesDecrypt(encryptedBase64, AES_128_BITS_KEY);

            Assert.AreEqual(expected, decryptedText);
        }
    }
}