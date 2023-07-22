using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Errors;
using Talabat.Core.IRepository;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepo basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepo basketRepo, IMapper mapper)
        {
            this.basketRepo = basketRepo;
            _mapper = mapper;
        }

        [HttpGet("~/Api/Baskets/GetBasket")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await basketRepo.GetBasketsAsync(id);
            return basket ?? new CustomerBasket(id);
        }

        [HttpPost("~/Api/Baskets/UpdateBasket")]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDTO Basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDTO, CustomerBasket>(Basket);
            var CreateOrUpdatebasket = await basketRepo.UpdateBasketsAsync(mappedBasket);
            if (CreateOrUpdatebasket is null) return BadRequest(new ApiResponse(404));
            return CreateOrUpdatebasket;
        }
        
        [HttpDelete("~/Api/Baskets/DeleteBasket")]
        public async Task<ActionResult<bool>> DeleteBasket(CustomerBasket Basket)
        {
            return await basketRepo.DeleteBasketsAsync(Basket.Id);
        }
    }
}
