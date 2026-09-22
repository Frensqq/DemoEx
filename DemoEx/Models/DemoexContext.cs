using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoEx.Models;

public partial class DemoexContext : DbContext
{
    public DemoexContext()
    {
    }

    public DemoexContext(DbContextOptions<DemoexContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DetailOrder> DetailOrders { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Stok> Stoks { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=frens-project.tech;Port=5432;Database=demoex;Username=frens;Password=adminadmin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pk");

            entity.ToTable("category");

            entity.HasIndex(e => e.Name, "category_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<DetailOrder>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId, e.SizeId }).HasName("detail_order_pk");

            entity.ToTable("detail_order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.Count).HasColumnName("count");

            entity.HasOne(d => d.Order).WithMany(p => p.DetailOrders)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("detail_order_order_fk");

            entity.HasOne(d => d.Product).WithMany(p => p.DetailOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("detail_order_product_fk");

            entity.HasOne(d => d.Size).WithMany(p => p.DetailOrders)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("detail_order_sizes_fk");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufacturers_pk");

            entity.ToTable("manufacturers");

            entity.HasIndex(e => e.Name, "manufacturers_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pk");

            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");

            entity.HasOne(d => d.LoginNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Login)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("order_user_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pk");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.ManufactureId).HasColumnName("manufacture_id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Structure)
                .HasColumnType("character varying")
                .HasColumnName("structure");
            entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("product_category_fk");

            entity.HasOne(d => d.Manufacture).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufactureId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("product_manufacturers_fk");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubcategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("product_sub_category_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pk");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "role_unique").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('\"Role_id_seq\"'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sizes_pk");

            entity.ToTable("sizes");

            entity.HasIndex(e => e.Name, "sizes_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Stok>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stok_pk");

            entity.ToTable("stok");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SizeId).HasColumnName("size_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Stoks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("stok_product_fk");

            entity.HasOne(d => d.Size).WithMany(p => p.Stoks)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("stok_sizes_fk");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_category_pk");

            entity.ToTable("sub_category");

            entity.HasIndex(e => e.Name, "sub_category_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Login).HasName("user_pk");

            entity.ToTable("user");

            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.IdRole)
                .HasDefaultValue(1)
                .HasColumnName("id_role");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Patronimyc)
                .HasColumnType("character varying")
                .HasColumnName("patronimyc");
            entity.Property(e => e.Surname)
                .HasColumnType("character varying")
                .HasColumnName("surname");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
