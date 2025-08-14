using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_MemberId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_OrganiserId",
                table: "Gatherings");

            migrationBuilder.RenameColumn(
                name: "OrganiserId",
                table: "Gatherings",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Gatherings_OrganiserId",
                table: "Gatherings",
                newName: "IX_Gatherings_AccountId");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Bookings",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_MemberId",
                table: "Bookings",
                newName: "IX_Bookings_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_AccountId",
                table: "Bookings",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gatherings_AspNetUsers_AccountId",
                table: "Gatherings",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_AccountId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_AccountId",
                table: "Gatherings");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Gatherings",
                newName: "OrganiserId");

            migrationBuilder.RenameIndex(
                name: "IX_Gatherings_AccountId",
                table: "Gatherings",
                newName: "IX_Gatherings_OrganiserId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Bookings",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_AccountId",
                table: "Bookings",
                newName: "IX_Bookings_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_MemberId",
                table: "Bookings",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
