using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Reflection;
using System.Web;

namespace Zephyr.PaymentGateway.Maybank2uPay.Core.Test
{
    /// <summary>
    ///     Verify decrypted string against result from Maybank2u Pay
    ///     UAT environment https://m2upayuat.maybank2u.com.my/testM2uPayment
    /// </summary>
    [TestClass]
    public class PrivateTestLab
    {
        private const string TargetNamespace = "Zephyr.PaymentGateway.Maybank2uPay.Core";

        private readonly MethodInfo DecryptMethod;

        public PrivateTestLab()
        {
            DecryptMethod = Assembly
                            .Load(TargetNamespace)
                            .GetType($"{ TargetNamespace }.M2uCryptoService")
                            .GetMethod("Decrypt", BindingFlags.Static | BindingFlags.NonPublic);
        }

        [TestMethod]
        public void Ensure_DecryptedString_Match_Case_Payee_Code_Only()
        {
            GetDecryptResult("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdi8YnU1x5D0hO4R1%2Fh%2FoJgQrRhmfVLGchMAEkpKFulZBIyGJ%2FjCi7r%2FqzWYwERw2UE05%0D%0AI%2F00uFyD8RZ7fMFf%2FRQ%3D")
                .ShouldBe("Login$Q0S$1$null$1$null$1$null$null");
        }

        [TestMethod]
        public void Ensure_DecryptedString_Match_Case_Complete_Payload()
        {
            GetDecryptResult("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdgHxhD3528ixuBhOsoMqDblGsKdTauSYl%2B%2FQ6KxVUc%2FwnQDGr%2FTmq87u8RhEmPrqtHw0%0D%0AOl906ExItoez%2FFFEr0U%3D")
                .ShouldBe("Login$Q0S$1$666$1$RFN$1$ACN$null");
        }

        [TestMethod]
        public void Ensure_DecryptedString_Match_Case_RefNo_IsNull_And_AccountNo_IsNotNull()
        {
            GetDecryptResult("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdq%2FQDOd3y9iqyNiIqZVbpIzYH5eN5ncEvZE4IQBX4HRKXNochCASO09TRHoKCPKZEzaG%0D%0AQTUG8J4XRE2HX3RhpXk%3D")
                .ShouldBe("Login$Q0S$1$null$$$1$ACN$null");
        }

        [TestMethod]
        public void Ensure_DecryptedString_Match_Case_AccountNo_IsNull_And_RefNo_IsNotNull()
        {
            GetDecryptResult("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdtM1sdIZ2GczRgtP0ich22uqJTxyzSCKwwQxhcDP6yUSA9xkxg8cdwBPe56oeE8PrV3u%0D%0A5ff94W1LvibJ%2FP2nFwY%3D")
                .ShouldBe("Login$Q0S$1$null$1$RFN$$$null");
        }

        private string GetDecryptResult(string encryptedString)
        {
            return DecryptMethod.Invoke(
                    obj: null,
                    parameters: new[]
                    {
                        HttpUtility.UrlDecode(encryptedString),
                        Type.Missing
                    }).ToString();
        }
    }
}