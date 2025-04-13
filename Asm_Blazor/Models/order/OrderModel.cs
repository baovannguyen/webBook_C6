namespace AsmC6_API.DTOs.order
{
	public class OrderDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string? UserName { get; set; } // Chỉ lấy tên user
		public DateTime OrderDate { get; set; }
		public string Status { get; set; } = "Pending";
		public decimal TotalPrice { get; set; }

		public List<OrderItemModel> Items { get; set; } = new();

/*
		public int UserId { get; set; }
		
		public List<OrderItemDto> Items { get; set; } = new();*/
	}
}
