using System.ComponentModel.DataAnnotations;

namespace AsmC6_API.Models
{
	public class UserModel
	{


		public int Id { get; set; }

		[Required, MaxLength(100)]
		public string Username { get; set; } = string.Empty;

		[Required]
		public string PasswordHash { get; set; } = string.Empty;

		[Required]
		public string Role { get; set; } = "Customer"; 

		[Required, MaxLength(100)]
		public string FullName { get; set; } = string.Empty;

		[Phone, MaxLength(20)]
		public string? PhoneNumber { get; set; }

		[DataType(DataType.Date)]
		public DateTime? DateOfBirth { get; set; }

		[MaxLength(255)]
		public string? Address { get; set; }

		public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
	}
}

