using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shouldly;
using static Zephyr.PaymentGateway.Maybank2uPay.Core.Test.MetaM2uPaymentGateway;

namespace Zephyr.PaymentGateway.Maybank2uPay.Core.Test
{
    /// <summary>
    ///     Integrating test which ensure result
    ///     produced matched with original Java SDK
    /// </summary>
    [TestClass]
    public class TestLab
    {
        private readonly M2uPayload DefaultPayload = new M2uPayload
        {
            Amount    = "60",
            PayeeCode = "***",
            AccountNo = "A123456"
        };

        [TestMethod]
        public void Ensure_Environment_Url_Match_Sandbox()
        {
            var encryptedJson = GetEncryptedJson(DefaultPayload, Environment.Sandbox);

            encryptedJson.ActionUrl.ShouldBe(Url_Sandbox);
        }

        [TestMethod]
        public void Ensure_Environment_Url_Match_UAT()
        {
            var encryptedJson = GetEncryptedJson(DefaultPayload, Environment.UAT);

            encryptedJson.ActionUrl.ShouldBe(Url_UAT);
        }

        [TestMethod]
        public void Ensure_Environment_Url_Match_Production()
        {
            var encryptedJson = GetEncryptedJson(DefaultPayload, Environment.Production);

            encryptedJson.ActionUrl.ShouldBe(Url_Production);
        }

        [TestMethod]
        public void Ensure_EnryptedString_Match_Case_Empty_Payload()
        {
            var encryptedJson = GetEncryptedJson(new M2uPayload());

            encryptedJson.EncryptedString.ShouldBe("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdgwFlMTnnxoEUkJXOW1SAWL1VYo4eltb9sklwfvLbGuHvtHG6L25KcGj0gHyBNdt%2FMPx%0D%0AUXHORsA7e3kMbby0XUU%3D");
        }

        [TestMethod]
        public void Ensure_EnryptedString_Match_With_SDK_Example()
        {
            var encryptedJson = GetEncryptedJson(DefaultPayload);

            encryptedJson.EncryptedString.ShouldBe("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdq%2FtgH0nkPdaVCIH8iAo41TQCuH6rXgPh75JTuhPU8qkfuH1lYm%2Fq%2FMeaDNp5F9Wm62r%0D%0Ajy6yf8UJ12n%2FpQYbEK4%3D");
        }

        [TestMethod]
        public void Ensure_EnryptedString_Match_Case_RefNo_IsNull_And_AccountNo_IsNotNull()
        {
            var encryptedJson = GetEncryptedJson(new M2uPayload
            {
                Amount    = "100.00",
                AccountNo = "M2U001",
                PayeeCode = "ABC"
            });

            encryptedJson.EncryptedString.ShouldBe("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdpTcIiKOv0d%2FTc5wj8n6KvRxirKQFxReQP5Bwriq2uP335vTQRdMTcBtiFrxsld80%2B2g%0D%0A%2FnhzTEWtRmhH5Ol9fMo%3D");
        }

        [TestMethod]
        public void Ensure_EnryptedString_Match_Case_AccountNo_IsNull_And_RefNo_IsNotNull()
        {
            var encryptedJson = GetEncryptedJson(new M2uPayload
            {
                Amount    = "100.00",
                PayeeCode = "ABC",
                RefNo     = "TRX001"
            });

            encryptedJson.EncryptedString.ShouldBe("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdijAg6%2Bjlqvts3Ruwh2vN1GRMKuxURwd1xTxe20GL%2FmMzO%2B6RtYbLAtH669qNvKyrLQr%0D%0AgFmumGW7jpx35aqYauw%3D");
        }

        [TestMethod]
        public void Ensure_EnryptedString_Match_Case_Complete_Payload()
        {
            var encryptedJson = GetEncryptedJson(new M2uPayload
            {
                Amount    = "100.00",
                AccountNo = "M2U001",
                PayeeCode = "ABC",
                RefNo     = "TRX002"
            });

            encryptedJson.EncryptedString.ShouldBe("uGJ6%2Bl4prlW5E%2BdLg%2FOaS%2BNAyl0xTJmTiSTUFbeyIUjjErW3eCZ7lSRB3dpQULlUl3YRbfCVHn%2Be%0D%0Am%2FDZowvTdijAg6%2Bjlqvts3Ruwh2vN1HV1SihpdhsXBEEE9KpZVI52Cj%2B%2FQMF5%2FjFEFokZRsq60GL%0D%0ArG%2F5YyT0Z1Kefdqq3jRMYGsklSZKnmLJZMv3D%2FMJ");
        }

        private M2uEncryptedJson GetEncryptedJson(
            M2uPayload payload,
            Environment environment = Environment.Sandbox)
        {
            return JsonConvert.DeserializeObject<M2uEncryptedJson>(
                    payload.GetEncryptionJsonString((int)environment));
        }
    }
}