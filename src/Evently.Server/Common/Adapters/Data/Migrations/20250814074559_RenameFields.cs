using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_HostId",
                table: "Gatherings");

            migrationBuilder.RenameColumn(
                name: "HostId",
                table: "Gatherings",
                newName: "OrganiserId");

            migrationBuilder.RenameIndex(
                name: "IX_Gatherings_HostId",
                table: "Gatherings",
                newName: "IX_Gatherings_OrganiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gatherings_AspNetUsers_OrganiserId",
                table: "Gatherings",
                column: "OrganiserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_OrganiserId",
                table: "Gatherings");

            migrationBuilder.RenameColumn(
                name: "OrganiserId",
                table: "Gatherings",
                newName: "HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Gatherings_OrganiserId",
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
    }
}
