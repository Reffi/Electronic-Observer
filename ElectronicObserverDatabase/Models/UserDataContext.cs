using Microsoft.EntityFrameworkCore;

namespace ElectronicObserverDatabase.Models
{
    public partial class UserDataContext : DbContext
    {
        public string DbPath { get; }

        public UserDataContext()
        {
        }

        public UserDataContext(string dbPath)
        {
            DbPath = dbPath;
        }

        public UserDataContext(DbContextOptions<UserDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserShipData> UserShipData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite($@"Data Source={DbPath}\UserData.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserShipData>(entity =>
            {
                entity.HasKey(e => e.DropId);

                entity.HasIndex(e => e.DropId)
                    .IsUnique();

                entity.Property(e => e.DropId)
                    .ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
