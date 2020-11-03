namespace Checkout.PaymentGateway.Api.Entities
{
    public enum PaymentStatus
    {
        /// <summary>
        /// Denotes a failed payment request
        /// </summary>
        Failure = -1,

        /// <summary>
        /// Denotes an undefined status for a payment request
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Denotes a successful payment request
        /// </summary>
        Success = 1
    }
}