using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Evently.Server.Common.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_Members_OrganiserId",
                table: "Gatherings");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DeleteData(
                table: "GatheringCategoryDetails",
                keyColumns: new[] { "CategoryId", "GatheringId" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "Gatherings",
                keyColumn: "GatheringId",
                keyValue: 1L);

            migrationBuilder.AlterColumn<string>(
                name: "OrganiserId",
                table: "Gatherings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Bookings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "LogoSrc",
                table: "AspNetUsers",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_MemberId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_AspNetUsers_OrganiserId",
                table: "Gatherings");

            migrationBuilder.DropColumn(
                name: "LogoSrc",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<long>(
                name: "OrganiserId",
                table: "Gatherings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<long>(
                name: "MemberId",
                table: "Bookings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityUserId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LogoSrc = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "Email", "IdentityUserId", "LogoSrc", "Name" },
                values: new object[] { 1L, "john.doe@gmail.com", null, "", "John Doe" });

            migrationBuilder.InsertData(
                table: "Gatherings",
                columns: new[] { "GatheringId", "CoverSrc", "Description", "End", "Location", "Name", "OrganiserId", "Start" },
                values: new object[] { 1L, "", "Meet and Greet", new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", "The Great Fair", 1L, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "GatheringCategoryDetails",
                columns: new[] { "CategoryId", "GatheringId" },
                values: new object[] { 1L, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_IdentityUserId",
                table: "Members",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gatherings_Members_OrganiserId",
                table: "Gatherings",
                column: "OrganiserId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
