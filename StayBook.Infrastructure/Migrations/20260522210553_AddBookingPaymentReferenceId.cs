using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingPaymentReferenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentReferenceId",
                table: "Bookings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentReferenceId",
                table: "Bookings");
        }
    }
}
