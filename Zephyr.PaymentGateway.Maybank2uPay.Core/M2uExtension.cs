using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Zephyr.PaymentGateway.Maybank2uPay.Core
{
    internal static class M2uExtension
    {
        public const string EmptyJsonString = "{}";
        public const string JavaNullString  = "null";

        [Obsolete("Reserved for standardization with Node.js API")]
        public const string JavaScriptNullString = "undefined"; 

        /// <summary>
        ///     Extension method which replicates the
        ///     default behaviour of null string concatenation
        ///     behaviour in Java
        /// </summary>
        /// <example>
        ///     String str = null;
        ///     str += "42"; // results "null42"
        /// </example>
        /// <remarks>
        ///     Reference :
        ///     <https://docs.oracle.com/javase/specs/jls/se8/html/jls-5.html#jls-5.1.11>
        /// </remarks>
        /// <param name="str"></param>
        /// <returns>Orginial string except null string as "null"</returns>
        public static string ToJavaNullString(this string str)
        {
            return str ?? JavaNullString;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns>
        ///     Url encoded string with uppercase in accordance to
        ///     <c>java.net.URLEncoder.encode</c> in Java
        /// </returns>
        /// <remarks>
        ///     Replace with <c>WebUtility.UrlEncode</c> if
        ///     minimum target .NET framework is 4.0
        /// </remarks>
        public static string UrlEncodeUpperCase(this string str)
        {
            return Regex.Replace(
                    HttpUtility.UrlEncode(str),
                    @"%([\da-f][\da-f])",
                    c => c.Value.ToUpper());
        }

        /// <summary>
        ///     Extension method which repliaces the
        ///     default behaviour of Base64 encoding in Java
        ///     library <c>sun.misc.BASE64Encoder</c>
        /// </summary>
        /// <param name="bin"></param>
        /// <returns>
        ///     Base64 encoded string with link break
        ///     after every 76 characters
        /// </returns>
        public static string ToBase64WithLineBreak(this byte[] bin)
        {
            return Convert.ToBase64String(bin, Base64FormattingOptions.InsertLineBreaks);
        }

        public static string ToJson(this M2uEncryptedJson request)
        {
            if (string.IsNullOrEmpty(request.ActionUrl) ||
                string.IsNullOrEmpty(request.EncryptedString))
                return EmptyJsonString;

            return JsonConvert.SerializeObject(request);
        }
    }
}