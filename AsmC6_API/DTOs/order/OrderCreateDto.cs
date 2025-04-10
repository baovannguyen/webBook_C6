namespace AsmC6_API.DTOs.order
{
	public class OrderCreateDto
	{
		public int UserId { get; set; }
		public List<OrderItemCreateDto> Items { get; set; } = new();
	}
}
