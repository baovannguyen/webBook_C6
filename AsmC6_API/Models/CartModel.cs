namespace AsmC6_API.Models
{
	public class CartModel
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public ICollection<CartItemModel> Items { get; set; } = new List<CartItemModel>();
	}
}
