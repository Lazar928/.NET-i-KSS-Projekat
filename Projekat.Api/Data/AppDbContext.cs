using Microsoft.EntityFrameworkCore;
using Projekat.Api.Entities;

namespace Projekat.Api.Data;
//predstavlja vezu izmedju baze i C#
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    //tabele u bazi
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Purchase> Purchases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CASCADE DELETE:
        // kada se obriše Owner (User),
        // automatski se brišu sva njegova vozila
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Owner) // jedno vozilo ima jednog owner-a
            .WithMany(u => u.Vehicles) // jedan user moze da ima vise vozila
            .HasForeignKey(v => v.OwnerId) //fk je OwnerId
            .OnDelete(DeleteBehavior.Cascade);
    }
}
