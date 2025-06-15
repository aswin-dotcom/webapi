using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) { }
       
        public DbSet<Record> Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<Record>().HasData(
                    new Record() { 
                        Id = 1,
                        Name = "aswin",
                        percentage="100%",
                        Standard="College",
                        City = "Coimbatore"

                    },
                    new Record()
                    {
                        Id = 2,
                        Name = "sherin",
                        percentage = "100%",
                        Standard = "College",
                        City = "katpadi"

                    },
                    new Record()
                    {
                        Id = 3,
                        Name = "edwin",
                        percentage = "100%",
                        Standard = "Driver",
                        City = "Komarpalayam"


                    },
                    new Record()
                    {
                        Id = 4,
                        Name = "shanthi",
                        percentage = "100%",
                        Standard = "school",
                        City = "Komarpalayam"

                    }



                );
        }
    }
}
