using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neon.Data.Migrations.Bases;

public abstract class SqlMigration : Migration
{
    private readonly string[] _sqlFolderNames;

    protected SqlMigration(params string[] sqlFolderNames)
    {
        _sqlFolderNames = sqlFolderNames;
    }

    protected override void Up(MigrationBuilder migrationBuilder) => SqlFromFile(migrationBuilder);
    protected override void Down(MigrationBuilder migrationBuilder) => SqlFromFile(migrationBuilder);

    private void SqlFromFile(
        MigrationBuilder migrationBuilder,
        [CallerFilePath] string filePath = null!,
        [CallerMemberName] string fileName = null!)
    {
        foreach (var sqlFolderName in _sqlFolderNames)
        {
            migrationBuilder.Sql(
                File.ReadAllText(
                    Path.GetFullPath(
                        Path.Combine(filePath, "../../../SQL", sqlFolderName, fileName + ".sql"))));
        }
    }
}