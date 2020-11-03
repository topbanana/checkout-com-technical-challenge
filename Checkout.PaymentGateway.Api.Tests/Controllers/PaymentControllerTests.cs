using System.Threading.Tasks;
using AutoFixture;
using Checkout.PaymentGateway.Api.Controllers;
using Checkout.PaymentGateway.Api.Dtos;
using Checkout.PaymentGateway.Api.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Checkout.PaymentGateway.Api.Tests.Controllers
{
    public class PaymentControllerTests
    {
        private readonly Fixture _fixture;
        private readonly AutoMocker _mocker;

        public PaymentControllerTests()
        {
            _fixture = new Fixture();
            _mocker = new AutoMocker();
            _fixture.Register(() => new CreatePaymentRequest());
            _fixture.Register(() => new GetPaymentResponse(_fixture.Create<string>(),
                _fixture.Create<string>().Substring(0, 4), _fixture.Create<decimal>(), _fixture.Create<string>()));
        }

        private PaymentController ClassUnderTest => _mocker.CreateInstance<PaymentController>();

        [Fact]
        public async Task CreatePayment_WithRequest_InvokesService()
        {
            // arrange
            // act
            await ClassUnderTest.CreatePayment(_fixture.Create<CreatePaymentRequest>());
            // assert
            _mocker.GetMock<IPaymentsService>().Verify(x => x.CreatePayment(It.IsAny<CreatePaymentRequest>()));
        }

        [Fact]
        public async Task CreatePayment_WithRequest_ResponseIsCorrect()
        {
            // arrange
            var expected = _fixture.Create<CreatePaymentResponse>();
            _mocker.GetMock<IPaymentsService>().Setup(x => x.CreatePayment(It.IsAny<CreatePaymentRequest>()))
                .ReturnsAsync(expected);
            // act
            var result = await ClassUnderTest.CreatePayment(_fixture.Create<CreatePaymentRequest>());
            // assert
            result.Should().BeSameAs(expected);
        }

        [Fact]
        public async Task CreatePayment_WithRequest_InvokesServiceWithRequest()
        {
            // arrange
            var createPaymentRequest = _fixture.Create<CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(createPaymentRequest);
            // assert
            _mocker.GetMock<IPaymentsService>().Verify(x => x.CreatePayment(createPaymentRequest));
        }

        [Fact]
        public async Task GetPayment_WithRequest_InvokesService()
        {
            // arrange
            // act
            await ClassUnderTest.GetPayment(_fixture.Create<GetPaymentRequest>());
            // assert
            _mocker.GetMock<IPaymentsService>().Verify(x => x.GetPayment(It.IsAny<GetPaymentRequest>()));
        }

        [Fact]
        public async Task GetPayment_WithRequest_ResponseIsCorrect()
        {
            // arrange
            var expected = _fixture.Create<GetPaymentResponse>();
            _mocker.GetMock<IPaymentsService>().Setup(x => x.GetPayment(It.IsAny<GetPaymentRequest>()))
                .ReturnsAsync(expected);
            // act
            var result = await ClassUnderTest.GetPayment(_fixture.Create<GetPaymentRequest>());
            // assert
            result.Should().BeSameAs(expected);
        }

        [Fact]
        public async Task GetPayment_WithRequest_InvokesServiceWithRequest()
        {
            // arrange
            var GetPaymentRequest = _fixture.Create<GetPaymentRequest>();
            // act
            await ClassUnderTest.GetPayment(GetPaymentRequest);
            // assert
            _mocker.GetMock<IPaymentsService>().Verify(x => x.GetPayment(GetPaymentRequest));
        }
    }
}