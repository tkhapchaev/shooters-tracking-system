using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShootersTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    UserRoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    WeaponTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_WeaponTypes_WeaponTypeId",
                        column: x => x.WeaponTypeId,
                        principalTable: "WeaponTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    WeaponId = table.Column<int>(type: "integer", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attempts_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Instructor" },
                    { 3, "Client" }
                });

            migrationBuilder.InsertData(
                table: "WeaponTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pistol" },
                    { 2, "Rifle" },
                    { 3, "Shotgun" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Password", "UserRoleId" },
                values: new object[,]
                {
                    { 1, "Admin", "mGyd58jTJYJSftGdmF8p/w==$CSSN/KvfydtzYHbK47N3tufiZ72ka3UKD9lbNTwNe7c=", 1 },
                    { 2, "Instructor", "6J8GMK/woJ7lepQ57xBAOg==$T4+RE2fJQOWMTwFft7fz+bLL4+STjR07+XWrJcN/uWk=", 2 },
                    { 3, "Client1", "xEK49XWDwgAjSF1bh3pbLA==$q8I1YPTEl/YBoFCVR3L8JS8ubC2bXluXcU5fRKAv4gQ=", 3 },
                    { 4, "Client2", "xEK49XWDwgAjSF1bh3pbLA==$q8I1YPTEl/YBoFCVR3L8JS8ubC2bXluXcU5fRKAv4gQ=", 3 },
                    { 5, "Client3", "xEK49XWDwgAjSF1bh3pbLA==$q8I1YPTEl/YBoFCVR3L8JS8ubC2bXluXcU5fRKAv4gQ=", 3 }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "Name", "WeaponTypeId" },
                values: new object[,]
                {
                    { 1, "ПМ", 1 },
                    { 2, "АК-74", 2 },
                    { 3, "Сайга-12", 3 }
                });

            migrationBuilder.InsertData(
                table: "Attempts",
                columns: new[] { "Id", "DateTime", "Score", "UserId", "WeaponId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3242), 100L, 3, 1 },
                    { 2, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3259), 90L, 3, 1 },
                    { 3, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3273), 60L, 3, 1 },
                    { 4, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3287), 50L, 3, 1 },
                    { 5, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3301), 60L, 3, 1 },
                    { 6, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3316), 100L, 3, 1 },
                    { 7, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3375), 90L, 3, 1 },
                    { 8, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3389), 70L, 3, 1 },
                    { 9, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3402), 70L, 3, 1 },
                    { 10, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3417), 60L, 3, 1 },
                    { 11, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3429), 100L, 4, 1 },
                    { 12, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3444), 90L, 4, 1 },
                    { 13, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3457), 90L, 4, 1 },
                    { 14, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3470), 90L, 4, 1 },
                    { 15, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3484), 90L, 4, 1 },
                    { 16, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3497), 90L, 4, 1 },
                    { 17, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3510), 90L, 4, 1 },
                    { 18, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3525), 80L, 4, 1 },
                    { 19, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3537), 80L, 4, 1 },
                    { 20, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3551), 70L, 4, 1 },
                    { 21, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3565), 90L, 5, 1 },
                    { 22, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3578), 90L, 5, 1 },
                    { 23, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3591), 90L, 5, 1 },
                    { 24, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3626), 90L, 5, 1 },
                    { 25, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3639), 90L, 5, 1 },
                    { 26, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3652), 90L, 5, 1 },
                    { 27, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3666), 80L, 5, 1 },
                    { 28, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3679), 80L, 5, 1 },
                    { 29, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3692), 80L, 5, 1 },
                    { 30, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3705), 80L, 5, 1 },
                    { 31, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3718), 20L, 3, 2 },
                    { 32, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3731), 30L, 3, 2 },
                    { 33, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3744), 30L, 3, 2 },
                    { 34, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3758), 20L, 3, 2 },
                    { 35, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3772), 60L, 3, 2 },
                    { 36, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3785), 70L, 3, 2 },
                    { 37, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3798), 30L, 3, 2 },
                    { 38, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3810), 20L, 3, 2 },
                    { 39, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3824), 40L, 3, 2 },
                    { 40, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3838), 40L, 3, 2 },
                    { 41, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3851), 50L, 4, 2 },
                    { 42, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3864), 50L, 4, 2 },
                    { 43, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3878), 50L, 4, 2 },
                    { 44, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3891), 60L, 4, 2 },
                    { 45, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3904), 60L, 4, 2 },
                    { 46, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3917), 70L, 4, 2 },
                    { 47, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3930), 80L, 4, 2 },
                    { 48, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3943), 80L, 4, 2 },
                    { 49, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3955), 80L, 4, 2 },
                    { 50, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3968), 20L, 4, 2 },
                    { 51, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(3981), 100L, 5, 2 },
                    { 52, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4017), 100L, 5, 2 },
                    { 53, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4030), 100L, 5, 2 },
                    { 54, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4043), 20L, 5, 2 },
                    { 55, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4056), 20L, 5, 2 },
                    { 56, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4069), 30L, 5, 2 },
                    { 57, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4082), 40L, 5, 2 },
                    { 58, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4094), 50L, 5, 2 },
                    { 59, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4108), 60L, 5, 2 },
                    { 60, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4121), 70L, 5, 2 },
                    { 61, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4134), 20L, 3, 3 },
                    { 62, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4148), 40L, 3, 3 },
                    { 63, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4160), 60L, 3, 3 },
                    { 64, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4173), 70L, 3, 3 },
                    { 65, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4186), 80L, 3, 3 },
                    { 66, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4200), 80L, 3, 3 },
                    { 67, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4213), 80L, 3, 3 },
                    { 68, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4226), 80L, 3, 3 },
                    { 69, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4240), 60L, 3, 3 },
                    { 70, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4253), 30L, 3, 3 },
                    { 71, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4265), 30L, 4, 3 },
                    { 72, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4278), 100L, 4, 3 },
                    { 73, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4291), 90L, 4, 3 },
                    { 74, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4304), 100L, 4, 3 },
                    { 75, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4317), 100L, 4, 3 },
                    { 76, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4330), 90L, 4, 3 },
                    { 77, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4369), 20L, 4, 3 },
                    { 78, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4382), 60L, 4, 3 },
                    { 79, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4395), 70L, 4, 3 },
                    { 80, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4408), 80L, 4, 3 },
                    { 81, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4421), 90L, 5, 3 },
                    { 82, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4435), 90L, 5, 3 },
                    { 83, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4447), 90L, 5, 3 },
                    { 84, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4460), 80L, 5, 3 },
                    { 85, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4473), 80L, 5, 3 },
                    { 86, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4486), 80L, 5, 3 },
                    { 87, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4499), 70L, 5, 3 },
                    { 88, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4512), 70L, 5, 3 },
                    { 89, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4525), 70L, 5, 3 },
                    { 90, new DateTime(2024, 11, 23, 22, 59, 49, 22, DateTimeKind.Utc).AddTicks(4539), 60L, 5, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attempts_UserId",
                table: "Attempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Attempts_WeaponId",
                table: "Attempts",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_WeaponTypeId",
                table: "Weapons",
                column: "WeaponTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attempts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WeaponTypes");
        }
    }
}
