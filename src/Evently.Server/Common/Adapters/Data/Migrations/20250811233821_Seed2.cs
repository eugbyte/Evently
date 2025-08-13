using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberCategoryDetails_Categories_CategoryId",
                table: "MemberCategoryDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberCategoryDetails_Gatherings_GatheringId",
                table: "MemberCategoryDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberCategoryDetails",
                table: "MemberCategoryDetails");

            migrationBuilder.RenameTable(
                name: "MemberCategoryDetails",
                newName: "GatheringCategoryDetails");

            migrationBuilder.RenameIndex(
                name: "IX_MemberCategoryDetails_CategoryId",
                table: "GatheringCategoryDetails",
                newName: "IX_GatheringCategoryDetails_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GatheringCategoryDetails",
                table: "GatheringCategoryDetails",
                columns: new[] { "GatheringId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GatheringCategoryDetails_Categories_CategoryId",
                table: "GatheringCategoryDetails",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GatheringCategoryDetails_Gatherings_GatheringId",
                table: "GatheringCategoryDetails",
                column: "GatheringId",
                principalTable: "Gatherings",
                principalColumn: "GatheringId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatheringCategoryDetails_Categories_CategoryId",
                table: "GatheringCategoryDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GatheringCategoryDetails_Gatherings_GatheringId",
                table: "GatheringCategoryDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GatheringCategoryDetails",
                table: "GatheringCategoryDetails");

            migrationBuilder.RenameTable(
                name: "GatheringCategoryDetails",
                newName: "MemberCategoryDetails");

            migrationBuilder.RenameIndex(
                name: "IX_GatheringCategoryDetails_CategoryId",
                table: "MemberCategoryDetails",
                newName: "IX_MemberCategoryDetails_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberCategoryDetails",
                table: "MemberCategoryDetails",
                columns: new[] { "GatheringId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MemberCategoryDetails_Categories_CategoryId",
                table: "MemberCategoryDetails",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberCategoryDetails_Gatherings_GatheringId",
                table: "MemberCategoryDetails",
                column: "GatheringId",
                principalTable: "Gatherings",
                principalColumn: "GatheringId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
