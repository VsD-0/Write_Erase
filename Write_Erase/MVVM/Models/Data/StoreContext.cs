using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Write_Erase.MVVM.Models.Data;

public partial class StoreContext : DbContext
{
    public StoreContext()
    {
    }

    public StoreContext(DbContextOptions<StoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderpickuppoint> Orderpickuppoints { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productcategory> Productcategories { get; set; }

    public virtual DbSet<Productmanufacturer> Productmanufacturers { get; set; }

    public virtual DbSet<Productname> Productnames { get; set; }

    public virtual DbSet<Productprovider> Productproviders { get; set; }

    public virtual DbSet<Productunit> Productunits { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=2,can&trA1xE;database=store", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.OrderPickupPointId, "OrderPickupPointID");

            entity.HasIndex(e => e.OrderStatusId, "OrderStatusID");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.DateOfOrder).HasColumnType("datetime");
            entity.Property(e => e.FullNameUser).HasMaxLength(100);
            entity.Property(e => e.OrderDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderPickupPointId).HasColumnName("OrderPickupPointID");
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");

            entity.HasOne(d => d.OrderPickupPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderPickupPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_ibfk_2");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_ibfk_1");
        });

        modelBuilder.Entity<Orderpickuppoint>(entity =>
        {
            entity.HasKey(e => e.PickupPointId).HasName("PRIMARY");

            entity.ToTable("orderpickuppoint");

            entity.Property(e => e.PickupPointId).HasColumnName("PickupPointID");
            entity.Property(e => e.City).HasMaxLength(80);
            entity.Property(e => e.House).HasMaxLength(20);
            entity.Property(e => e.PickupPoint).HasMaxLength(100);
            entity.Property(e => e.Street).HasMaxLength(80);
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ParticleNumber })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("orderproduct");

            entity.HasIndex(e => e.ParticleNumber, "PArticleNumber");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ParticleNumber)
                .HasMaxLength(100)
                .HasColumnName("PArticleNumber");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_ibfk_1");

            entity.HasOne(d => d.ParticleNumberNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.ParticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_ibfk_2");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("orderstatus");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ParticleNumber).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.PcategoryId, "PCategoryID");

            entity.HasIndex(e => e.PmanufacturerId, "PManufacturerID");

            entity.HasIndex(e => e.PnameId, "PNameID");

            entity.HasIndex(e => e.PproviderId, "PProviderId");

            entity.HasIndex(e => e.PunitId, "PUnitID");

            entity.Property(e => e.ParticleNumber)
                .HasMaxLength(100)
                .HasColumnName("PArticleNumber");
            entity.Property(e => e.PcategoryId).HasColumnName("PCategoryID");
            entity.Property(e => e.Pcost)
                .HasPrecision(19, 2)
                .HasColumnName("PCost");
            entity.Property(e => e.Pdescription)
                .HasColumnType("text")
                .HasColumnName("PDescription");
            entity.Property(e => e.PdiscountAmount)
                .HasPrecision(6, 2)
                .HasColumnName("PDiscountAmount");
            entity.Property(e => e.PmanufacturerId).HasColumnName("PManufacturerID");
            entity.Property(e => e.PmaxDiscount).HasColumnName("PMaxDiscount");
            entity.Property(e => e.PnameId).HasColumnName("PNameID");
            entity.Property(e => e.Pphoto)
                .HasMaxLength(50)
                .HasColumnName("PPhoto");
            entity.Property(e => e.PproviderId).HasColumnName("PProviderId");
            entity.Property(e => e.PquantityInStock).HasColumnName("PQuantityInStock");
            entity.Property(e => e.Pstatus)
                .HasColumnType("text")
                .HasColumnName("PStatus");
            entity.Property(e => e.PunitId).HasColumnName("PUnitID");

            entity.HasOne(d => d.Pcategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.PcategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_ibfk_3");

            entity.HasOne(d => d.Pmanufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.PmanufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_ibfk_1");

            entity.HasOne(d => d.Pname).WithMany(p => p.Products)
                .HasForeignKey(d => d.PnameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_ibfk_2");

            entity.HasOne(d => d.Pprovider).WithMany(p => p.Products)
                .HasForeignKey(d => d.PproviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_ibfk_4");

            entity.HasOne(d => d.Punit).WithMany(p => p.Products)
                .HasForeignKey(d => d.PunitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_ibfk_5");
        });

        modelBuilder.Entity<Productcategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("productcategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        modelBuilder.Entity<Productmanufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PRIMARY");

            entity.ToTable("productmanufacturer");

            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
        });

        modelBuilder.Entity<Productname>(entity =>
        {
            entity.HasKey(e => e.NameId).HasName("PRIMARY");

            entity.ToTable("productname");

            entity.Property(e => e.NameId).HasColumnName("NameID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Productprovider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PRIMARY");

            entity.ToTable("productprovider");

            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.Provider).HasMaxLength(100);
        });

        modelBuilder.Entity<Productunit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PRIMARY");

            entity.ToTable("productunit");

            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.Unit).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserRole, "UserRole");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserLogin).HasColumnType("text");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserPassword).HasColumnType("text");
            entity.Property(e => e.UserPatronymic).HasMaxLength(100);
            entity.Property(e => e.UserSurname).HasMaxLength(100);

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
