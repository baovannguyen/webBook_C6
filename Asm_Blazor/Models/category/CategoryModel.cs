using System.ComponentModel.DataAnnotations;

namespace Asm_Blazor.Models
{
	public class CategoryModel
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;
	}
}
