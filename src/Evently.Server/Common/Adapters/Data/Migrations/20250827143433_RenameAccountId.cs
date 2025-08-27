using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_AccountId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Bookings",
                newName: "AttendeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_AccountId",
                table: "Bookings",
                newName: "IX_Bookings_AttendeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_AttendeeId",
                table: "Bookings",
                column: "AttendeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_AttendeeId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "AttendeeId",
                table: "Bookings",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_AttendeeId",
                table: "Bookings",
                newName: "IX_Bookings_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_AccountId",
                table: "Bookings",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
