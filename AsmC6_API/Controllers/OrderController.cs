using AsmC6_API.Data;
using AsmC6_API.DTOs.NewFolder;
using AsmC6_API.DTOs.order;
using AsmC6_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsmC6_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public OrderController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper= mapper;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
		{
			var order = new OrderModel
			{
				UserId = orderDto.UserId,
				OrderDate = DateTime.Now,
				Status = "Pending",
				Items = new List<OrderItemModel>()
			};

			decimal totalPrice = 0;

			foreach (var item in orderDto.Items)
			{
				var book = await _context.Books.FindAsync(item.BookId);
				if (book == null)
					return NotFound($"Book with ID {item.BookId} not found.");

				if (book.Quantity < item.Quantity)
					return BadRequest($"Not enough stock for book ID {item.BookId} (available: {book.Quantity}, requested: {item.Quantity})");

				// Trừ tồn kho
				book.Quantity -= item.Quantity;

				// Tính đơn giá cho item
				var unitPrice = book.Price * item.Quantity;
				totalPrice += unitPrice;

				order.Items.Add(new OrderItemModel
				{
					BookId = item.BookId,
					Quantity = item.Quantity,
					UnitPrice = unitPrice
				});
			}

			// Tổng giá đơn hàng
			order.TotalPrice = totalPrice;

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			return Ok(_mapper.Map<OrderDto>(order));
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order == null)
				return NotFound();

			order.Status = newStatus;
			await _context.SaveChangesAsync();

			return Ok(_mapper.Map<OrderDto>(order));
		}





		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var orders = await _context.Orders
				.Include(o => o.User)
				.Include(o => o.Items)
					.ThenInclude(i => i.Book)
				.ToListAsync();

			var result = _mapper.Map<IEnumerable<OrderDto>>(orders);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var order = await _context.Orders
				.Include(o => o.Items)
					.ThenInclude(i => i.Book)
				.Include(o => o.User)
				.FirstOrDefaultAsync(o => o.Id == id);

			if (order == null)
				return NotFound();

			var result = _mapper.Map<OrderDto>(order);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var order = await _context.Orders
				.Include(o => o.Items)
				.FirstOrDefaultAsync(o => o.Id == id);

			if (order == null)
				return NotFound();

			_context.OrderItems.RemoveRange(order.Items);
			_context.Orders.Remove(order);

			await _context.SaveChangesAsync();
			return NoContent();
		}
	}

}
