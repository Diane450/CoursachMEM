using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace coursach.Models;

public partial class Ispr2438MageramovEmCoursachContext : DbContext
{
    public Ispr2438MageramovEmCoursachContext()
    {
    }

    public Ispr2438MageramovEmCoursachContext(DbContextOptions<Ispr2438MageramovEmCoursachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeInformation> EmployeeInformations { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=cfif31.ru;Database=ISPr24-38_MageramovEM_Coursach;Uid=ISPr24-38_MageramovEM;Pwd=ISPr24-38_MageramovEM");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Salt).HasMaxLength(255);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.RoleId, "FK_Employee_Role_idx");

            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Sallt).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Employee_Role");
        });

        modelBuilder.Entity<EmployeeInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("EmployeeInformation");

            entity.HasIndex(e => e.EmployeeId, "FK_Information_Employee_idx");

            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.FullName).HasMaxLength(60);
            entity.Property(e => e.Phone).HasMaxLength(45);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeInformations)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Information_Employee");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Request");

            entity.HasIndex(e => e.ClientId, "FK_Request_Client_idx");

            entity.HasIndex(e => e.EmployeeInfId, "FK_Request_EmployeeInf_idx");

            entity.HasIndex(e => e.StatusId, "FK_Request_Status_idx");

            entity.Property(e => e.CreationDate).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.FullNameClient).HasMaxLength(45);
            entity.Property(e => e.Phone).HasMaxLength(45);
            entity.Property(e => e.TakeDate).HasColumnType("date");
            entity.Property(e => e.TechnicalTask).HasColumnType("text");

            entity.HasOne(d => d.Client).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Request_Client");

            entity.HasOne(d => d.EmployeeInf).WithMany(p => p.Requests)
                .HasForeignKey(d => d.EmployeeInfId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Request_EmployeeInf");

            entity.HasOne(d => d.Status).WithMany(p => p.Requests)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Request_Status");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Status");

            entity.Property(e => e.Name).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
