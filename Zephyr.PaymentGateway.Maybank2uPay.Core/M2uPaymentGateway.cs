using System;
using System.Text;

namespace Zephyr.PaymentGateway.Maybank2uPay.Core
{
    /// <summary>
    ///     Direct port of <c>PaymentGateway.java</c> which
    ///     expose method to get encryptedJson in string
    /// </summary>
    public static class M2uPaymentGateway
    {
        public enum Environment
        {
            Sandbox    = 0,
            UAT        = 1,
            Production = 2
        }

        private const string Url_UAT        = "https://m2upayuat.maybank2u.com.my/testM2uPayment";
        private const string Url_Production = "https://www.maybank2u.com.my/mbb/m2u/m9006_enc/m2uMerchantLogin.do";
        private const string Url_Sandbox    = "https://api.maybanksandbox.com/v1.0/testM2uPayment";

        public static M2uPayload CreatePayload(
            decimal amount,
            string accountNo,
            string payeeCode,
            string refNo)
        {
            return new M2uPayload()
            {
                Amount    = amount.ToString(),
                AccountNo = accountNo,
                PayeeCode = payeeCode,
                RefNo     = refNo
            };
        }

        public static string GetEncryptionJsonString(
            this M2uPayload payload,
            int environment)
        {
            var actionUrl       = string.Empty;
            var encryptedString = string.Empty;

            try
            {
                actionUrl = GetActionUrl((Environment)Enum.Parse(
                                typeof(Environment), environment.ToString()));

                encryptedString = GetEncryptedString(payload);
            }
            catch (Exception)
            {
                encryptedString = "FAIL";
            }

            return new M2uEncryptedJson(actionUrl, encryptedString).ToJson();
        }

        private static string GetActionUrl(Environment environment)
        {
            if (environment == Environment.UAT)
                return Url_UAT;
            else if (environment == Environment.Production)
                return Url_Production;

            return Url_Sandbox;
        }

        private static string GetEncryptedString(M2uPayload payload)
        {
            var paymentInfo = new StringBuilder();

            paymentInfo.Append($"Login${ payload.PayeeCode.ToJavaNullString() }");
            paymentInfo.Append($"$1${ payload.Amount.ToJavaNullString() }");

            /*
             * Case 1:
             *  ( RefNo isNull AND AccountNo isNotNull )
             *
             *  Format : Login$( )$1$( )$$$1$( )$( )
             *  Param  : PayeeCode, Amount, AccountNo, CallbackUrl
             *
             * Case 2:
             *  ( accountNo isNull AND RefNo isNotNull )
             *
             *  Format : Login$( )$1$( )$1$( )$$$( )
             *  Param  : PayeeCode, Amount, RefNo, CallbackUrl
             *
             * Case 3:
             *  ( any other cases )
             *
             *  Format : Login$( )$1$( )$1$( )$1$( )$( )
             *  Param  : PayeeCode, Amount, RefNo, AccountNo, CallbackUrl
             */

            if (string.IsNullOrEmpty(payload.RefNo) &&
                !string.IsNullOrEmpty(payload.AccountNo))
            {
                paymentInfo.Append($"$$$1${ payload.AccountNo }");
            }
            else if (string.IsNullOrEmpty(payload.AccountNo) &&
                     !string.IsNullOrEmpty(payload.RefNo))
            {
                paymentInfo.Append($"$1${ payload.RefNo.ToJavaNullString() }$$");
            }
            else
            {
                paymentInfo.Append($"$1${ payload.RefNo.ToJavaNullString() }");
                paymentInfo.Append($"$1${ payload.AccountNo.ToJavaNullString() }");
            }

            paymentInfo.Append($"${ payload.CallbackUrl.ToJavaNullString() }");

            return M2uCryptoService.Encrypt(paymentInfo.ToString()).UrlEncodeUpperCase();
        }
    }
}