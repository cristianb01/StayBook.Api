using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingGuestAndHostIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bookings",
                newName: "GuestId");

            migrationBuilder.AddColumn<int>(
                name: "HostId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE Bookings b
                INNER JOIN Properties p ON p.Id = b.PropertyId
                SET b.HostId = p.HostId;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "GuestId",
                table: "Bookings",
                newName: "UserId");
        }
    }
}
