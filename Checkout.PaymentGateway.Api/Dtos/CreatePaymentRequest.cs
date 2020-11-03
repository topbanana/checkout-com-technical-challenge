using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Checkout.PaymentGateway.Api.Dtos
{
    /// <summary>
    /// Model for requesting creation of a payment
    /// </summary>
    public class CreatePaymentRequest
    {
        /// <summary>
        /// The Card Number which matches a pattern of 16 numeric characters
        /// </summary>
        [RegularExpression(@"^\d{16}$")]
        [Required]
        public string CardNumber { get; set; }

        /// <summary>
        /// The Card Holder's name, of at least 4 characters
        /// </summary>
        [RegularExpression(@"^[\w\s]{4,}$")]
        [Required]
        public string CardHolderName { get; set; }

        /// <summary>
        /// The Card's Expiry-Date in the form of 2 numeric characters for the month component, a delimiter of a forward-slash
        /// or dash, and two numeric characters for the year component
        /// </summary>
        [RegularExpression(@"^\d{2}[\-\/]\d{2}$")]
        [Required]
        public string ExpiryMonthYear { get; set; }

        /// <summary>
        /// The amount of the payment
        /// </summary>
        [PositiveDecimal]
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// 3-character depiction of the payment-current, say using ISO-4217
        /// </summary>
        [RegularExpression(@"^\w{3}$")]
        [Required]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// The Card Verification Value stamped on the card
        /// </summary>
        [RegularExpression(@"^\d{3}$")]
        [JsonPropertyName("ccv")]
        [Required]
        public string CardVerificationValue { get; set; }
    }
}