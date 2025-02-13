using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EMS.DataAccess.Models;

public partial class EmsDbContext : DbContext
{
    public EmsDbContext(DbContextOptions<EmsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Audit> Audits { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CompanyType> CompanyTypes { get; set; }

    public virtual DbSet<Display> Displays { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<IncidentCategory> IncidentCategories { get; set; }

    public virtual DbSet<IncidentStatus> IncidentStatuses { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Replacement> Replacements { get; set; }

    public virtual DbSet<RequestType> RequestTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleModuleMapper> RoleModuleMappers { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRoleMapper> UserRoleMappers { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Audit__3213E83F662B0D23");

            entity.ToTable("Audit");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(1024)
                .HasColumnName("description");
            entity.Property(e => e.ModuleId).HasColumnName("moduleId");
            entity.Property(e => e.TimeStamp)
                .HasColumnType("datetime")
                .HasColumnName("timeStamp");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.WebActivity)
                .HasMaxLength(50)
                .HasColumnName("webActivity");

            entity.HasOne(d => d.Module).WithMany(p => p.Audits)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Audit__moduleId__6EC0713C");

            entity.HasOne(d => d.User).WithMany(p => p.Audits)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Audit__userId__6FB49575");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F2F2985D0");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CompanyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyC__3213E83FFACB4F7D");

            entity.ToTable("CompanyType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Display>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Display");

            entity.Property(e => e.ApiKey).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__IncidentId");

            entity.ToTable("Incident");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.AdminId).HasColumnName("adminId");
            entity.Property(e => e.ArrivalDateTime)
                .HasColumnType("datetime")
                .HasColumnName("arrivalDateTime");
            entity.Property(e => e.Company)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.CompanyTypeId).HasColumnName("companyTypeId");
            entity.Property(e => e.CompletedDateTime)
                .HasColumnType("datetime")
                .HasColumnName("completedDateTime");
            entity.Property(e => e.Customer)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("customer");
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("customerPhone");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IncidentCategoryId).HasColumnName("incidentCategoryId");
            entity.Property(e => e.IncidentCreatedDateTime)
                .HasColumnType("datetime")
                .HasColumnName("incidentCreatedDateTime");
            entity.Property(e => e.IncidentStatusId).HasColumnName("incidentStatusId");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("ipAddress");
            entity.Property(e => e.RefNum)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasComputedColumnSql("(CONVERT([varchar](4),datepart(year,[incidentCreatedDateTime]))+right('00000'+CONVERT([varchar](5),[ID]),(5)))", true)
                .HasColumnName("refNum");
            entity.Property(e => e.RequestTypeId).HasColumnName("requestTypeId");
            entity.Property(e => e.ResponseDateTime)
                .HasColumnType("datetime")
                .HasColumnName("responseDateTime");
            entity.Property(e => e.Signature)
                .IsUnicode(false)
                .HasColumnName("signature");
            entity.Property(e => e.Solution)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("solution");
            entity.Property(e => e.SubItem)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("subItem");
            entity.Property(e => e.SubjectId).HasColumnName("subjectId");
            entity.Property(e => e.WorkOrderNo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("workOrderNo");

            entity.HasOne(d => d.Admin).WithMany(p => p.IncidentsNavigation)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incident_Admin");

            entity.HasOne(d => d.CompanyType).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.CompanyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incident__CompanyType");

            entity.HasOne(d => d.IncidentCategory).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.IncidentCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incident__IncidentCategory");

            entity.HasOne(d => d.IncidentStatus).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.IncidentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incident__IncidentStatus");

            entity.HasOne(d => d.RequestType).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.RequestTypeId)
                .HasConstraintName("FK_Incident_RequestType");

            entity.HasOne(d => d.Subject).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incident__Subject");

            entity.HasMany(d => d.Engineers).WithMany(p => p.Incidents)
                .UsingEntity<Dictionary<string, object>>(
                    "EngineerIncidentMapper",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("EngineerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EngineerIncidentMapper_User"),
                    l => l.HasOne<Incident>().WithMany()
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EngineerIncidentMapper_Incident"),
                    j =>
                    {
                        j.HasKey("IncidentId", "EngineerId");
                        j.ToTable("EngineerIncidentMapper");
                        j.IndexerProperty<int>("IncidentId").HasColumnName("incidentId");
                        j.IndexerProperty<int>("EngineerId").HasColumnName("engineerId");
                    });
        });

        modelBuilder.Entity<IncidentCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incident__3213E83F7A29A4D6");

            entity.ToTable("IncidentCategory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<IncidentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incident__3213E83F7942CD3C");

            entity.ToTable("IncidentStatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Module__3213E83FC463F9C0");

            entity.ToTable("Module");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ModuleName)
                .HasMaxLength(50)
                .HasColumnName("moduleName");
            entity.Property(e => e.Url)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Replacement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Replacem__3213E83FEB437738");

            entity.ToTable("Replacement");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IncidentId).HasColumnName("incidentId");
            entity.Property(e => e.Model)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("model");
            entity.Property(e => e.NewSerialNo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("newSerialNo");
            entity.Property(e => e.OldSerialNo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("oldSerialNo");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("remarks");

            entity.HasOne(d => d.Incident).WithMany(p => p.Replacements)
                .HasForeignKey(d => d.IncidentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Replaceme__incid__22401542");
        });

        modelBuilder.Entity<RequestType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incident__3213E83FA22245EB");

            entity.ToTable("RequestType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83FC04208E6");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RoleModuleMapper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleModu__3213E83F600208EB");

            entity.ToTable("RoleModuleMapper");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ModuleId).HasColumnName("moduleID");
            entity.Property(e => e.RoleId).HasColumnName("roleID");

            entity.HasOne(d => d.Module).WithMany(p => p.RoleModuleMappers)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModuleMapper_ModuleID");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleModuleMappers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModuleMapper_RoleID");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3213E83F31E9AAD5");

            entity.ToTable("Status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Statuses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Status_Category_FK");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3213E83F709D7B58");

            entity.ToTable("Subject");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FA69FE96A");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(320)
                .HasColumnName("emailAddress");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LoginName)
                .HasMaxLength(50)
                .HasColumnName("loginName");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("passwordSalt");
            entity.Property(e => e.UserStatusId).HasColumnName("userStatusID");

            entity.HasOne(d => d.UserStatus).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserStatusId)
                .HasConstraintName("User_UserStatus_FK");
        });

        modelBuilder.Entity<UserRoleMapper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3213E83F67D6A0A1");

            entity.ToTable("UserRoleMapper");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoleMappers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK1");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleMappers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK2");
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserStat__3213E83FE1774F53");

            entity.ToTable("UserStatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdated");
            entity.Property(e => e.Note)
                .HasColumnType("text")
                .HasColumnName("note");
            entity.Property(e => e.StatusId).HasColumnName("statusID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Status).WithMany(p => p.UserStatuses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserStatus_Status_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UserStatuses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_userID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
