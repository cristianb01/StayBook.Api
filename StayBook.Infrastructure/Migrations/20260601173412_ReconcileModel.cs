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

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HostId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
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
