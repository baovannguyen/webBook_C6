namespace AsmC6_API.DTOs.order
{
	public class OrderCreateModel
	{
		public int UserId { get; set; }
		public List<OrderItemCreateModel> Items { get; set; } = new();
	}
}
