using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Server.Models
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public virtual DbSet<ConcurrentSessionsEveryHour> ConcurrentSessionsEveryHours { get; set; }

        public virtual DbSet<ConcurrentUniqueSessionsWithMultipleDevice> ConcurrentUniqueSessionsWithMultipleDevices
        {
            get;
            set;
        }

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
                optionsBuilder.UseSqlServer("Server=localhost;Database=Contoso_Authentication_Logs;User Id=sa;Password=Password123;"); // connection string to my local db
                // optionsBuilder.UseSqlServer("Server=db;Database=Contoso_Authentication_Logs;User Id=sa;Password=Your_password123;"); // connection string to my db container instance
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<ConcurrentSessionsEveryHour>(entity =>
            {
                entity.ToTable("ConcurrentSessionsEveryHour");
                entity.HasIndex(e => e.Hour, "UQ__Concurre__6D0E39A45D2E4F6F").IsUnique();
                entity.Property(e => e.ConcurrentSessionsEveryHourId).HasColumnName("ConcurrentSessionsEveryHourID");
                entity.Property(e => e.Hour).HasColumnType("datetime");
            });
            modelBuilder.Entity<ConcurrentUniqueSessionsWithMultipleDevice>(entity =>
            {
                entity.HasKey(e => e.ConcurrentUniqueSessionsWithMultipleDevicesId)
                    .HasName("PK__Concurre__C7A3CA1573F9BE3B");
                entity.Property(e => e.ConcurrentUniqueSessionsWithMultipleDevicesId)
                    .HasColumnName("ConcurrentUniqueSessionsWithMultipleDevicesID");
                entity.Property(e => e.DeviceName).HasMaxLength(50);
                entity.Property(e => e.LoginTs).HasColumnType("datetime").HasColumnName("LoginTS");
                entity.Property(e => e.UserName).HasMaxLength(50);
            });
            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.HasKey(e => e.DeviceId).HasName("PK__DeviceTy__49E12331C70483BE");
                entity.Property(e => e.DeviceId).ValueGeneratedOnAdd().HasColumnName("DeviceID");
                entity.Property(e => e.DeviceName).HasMaxLength(20);
            });
            modelBuilder.Entity<Month>(entity =>
            {
                entity.Property(e => e.MonthId).ValueGeneratedOnAdd().HasColumnName("MonthID");
                entity.Property(e => e.MonthName).HasMaxLength(20);
            });
            modelBuilder.Entity<RegistrationCountByDevicesAndMonth>(entity =>
            {
                entity.ToTable("RegistrationCountByDevicesAndMonth");
                entity.Property(e => e.RegistrationCountByDevicesAndMonthId)
                    .HasColumnName("RegistrationCountByDevicesAndMonthID");
                entity.HasOne(d => d.DeviceTypeNavigation).WithMany(p => p.RegistrationCountByDevicesAndMonths)
                    .HasForeignKey(d => d.DeviceType).HasConstraintName("FK__Registrat__Devic__2C3393D0");
                entity.HasOne(d => d.MonthNavigation).WithMany(p => p.RegistrationCountByDevicesAndMonths)
                    .HasForeignKey(d => d.Month).HasConstraintName("FK__Registrat__Month__2B3F6F97");
            });
            modelBuilder.Entity<RegistrationCountByMonth>(entity =>
            {
                entity.ToTable("RegistrationCountByMonth");
                entity.Property(e => e.RegistrationCountByMonthId).HasColumnName("RegistrationCountByMonthID");
                entity.HasOne(d => d.MonthNavigation).WithMany(p => p.RegistrationCountByMonths)
                    .HasForeignKey(d => d.Month).HasConstraintName("FK__Registrat__Month__286302EC");
            });
            modelBuilder.Entity<TotalSessionDurationByHour>(entity =>
            {
                entity.ToTable("TotalSessionDurationByHour");
                entity.Property(e => e.TotalSessionDurationByHourId).HasColumnName("TotalSessionDurationByHourID");
                entity.Property(e => e.Date).HasColumnType("date");
            });
            modelBuilder.Entity<UniqueCountriesByDay>(entity =>
            {
                entity.ToTable("UniqueCountriesByDay");
                entity.Property(e => e.UniqueCountriesByDayId).HasColumnName("UniqueCountriesByDayID");
                entity.Property(e => e.Country).HasMaxLength(30);
                entity.Property(e => e.LoginTs).HasColumnType("datetime").HasColumnName("LoginTS");
                entity.Property(e => e.UserName).HasMaxLength(50);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}