Maybank2U Pay SDK (.NET)
------------------------

Live URL: https://maybank2upay-sdk.azurewebsites.net


Project structure:
- **Zephyr.PaymentGateway.Maybank2uPay.Core**
  - Written in .NET Framework 3.5 for compatibility
  - Class library with business logic to perform tasks
  - External libary
    - Newtonsoft.Json
- **Zephyr.PaymentGateway.Maybank2uPay.Core.Test**
  - .NET Framework 4.5
  - Unit test project to verify business rules
  - External libary
    - Shoudly
- **Zephyr.PaymentGateway.Maybank2uPay.Web**
  - .NET Framework 4.5
  - Proof-of-concept and web demo of M2U feature
  - External libary
    - Newtonsoft.Json

Usage

(1) Build `M2UPayload` object with parameters

`var payload = M2uPaymentGateway.CreatePayload(amount, accountNo, payeeCode, refNo);`


(2) Get encrypted JSON with predefined `environment`

`var encryptedJson = payload.GetEncryptionJsonString(environment);`
