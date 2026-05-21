using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StayBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Description", "HostId", "Name" },
                values: new object[,]
                {
                    { 1, "A beautiful villa with ocean views.", 1, "Seaside Villa" },
                    { 2, "Cozy cabin surrounded by pine trees.", 1, "Mountain Cabin" },
                    { 3, "Modern loft in the heart of downtown.", 2, "City Loft" },
                    { 4, "Quiet cottage in the countryside.", 2, "Country Cottage" },
                    { 5, "Relaxing retreat by the lake.", 3, "Lakehouse Retreat" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
