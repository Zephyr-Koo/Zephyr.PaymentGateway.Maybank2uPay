using Newtonsoft.Json;

namespace Zephyr.PaymentGateway.Maybank2uPay.Core.Test
{
    /// <summary>
    ///     Metaclass which contains information of Maybank2uPay SDK
    /// </summary>
    public class MetaM2uPaymentGateway
    {
        public enum Environment
        {
            Sandbox    = 0,
            UAT        = 1,
            Production = 2
        }

        public const string Url_UAT        = "https://m2upayuat.maybank2u.com.my/testM2uPayment";
        public const string Url_Production = "https://www.maybank2u.com.my/mbb/m2u/m9006_enc/m2uMerchantLogin.do";
        public const string Url_Sandbox    = "https://api.maybanksandbox.com/v1.0/testM2uPayment";

        public class M2uEncryptedJson
        {
            [JsonProperty(PropertyName = "actionUrl", Order = 1)]
            public string ActionUrl { get; set; }

            [JsonProperty(PropertyName = "encryptedString")]
            public string EncryptedString { get; set; }
        }
    }
}