using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipPortal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AvatarPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternshipPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipPeriods", x => x.Id);
                    table.CheckConstraint("CK_InternshipPeriods_Dates", "[StartDate] <= [EndDate]");
                    table.CheckConstraint("CK_InternshipPeriods_Status", "[Status] IN (0,1,2)");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    BusinessField = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    ApprovalNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.CheckConstraint("CK_Companies_ApprovalStatus", "[ApprovalStatus] IN (0,1,2)");
                    table.ForeignKey(
                        name: "FK_Companies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeduplicationKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.CheckConstraint("CK_Notifications_Type", "[Type] IN (0,1,2,3,4,5,6,7,8)");
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Majors_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternshipPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    MajorId = table.Column<int>(type: "int", nullable: false),
                    InternshipPeriodId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requirements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WorkMode = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipPositions", x => x.Id);
                    table.CheckConstraint("CK_InternshipPositions_Dates", "[StartDate] <= [EndDate]");
                    table.CheckConstraint("CK_InternshipPositions_Quantity", "[Quantity] > 0");
                    table.CheckConstraint("CK_InternshipPositions_Status", "[Status] IN (0,1)");
                    table.CheckConstraint("CK_InternshipPositions_WorkMode", "[WorkMode] IN (0,1,2)");
                    table.ForeignKey(
                        name: "FK_InternshipPositions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternshipPositions_InternshipPeriods_InternshipPeriodId",
                        column: x => x.InternshipPeriodId,
                        principalTable: "InternshipPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternshipPositions_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    StudentCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MajorId = table.Column<int>(type: "int", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gpa = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    CvPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    InternshipPositionId = table.Column<int>(type: "int", nullable: false),
                    CvPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CoverLetter = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.CheckConstraint("CK_Applications_RejectionNote", "[Status] <> 4 OR ([Note] IS NOT NULL AND LEN(LTRIM(RTRIM([Note]))) > 0)");
                    table.CheckConstraint("CK_Applications_Status", "[Status] IN (0,1,2,3,4,5,6)");
                    table.ForeignKey(
                        name: "FK_Applications_InternshipPositions_InternshipPositionId",
                        column: x => x.InternshipPositionId,
                        principalTable: "InternshipPositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Applications_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Internships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    InternshipPeriodId = table.Column<int>(type: "int", nullable: false),
                    SupervisorName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SupervisorEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SupervisorPhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internships", x => x.Id);
                    table.CheckConstraint("CK_Internships_Dates", "[StartDate] <= [EndDate]");
                    table.CheckConstraint("CK_Internships_Status", "[Status] IN (0,1,2,3)");
                    table.ForeignKey(
                        name: "FK_Internships_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Internships_InternshipPeriods_InternshipPeriodId",
                        column: x => x.InternshipPeriodId,
                        principalTable: "InternshipPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Internships_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternshipId = table.Column<int>(type: "int", nullable: false),
                    ProfessionalKnowledge = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    WorkSkills = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Attitude = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Responsibility = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Communication = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    TaskCompletion = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Id);
                    table.CheckConstraint("CK_Evaluations_Attitude", "[Attitude] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Evaluations_Communication", "[Communication] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Evaluations_ProfessionalKnowledge", "[ProfessionalKnowledge] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Evaluations_Responsibility", "[Responsibility] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Evaluations_TaskCompletion", "[TaskCompletion] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Evaluations_WorkSkills", "[WorkSkills] BETWEEN 0 AND 10");
                    table.ForeignKey(
                        name: "FK_Evaluations_Internships_InternshipId",
                        column: x => x.InternshipId,
                        principalTable: "Internships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternshipDiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternshipId = table.Column<int>(type: "int", nullable: false),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Results = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    Difficulties = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipDiaries", x => x.Id);
                    table.CheckConstraint("CK_InternshipDiaries_Dates", "[StartDate] <= [EndDate]");
                    table.CheckConstraint("CK_InternshipDiaries_Week", "[WeekNumber] > 0");
                    table.ForeignKey(
                        name: "FK_InternshipDiaries_Internships_InternshipId",
                        column: x => x.InternshipId,
                        principalTable: "Internships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternshipDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternshipId = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipDocuments", x => x.Id);
                    table.CheckConstraint("CK_InternshipDocuments_FileSize", "[FileSize] > 0");
                    table.CheckConstraint("CK_InternshipDocuments_Type", "[DocumentType] IN (0,1)");
                    table.ForeignKey(
                        name: "FK_InternshipDocuments_Internships_InternshipId",
                        column: x => x.InternshipId,
                        principalTable: "Internships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_InternshipPositionId_Status",
                table: "Applications",
                columns: new[] { "InternshipPositionId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_StudentId_InternshipPositionId",
                table: "Applications",
                columns: new[] { "StudentId", "InternshipPositionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_InternshipId",
                table: "Evaluations",
                column: "InternshipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_Code",
                table: "Faculties",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipDiaries_InternshipId_WeekNumber",
                table: "InternshipDiaries",
                columns: new[] { "InternshipId", "WeekNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipDocuments_InternshipId",
                table: "InternshipDocuments",
                column: "InternshipId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipPeriods_Code",
                table: "InternshipPeriods",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipPositions_Code",
                table: "InternshipPositions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipPositions_CompanyId",
                table: "InternshipPositions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipPositions_InternshipPeriodId",
                table: "InternshipPositions",
                column: "InternshipPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipPositions_MajorId",
                table: "InternshipPositions",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipPositions_Status_ApplicationDeadline",
                table: "InternshipPositions",
                columns: new[] { "Status", "ApplicationDeadline" });

            migrationBuilder.CreateIndex(
                name: "IX_Internships_ApplicationId",
                table: "Internships",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Internships_InternshipPeriodId_Status",
                table: "Internships",
                columns: new[] { "InternshipPeriodId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Internships_StudentId_InternshipPeriodId",
                table: "Internships",
                columns: new[] { "StudentId", "InternshipPeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Majors_Code",
                table: "Majors",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Majors_FacultyId",
                table: "Majors",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_DeduplicationKey",
                table: "Notifications",
                columns: new[] { "UserId", "DeduplicationKey" },
                unique: true,
                filter: "[DeduplicationKey] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead_CreatedAt",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_MajorId",
                table: "Students",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentCode",
                table: "Students",
                column: "StudentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Evaluations");

            migrationBuilder.DropTable(
                name: "InternshipDiaries");

            migrationBuilder.DropTable(
                name: "InternshipDocuments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Internships");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "InternshipPositions");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "InternshipPeriods");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Faculties");
        }
    }
}
