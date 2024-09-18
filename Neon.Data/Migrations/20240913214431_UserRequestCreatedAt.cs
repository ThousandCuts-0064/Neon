using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neon.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserRequestCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TradeRequests",
                type: "timestamp with time zone",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FriendRequests",
                type: "timestamp with time zone",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DuelRequests",
                type: "timestamp with time zone",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_User1Id",
                table: "Friendships",
                column: "User1Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friendships_User1Id",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TradeRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FriendRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DuelRequests");
        }
    }
}
