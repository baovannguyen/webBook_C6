namespace AsmC6_API.DTOs.user
{
	public class UserUpdateAdminDto
	{
		public string FullName { get; set; } = string.Empty;
		public string? PhoneNumber { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Address { get; set; }
		public string Role { get; set; } = string.Empty;
	}
}
