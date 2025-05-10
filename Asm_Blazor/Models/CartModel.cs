namespace Asm_Blazor.Models
{
	public class CartModel
	{
		public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();
		public int GrandTotal => Items.Sum(x => x.Total);
	}
}
