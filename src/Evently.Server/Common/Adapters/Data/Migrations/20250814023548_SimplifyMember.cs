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

            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_IdentityUserId",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_IdentityUserId",
                table: "Members");

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyColumnType: "bigint",
                keyValue: 1L);

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Members",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Members",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Members",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Members",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Members",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Members",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Members",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Members",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Members",
                type: "text",
                nullable: true);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Gatherings",
                keyColumn: "GatheringId",
                keyValue: 1L,
                column: "OrganiserId",
                value: "ABC");

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "LogoSrc", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ABC", 0, "DEF", "john.doe@gmail.com", false, false, null, null, "John Doe", null, null, "AQAAAAIAAYagAAAAEAb7czoxSy1+HJDSlCugt0C7O8OLnDLdpK5gGlJFZ2dcA9vO4wgBASbGckRrx7SXyQ==", null, false, "DEF", false, null });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gatherings_Members_OrganiserId",
                table: "Gatherings",
                column: "OrganiserId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Gatherings_Members_OrganiserId",
                table: "Gatherings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyColumnType: "text",
                keyValue: "ABC");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Members",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MemberId",
                table: "Members",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Members",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "MemberId");

            migrationBuilder.UpdateData(
                table: "Gatherings",
                keyColumn: "GatheringId",
                keyValue: 1L,
                column: "OrganiserId",
                value: 1L);

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "Email", "IdentityUserId", "LogoSrc", "Name" },
                values: new object[] { 1L, "john.doe@gmail.com", null, "", "John Doe" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_IdentityUserId",
                table: "Members",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
