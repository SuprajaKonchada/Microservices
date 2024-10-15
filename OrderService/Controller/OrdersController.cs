using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Model;

namespace OrderService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public OrdersController(OrderDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(_context.Orders.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            // Use IHttpClientFactory to create a client
            var client = _clientFactory.CreateClient();

            // Communicate with ProductService to validate the ProductId
            var productServiceUrl = $"http://localhost:5000/api/products/{order.ProductId}";
            var response = await client.GetAsync(productServiceUrl);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Product not found");
            }

            // If Product is valid, proceed to place the order
            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }
    }
}
