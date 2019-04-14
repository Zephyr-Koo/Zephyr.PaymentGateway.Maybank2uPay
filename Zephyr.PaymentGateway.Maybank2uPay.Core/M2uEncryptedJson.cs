using Newtonsoft.Json;

namespace Zephyr.PaymentGateway.Maybank2uPay.Core
{
    internal class M2uEncryptedJson
    {
        [JsonProperty(PropertyName = "actionUrl", Order = 1)]
        public string ActionUrl { get; set; }

        [JsonProperty(PropertyName = "encryptedString")]
        public string EncryptedString { get; set; }

        public M2uEncryptedJson(string actionUrl, string encryptedString)
        {
            ActionUrl       = actionUrl;
            EncryptedString = encryptedString;
        }
    }
}