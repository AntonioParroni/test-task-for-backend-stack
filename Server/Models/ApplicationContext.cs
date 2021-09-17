using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Server.Models
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<ConcurrentSessionsEveryHour> ConcurrentSessionsEveryHours { get; set; }
        public virtual DbSet<ConcurrentUniqueSessionsWithMultipleDevice> ConcurrentUniqueSessionsWithMultipleDevices { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<Month> Months { get; set; }
        public virtual DbSet<RegistrationCountByDevicesAndMonth> RegistrationCountByDevicesAndMonths { get; set; }
        public virtual DbSet<RegistrationCountByMonth> RegistrationCountByMonths { get; set; }
        public virtual DbSet<TotalSessionDurationByHour> TotalSessionDurationByHours { get; set; }
        public virtual DbSet<UniqueCountriesByDay> UniqueCountriesByDays { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=Contoso_Authentication_Logs;User Id=sa;Password=Password123;");
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConcurrentSessionsEveryHour>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ConcurrentSessionsEveryHour");

                entity.HasIndex(e => e.Hour, "UQ__Concurre__6D0E39A478045F65")
                    .IsUnique();

                entity.Property(e => e.Hour).HasColumnType("datetime");
            });

            modelBuilder.Entity<ConcurrentUniqueSessionsWithMultipleDevice>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DeviceName).HasMaxLength(50);

                entity.Property(e => e.LoginTs)
                    .HasColumnType("datetime")
                    .HasColumnName("LoginTS");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.HasKey(e => e.DeviceId)
                    .HasName("PK__DeviceTy__49E12331A9DBC7E6");

                entity.Property(e => e.DeviceId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DeviceID");

                entity.Property(e => e.DeviceName).HasMaxLength(20);
            });

            modelBuilder.Entity<Month>(entity =>
            {
                entity.Property(e => e.MonthId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MonthID");

                entity.Property(e => e.MonthName).HasMaxLength(20);
            });

            modelBuilder.Entity<RegistrationCountByDevicesAndMonth>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RegistrationCountByDevicesAndMonth");

                entity.HasOne(d => d.DeviceTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.DeviceType)
                    .HasConstraintName("FK__Registrat__Devic__2A4B4B5E");

                entity.HasOne(d => d.MonthNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Month)
                    .HasConstraintName("FK__Registrat__Month__29572725");
            });

            modelBuilder.Entity<RegistrationCountByMonth>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RegistrationCountByMonth");

                entity.HasOne(d => d.MonthNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Month)
                    .HasConstraintName("FK__Registrat__Month__276EDEB3");
            });

            modelBuilder.Entity<TotalSessionDurationByHour>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TotalSessionDurationByHour");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<UniqueCountriesByDay>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UniqueCountriesByDay");

                entity.Property(e => e.Country).HasMaxLength(30);

                entity.Property(e => e.LoginTs)
                    .HasColumnType("datetime")
                    .HasColumnName("LoginTS");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
