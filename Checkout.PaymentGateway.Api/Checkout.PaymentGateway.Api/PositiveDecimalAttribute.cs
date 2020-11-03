using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.PaymentGateway.Api
{
    /// <summary>
    /// Validation attribute that accepts positive decimals
    /// </summary>
    public class PositiveDecimalAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            return Convert.ToDecimal(value) >= 0;
        }
    }
}