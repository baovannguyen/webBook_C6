using System.ComponentModel.DataAnnotations;

namespace AsmC6_API.Models
{
	public class CategoryModel
	{
		public int Id { get; set; }

		[Required, MaxLength(100)]
		public string Name { get; set; } = string.Empty;

	
	}
}
