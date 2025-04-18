using AsmC6_API.Data;
using AsmC6_API.Models;
using AsmC6_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AsmC6_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PaymentController : ControllerBase
	{
		private readonly VnPayLibrary _vnPayLibrary;
		private readonly string _hashSecret;
		private readonly IOptions<VnPayConfig> _config;
		private readonly ApplicationDbContext _context;
		private readonly ICartService _cartService;

		public PaymentController(
			IOptions<VnPayConfig> config,
			ApplicationDbContext context,
			ICartService cartService)
		{
			_vnPayLibrary = new VnPayLibrary();
			_config = config;
			_hashSecret = config.Value.Vnp_HashSecret;
			_context = context;
			_cartService = cartService;
		}

		[HttpPost("create-payment")]
		public IActionResult CreatePayment([FromBody] VnPayModel model)
		{
			var configValue = _config.Value;
			int userId = GetCurrentUserId();
			Console.WriteLine("CurrCode: " + configValue.CurrCode);
			Console.WriteLine("Locale: " + configValue.Locale);
			Console.WriteLine("UserId: " + userId);

			// Thêm dữ liệu vào yêu cầu
			_vnPayLibrary.AddRequestData("vnp_Version", configValue.Vnp_Version);
			_vnPayLibrary.AddRequestData("vnp_Command", configValue.Vnp_Command);
			_vnPayLibrary.AddRequestData("vnp_TmnCode", "9U4CW4IM");
			_vnPayLibrary.AddRequestData("vnp_Amount", (model.Amount * 100).ToString()); // VNPay yêu cầu số tiền tính bằng đơn vị nhỏ nhất (VND)26.99.131.68
			_vnPayLibrary.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
			_vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
			_vnPayLibrary.AddRequestData("vnp_IpAddr", "127.0.0.1");
			_vnPayLibrary.AddRequestData("vnp_Locale", "vn");
			_vnPayLibrary.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng");
			_vnPayLibrary.AddRequestData("vnp_OrderType", "billpayment");
			_vnPayLibrary.AddRequestData("vnp_ReturnUrl", configValue.Vnp_ReturnUrl);
			_vnPayLibrary.AddRequestData("vnp_TxnRef", model.orderid.ToString());


			// Log ra các dữ liệu request
			foreach (var item in _vnPayLibrary.RequestData)
			{
				Console.WriteLine($"{item.Key}: {item.Value}");
			}

			var paymentUrl = _vnPayLibrary.CreateRequestUrl(configValue.Vnp_Url, configValue.Vnp_HashSecret);
			return Ok(new { url = paymentUrl });
		}

		[HttpGet("payment-return")]
		public async Task<IActionResult> PaymentReturn()
		{
			var vnpayData = Request.Query;
			var isValid = _vnPayLibrary.ValidateSignature(vnpayData, _hashSecret);

			if (isValid)
			{
				int userId = GetCurrentUserId();
				var cartItems = await _cartService.GetCartAsync(userId);

				if (cartItems == null || !cartItems.Any())
				{
					return Redirect("https://your-blazor-site.com/payment-fail?reason=empty-cart");
				}

				// Tạo đơn hàng
				var order = new OrderModel
				{
					UserId = userId,
					OrderDate = DateTime.Now,
					Status = "Đã thanh toán",
					TotalPrice = cartItems.Sum(i => i.Quantity * i.Price),
					Items = cartItems.Select(i => new OrderItemModel
					{
						BookId = i.BookId,
						Quantity = i.Quantity,
						UnitPrice = i.Price
					}).ToList()
				};

				_context.Orders.Add(order);
				await _context.SaveChangesAsync();

				// Xóa giỏ hàng
				await _cartService.ClearCartAsync(userId);

				return Redirect("https://your-blazor-site.com/payment-success");
			}
			else
			{
				return Redirect("https://your-blazor-site.com/payment-fail?reason=invalid-signature");
			}
		}

		private int GetCurrentUserId()
		{
			string chu = (User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException());
			int so = int.Parse(chu);
			return so;
		}
	}
}
