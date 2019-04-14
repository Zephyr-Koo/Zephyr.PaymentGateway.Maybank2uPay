using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zephyr.PaymentGateway.Maybank2uPay.Core;
using Zephyr.PaymentGateway.Maybank2uPay.Web.Models;

namespace Zephyr.PaymentGateway.Maybank2uPay.Web.Controllers
{
    [RoutePrefix("")]
    public class DefaultController : Controller
    {
        [Route("")]
        public ViewResult Index()
        {
            return View(new DemoViewModel());
        }

        [HttpPost]
        [Route("encrypted-json")]
        public ContentResult GetEncryptedJson(
            [Bind(Prefix = "payload")] DemoViewModel model,
            string environment)
        {
            var payload = M2uPaymentGateway.CreatePayload(
                            model.Amount,
                            model.AccountNo,
                            model.PayeeCode,
                            model.RefNo);

            var isValidEnvironment = Enum.TryParse(
                                        environment,
                                        out M2uPaymentGateway.Environment targetEnvironment);

            var encryptedJson = payload.GetEncryptionJsonString(
                                 isValidEnvironment ?
                                 (int) targetEnvironment :
                                 Convert.ToInt32(environment));

            return Content(encryptedJson, "application/json");
        }
    }
}