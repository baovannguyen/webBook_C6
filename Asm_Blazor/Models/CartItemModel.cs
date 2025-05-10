using System.ComponentModel.DataAnnotations;

namespace Asm_Blazor.Models
{
	public class CartItemModel
	{
		[Key]
		public int BookId { get; set; }
		public string Title { get; set; } = string.Empty;
		public int Price { get; set; }
		public int Quantity { get; set; }
		public string ImageUrl { get; set; }

		public int Total => Price * Quantity;



	}
}
