namespace AsmC6_API.Models
{
	public class CartItemModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int BookId { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }
		public string ImageUrl { get; set; } = string.Empty;


		public BookModel Book { get; set; }
	}
}
