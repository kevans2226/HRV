using System;
using HRV.Data.Db;
using Microsoft.EntityFrameworkCore;
using Range = HRV.Data.Db.Range;

namespace HRV.Data
{
    public class HRVDb : DbContext
    {
        public HRVDb(DbContextOptions<HRVDb> options) : base (options) { }

        public DbSet<HRVRecord> HRVs { get; set; }
        public DbSet<Range> Ranges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HRVRecord>(hrvRecords =>
            {
                hrvRecords.HasKey(h => h.Id);
                hrvRecords.Property(h => h.HRV).IsRequired();
                hrvRecords.Property(h => h.Date).IsRequired();
                hrvRecords.Property(h => h.UserSid).IsRequired();
            });

            modelBuilder.Entity<Range>(range =>
            {
                range.HasKey(r => r.RangeId);
                range.Property(r => r.Name).IsRequired();
                range.Property(r => r.Low).IsRequired();
                range.Property(r => r.High).IsRequired();
                range.Property(r => r.Name).HasMaxLength(25);

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
