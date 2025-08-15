using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_OrganiserId",
                table: "Gatherings");

            migrationBuilder.DropIndex(
                name: "IX_Gatherings_OrganiserId",
                table: "Gatherings");

            migrationBuilder.RenameColumn(
                name: "RegistrationDateTime",
                table: "Bookings",
                newName: "CreationDateTime");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Gatherings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOrganiser",
                table: "Bookings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsOrganiser",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "CreationDateTime",
                table: "Bookings",
                newName: "RegistrationDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Gatherings_OrganiserId",
                table: "Gatherings",
                column: "OrganiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gatherings_AspNetUsers_OrganiserId",
                table: "Gatherings",
                column: "OrganiserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
