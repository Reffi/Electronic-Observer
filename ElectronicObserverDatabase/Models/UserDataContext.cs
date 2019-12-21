using Microsoft.EntityFrameworkCore;

namespace ElectronicObserverDatabase.Models
{
    public partial class UserDataContext : DbContext
    {
        public UserDataContext()
        {
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
                optionsBuilder.UseSqlite("Data Source=UserData.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserShipData>(entity =>
            {
                entity.HasKey(e => e.ShipId);

                entity.HasIndex(e => e.ShipId)
                    .IsUnique();

                entity.Property(e => e.ShipId)
                    .ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
