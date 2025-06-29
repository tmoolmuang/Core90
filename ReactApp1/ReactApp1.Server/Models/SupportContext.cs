using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReactApp1.Server.Models;

public partial class SupportContext : DbContext
{
    public SupportContext()
    {
    }

    public SupportContext(DbContextOptions<SupportContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspnetApplication> AspnetApplications { get; set; }

    public virtual DbSet<AspnetRole> AspnetRoles { get; set; }

    public virtual DbSet<AspnetUser> AspnetUsers { get; set; }

    public virtual DbSet<AspnetUsersInRole> AspnetUsersInRoles { get; set; }

    public virtual DbSet<LuPermission> LuPermissions { get; set; }

    public virtual DbSet<PermissionInRole> PermissionInRoles { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=SupportDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspnetApplication>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("aspnet_Applications");

            entity.Property(e => e.ApplicationName).HasMaxLength(256);
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.LoweredApplicationName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspnetRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasFillFactor(80);

            entity.ToTable("aspnet_Roles");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.LoweredRoleName).HasMaxLength(256);
            entity.Property(e => e.RoleName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspnetUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("aspnet_Users");

            entity.Property(e => e.LastActivityDate).HasColumnType("datetime");
            entity.Property(e => e.LoweredUserName).HasMaxLength(256);
            entity.Property(e => e.MobileAlias).HasMaxLength(16);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspnetUsersInRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("aspnet_UsersInRoles");
        });

        modelBuilder.Entity<LuPermission>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("lu_Permission");

            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.PermissionDescription)
                .HasMaxLength(127)
                .IsUnicode(false);
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.PermissionTitle)
                .HasMaxLength(63)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PermissionInRole>(entity =>
        {
            entity.HasKey(e => e.PermissionInRoleId).HasFillFactor(80);

            entity.ToTable("PermissionInRole");

            entity.Property(e => e.PermissionInRoleId).HasColumnName("PermissionInRoleID");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasFillFactor(80);

            entity.ToTable("Profile");

            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.OneHealthcareUuid)
                .HasMaxLength(50)
                .HasColumnName("OneHealthcareUUID");
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
