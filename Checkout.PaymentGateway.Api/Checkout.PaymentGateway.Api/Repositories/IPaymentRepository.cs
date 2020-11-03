using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Api.Entities;

namespace Checkout.PaymentGateway.Api.Repositories
{
    /// <summary>
    /// Payment repository for storage of payments
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// Adds a payment to storage
        /// </summary>
        /// <param name="payment">The payment to store</param>
        /// <returns>An awaitable task</returns>
        Task AddPayment(Payment payment);

        /// <summary>
        /// Retrieves a payment from storage matching the payment id
        /// </summary>
        /// <param name="paymentId">The payment-id to search for</param>
        /// <returns>The payment matching the id</returns>
        Task<Payment> GetPaymentByPaymentId(Guid paymentId);
    }
}