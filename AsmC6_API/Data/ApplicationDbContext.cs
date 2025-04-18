using AsmC6_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AsmC6_API.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<UserModel> Users => Set<UserModel>();
		public DbSet<BookModel> Books => Set<BookModel>();
		public DbSet<OrderModel> Orders => Set<OrderModel>();
		public DbSet<OrderItemModel> OrderItems => Set<OrderItemModel>();
		public DbSet<CategoryModel> Categories => Set<CategoryModel>();
		public DbSet<CartItemModel> CartItems { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			

			modelBuilder.Entity<UserModel>().HasIndex(u => u.Username).IsUnique();
			modelBuilder.Entity<OrderItemModel>()
				.HasOne(i => i.Book)
				.WithMany()
				.HasForeignKey(i => i.BookId);
		}
	}
}
