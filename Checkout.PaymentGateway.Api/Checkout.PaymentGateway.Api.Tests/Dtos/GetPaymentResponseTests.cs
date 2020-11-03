using System;
using AutoFixture;
using Checkout.PaymentGateway.Api.Dtos;
using FluentAssertions;
using Xunit;

namespace Checkout.PaymentGateway.Api.Tests.Dtos
{
    public class GetPaymentResponseTests
    {
        private readonly Fixture _fixture;
        private string _attemptedEndOfCardNumber = "0000";

        public GetPaymentResponseTests()
        {
            _fixture = new Fixture();
            _fixture.Register(
                () => new GetPaymentResponse(
                    _fixture.Create<string>(),
                    _attemptedEndOfCardNumber,
                    _fixture.Create<decimal>(),
                    _fixture.Create<string>()));
        }

        [Fact]
        public void Ctor_WhenGivenCardNumber_MasksNumber()
        {
            // arrange
            var response = _fixture.Create<GetPaymentResponse>();
            // act
            var result = response.MaskedCardNumber;
            // assert
            result.Should().StartWith(new string('X', 12)).And.EndWith(_attemptedEndOfCardNumber).And.HaveLength(16);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        public void Ctor_WhenGivenCardNumberNotOfFourCharacters_ThrowsException(int numberOfCardCharacters)
        {
            // arrange
            _attemptedEndOfCardNumber = new string('x', numberOfCardCharacters);
            // act
            Action act = () => _fixture.Create<GetPaymentResponse>();
            // assert
            act.Should().Throw<Exception>().WithInnerException<ArgumentException>().Which.ParamName.Should()
                .Be("endOfCardNumber");
        }
    }
}