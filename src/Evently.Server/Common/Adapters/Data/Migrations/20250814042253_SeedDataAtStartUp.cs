using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataAtStartUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_AccountId",
                table: "Gatherings");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1L);

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Gatherings",
                newName: "HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Gatherings_AccountId",
                table: "Gatherings",
                newName: "IX_Gatherings_HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gatherings_AspNetUsers_HostId",
                table: "Gatherings",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_HostId",
                table: "Gatherings");

            migrationBuilder.RenameColumn(
                name: "HostId",
                table: "Gatherings",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Gatherings_HostId",
                table: "Gatherings",
                newName: "IX_Gatherings_AccountId");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Approved", "CategoryName" },
                values: new object[] { 1L, false, "Information Technology" });

            migrationBuilder.AddForeignKey(
                name: "FK_Gatherings_AspNetUsers_AccountId",
                table: "Gatherings",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
