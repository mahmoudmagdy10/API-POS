using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.API.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Errors;
using Talabat.Core.IService;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_c12fbe2ce60af9692a2e5859bdeafaca7432254191423c46364b791fab133c67";


        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost("~/Api/Payment/CreateOrUpdatePaymentIntent")]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(CustomerBasketDTO Basket)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(Basket.Id);
            if (basket is null) return BadRequest(new ApiResponse(400, "Your Basket has Some Problems"));

            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDTO>(basket);

            return Ok(MappedBasket);
        }
        
        [HttpPost("~/Api/Payment/webhook")]
        public async Task<IActionResult> PaymentCallBack()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);
            var PaymentIntent = (PaymentIntent) stripeEvent.Data.Object;
            Order order;

            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentCreated)
            {
                order = await _paymentService.UpdatePaymentToSucceededOrFailed(PaymentIntent.Id,true);
            }
            else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                order = await _paymentService.UpdatePaymentToSucceededOrFailed(PaymentIntent.Id, false);
            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

            return new EmptyResult();
        }
    }
}
