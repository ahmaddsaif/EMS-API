using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EventManagementSystem.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<RSVP> RSVPs { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Many-to-Many Relationship between Events and Categories
        modelBuilder.Entity<EventCategory>()
        .HasKey(ec => new { ec.EventId, ec.CategoryId });

        modelBuilder.Entity<EventCategory>()
            .HasOne(ec => ec.Event)
            .WithMany(e => e.EventCategories)
            .HasForeignKey(ec => ec.EventId);

        modelBuilder.Entity<EventCategory>()
            .HasOne(ec => ec.Category)
            .WithMany(c => c.EventCategories)
            .HasForeignKey(ec => ec.CategoryId);

        // Notification -> User: Disable cascade delete
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // Notification -> Event: Disable cascade delete
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Event)
            .WithMany(e => e.Notifications)
            .HasForeignKey(n => n.EventId)
            .OnDelete(DeleteBehavior.SetNull); // Set EventId to null if event is deleted

        // RSVP -> User: Disable cascade delete
        modelBuilder.Entity<RSVP>()
            .HasOne(n => n.User)
            .WithMany(u => u.RSVPs)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        modelBuilder.Entity<User>().HasData(new User
        {
            UserId = 1,
            Name = "Saif",
            Email = "asaif5822@gmail.com",
            Password = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("admin@321"))),
            Role = Role.SuperAdmin,
            MustChangePassword = false
        });

        base.OnModelCreating(modelBuilder);
    }
}
