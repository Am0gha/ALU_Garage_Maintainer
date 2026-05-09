using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ALU_DAL.Models;

public partial class AluContext : DbContext
{
    public AluContext()
    {
    }

    public AluContext(DbContextOptions<AluContext> options)
        : base(options)
    {
    }
    public virtual DbSet<FuelEip> FuelEips { get; set; }
    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Rarity> Rarities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data source=(localdb)\\MSSQLLocalDB;Initial catalog = ALU;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cars__3213E83F2488124B");

            entity.ToTable("cars");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Acceleration)
                .HasColumnType("numeric(4, 2)")
                .HasColumnName("acceleration");
            entity.Property(e => e.Bp1sCount).HasColumnName("bp_1s_count");
            entity.Property(e => e.Bp2sCount).HasColumnName("bp_2s_count");
            entity.Property(e => e.Bp3sCount).HasColumnName("bp_3s_count");
            entity.Property(e => e.Bp4sCount).HasColumnName("bp_4s_count");
            entity.Property(e => e.Bp5sCount).HasColumnName("bp_5s_count");
            entity.Property(e => e.Bp6sCount).HasColumnName("bp_6s_count");
            entity.Property(e => e.Class)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("class");
            entity.Property(e => e.Fuel).HasColumnName("fuel");
            entity.Property(e => e.Handling)
                .HasColumnType("numeric(4, 2)")
                .HasColumnName("handling");
            entity.Property(e => e.HasEips).HasColumnName("has_eips");
            entity.Property(e => e.MaxRank)
                .HasColumnType("numeric(4, 0)")
                .HasColumnName("max_rank");
            entity.Property(e => e.MaxStars).HasColumnName("max_stars");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Nitro)
                .HasColumnType("numeric(4, 2)")
                .HasColumnName("nitro");
            entity.Property(e => e.NoEips)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("no_eips");
            entity.Property(e => e.Rarity)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("rarity");
            entity.Property(e => e.RequiresKey).HasColumnName("requires_key");
            entity.Property(e => e.TopSpeed)
                .HasColumnType("numeric(4, 1)")
                .HasColumnName("top_speed");

            entity.HasOne(d => d.ClassNavigation).WithMany(p => p.Cars)
                .HasForeignKey(d => d.Class)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_class");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Class1).HasName("PK__class__71DF78EC74084A12");

            entity.ToTable("class");

            entity.Property(e => e.Class1)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("class");
            entity.Property(e => e.MaxFuel).HasColumnName("max_fuel");
            entity.Property(e => e.MaxStars).HasColumnName("max_stars");
            entity.Property(e => e.MinFuel).HasColumnName("min_fuel");
            entity.Property(e => e.MinStars).HasColumnName("min_stars");
            entity.Property(e => e.ValidRarity).HasColumnName("valid_rarity");
        });

        modelBuilder.Entity<Rarity>(entity =>
        {
            entity.HasKey(e => e.Rarity1).HasName("PK__rarity__068B639D7C20BBB1");

            entity.ToTable("rarity");

            entity.Property(e => e.Rarity1)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("rarity");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<FuelEip>().HasNoKey();
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
