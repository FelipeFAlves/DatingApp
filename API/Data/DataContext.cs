using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options) //DbCOntext é uma sessão com o BD
{
    public DbSet<AppUser> Users { get; set; } // Tabela "User" com atributos da classe "AppUser"
    public DbSet<UserLike> Likes {get; set;}
    public DbSet<Message> Messages {get; set;}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserLike>()
            .HasKey(k => new {k.SourceUserId, k.TargetUserId});
        
        builder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l =>l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLike>()
            .HasOne(s => s.TargetUser)
            .WithMany(l =>l.LikedByUsers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Message>()
            .HasOne(x => x.Recipient)
            .WithMany(x => x.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
