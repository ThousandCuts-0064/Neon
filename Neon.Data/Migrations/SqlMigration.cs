using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neon.Data.Migrations;

public abstract class SqlMigration : Migration
{
    private readonly string _sqlFolderName;

    protected SqlMigration(string sqlFolderName) => _sqlFolderName = sqlFolderName;

    protected override void Up(MigrationBuilder migrationBuilder) => SqlFromFile(migrationBuilder);
    protected override void Down(MigrationBuilder migrationBuilder) => SqlFromFile(migrationBuilder);

    private void SqlFromFile(
        MigrationBuilder migrationBuilder,
        [CallerFilePath] string filePath = null!,
        [CallerMemberName] string fileName = null!)
    {
        migrationBuilder.Sql(
            File.ReadAllText(
                Path.GetFullPath(
                    Path.Combine(
                        filePath, "../../SQL", _sqlFolderName, fileName + ".sql"))));
    }
}