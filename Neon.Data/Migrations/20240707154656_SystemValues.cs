using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neon.Data.Migrations
{
    /// <inheritdoc />
    public partial class SystemValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemValues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemValues", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemValues");
        }
    }
}
