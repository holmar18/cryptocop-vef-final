using System.Data;
using Microsoft.EntityFrameworkCore;
using Cryptocop.Software.API.Models;

namespace Cryptocop.Software.API.Repositories.Context
{
    public class CryptoCopDbContext : DbContext
    {
        public CryptoCopDbContext(DbContextOptions<CryptoCopDbContext> options) : base(options) {}

        // Þegar taflan er búinn til, hvernig hún er vensluð samann
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1 to many Relations for User and address
             modelBuilder.Entity<User>()
                 .HasMany(a => a.Address)
                 .WithOne(b => b.User);

            // 1 to 1 Relations for User and Shopping car
            // Just initialize User and ShoppingCart in Entity and Shopping cart carries user Id 
            // according to docs that is one-to-one

            // ShoppingCart -> ShoppingCartItem
            // 1-to-many -> Item can have 1 cart but cart can have many items
             modelBuilder.Entity<ShoppingCart>()
                 .HasMany(a => a.ShoppingCartItem)
                 .WithOne(b => b.ShoppingCart);

            // 1-to-many Rel: User and PaymentCard
             modelBuilder.Entity<User>()
                 .HasMany(a => a.PaymentCard)
                 .WithOne(b => b.User);

            // 1-to-many Rel: User and Order
             modelBuilder.Entity<User>()
                 .HasMany(a => a.Order)
                 .WithOne(b => b.User);

            // 1-to-many Rel: Order and OrderItem
             modelBuilder.Entity<Order>()
                 .HasMany(a => a.OrderItem)
                 .WithOne(b => b.Order);
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<JwtToken> JwtToken { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<PaymentCard> PaymentCard { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }
        public DbSet<User> User { get; set; }
    }
}