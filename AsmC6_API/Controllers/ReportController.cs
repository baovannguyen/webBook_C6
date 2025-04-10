using AsmC6_API.Data;
using Microsoft.AspNetCore.Mvc;

namespace AsmC6_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ReportController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("daily-sales")]
		public IActionResult GetDailySales()
		{
			var today = DateTime.Today;

			var result = _context.OrderItems
				.Where(x => x.Order.OrderDate.Date == today)
				.GroupBy(x => x.BookId)
				.Select(g => new
				{
					BookId = g.Key,
					QuantitySold = g.Sum(x => x.Quantity),
					BookTitle = g.First().Book.Title
				}).ToList();

			return Ok(result);
		}

		[HttpGet("monthly-sales")]
		public IActionResult GetMonthlySales()
		{
			var now = DateTime.Now;

			var result = _context.OrderItems
				.Where(x => x.Order.OrderDate.Month == now.Month && x.Order.OrderDate.Year == now.Year)
				.GroupBy(x => x.BookId)
				.Select(g => new
				{
					BookId = g.Key,
					QuantitySold = g.Sum(x => x.Quantity),
					BookTitle = g.First().Book.Title
				}).ToList();

			return Ok(result);
		}
	}

}
