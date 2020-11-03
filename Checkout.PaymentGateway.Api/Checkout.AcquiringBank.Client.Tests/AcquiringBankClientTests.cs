using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Checkout.AcquiringBank.Client.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Checkout.AcquiringBank.Client.Tests
{
    public class AcquiringBankClientTests
    {
        private readonly Mock<FakeHttpMessageHandler> _fakeHttpMessageHandler;
        private readonly Fixture _fixture;
        private readonly HttpClient _httpClient;
        private readonly HttpResponseMessage _response;
        private HttpRequestMessage _capturedHttpRequestMessage;

        public AcquiringBankClientTests()
        {
            _fixture = new Fixture();
            _fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> {CallBase = true};
            _httpClient = new HttpClient(_fakeHttpMessageHandler.Object) {BaseAddress = _fixture.Create<Uri>()};
            _fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>()))
                .Callback<HttpRequestMessage>(req => _capturedHttpRequestMessage = req).Returns(() => _response);
            _response = new HttpResponseMessage {Content = new StringContent("")};
        }

        private AcquiringBankClient ClassUnderTest =>
            new AcquiringBankClient(_httpClient, new NullLogger<AcquiringBankClient>());

        [Fact]
        public async Task CreatePayment_WithRequest_InvokesRemoteEndpoint()
        {
            // arrange
            // act
            await ClassUnderTest.CreatePayment(_fixture.Create<CreatePaymentRequest>());
            // assert
            _fakeHttpMessageHandler.Verify(x => x.Send(It.IsAny<HttpRequestMessage>()));
        }


        [Fact]
        public async Task CreatePayment_WithRequest_InvokesRemoteEndpointWithPost()
        {
            // arrange
            // act
            await ClassUnderTest.CreatePayment(_fixture.Create<CreatePaymentRequest>());
            // assert
            _capturedHttpRequestMessage.Method.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public async Task CreatePayment_WithRequest_InvokesRemoteEndpointWithCorrectPath()
        {
            // arrange
            // act
            await ClassUnderTest.CreatePayment(_fixture.Create<CreatePaymentRequest>());
            // assert
            _capturedHttpRequestMessage.RequestUri.AbsolutePath.Should().Be("/payment");
        }

        [Fact]
        public async Task CreatePayment_WithSuccessfulResponse_ReturnsRemoteResponse()
        {
            // arrange
            var expectedResponse = _fixture.Create<CreatePaymentResponse>();
            _response.StatusCode = HttpStatusCode.OK;
            _response.Content = new StringContent(JsonSerializer.Serialize(expectedResponse), Encoding.UTF8,
                "application/json");
            // act
            var result = await ClassUnderTest.CreatePayment(_fixture.Create<CreatePaymentRequest>());
            // assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task CreatePayment_WithFailureResponse_ReturnCorrectResponse()
        {
            // arrange
            _response.StatusCode = HttpStatusCode.InternalServerError;
            // act
            var result = await ClassUnderTest.CreatePayment(_fixture.Create<CreatePaymentRequest>());
            // assert
            result.PaymentStatus.Should().Be(PaymentStatus.Failure);
            result.PaymentId.Should().Be(Guid.Empty);
        }

        [Fact]
        public async Task CreatePayment_WithRequest_InvokesRemoteEndpointWithPayload()
        {
            // arrange
            var payload = _fixture.Create<CreatePaymentRequest>();
            // act
            await ClassUnderTest.CreatePayment(payload);
            // assert
            (await _capturedHttpRequestMessage.Content.ReadAsAsync<CreatePaymentRequest>()).Should()
                .BeEquivalentTo(payload);
        }

        public class FakeHttpMessageHandler : HttpMessageHandler
        {
            public virtual HttpResponseMessage Send(HttpRequestMessage request)
            {
                throw new NotImplementedException();
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(Send(request));
            }
        }
    }
}