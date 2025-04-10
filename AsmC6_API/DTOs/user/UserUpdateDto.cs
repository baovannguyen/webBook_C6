namespace AsmC6_API.DTOs.user
{
	public class UserUpdateDto
	{
		public string FullName { get; set; } = string.Empty;
		public string? PhoneNumber { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Address { get; set; }
	}
}
