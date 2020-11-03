using System.Threading.Tasks;
using Checkout.PaymentGateway.Api.Dtos;
using Checkout.PaymentGateway.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentsService _paymentsService;

        public PaymentController(IPaymentsService paymentsService, ILogger<PaymentController> logger)
        {
            _paymentsService = paymentsService;
            _logger = logger;
        }

        [HttpPost]
        public Task<CreatePaymentResponse> CreatePayment(
            [FromBody]
            CreatePaymentRequest request)
        {
            return _paymentsService.CreatePayment(request);
        }

        [HttpGet]
        public Task<GetPaymentResponse> GetPayment(
            [FromBody]
            GetPaymentRequest request)
        {
            return _paymentsService.GetPayment(request);
        }
    }
}