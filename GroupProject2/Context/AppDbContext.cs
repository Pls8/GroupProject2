using GroupProject2.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject2.Context
{
    public class AppDbContext : DbContext
    {
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Data Source = . ; " +
                "database = RepaireDB; " +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=True;" +
                "Trust Server Certificate=True;";
            optionsBuilder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customer table + relation to RepairOrder
            modelBuilder.Entity<CustomerClass>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(c => c.CustomerId);

                entity.Property(c => c.Name)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(c => c.Phone)
                      .HasMaxLength(50);

                entity.Property(c => c.Address)
                      .HasMaxLength(500);

                entity.HasMany(c => c.RepairOrders)
                      .WithOne(r => r.Customer)
                      .HasForeignKey(r => r.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Technician table + optional relation to RepairOrder
            modelBuilder.Entity<TechnicianClass>(entity =>
            {
                entity.ToTable("Technician");
                entity.HasKey(t => t.TechnicianId);

                entity.Property(t => t.Name).HasMaxLength(200);
                entity.Property(t => t.Phone).HasMaxLength(50);
                entity.Property(t => t.Specialty).HasMaxLength(200);

                entity.HasMany(t => t.RepairOrders)
                      .WithOne(r => r.Technician)
                      .HasForeignKey(r => r.TechnicianId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // RepairOrder table and its relations
            modelBuilder.Entity<RepairOrderClass>(entity =>
            {
                entity.ToTable("RepairOrder");
                entity.HasKey(r => r.RepairOrderId);

                entity.Property(r => r.ApplianceType).HasMaxLength(200);
                entity.Property(r => r.ProblemDescription).HasMaxLength(2000);
                entity.Property(r => r.DateCreated).IsRequired();

                // Ensure FK to Customer is configured (required)
                entity.HasOne(r => r.Customer)
                      .WithMany(c => c.RepairOrders)
                      .HasForeignKey(r => r.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Optional FK to Technician
                entity.HasOne(r => r.Technician)
                      .WithMany(t => t.RepairOrders)
                      .HasForeignKey(r => r.TechnicianId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);

                // One-to-one with Invoice (Invoice.RepairOrderId is the FK)
                entity.HasOne(r => r.Invoice)
                      .WithOne(i => i.RepairOrder)
                      .HasForeignKey<InvoiceClass>(i => i.RepairOrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Invoice table
            modelBuilder.Entity<InvoiceClass>(entity =>
            {
                entity.ToTable("Invoice");
                entity.HasKey(i => i.InvoiceId);

                entity.Property(i => i.ServiceCost)
                      .HasColumnType("decimal(18,2)");

                entity.Property(i => i.PartsCost)
                      .HasColumnType("decimal(18,2)");

                entity.Property(i => i.InvoiceDate).IsRequired();

                // enforce uniqueness of RepairOrderId to guarantee 1:1
                entity.HasIndex(i => i.RepairOrderId).IsUnique();
            });

            // RepairPart table
            modelBuilder.Entity<RepairPartClass>(entity =>
            {
                entity.ToTable("RepairPart");
                entity.HasKey(p => p.PartId);

                entity.Property(p => p.PartName).HasMaxLength(200);
                entity.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
            });

            // OrderPart join table configuration
            modelBuilder.Entity<OrderPartClass>(entity =>
            {
                entity.ToTable("OrderPart");
                entity.HasKey(op => op.OrderPartId);

                entity.HasOne(op => op.RepairOrder)
                      .WithMany(r => r.OrderParts)
                      .HasForeignKey(op => op.RepairOrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(op => op.RepairPart)
                      .WithMany(p => p.OrderParts)
                      .HasForeignKey(op => op.PartId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(op => op.Quantity).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<CustomerClass> Customers { get; set; }
        public DbSet<TechnicianClass> Technicians { get; set; }
        public DbSet<RepairOrderClass> RepairOrders { get; set; }
        public DbSet<InvoiceClass> Invoices { get; set; }
        public DbSet<RepairPartClass> RepairParts { get; set; }
        public DbSet<OrderPartClass> OrderParts { get; set; }
    }
}
