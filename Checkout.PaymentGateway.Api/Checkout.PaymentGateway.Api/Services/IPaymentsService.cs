using System.Threading.Tasks;
using Checkout.PaymentGateway.Api.Dtos;

namespace Checkout.PaymentGateway.Api.Services
{
    /// <summary>
    /// Domain service for payments
    /// </summary>
    public interface IPaymentsService
    {
        /// <summary>
        /// Creates a payment with the acquiring bank and store desensitised data
        /// </summary>
        /// <param name="request">The create-payment request details</param>
        /// <returns>The response from the attempt</returns>
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request);

        /// <summary>
        /// Retrieves a payment from storage
        /// </summary>
        /// <param name="request">The get-payment request details</param>
        /// <returns>The payment matching the get-payment request</returns>
        Task<GetPaymentResponse> GetPayment(GetPaymentRequest request);
    }
}