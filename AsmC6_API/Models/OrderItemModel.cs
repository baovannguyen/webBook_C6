namespace AsmC6_API.Models
{
	public class OrderItemModel
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public OrderModel? Order { get; set; }

		public int BookId { get; set; }
		public BookModel? Book { get; set; }

		public int Quantity { get; set; }
		public int UnitPrice { get; set; }
	}
}
