using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Checkout.AcquiringBank.Client.Models
{
    /// <summary>
    /// Model for requesting creation of a payment
    /// </summary>
    public class CreatePaymentRequest
    {
        public CreatePaymentRequest(
            string cardNumber,
            string cardHolderName,
            string expiryMonthYear,
            decimal amount,
            string currencyCode,
            string cardVerificationValue)
        {
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpiryMonthYear = expiryMonthYear;
            Amount = amount;
            CurrencyCode = currencyCode;
            CardVerificationValue = cardVerificationValue;
        }

        /// <summary>
        /// The Card Number which matches a pattern of 16 numeric characters
        /// </summary>
        [RegularExpression(@"^\d{16}$")]
        [Required]
        public string CardNumber { get; }

        /// <summary>
        /// The Card Holder's name, of at least 4 characters
        /// </summary>
        [RegularExpression(@"^[\w\s]{4,}$")]
        [Required]
        public string CardHolderName { get; }

        /// <summary>
        /// The Card's Expiry-Date in the form of 2 numeric characters for the month component, a delimiter of a forward-slash
        /// or dash, and two numeric characters for the year component
        /// </summary>
        [RegularExpression(@"^\d{2}[\-\/]\d{2}$")]
        [Required]
        public string ExpiryMonthYear { get; }

        /// <summary>
        /// The amount of the payment
        /// </summary>
        [Range(0, double.PositiveInfinity)]
        [Required]
        public decimal Amount { get; }

        /// <summary>
        /// 3-character depiction of the payment-current, say using ISO-4217
        /// </summary>
        [RegularExpression(@"^\w{3}$")]
        [Required]
        public string CurrencyCode { get; }

        /// <summary>
        /// The Card Verification Value stamped on the card
        /// </summary>
        [RegularExpression(@"^\d{3}$")]
        [JsonPropertyName("ccv")]
        [Required]
        public string CardVerificationValue { get; }
    }
}