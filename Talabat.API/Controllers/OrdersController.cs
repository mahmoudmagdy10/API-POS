using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.API.DTOs;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Errors;
using Talabat.Core.IService;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("~/Api/Orders/CreateOrder")]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO order)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var ShippingAddress = _mapper.Map<AddressDTO, Address>(order.ShippingAddress);
            var OrderModel = await _orderService.CreateOrderAsync(BuyerEmail, order.BasketId, order.DeliveryMethodId, ShippingAddress);

            if (OrderModel is null) return BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<Order,OrderToReturnDTO>(OrderModel));
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpGet("~/Api/Orders/GetOrdersOfCurrentUser")]
        public async Task<IActionResult> GetOrdersOfCurrentUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(BuyerEmail);
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpGet("~/Api/Orders/GetOrdersOfUserById/{id}")]
        public async Task<IActionResult> GetOrdersOfUserById(int id)
            {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(BuyerEmail, id);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(order);
        }
    }
}
