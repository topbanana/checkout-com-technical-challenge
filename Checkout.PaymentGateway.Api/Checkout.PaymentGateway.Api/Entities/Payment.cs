using System;

namespace Checkout.PaymentGateway.Api.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string CardHolderName { get; set; }
        public string LastFourDigitsOfCard { get; set; }
        public decimal Amount { get; set; }
        public string ExpiryMonthYear { get; set; }
        public PaymentStatus Status { get; set; }
        public string CurrencyCode { get; set; }
    }
}