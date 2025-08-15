using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBE2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_AccountId",
                table: "Gatherings");

            migrationBuilder.DropIndex(
                name: "IX_Gatherings_AccountId",
                table: "Gatherings");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Gatherings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Gatherings",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gatherings_AccountId",
                table: "Gatherings",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gatherings_AspNetUsers_AccountId",
                table: "Gatherings",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
