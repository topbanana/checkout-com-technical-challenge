using System;
using System.Threading.Tasks;
using AutoFixture;
using Checkout.AcquiringBank.Client;
using Checkout.PaymentGateway.Api.Dtos;
using Checkout.PaymentGateway.Api.Entities;
using Checkout.PaymentGateway.Api.Repositories;
using Checkout.PaymentGateway.Api.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;
using CreatePaymentRequest = Checkout.AcquiringBank.Client.Models.CreatePaymentRequest;
using CreatePaymentResponse = Checkout.AcquiringBank.Client.Models.CreatePaymentResponse;

namespace Checkout.PaymentGateway.Api.Tests.Services
{
    public class PaymentsServiceTests
    {
        private readonly CreatePaymentResponse _createPaymentResponse;
        private readonly Fixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly Payment _expectedPayment;

        public PaymentsServiceTests()
        {
            _fixture = new Fixture();
            _mocker = new AutoMocker();

            _fixture.Register(() => new GetPaymentResponse(_fixture.Create<string>(),
                _fixture.Create<string>().Substring(0, 4), _fixture.Create<decimal>(), _fixture.Create<string>()));

            _createPaymentResponse = _fixture.Create<CreatePaymentResponse>();
            _mocker.GetMock<IAcquiringBankClient>().Setup(x =>
                    x.CreatePayment(It.IsAny<CreatePaymentRequest>()))
                .ReturnsAsync(_createPaymentResponse);

            _expectedPayment = _fixture.Build<Payment>().With(x => x.LastFourDigitsOfCard, "1234").WithAutoProperties()
                .Create();
            _mocker.GetMock<IPaymentRepository>().Setup(x => x.GetPaymentByPaymentId(It.IsAny<Guid>()))
                .ReturnsAsync(_expectedPayment);
        }

        private PaymentsService ClassUnderTest => _mocker.CreateInstance<PaymentsService>();

        [Fact]
        public async Task CreatePayment_WithRequest_IssuesRequestToAcquiringBank()
        {
            // arrange
            // act
            await ClassUnderTest.CreatePayment(_fixture.Create<Api.Dtos.CreatePaymentRequest>());
            // assert
            _mocker.GetMock<IAcquiringBankClient>().Verify(x =>
                x.CreatePayment(It.IsAny<CreatePaymentRequest>()));
        }

        [Fact]
        public async Task CreatePayment_WithRequest_IssuesRequestToAcquiringBankWithCardNumber()
        {
            // arrange
            var request = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(request);
            // assert
            _mocker.GetMock<IAcquiringBankClient>().Verify(x =>
                x.CreatePayment(
                    It.Is<CreatePaymentRequest>(req =>
                        req.CardNumber == request.CardNumber)));
        }

        [Fact]
        public async Task CreatePayment_WithRequest_IssuesRequestToAcquiringBankWithCardHolderName()
        {
            // arrange
            var request = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(request);
            // assert
            _mocker.GetMock<IAcquiringBankClient>().Verify(x =>
                x.CreatePayment(
                    It.Is<CreatePaymentRequest>(req =>
                        req.CardHolderName == request.CardHolderName)));
        }

        [Fact]
        public async Task CreatePayment_WithRequest_IssuesRequestToAcquiringBankWithExpiry()
        {
            // arrange
            var request = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(request);
            // assert
            _mocker.GetMock<IAcquiringBankClient>().Verify(x =>
                x.CreatePayment(
                    It.Is<CreatePaymentRequest>(req =>
                        req.ExpiryMonthYear == request.ExpiryMonthYear)));
        }

        [Fact]
        public async Task CreatePayment_WithRequest_IssuesRequestToAcquiringBankWithAmount()
        {
            // arrange
            var request = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(request);
            // assert
            _mocker.GetMock<IAcquiringBankClient>().Verify(x =>
                x.CreatePayment(
                    It.Is<CreatePaymentRequest>(req =>
                        req.Amount == request.Amount)));
        }

        [Fact]
        public async Task CreatePayment_WithRequest_IssuesRequestToAcquiringBankWithCurrencyCode()
        {
            // arrange
            var request = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(request);
            // assert
            _mocker.GetMock<IAcquiringBankClient>().Verify(x =>
                x.CreatePayment(
                    It.Is<CreatePaymentRequest>(req =>
                        req.CurrencyCode == request.CurrencyCode)));
        }

