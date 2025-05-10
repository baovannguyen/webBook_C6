namespace AsmC6_API.DTOs.order
{
	public class OrderItemDto
	{/*
		public int BookId { get; set; }
		public int Quantity { get; set; }
		public int UnitPrice { get; set; }*/


		public int BookId { get; set; }
		public string? BookTitle { get; set; } // Optionally show Book Title
		public int Quantity { get; set; }
		public int UnitPrice { get; set; }
	}
}
