using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServerMM.Models;

namespace ServerMM
{
    public class SqliteDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<SensorData> SensorData { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserOptions> UserOptions { get; set; }


        public SqliteDBContext(DbContextOptions<SqliteDBContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<UserOptions>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasOne(u => u.User)
                      .WithOne(u => u.userOptions)
                      .HasForeignKey<UserOptions>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.DeviceId);
                entity.HasIndex(e => e.SerialNumber).IsUnique();

            });

            modelBuilder.Entity<SensorData>(entity =>
            {
                entity.HasKey(e => e.DataId);
            });

            modelBuilder.Entity<Alert>(entity =>
            {
                entity.HasKey(e => e.AlertId);

            });

            modelBuilder.Entity<Recommendation>(entity =>
            {
                entity.HasKey(e => e.RecommendationId);

            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => e.LoginId);

            });
        }
    }
}
