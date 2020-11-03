using System;

namespace Checkout.PaymentGateway.Api.Dtos
{
    public class GetPaymentResponse
    {
        private const char CardNumberMask = 'X';
        private const int ExpectedCardCharacters = 4;


        public GetPaymentResponse(string cardHolderName, string endOfCardNumber, decimal amount, string currencyCode)
        {
            if (endOfCardNumber.Length != ExpectedCardCharacters)
            {
                throw new ArgumentException($"Length exceeds {ExpectedCardCharacters} characters",
                    nameof(endOfCardNumber));
            }

            CardHolderName = cardHolderName;
            MaskedCardNumber = new string(CardNumberMask, 12) + endOfCardNumber;
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        /// <summary>
        /// The masked card number, exposing only the last 4 characters
        /// </summary>
        public string MaskedCardNumber { get; }


        /// <summary>
        /// The card holder name
        /// </summary>
        public string CardHolderName { get; }

        /// <summary>
        /// The amount of the payment
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// 3-character depiction of the payment-current, say using ISO-4217
        /// </summary>
        public string CurrencyCode { get; }
    }
}