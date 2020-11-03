using System.Threading.Tasks;
using Checkout.AcquiringBank.Client.Models;

namespace Checkout.AcquiringBank.Client
{
    /// <summary>
    /// Client which integrates with remote Acquiring bank
    /// </summary>
    public interface IAcquiringBankClient
    {
        /// <summary>
        /// Creates a payment on Acquiring bank
        /// </summary>
        /// <param name="request">The payment creation request</param>
        /// <returns>The response of the acquiring bank</returns>
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request);
    }
}