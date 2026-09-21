using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/productReview")]
    public class ProductReviewController : Controller
    {
        private readonly IProductReviewService _productReviewService;
        public ProductReviewController(IProductReviewService productReviewService) => _productReviewService = productReviewService;

        [HttpGet("{id:int}/getProductWithReviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseDto<ProductWithReviewsDto>>> GetWithProductReviewsProductByIdAsync(
           int id, CancellationToken cancellationToken)
        {
            var response = await _productReviewService.GetByIdProductsWithReviewsAsync(id, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
