using System;
using System.Collections.Generic;
using Zephyr.PaymentGateway.Maybank2uPay.Core;

namespace Zephyr.PaymentGateway.Maybank2uPay.Web.Models
{
    public class DemoViewModel
    {
        public decimal Amount { get; set; }

        public string AccountNo { get; set; }

        public string PayeeCode { get; set; }

        public string RefNo { get; set; }

        public Dictionary<int, string> Environments { get; private set; }

        public DemoViewModel()
        {
            Environments = new Dictionary<int, string>();

            foreach (var environment in
                Enum.GetValues(typeof(M2uPaymentGateway.Environment)))
            {
                Environments.Add(
                    (int)Enum.Parse(
                        typeof(M2uPaymentGateway.Environment),
                        environment.ToString()),
                    environment.ToString());
            }
        }
    }
}