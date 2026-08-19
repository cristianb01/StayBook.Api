using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StayBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHostUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT IGNORE INTO `Users` (`Id`, `Email`, `PasswordHash`, `Role`, `UserName`) VALUES (1, 'alice@staybook.com', '$2a$11$MpFpbSFLtd5hKUoiTrSWsep.NlFQ6jVYcFX1pSnJ5SZMJPMjT.Wp6', 1, 'host-alice');");
            migrationBuilder.Sql("INSERT IGNORE INTO `Users` (`Id`, `Email`, `PasswordHash`, `Role`, `UserName`) VALUES (2, 'bruno@staybook.com', '$2a$11$MpFpbSFLtd5hKUoiTrSWsep.NlFQ6jVYcFX1pSnJ5SZMJPMjT.Wp6', 1, 'host-bruno');");
            migrationBuilder.Sql("INSERT IGNORE INTO `Users` (`Id`, `Email`, `PasswordHash`, `Role`, `UserName`) VALUES (3, 'carmen@staybook.com', '$2a$11$MpFpbSFLtd5hKUoiTrSWsep.NlFQ6jVYcFX1pSnJ5SZMJPMjT.Wp6', 1, 'host-carmen');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
