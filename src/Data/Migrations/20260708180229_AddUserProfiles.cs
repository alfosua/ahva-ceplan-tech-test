using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahva.Ceplan.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstNames = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastNamePaternal = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastNameMaternal = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    SecondaryEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SecondaryPhoneType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SecondaryPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ContractType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Entity = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PideValidationStatus = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DocumentType_DocumentNumber",
                table: "UserProfiles",
                columns: new[] { "DocumentType", "DocumentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
