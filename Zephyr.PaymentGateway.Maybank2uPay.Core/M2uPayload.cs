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

        public string Amount { get; set; }

        public string AccountNo { get; set; }

        public string PayeeCode { get; set; }

        public string RefNo { get; set; }

        [Obsolete("This field have no reference in recent Maybank2UPay SDK documentation.")]
        public string CallbackUrl { get; set; }
    }
}