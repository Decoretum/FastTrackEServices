namespace FastTrackEServices.Data;
using Microsoft.EntityFrameworkCore;
using FastTrackEServices.Model;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Client> Clients {get; set;}
    public DbSet<ShoewareRepair> ShoewareRepairs {get; set;}
    public DbSet<Shoeware> Shoewares {get; set;}
    public DbSet<OwnedShoeware> OwnedShoewares {get; set;}
    public DbSet<ShoewareColor> ShoewareColors {get; set;}
    public DbSet<OrderCart> OrderCarts {get; set;}
    public DbSet<ShoewareOrder> ShoewareOrders {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoewareRepair>()
        .Property(sp => sp.dateConfirmed)
        .IsRequired(false);
    }
}