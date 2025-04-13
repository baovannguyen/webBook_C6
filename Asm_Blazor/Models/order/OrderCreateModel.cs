namespace Asm_Blazor.Models.order
{
	public class OrderCreateModel
	{
		public int UserId { get; set; }
		public List<OrderItemCreateModel> Items { get; set; } = new();
	}
}
