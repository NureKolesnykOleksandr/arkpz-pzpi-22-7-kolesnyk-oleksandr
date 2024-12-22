using Microsoft.EntityFrameworkCore;
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

        // Конструктор, принимающий DbContextOptions
        public SqliteDBContext(DbContextOptions<SqliteDBContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Этот метод можно оставить пустым или использовать для дополнительной конфигурации
            // optionsBuilder.UseSqlite("Data Source=MedMon.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.DeviceID);
                entity.HasIndex(e => e.SerialNumber).IsUnique();
                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserID);
            });

            modelBuilder.Entity<SensorData>(entity =>
            {
                entity.HasKey(e => e.DataID);
                entity.HasOne<Device>().WithMany().HasForeignKey(e => e.DeviceID);
                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserID);
            });

            modelBuilder.Entity<Alert>(entity =>
            {
                entity.HasKey(e => e.AlertID);
                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserID);
            });

            modelBuilder.Entity<Recommendation>(entity =>
            {
                entity.HasKey(e => e.RecommendationID);
                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserID);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => e.LoginID);
                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserID);
            });
        }
    }
}
