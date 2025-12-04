using Microsoft.EntityFrameworkCore; 
using Backend.Domain;
using Backend.Domain.Entities;
using Backend.Domain.Roles;

namespace Backend.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options)
    : base (options) 
    {}

    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>{
        b.ToTable("Users");
        b.HasKey(u=> u.Id);
        b.OwnsOne(u => u.Username, nav =>
        {
            nav.Property(v => v.Value)
            .HasColumnName("UserName")
            .IsRequired()
            .HasMaxLength(100);
        });
        
        b.OwnsOne(u => u.Password, nav =>
        {
            nav.Property(v => v.Value)
            .HasColumnName("Password")
            .IsRequired();
        });

        b.OwnsOne(u => u.Role, nav =>
        {nav.Property(r=>r.Type)
        .HasColumnName("RoleType")
        .IsRequired();
        
        nav.Property(r=>r.Permissions)
        .HasColumnName("RolePermissions")
        .HasConversion(
            v => string.Join(',', v.Select(p => p.ToString())),
            v => new HashSet<Permission>(
                v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(Enum.Parse<Permission>)
            )
        );});

        b.HasMany(u => u.Orders)
        .WithOne()
        .HasForeignKey("UserId")
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

        });

        modelBuilder.Entity<Order>(b =>
{
    b.ToTable("Orders");
    b.HasKey(o => o.Id);

    // Map VOs
    b.OwnsOne(o => o.Name, nav =>
    {
        nav.Property(n => n.Value)
           .HasColumnName("OrderName")
           .IsRequired()
           .HasMaxLength(200);
    });

    b.OwnsOne(o => o.Date, nav =>
    {
        nav.Property(d => d.Value)
           .HasColumnName("OrderDate")
           .IsRequired();
    });

    b.Property(o => o.Price)
     .HasColumnType("decimal(18,2)")
     .IsRequired();

    // Shadow FK for User
    b.Property<Guid>("UserId");
    });


}
}
