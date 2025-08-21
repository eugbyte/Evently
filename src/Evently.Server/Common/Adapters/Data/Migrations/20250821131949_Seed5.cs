using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seed5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "empty-user-12345",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "empty-user-12345",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "EMPTY-CONCURRENCY-STAMP-12345", "EMPTY-SECURITY-STAMP-12345" });
        }
    }
}
