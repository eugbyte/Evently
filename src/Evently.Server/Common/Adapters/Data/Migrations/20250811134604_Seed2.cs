using System;
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
            migrationBuilder.InsertData(
                table: "Gatherings",
                columns: new[] { "GatheringId", "CoverSrc", "Description", "End", "Location", "Name", "OrganiserId", "Start" },
                values: new object[] { 1L, "", "Meet and Greet", new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", "The Great Fair", 1L, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Gatherings",
                keyColumn: "GatheringId",
                keyValue: 1L);
        }
    }
}
