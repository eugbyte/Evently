using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Members",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 1L,
                column: "Phone",
                value: "088888888");
        }
    }
}
