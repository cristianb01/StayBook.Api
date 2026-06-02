using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReconcileModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `OutboxMessages` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Type` longtext NOT NULL,
                    `Payload` longtext NOT NULL,
                    `OccurredOn` datetime(6) NOT NULL,
                    `ProcessedOn` datetime(6) NULL,
                    CONSTRAINT `PK_OutboxMessages` PRIMARY KEY (`Id`)
                ) CHARACTER SET=utf8mb4;");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Properties` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `HostId` int NOT NULL,
                    `Name` longtext NOT NULL,
                    `Description` longtext NOT NULL,
                    CONSTRAINT `PK_Properties` PRIMARY KEY (`Id`)
                ) CHARACTER SET=utf8mb4;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.Sql("DROP TABLE IF EXISTS `OutboxMessages`;");
        }
    }
}
