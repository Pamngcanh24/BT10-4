using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // ✅ API GET hỗ trợ phân trang
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var products = await _productService.GetAllProductsAsync();

            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(pagedProducts);
        }

        // ✅ GET theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // ✅ Thêm sản phẩm có xác thực bằng JWT
        [HttpPost]
        [Authorize] // ⬅️ chỉ ai đăng nhập mới thêm được
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
                return BadRequest("Name is required");

            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.ProductId }, createdProduct);
        }

        // ✅ Cập nhật sản phẩm
        [HttpPut("{id}")]
        [Authorize] // (tuỳ chọn) Chỉ ai đăng nhập mới được sửa
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, product);
            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }

        // ✅ Xoá sản phẩm
        [HttpDelete("{id}")]
        [Authorize] // (tuỳ chọn) Chỉ ai đăng nhập mới được xoá
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
