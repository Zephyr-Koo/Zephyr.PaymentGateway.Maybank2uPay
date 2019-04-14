using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Zephyr.PaymentGateway.Maybank2uPay.Core
{
    internal static class M2uCryptoService
    {
        private const int NoOfIteration = 2;
        private const string Salt = "Maybank2u simple encryption";

        /// <summary>
        ///     Generate secret key for encryption/decryption
        /// </summary>
        /// <returns>
        ///     16-bit/2 bytes key which similar to
        ///     new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }
        /// </returns>
        private static byte[] GenerateKey()
        {
            return Enumerable.Range(0, 16).Select(i => (byte)i).ToArray();
        }

        private readonly static SymmetricAlgorithm CryptoAlgorithm = new AesManaged()
        {
            Mode    = CipherMode.ECB,
            Padding = PaddingMode.PKCS7,
            Key     = GenerateKey()
        };

        private static CryptoStream GetEncryptCryptoStream(MemoryStream stream)
        {
            return new CryptoStream(
                        stream,
                        CryptoAlgorithm.CreateEncryptor(),
                        CryptoStreamMode.Write);
        }

        private static CryptoStream GetDecryptCryptoStream(MemoryStream stream)
        {
            return new CryptoStream(
                        stream,
                        CryptoAlgorithm.CreateDecryptor(),
                        CryptoStreamMode.Read);
        }

        /// <summary>
        ///     Encrypt plaintext provided as <c>input</c> iteratively with
        ///     salt and returns base64-encoded cipher
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Encrypt(string input, string salt = Salt)
        {
            var plainText    = string.Copy(input);
            var cipherBase64 = string.Empty;

            for (int n = 1; n <= NoOfIteration; ++n)
            {
                var msEncrypt = new MemoryStream();
                var csEncrypt = GetEncryptCryptoStream(msEncrypt);

                using (var writer = new StreamWriter(csEncrypt))
                {
                    writer.Write($"{ salt }{ plainText }");
                }

                cipherBase64 = msEncrypt.ToArray().ToBase64WithLineBreak();
                plainText    = string.Copy(cipherBase64);
            }

            return cipherBase64;
        }

        [Obsolete("Reserved for reference purpose. (not needed for SDK)")]
        private static string Decrypt(string input, string salt = Salt)
        {
            var cipherBase64 = string.Copy(input);
            var plainText    = string.Empty;

            for (int n = 1; n <= NoOfIteration; ++n)
            {
                var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherBase64));
                var csDecrypt = GetDecryptCryptoStream(msDecrypt);

                using (var reader = new StreamReader(csDecrypt))
                {
                    plainText = reader.ReadToEnd();
                }

                plainText    = plainText.Substring(salt.Length);
                cipherBase64 = string.Copy(plainText);
            }

            return plainText;
        }
    }
}