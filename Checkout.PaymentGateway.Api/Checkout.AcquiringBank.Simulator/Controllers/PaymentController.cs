using System;
using System.Text.Json;
using System.Threading.Tasks;
using Checkout.AcquiringBank.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkout.AcquiringBank.Simulator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Task<CreatePaymentResponse> CreatePayment(
            [FromBody]
            CreatePaymentRequest request)
        {
            _logger.LogInformation($"{nameof(CreatePayment)} : {JsonSerializer.Serialize(request)}");
            CreatePaymentResponse response = null;
            if (request.CardNumber.EndsWith("1"))
            {
                response = new CreatePaymentResponse(Guid.Empty, PaymentStatus.Failure);
            }
            else if (request.CardNumber.EndsWith("2"))
            {
                response = new CreatePaymentResponse(Guid.Empty, PaymentStatus.Undefined);
            }
            else
            {
                response = new CreatePaymentResponse(Guid.NewGuid(), PaymentStatus.Success);
            }

            return Task.FromResult(response);
        }
    }
}