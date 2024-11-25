namespace FastTrackEServices.Data;
using Microsoft.EntityFrameworkCore;
using FastTrackEServices.Model;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Client> Clients {get; set;}
    public DbSet<ShoeRepair> ShoeRepairs {get; set;}
    public DbSet<Shoe> Shoes {get; set;}
    public DbSet<OwnedShoe> OwnedShoes {get; set;}
    public DbSet<ShoeColor> ShoeColors {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoeRepair>()
        .Property(sp => sp.dateConfirmed)
        .IsRequired(false);
    }
}