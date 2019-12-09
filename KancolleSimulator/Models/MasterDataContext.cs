using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KancolleSimulator.Models
{
    public partial class MasterDataContext : DbContext
    {
        public MasterDataContext()
        {
        }

        public MasterDataContext(DbContextOptions<MasterDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ships> Ships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=MasterData.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ships>(entity =>
            {
                entity.HasKey(e => e.ShipId);

                entity.HasIndex(e => e.ShipId)
                    .IsUnique();

                entity.Property(e => e.ShipId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
