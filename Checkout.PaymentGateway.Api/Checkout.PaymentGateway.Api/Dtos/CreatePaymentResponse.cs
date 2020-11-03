using System;

namespace Checkout.PaymentGateway.Api.Dtos
{
    /// <summary>
    /// Response to a Create-Payment request <see cref="CreatePaymentRequest"/>
    /// </summary>
    public class CreatePaymentResponse
    {
        public CreatePaymentResponse(Guid paymentId, PaymentStatus paymentStatus)
        {
            PaymentId = paymentId;
            PaymentStatus = paymentStatus;
        }

        /// <summary>
        /// The designated identifier for a payment
        /// </summary>
        public Guid PaymentId { get; }

        /// <summary>
        /// The status for 
        /// </summary>
        public PaymentStatus PaymentStatus { get; }
    }
}