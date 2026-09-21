using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) => _productService = productService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseDto<IReadOnlyList<ProductDetailDto>>>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var response = await _productService.GetListAsync(cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseDto<IReadOnlyList<ProductDetailDto>>>> GetByIdAsync(int id,
             CancellationToken cancellationToken)
        {
            var response = await _productService.GetByIdAsync(id, cancellationToken);
            return StatusCode(response.Code, response);
        }

    }
}
