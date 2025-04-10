namespace AsmC6_API.DTOs.cart
{
	public class CartItemAddDto
	{
		public string UserId { get; set; } = null!;
		public int BookId { get; set; }
		public int Quantity { get; set; }
	}
}
