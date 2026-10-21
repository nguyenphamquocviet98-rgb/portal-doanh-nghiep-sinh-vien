using InternshipPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Faculty> Faculties => Set<Faculty>();
    public DbSet<Major> Majors => Set<Major>();
    public DbSet<InternshipPeriod> InternshipPeriods => Set<InternshipPeriod>();
    public DbSet<InternshipPosition> InternshipPositions => Set<InternshipPosition>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Internship> Internships => Set<Internship>();
    public DbSet<InternshipDiary> InternshipDiaries => Set<InternshipDiary>();
    public DbSet<InternshipDocument> InternshipDocuments => Set<InternshipDocument>();
    public DbSet<Evaluation> Evaluations => Set<Evaluation>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigureIdentityLengths(builder);
        builder.Entity<ApplicationUser>().HasIndex(x => x.NormalizedEmail)
            .HasDatabaseName("EmailIndex").IsUnique().HasFilter("[NormalizedEmail] IS NOT NULL");

        builder.Entity<Faculty>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<Major>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<Major>().HasOne(x => x.Faculty).WithMany().HasForeignKey(x => x.FacultyId);

        builder.Entity<Student>().HasIndex(x => x.StudentCode).IsUnique();
        builder.Entity<Student>().HasOne(x => x.User).WithOne().HasForeignKey<Student>(x => x.UserId);
        builder.Entity<Student>().HasOne(x => x.Major).WithMany().HasForeignKey(x => x.MajorId);
        builder.Entity<Student>().Property(x => x.Gpa).HasPrecision(4, 2);
        builder.Entity<Student>().ToTable(t => t.HasCheckConstraint("CK_Students_Gpa", "[Gpa] IS NULL OR ([Gpa] >= 0 AND [Gpa] <= 4.00)"));
        builder.Entity<Company>().HasOne(x => x.User).WithOne().HasForeignKey<Company>(x => x.UserId);
        builder.Entity<Company>().ToTable(t => t.HasCheckConstraint("CK_Companies_ApprovalStatus", "[ApprovalStatus] IN (0,1,2)"));

        builder.Entity<InternshipPeriod>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<InternshipPeriod>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_InternshipPeriods_Dates", "[StartDate] <= [EndDate]");
            t.HasCheckConstraint("CK_InternshipPeriods_Status", "[Status] IN (0,1,2)");
        });

        builder.Entity<InternshipPosition>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<InternshipPosition>().HasIndex(x => new { x.Status, x.ApplicationDeadline });
        builder.Entity<InternshipPosition>().HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
        builder.Entity<InternshipPosition>().HasOne(x => x.Major).WithMany().HasForeignKey(x => x.MajorId);
        builder.Entity<InternshipPosition>().HasOne(x => x.InternshipPeriod).WithMany().HasForeignKey(x => x.InternshipPeriodId);
        builder.Entity<InternshipPosition>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_InternshipPositions_Quantity", "[Quantity] > 0");
            t.HasCheckConstraint("CK_InternshipPositions_Dates", "[StartDate] <= [EndDate]");
            t.HasCheckConstraint("CK_InternshipPositions_Status", "[Status] IN (0,1)");
            t.HasCheckConstraint("CK_InternshipPositions_WorkMode", "[WorkMode] IN (0,1,2)");
        });

        builder.Entity<Application>().HasIndex(x => new { x.StudentId, x.InternshipPositionId }).IsUnique();
        builder.Entity<Application>().HasIndex(x => new { x.InternshipPositionId, x.Status });
        builder.Entity<Application>().HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId);
        builder.Entity<Application>().HasOne(x => x.InternshipPosition).WithMany().HasForeignKey(x => x.InternshipPositionId);
        builder.Entity<Application>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_Applications_Status", "[Status] IN (0,1,2,3,4,5,6)");
            t.HasCheckConstraint("CK_Applications_RejectionNote", "[Status] <> 4 OR ([Note] IS NOT NULL AND LEN(LTRIM(RTRIM([Note]))) > 0)");
        });

        builder.Entity<Internship>().HasOne(x => x.Application).WithOne().HasForeignKey<Internship>(x => x.ApplicationId);
        builder.Entity<Internship>().HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId);
        builder.Entity<Internship>().HasOne(x => x.InternshipPeriod).WithMany().HasForeignKey(x => x.InternshipPeriodId);
        builder.Entity<Internship>().HasIndex(x => new { x.StudentId, x.InternshipPeriodId }).IsUnique();
        builder.Entity<Internship>().HasIndex(x => new { x.InternshipPeriodId, x.Status });
        builder.Entity<Internship>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_Internships_Dates", "[StartDate] <= [EndDate]");
            t.HasCheckConstraint("CK_Internships_Status", "[Status] IN (0,1,2,3)");
        });

        builder.Entity<InternshipDiary>().HasOne(x => x.Internship).WithMany().HasForeignKey(x => x.InternshipId);
        builder.Entity<InternshipDiary>().HasIndex(x => new { x.InternshipId, x.WeekNumber }).IsUnique();
        builder.Entity<InternshipDiary>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_InternshipDiaries_Week", "[WeekNumber] > 0");
            t.HasCheckConstraint("CK_InternshipDiaries_Dates", "[StartDate] <= [EndDate]");
        });
        builder.Entity<InternshipDocument>().HasOne(x => x.Internship).WithMany().HasForeignKey(x => x.InternshipId);
        builder.Entity<InternshipDocument>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_InternshipDocuments_FileSize", "[FileSize] > 0");
            t.HasCheckConstraint("CK_InternshipDocuments_Type", "[DocumentType] IN (0,1)");
        });

        builder.Entity<Evaluation>().HasOne(x => x.Internship).WithOne().HasForeignKey<Evaluation>(x => x.InternshipId);
        var scoreColumns = new[] { nameof(Evaluation.ProfessionalKnowledge), nameof(Evaluation.WorkSkills),
            nameof(Evaluation.Attitude), nameof(Evaluation.Responsibility), nameof(Evaluation.Communication), nameof(Evaluation.TaskCompletion) };
        foreach (var column in scoreColumns)
        {
            builder.Entity<Evaluation>().Property<decimal>(column).HasPrecision(4, 2);
            builder.Entity<Evaluation>().ToTable(t => t.HasCheckConstraint($"CK_Evaluations_{column}", $"[{column}] BETWEEN 0 AND 10"));
        }

        builder.Entity<Notification>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        builder.Entity<Notification>().HasIndex(x => new { x.UserId, x.IsRead, x.CreatedAt });
        builder.Entity<Notification>().HasIndex(x => new { x.UserId, x.DeduplicationKey })
            .IsUnique().HasFilter("[DeduplicationKey] IS NOT NULL");
        builder.Entity<Notification>().ToTable(t => t.HasCheckConstraint("CK_Notifications_Type", "[Type] IN (0,1,2,3,4,5,6,7,8)"));

        // Chặn xóa dây chuyền để giữ lịch sử và tránh nhiều đường cascade trên SQL Server.
        foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
    }

    private static void ConfigureIdentityLengths(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(user =>
        {
            user.Property(x => x.Id).HasMaxLength(440);
            user.Property(x => x.UserName).HasMaxLength(246);
            user.Property(x => x.NormalizedUserName).HasMaxLength(246);
            user.Property(x => x.Email).HasMaxLength(246);
            user.Property(x => x.NormalizedEmail).HasMaxLength(246);
            user.Property(x => x.PasswordHash).HasMaxLength(200);
            user.Property(x => x.SecurityStamp).HasMaxLength(200);
            user.Property(x => x.ConcurrencyStamp).HasMaxLength(200);
            user.Property(x => x.PhoneNumber).HasMaxLength(30);
        });
        builder.Entity<IdentityRole>(role =>
        {
            role.Property(x => x.Id).HasMaxLength(440);
            role.Property(x => x.Name).HasMaxLength(246);
            role.Property(x => x.NormalizedName).HasMaxLength(246);
            role.Property(x => x.ConcurrencyStamp).HasMaxLength(200);
        });
        builder.Entity<IdentityUserClaim<string>>(claim =>
        {
            claim.Property(x => x.UserId).HasMaxLength(440);
            claim.Property(x => x.ClaimType).HasMaxLength(200);
            claim.Property(x => x.ClaimValue).HasMaxLength(1990);
        });
        builder.Entity<IdentityRoleClaim<string>>(claim =>
        {
            claim.Property(x => x.RoleId).HasMaxLength(440);
            claim.Property(x => x.ClaimType).HasMaxLength(200);
            claim.Property(x => x.ClaimValue).HasMaxLength(1990);
        });
        builder.Entity<IdentityUserLogin<string>>(login =>
        {
            login.Property(x => x.UserId).HasMaxLength(440);
            login.Property(x => x.LoginProvider).HasMaxLength(440);
            login.Property(x => x.ProviderKey).HasMaxLength(440);
            login.Property(x => x.ProviderDisplayName).HasMaxLength(200);
        });
        builder.Entity<IdentityUserRole<string>>(role =>
        {
            role.Property(x => x.UserId).HasMaxLength(440);
            role.Property(x => x.RoleId).HasMaxLength(440);
        });
        builder.Entity<IdentityUserToken<string>>(token =>
        {
            token.Property(x => x.UserId).HasMaxLength(440);
            token.Property(x => x.LoginProvider).HasMaxLength(440);
            token.Property(x => x.Name).HasMaxLength(440);
            token.Property(x => x.Value).HasMaxLength(1990);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Modified))
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        return base.SaveChangesAsync(cancellationToken);
    }
}
