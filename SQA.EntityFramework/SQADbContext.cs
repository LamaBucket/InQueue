using Microsoft.EntityFrameworkCore;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework;

public class SQADbContext : DbContext
{
    public DbSet<UserItem> Users { get; set; } = null!;

    public DbSet<QueueItem> Queues { get; set; } = null!;

    public DbSet<UserRoleItem> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserItem>().HasKey(e => e.Username);
        modelBuilder.Entity<QueueItem>().HasKey(e => e.QueueId);
        modelBuilder.Entity<UserRoleItem>().HasKey(e => e.Id);

        modelBuilder.Entity<UserItem>().HasOne(x => x.Role).WithMany(x => x.User).HasForeignKey(x => x.RoleId);
        modelBuilder.Entity<UserItem>().HasMany(x => x.Queues).WithOne(x => x.User).HasForeignKey(x => x.OwnerUsername);


        modelBuilder.Entity<UserItem>()
            .HasMany<QueueItem>()
            .WithMany()
            .UsingEntity<QueueRecordItem>(qri => qri.HasOne(x => x.Queue).WithMany(q => q.Records).HasForeignKey(key => key.QueueId),
            qri => qri.HasOne(x => x.User).WithMany(u => u.Records).HasForeignKey(key => key.Username)).ToTable("QueueRecords");

        base.OnModelCreating(modelBuilder);
    }

    public SQADbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}