using LKDin.Server.Domain;
using Microsoft.EntityFrameworkCore;

namespace LKDin.Server.DataAccess
{
    public class LKDinContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<WorkProfile> WorkProfiles { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public string DbPath { get; }

        public LKDinContext()
        {
            var folder = Environment.SpecialFolder.ApplicationData;

            var path = Environment.GetFolderPath(folder);

            DbPath = Path.Join(path, "LKDin.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .Property(c => c.Version)
                .HasDefaultValue(0)
                .IsRowVersion();

            modelBuilder
                .Entity<WorkProfile>()
                .Property(c => c.Version)
                .HasDefaultValue(0)
                .IsRowVersion();

            modelBuilder
                .Entity<ChatMessage>()
                .HasOne(cm => cm.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(cm => cm.SenderId);

            modelBuilder
                .Entity<ChatMessage>()
                .HasOne(cm => cm.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(cm => cm.ReceiverId);

            modelBuilder.Entity<ChatMessage>()
                .Property(c => c.Version)
                .HasDefaultValue(0)
                .IsRowVersion();

        }
    }
}
