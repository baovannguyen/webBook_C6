namespace AsmC6_API.Models
{
	public class CartItemModel
	{
		public int Id { get; set; }
		public int CartId { get; set; }
		public CartModel? Cart { get; set; }

		public int BookId { get; set; }
		public BookModel? Book { get; set; }

		public int Quantity { get; set; }
	}
}
