using DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataLayer
{

    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=CartDemo;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             
             modelBuilder.Entity<User>().HasData(new User
             {
                 UserId =1,
                 Username = "TestUser",
                 Password = "tstUser*",
                 Family = "TestUserFamily",
                 IsEnabled = true
             });
             modelBuilder.Entity<Product>().HasData(new Product
             {
                 DateCreated = DateTime.Now,
                 DateDeleted = null,
                 Id = 1,
                 Name = "product1",
                 ImageName = "img",
                 Describtion = "productDescription",
                 IsEnabled = true,
                 CartDetails = new List<CartDetail>()
             }, new Product
             {
                 DateCreated = DateTime.Now,
                 DateDeleted = null,
                 Id = 2,
                 Name = "product2",
                 ImageName = "img2",
                 Describtion = "productDescription2",
                 IsEnabled = true,
                 CartDetails = new List<CartDetail>()
             }, new Product
             {
                 DateCreated = DateTime.Now,
                 DateDeleted = null,
                 Id = 3,
                 Name = "product3",
                 ImageName = "img3",
                 Describtion = "productDescription3",
                 IsEnabled = true,
                 CartDetails = new List<CartDetail>()
             });
            base.OnModelCreating(modelBuilder);
        }
    }
}