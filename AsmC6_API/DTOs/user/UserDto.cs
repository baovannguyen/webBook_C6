namespace AsmC6_API.DTOs.user
{
	public class UserDto
	{
		public int Id { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string? PhoneNumber { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Address { get; set; }
	}
}
