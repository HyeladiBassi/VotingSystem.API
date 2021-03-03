using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VotingSystem.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    ElectionDate = table.Column<DateTime>(nullable: false),
                    ElectionType = table.Column<string>(nullable: true),
                    TotalVotes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NationalId = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    DOB = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserRole = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ElectionId = table.Column<int>(nullable: true),
                    CanApply = table.Column<bool>(nullable: true),
                    IsVerified = table.Column<bool>(nullable: true),
                    Occupation = table.Column<string>(nullable: true),
                    PoliticalParty = table.Column<string>(nullable: true),
                    TotalVotes = table.Column<int>(nullable: true),
                    ParticipatingIn = table.Column<int>(nullable: true),
                    CanVote = table.Column<bool>(nullable: true),
                    HasVoted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ElectionId",
                table: "Users",
                column: "ElectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Elections");
        }
    }
}
