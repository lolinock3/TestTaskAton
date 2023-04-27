using Microsoft.EntityFrameworkCore;

namespace TestTask.Models
{
    public class DataContext:DbContext
    {
        public DataContext (DbContextOptions<DataContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new User
            {
                Guid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Login = "admin",
                Password = "admin",
                Birthday = null,
                CreatedBy = "admin",
                CreatedOn = DateTime.Now,
                Gender = 1,
                IsAdmin = true,
                Name = "Admin",
                ModifiedOn = null,
                ModifiedBy = null,
                RevokedBy = null,
                RevokedOn = null
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UserDb;Trusted_Connection=True;");
        }
    }
}
