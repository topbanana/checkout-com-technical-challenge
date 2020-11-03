namespace Checkout.AcquiringBank.Client.Models
{
    /// <summary>
    /// Series of statuses that determine the status of a payment
    /// </summary>
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