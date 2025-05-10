using System.ComponentModel.DataAnnotations;

namespace AsmC6_API.DTOs.NewFolder
{
	public class BookCreateDto
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Author { get; set; }

		public string Description { get; set; }

		public int Price { get; set; }

		public int CategoryId { get; set; }
		public string ImageUrl { get; set; }
		public int Quantity { get; set; }
	}
}
