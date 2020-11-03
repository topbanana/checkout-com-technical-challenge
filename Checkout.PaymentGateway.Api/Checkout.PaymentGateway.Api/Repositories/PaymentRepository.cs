using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Api.Entities;

namespace Checkout.PaymentGateway.Api.Repositories
{
    /// <summary>
    /// For brevity I am not implementing this, other than to say I would wrap a data-context
    /// with the Payment-entity available within it. I felt I exhausted enough time I could
    /// give to this technical test/challenge
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        /// <inheritdoc />
        public Task AddPayment(Payment payment)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Payment> GetPaymentByPaymentId(Guid paymentId)
        {
            throw new NotImplementedException();
        }
    }
}