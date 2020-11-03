using System;
using System.Threading.Tasks;
using Checkout.AcquiringBank.Client;
using Checkout.PaymentGateway.Api.Dtos;
using Checkout.PaymentGateway.Api.Entities;
using Checkout.PaymentGateway.Api.Repositories;
using Microsoft.Extensions.Logging;
using PaymentStatus = Checkout.PaymentGateway.Api.Entities.PaymentStatus;

namespace Checkout.PaymentGateway.Api.Services
{
    /// <inheritdoc />
    public class PaymentsService : IPaymentsService
    {
        private readonly IAcquiringBankClient _acquiringBankClient;
        private readonly ILogger<PaymentsService> _logger;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsService(
            IAcquiringBankClient acquiringBankClient,
            IPaymentRepository paymentRepository,
            ILogger<PaymentsService> logger)
        {
            _acquiringBankClient = acquiringBankClient ?? throw new ArgumentNullException(nameof(acquiringBankClient));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request)
        {
            var remoteCreatePaymentRequest =
                new AcquiringBank.Client.Models.CreatePaymentRequest(
                    request.CardNumber,
                    request.CardHolderName,
                    request.ExpiryMonthYear,
                    request.Amount,
                    request.CurrencyCode,
                    request.CardVerificationValue);
            var remoteCreatePaymentResult = await _acquiringBankClient.CreatePayment(
                remoteCreatePaymentRequest);
            await _paymentRepository.AddPayment(new Payment
            {
                PaymentId = remoteCreatePaymentResult.PaymentId,
                CardHolderName = request.CardHolderName,
                CurrencyCode = request.CurrencyCode,
                Status = Enum.Parse<PaymentStatus>(remoteCreatePaymentResult.PaymentStatus.ToString()),
                Amount = request.Amount,
                ExpiryMonthYear = request.ExpiryMonthYear,
                LastFourDigitsOfCard = request.CardNumber.Substring(request.CardNumber.Length - 4)
            });
            return new CreatePaymentResponse(remoteCreatePaymentResult.PaymentId,
                Enum.Parse<Dtos.PaymentStatus>(remoteCreatePaymentResult.PaymentStatus.ToString()));
        }

        /// <inheritdoc />
        public async Task<GetPaymentResponse> GetPayment(GetPaymentRequest request)
        {
            var payment = await _paymentRepository.GetPaymentByPaymentId(request.PaymentId);
            return new GetPaymentResponse(payment.CardHolderName, payment.LastFourDigitsOfCard, payment.Amount,
                payment.CurrencyCode);
        }
    }
}