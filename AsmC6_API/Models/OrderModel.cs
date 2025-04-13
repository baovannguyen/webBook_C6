namespace AsmC6_API.Models
{
	public class OrderModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public UserModel? User { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public string Status { get; set; } = "ĐÃ THANH TOÁN";
		public decimal TotalPrice { get; set; }

		public ICollection<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
	}
}
