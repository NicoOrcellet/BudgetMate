using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BudgetMate.Models;

public partial class MoneyManagerContext : DbContext
{
    public MoneyManagerContext()
    {
    }

    public MoneyManagerContext(DbContextOptions<MoneyManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<MoneyTransaction> MoneyTransactions { get; set; }

    public virtual DbSet<SavingLimit> SavingLimits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()

            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("MoneyManagerContext"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B75107B6B");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MoneyTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__MoneyTra__55433A4BA840B822");

            entity.ToTable("MoneyTransaction");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(11, 2)");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TransactionDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.MoneyTransactions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MoneyTran__Categ__22751F6C");
        });

        modelBuilder.Entity<SavingLimit>(entity =>
        {
            entity.HasKey(e => e.SavingLimitId).HasName("PK__SavingLi__BAA024C0A8AC848B");

            entity.ToTable("SavingLimit");

            entity.Property(e => e.SavingLimitId).HasColumnName("SavingLimitID");
            entity.Property(e => e.Amount).HasColumnType("decimal(11, 2)");
            entity.Property(e => e.Period)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StartDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
