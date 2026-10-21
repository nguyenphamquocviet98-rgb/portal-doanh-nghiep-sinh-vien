using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipPortal.Migrations
{
    /// <inheritdoc />
    public partial class BoundStringLengths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Students] WHERE DATALENGTH([UserId]) > 440 * 2) THROW 51001, N'Data exceeds new length: Students.UserId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Students] WHERE DATALENGTH([Skills]) > 990 * 2) THROW 51001, N'Data exceeds new length: Students.Skills', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Students] WHERE DATALENGTH([CvPath]) > 490 * 2) THROW 51001, N'Data exceeds new length: Students.CvPath', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Students] WHERE DATALENGTH([Bio]) > 1990 * 2) THROW 51001, N'Data exceeds new length: Students.Bio', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Notifications] WHERE DATALENGTH([UserId]) > 440 * 2) THROW 51001, N'Data exceeds new length: Notifications.UserId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Notifications] WHERE DATALENGTH([TargetUrl]) > 490 * 2) THROW 51001, N'Data exceeds new length: Notifications.TargetUrl', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Notifications] WHERE DATALENGTH([Message]) > 1990 * 2) THROW 51001, N'Data exceeds new length: Notifications.Message', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Internships] WHERE DATALENGTH([SupervisorEmail]) > 246 * 2) THROW 51001, N'Data exceeds new length: Internships.SupervisorEmail', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [InternshipPositions] WHERE DATALENGTH([Requirements]) > 1990 * 2) THROW 51001, N'Data exceeds new length: InternshipPositions.Requirements', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [InternshipPositions] WHERE DATALENGTH([Description]) > 2990 * 2) THROW 51001, N'Data exceeds new length: InternshipPositions.Description', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [InternshipDocuments] WHERE DATALENGTH([OriginalFileName]) > 200 * 2) THROW 51001, N'Data exceeds new length: InternshipDocuments.OriginalFileName', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [InternshipDocuments] WHERE DATALENGTH([FilePath]) > 490 * 2) THROW 51001, N'Data exceeds new length: InternshipDocuments.FilePath', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [InternshipDiaries] WHERE DATALENGTH([WorkContent]) > 2990 * 2) THROW 51001, N'Data exceeds new length: InternshipDiaries.WorkContent', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [InternshipDiaries] WHERE DATALENGTH([Results]) > 2990 * 2) THROW 51001, N'Data exceeds new length: InternshipDiaries.Results', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [InternshipDiaries] WHERE DATALENGTH([Note]) > 1990 * 2) THROW 51001, N'Data exceeds new length: InternshipDiaries.Note', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [InternshipDiaries] WHERE DATALENGTH([Difficulties]) > 2990 * 2) THROW 51001, N'Data exceeds new length: InternshipDiaries.Difficulties', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Evaluations] WHERE DATALENGTH([Comment]) > 2990 * 2) THROW 51001, N'Data exceeds new length: Evaluations.Comment', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Companies] WHERE DATALENGTH([Website]) > 290 * 2) THROW 51001, N'Data exceeds new length: Companies.Website', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Companies] WHERE DATALENGTH([UserId]) > 440 * 2) THROW 51001, N'Data exceeds new length: Companies.UserId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Companies] WHERE DATALENGTH([Description]) > 1990 * 2) THROW 51001, N'Data exceeds new length: Companies.Description', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Companies] WHERE DATALENGTH([ApprovalNote]) > 990 * 2) THROW 51001, N'Data exceeds new length: Companies.ApprovalNote', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Companies] WHERE DATALENGTH([Address]) > 290 * 2) THROW 51001, N'Data exceeds new length: Companies.Address', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserTokens] WHERE DATALENGTH([Value]) > 1990 * 2) THROW 51001, N'Data exceeds new length: AspNetUserTokens.Value', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserTokens] WHERE DATALENGTH([Name]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserTokens.Name', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserTokens] WHERE DATALENGTH([LoginProvider]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserTokens.LoginProvider', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserTokens] WHERE DATALENGTH([UserId]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserTokens.UserId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([UserName]) > 246 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.UserName', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([SecurityStamp]) > 200 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.SecurityStamp', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([PhoneNumber]) > 30 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.PhoneNumber', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([PasswordHash]) > 200 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.PasswordHash', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([NormalizedUserName]) > 246 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.NormalizedUserName', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([NormalizedEmail]) > 246 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.NormalizedEmail', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([Email]) > 246 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.Email', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([ConcurrencyStamp]) > 200 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.ConcurrencyStamp', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([AvatarPath]) > 490 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.AvatarPath', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE DATALENGTH([Id]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUsers.Id', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE DATALENGTH([RoleId]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserRoles.RoleId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE DATALENGTH([UserId]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserRoles.UserId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserLogins] WHERE DATALENGTH([UserId]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserLogins.UserId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserLogins] WHERE DATALENGTH([ProviderDisplayName]) > 200 * 2) THROW 51001, N'Data exceeds new length: AspNetUserLogins.ProviderDisplayName', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserLogins] WHERE DATALENGTH([ProviderKey]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserLogins.ProviderKey', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserLogins] WHERE DATALENGTH([LoginProvider]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserLogins.LoginProvider', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserClaims] WHERE DATALENGTH([UserId]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetUserClaims.UserId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserClaims] WHERE DATALENGTH([ClaimValue]) > 1990 * 2) THROW 51001, N'Data exceeds new length: AspNetUserClaims.ClaimValue', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetUserClaims] WHERE DATALENGTH([ClaimType]) > 200 * 2) THROW 51001, N'Data exceeds new length: AspNetUserClaims.ClaimType', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetRoles] WHERE DATALENGTH([NormalizedName]) > 246 * 2) THROW 51001, N'Data exceeds new length: AspNetRoles.NormalizedName', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetRoles] WHERE DATALENGTH([Name]) > 246 * 2) THROW 51001, N'Data exceeds new length: AspNetRoles.Name', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetRoles] WHERE DATALENGTH([ConcurrencyStamp]) > 200 * 2) THROW 51001, N'Data exceeds new length: AspNetRoles.ConcurrencyStamp', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetRoles] WHERE DATALENGTH([Id]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetRoles.Id', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetRoleClaims] WHERE DATALENGTH([RoleId]) > 440 * 2) THROW 51001, N'Data exceeds new length: AspNetRoleClaims.RoleId', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetRoleClaims] WHERE DATALENGTH([ClaimValue]) > 1990 * 2) THROW 51001, N'Data exceeds new length: AspNetRoleClaims.ClaimValue', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [AspNetRoleClaims] WHERE DATALENGTH([ClaimType]) > 200 * 2) THROW 51001, N'Data exceeds new length: AspNetRoleClaims.ClaimType', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Applications] WHERE DATALENGTH([Note]) > 1990 * 2) THROW 51001, N'Data exceeds new length: Applications.Note', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Applications] WHERE DATALENGTH([CvPath]) > 490 * 2) THROW 51001, N'Data exceeds new length: Applications.CvPath', 1;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM [Applications] WHERE DATALENGTH([CoverLetter]) > 1990 * 2) THROW 51001, N'Data exceeds new length: Applications.CoverLetter', 1;");

            DropIdentityConstraints(migrationBuilder);
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Students",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Students",
                type: "nvarchar(990)",
                maxLength: 990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CvPath",
                table: "Students",
                type: "nvarchar(490)",
                maxLength: 490,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Students",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Notifications",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "TargetUrl",
                table: "Notifications",
                type: "nvarchar(490)",
                maxLength: 490,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "SupervisorEmail",
                table: "Internships",
                type: "nvarchar(246)",
                maxLength: 246,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "InternshipPositions",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InternshipPositions",
                type: "nvarchar(2990)",
                maxLength: 2990,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalFileName",
                table: "InternshipDocuments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "InternshipDocuments",
                type: "nvarchar(490)",
                maxLength: 490,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "WorkContent",
                table: "InternshipDiaries",
                type: "nvarchar(2990)",
                maxLength: 2990,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Results",
                table: "InternshipDiaries",
                type: "nvarchar(2990)",
                maxLength: 2990,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "InternshipDiaries",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Difficulties",
                table: "InternshipDiaries",
                type: "nvarchar(2990)",
                maxLength: 2990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Evaluations",
                type: "nvarchar(2990)",
                maxLength: 2990,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Companies",
                type: "nvarchar(290)",
                maxLength: 290,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Companies",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Companies",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalNote",
                table: "Companies",
                type: "nvarchar(990)",
                maxLength: 990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(290)",
                maxLength: 290,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AspNetUserTokens",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(246)",
                maxLength: 246,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "nvarchar(246)",
                maxLength: 246,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "nvarchar(246)",
                maxLength: 246,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(246)",
                maxLength: 246,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarPath",
                table: "AspNetUsers",
                type: "nvarchar(490)",
                maxLength: 490,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "AspNetUserLogins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetUserClaims",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetUserClaims",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "AspNetRoles",
                type: "nvarchar(246)",
                maxLength: 246,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "nvarchar(246)",
                maxLength: 246,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetRoles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "nvarchar(440)",
                maxLength: 440,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetRoleClaims",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetRoleClaims",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Applications",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CvPath",
                table: "Applications",
                type: "nvarchar(490)",
                maxLength: 490,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CoverLetter",
                table: "Applications",
                type: "nvarchar(1990)",
                maxLength: 1990,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
            RestoreIdentityConstraints(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            DropIdentityConstraints(migrationBuilder);
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Students",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Students",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(990)",
                oldMaxLength: 990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CvPath",
                table: "Students",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(490)",
                oldMaxLength: 490,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Students",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Notifications",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "TargetUrl",
                table: "Notifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(490)",
                oldMaxLength: 490,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990);

            migrationBuilder.AlterColumn<string>(
                name: "SupervisorEmail",
                table: "Internships",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(246)",
                oldMaxLength: 246,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "InternshipPositions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "InternshipPositions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2990)",
                oldMaxLength: 2990);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalFileName",
                table: "InternshipDocuments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "InternshipDocuments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(490)",
                oldMaxLength: 490);

            migrationBuilder.AlterColumn<string>(
                name: "WorkContent",
                table: "InternshipDiaries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2990)",
                oldMaxLength: 2990);

            migrationBuilder.AlterColumn<string>(
                name: "Results",
                table: "InternshipDiaries",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2990)",
                oldMaxLength: 2990);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "InternshipDiaries",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Difficulties",
                table: "InternshipDiaries",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2990)",
                oldMaxLength: 2990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Evaluations",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2990)",
                oldMaxLength: 2990);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Companies",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(290)",
                oldMaxLength: 290,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Companies",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalNote",
                table: "Companies",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(990)",
                oldMaxLength: 990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(290)",
                oldMaxLength: 290);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AspNetUserTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(246)",
                oldMaxLength: 246,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(246)",
                oldMaxLength: 246,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(246)",
                oldMaxLength: 246,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(246)",
                oldMaxLength: 246,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarPath",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(490)",
                oldMaxLength: 490,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "AspNetUserLogins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetUserClaims",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetUserClaims",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(246)",
                oldMaxLength: 246,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(246)",
                oldMaxLength: 246,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(440)",
                oldMaxLength: 440);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "AspNetRoleClaims",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "AspNetRoleClaims",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Applications",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CvPath",
                table: "Applications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(490)",
                oldMaxLength: 490);

            migrationBuilder.AlterColumn<string>(
                name: "CoverLetter",
                table: "Applications",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1990)",
                oldMaxLength: 1990,
                oldNullable: true);
            RestoreIdentityConstraints(migrationBuilder);
        }

        // Đổi độ dài khóa phải tháo FK/PK rồi tạo lại; giữ nguyên quan hệ và dữ liệu.
        private static readonly (string Table, string Column, string Principal)[] IdentityLinks =
        {
            ("Students", "UserId", "AspNetUsers"),
            ("Companies", "UserId", "AspNetUsers"),
            ("Notifications", "UserId", "AspNetUsers"),
            ("AspNetUserClaims", "UserId", "AspNetUsers"),
            ("AspNetUserLogins", "UserId", "AspNetUsers"),
            ("AspNetUserRoles", "UserId", "AspNetUsers"),
            ("AspNetUserTokens", "UserId", "AspNetUsers"),
            ("AspNetRoleClaims", "RoleId", "AspNetRoles"),
            ("AspNetUserRoles", "RoleId", "AspNetRoles")
        };

        private static readonly (string Table, string[] Columns)[] IdentityKeys =
        {
            ("AspNetUsers", new[] { "Id" }),
            ("AspNetRoles", new[] { "Id" }),
            ("AspNetUserRoles", new[] { "UserId", "RoleId" }),
            ("AspNetUserLogins", new[] { "LoginProvider", "ProviderKey" }),
            ("AspNetUserTokens", new[] { "UserId", "LoginProvider", "Name" })
        };

        private static void DropIdentityConstraints(MigrationBuilder migrationBuilder)
        {
            foreach (var link in IdentityLinks)
                migrationBuilder.DropForeignKey($"FK_{link.Table}_{link.Principal}_{link.Column}", link.Table);
            foreach (var key in IdentityKeys)
                migrationBuilder.DropPrimaryKey($"PK_{key.Table}", key.Table);
        }

        private static void RestoreIdentityConstraints(MigrationBuilder migrationBuilder)
        {
            foreach (var key in IdentityKeys)
                migrationBuilder.AddPrimaryKey($"PK_{key.Table}", key.Table, key.Columns);
            foreach (var link in IdentityLinks)
                migrationBuilder.AddForeignKey($"FK_{link.Table}_{link.Principal}_{link.Column}",
                    link.Table, link.Column, link.Principal, principalColumn: "Id", onDelete: ReferentialAction.NoAction);
        }
    }
}
