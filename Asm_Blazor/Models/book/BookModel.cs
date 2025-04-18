namespace Asm_Blazor.Models.book
{
	public class BookModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }
		public string ImageUrl { get; set; }
		public int Quantity { get; set; }
		public string? CategoryName { get; set; }
		public int CategoryId { get; set; }

	}
}