        [Fact]
        public async Task CreatePayment_WithRequest_IssuesRequestToAcquiringBankWithCardVerificationValue()
        {
            // arrange
            var request = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(request);
            // assert
            _mocker.GetMock<IAcquiringBankClient>().Verify(x =>
                x.CreatePayment(
                    It.Is<CreatePaymentRequest>(req =>
                        req.CardVerificationValue == request.CardVerificationValue)));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_AddsPaymentToRepository()
        {
            // arrange
            // act
            await ClassUnderTest.CreatePayment(_fixture.Create<Api.Dtos.CreatePaymentRequest>());
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x => x.AddPayment(It.IsAny<Payment>()));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_AddsPaymentToRepositoryWithPaymentId()
        {
            // arrange
            // act
            await ClassUnderTest.CreatePayment(_fixture.Create<Api.Dtos.CreatePaymentRequest>());
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x =>
                x.AddPayment(It.Is<Payment>(payment => payment.PaymentId == _createPaymentResponse.PaymentId)));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_AddsPaymentToRepositoryWithPaymentStatus()
        {
            // arrange
            var createPaymentResponse = _fixture.Create<CreatePaymentResponse>();
            _mocker.GetMock<IAcquiringBankClient>().Setup(x =>
                    x.CreatePayment(It.IsAny<CreatePaymentRequest>()))
                .ReturnsAsync(createPaymentResponse);
            // act
            await ClassUnderTest.CreatePayment(_fixture.Create<Api.Dtos.CreatePaymentRequest>());
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x =>
                x.AddPayment(
                    It.Is<Payment>(payment => (int) payment.Status == (int) createPaymentResponse.PaymentStatus)));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_AddsPaymentToRepositoryWithAmount()
        {
            // arrange
            var createPaymentRequest = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(createPaymentRequest);
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x =>
                x.AddPayment(It.Is<Payment>(payment => payment.Amount == createPaymentRequest.Amount)));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_AddsPaymentToRepositoryWithExpiryMonthYear()
        {
            // arrange
            var createPaymentRequest = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(createPaymentRequest);
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x =>
                x.AddPayment(It.Is<Payment>(payment =>
                    payment.ExpiryMonthYear == createPaymentRequest.ExpiryMonthYear)));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_AddsPaymentToRepositoryWithCurrencyCode()
        {
            // arrange
            var createPaymentRequest = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(createPaymentRequest);
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x =>
                x.AddPayment(It.Is<Payment>(payment =>
                    payment.CurrencyCode == createPaymentRequest.CurrencyCode)));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_AddsPaymentToRepositoryWithLastFourDigitsOfCardNumber()
        {
            // arrange
            var createPaymentRequest = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(createPaymentRequest);
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x =>
                x.AddPayment(It.Is<Payment>(payment =>
                    payment.LastFourDigitsOfCard.Length == 4 && payment.LastFourDigitsOfCard ==
                    createPaymentRequest.CardNumber.Substring(createPaymentRequest.CardNumber.Length - 4))));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_AddsPaymentToRepositoryWithCardHolderName()
        {
            // arrange
            var createPaymentRequest = _fixture.Create<Api.Dtos.CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(createPaymentRequest);
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x =>
                x.AddPayment(It.Is<Payment>(payment => payment.CardHolderName == createPaymentRequest.CardHolderName)));
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_ReturnsPaymentId()
        {
            // arrange

            // act
            var result = await ClassUnderTest.CreatePayment(_fixture.Create<Api.Dtos.CreatePaymentRequest>());
            // assert
            result.PaymentId.Should().Be(_createPaymentResponse.PaymentId);
        }

        [Fact]
        public async Task CreatePayment_WithResponseFromClient_ReturnsPaymentStatus()
        {
            // arrange
            // act
            var result = await ClassUnderTest.CreatePayment(_fixture.Create<Api.Dtos.CreatePaymentRequest>());
            // assert
            ((int) result.PaymentStatus).Should().Be((int) _createPaymentResponse.PaymentStatus);
        }

        [Fact]
        public async Task GetPayment_WithRequest_InvokesRepository()
        {
            // arrange
            var getPaymentRequest = _fixture.Create<GetPaymentRequest>();
            // act
            await ClassUnderTest.GetPayment(getPaymentRequest);
            // assert
            _mocker.GetMock<IPaymentRepository>().Verify(x => x.GetPaymentByPaymentId(getPaymentRequest.PaymentId));
        }

        [Fact]
        public async Task GetPayment_WithRequest_ReturnsCardHolderName()
        {
            // arrange
            // act
            var result = await ClassUnderTest.GetPayment(_fixture.Create<GetPaymentRequest>());
            // assert
            result.CardHolderName.Should().BeSameAs(_expectedPayment.CardHolderName);
        }

        [Fact]
        public async Task GetPayment_WithRequest_ReturnsAmount()
        {
            // arrange
            // act
            var result = await ClassUnderTest.GetPayment(_fixture.Create<GetPaymentRequest>());
            // assert
            result.Amount.Should().Be(_expectedPayment.Amount);
        }

        [Fact]
        public async Task GetPayment_WithRequest_ReturnsCurrencyCode()
        {
            // arrange
            // act
            var result = await ClassUnderTest.GetPayment(_fixture.Create<GetPaymentRequest>());
            // assert
            result.CurrencyCode.Should().Be(_expectedPayment.CurrencyCode);
        }

        [Fact]
        public async Task GetPayment_WithRequest_ReturnsMaskedCardNumber()
        {
            // arrange
            // act
            var result = await ClassUnderTest.GetPayment(_fixture.Create<GetPaymentRequest>());
            // assert
            result.MaskedCardNumber.Should().EndWith(_expectedPayment.LastFourDigitsOfCard).And
                .StartWith(new string('X', 12)).And.HaveLength(16);
        }
    }
}