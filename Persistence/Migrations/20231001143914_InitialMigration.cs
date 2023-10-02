using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AggregatedData",
                columns: table => new
                {
                    Tinklas = table.Column<string>(type: "text", nullable: false),
                    TotalRecords = table.Column<int>(type: "integer", nullable: false),
                    PPlusSum = table.Column<decimal>(type: "numeric", nullable: true),
                    PMinusSum = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregatedData", x => x.Tinklas);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AggregatedData");
        }
    }
}
