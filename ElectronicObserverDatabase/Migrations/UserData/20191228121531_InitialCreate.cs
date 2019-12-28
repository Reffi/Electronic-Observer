using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicObserverDatabase.Migrations.UserData
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserShipData",
                columns: table => new
                {
                    DropId = table.Column<int>(nullable: false),
                    ShipId = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShipData", x => x.DropId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserShipData_DropId",
                table: "UserShipData",
                column: "DropId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserShipData");
        }
    }
}
