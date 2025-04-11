namespace Asm_Blazor.Models.book
{
	public class BookUpdateModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
		public string ImageUrl { get; set; }
		public int Quantity { get; set; }
	}
}
