using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seed3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GatheringCategoryDetails",
                columns: new[] { "CategoryId", "GatheringId" },
                values: new object[] { 1L, 1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GatheringCategoryDetails",
                keyColumns: new[] { "CategoryId", "GatheringId" },
                keyValues: new object[] { 1L, 1L });
        }
    }
}
