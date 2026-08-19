using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixConversationBookingRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Bookings_BookingId1",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_BookingId1",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "Conversations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId1",
                table: "Conversations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_BookingId1",
                table: "Conversations",
                column: "BookingId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Bookings_BookingId1",
                table: "Conversations",
                column: "BookingId1",
                principalTable: "Bookings",
                principalColumn: "Id");
        }
    }
}
