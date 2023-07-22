using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Errors;
using Talabat.Core.IRepository;
using Talabat.Repository.Helpers;
using Talabat.Repository.Specification;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IGenericRep<Product> product;
        private readonly IMapper mapper;

        public ProductController(IGenericRep<Product> product, IMapper mapper)
        {
            this.product = product;
            this.mapper = mapper;
        }

        [Route("~/Api/Product/GetProducts")]
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductSpecParams specs)
        {
            //var products =  await product.GetAllAsync();

            var spec = new ProductWithBrandAndTypeSpecifications(specs);
            var products = await product.GetAllWithSpecAsync(spec);
            var ProductsCount = new ProductsCountSpecifications(specs);
            var Count = await product.GetCountWithSpecAsync(ProductsCount);

            var MappedProducts = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products);
            return Ok(new PaginationReponse<ProductDTO>(specs.PageIndex, specs.PageSize, Count, MappedProducts));
        }

        [Route("~/Api/Product/GetProduct/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ProductDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            //var productModel = await product.GetByIdAsync(id);


            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var productModel = await product.GetEntityWithSpecAsync(spec);

            if (productModel is null)
                return BadRequest(new ApiResponse(404));

            var MappedProduct = mapper.Map<Product, ProductDTO>(productModel);

            return Ok(MappedProduct);
        }
    }
}
