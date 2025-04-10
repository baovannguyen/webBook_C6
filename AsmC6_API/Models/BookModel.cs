using System.ComponentModel.DataAnnotations;

namespace AsmC6_API.Models
{
	public class BookModel
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; } = string.Empty;

		public string Author { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public string? ImageUrl { get; set; }
		public int Quantity {  get; set; }

	
		[Required]
		public int CategoryId { get; set; }
		public CategoryModel ? Category { get; set; }
	}
}
