using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.AcquiringBank.Client.Models;
using Microsoft.Extensions.Logging;

namespace Checkout.AcquiringBank.Client
{
    /// <inheritdoc />
    public class AcquiringBankClient : IAcquiringBankClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AcquiringBankClient> _logger;

        public AcquiringBankClient(HttpClient httpClient, ILogger<AcquiringBankClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("/payment", request);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return new CreatePaymentResponse(Guid.Empty, PaymentStatus.Failure);
            }

            return await result.Content.ReadAsAsync<CreatePaymentResponse>();
        }
    }
}