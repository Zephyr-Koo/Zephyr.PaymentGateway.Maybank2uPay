using System;

namespace Zephyr.PaymentGateway.Maybank2uPay.Core
{
    /// <summary>
    ///     Direct port of <c>PGParams.java</c> which
    ///     represents parameters that needed to
    ///     generate encryption string.
    /// </summary>
    public class M2uPayload
    {
        public const long SerialVersionUID = 1L;

        public string Amount { get; private set; }

        public string AccountNo { get; private set; }

        public string PayeeCode { get; private set; }

        public string RefNo { get; private set; }

        [Obsolete("This field have no reference in recent Maybank2UPay SDK documentation.")]
        public string CallbackUrl { get; private set; }

        public M2uPayload(
            string amount,
            string accountNo,
            string payeeCode,
            string refNo)
        {
            this.Amount    = amount;
            this.AccountNo = accountNo;
            this.PayeeCode = payeeCode;
            this.RefNo     = refNo;
        }
    }
}