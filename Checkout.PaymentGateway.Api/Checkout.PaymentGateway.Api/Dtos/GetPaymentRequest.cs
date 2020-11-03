using System;

namespace Checkout.PaymentGateway.Api.Dtos
{
    /// <summary>
    /// Model for request the details for a payment
    /// </summary>
    public class GetPaymentRequest
    {
        /// <summary>
        /// The identifier for the payment
        /// </summary>
        public Guid PaymentId { get; set; }
    }
}