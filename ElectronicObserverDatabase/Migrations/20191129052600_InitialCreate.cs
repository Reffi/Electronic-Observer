using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicObserverDatabase.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    ShipId = table.Column<int>(nullable: false),
                    RemodelBeforeShipId = table.Column<int>(nullable: false),
                    SortId = table.Column<int>(nullable: false),
                    ShipType = table.Column<int>(nullable: false),
                    ShipClass = table.Column<int>(nullable: false),
                    ShipName = table.Column<string>(nullable: true),
                    HpMin = table.Column<int>(nullable: true),
                    HpMax = table.Column<int>(nullable: true),
                    FirepowerMin = table.Column<int>(nullable: true),
                    FirepowerMax = table.Column<int>(nullable: true),
                    TorpedoMin = table.Column<int>(nullable: true),
                    TorpedoMax = table.Column<int>(nullable: true),
                    AaMin = table.Column<int>(nullable: true),
                    AaMax = table.Column<int>(nullable: true),
                    ArmorMin = table.Column<int>(nullable: true),
                    ArmorMax = table.Column<int>(nullable: true),
                    AswMinLowerBound = table.Column<int>(nullable: true),
                    AswMinUpperBound = table.Column<int>(nullable: true),
                    AswMax = table.Column<int>(nullable: true),
                    EvasionMinLowerBound = table.Column<int>(nullable: true),
                    EvasionMinUpperBound = table.Column<int>(nullable: true),
                    EvasionMax = table.Column<int>(nullable: true),
                    LosMinLowerBound = table.Column<int>(nullable: true),
                    LosMinUpperBound = table.Column<int>(nullable: true),
                    LosMax = table.Column<int>(nullable: true),
                    LuckMin = table.Column<int>(nullable: true),
                    LuckMax = table.Column<int>(nullable: true),
                    Range = table.Column<int>(nullable: true),
                    Equipment1 = table.Column<int>(nullable: true),
                    Equipment2 = table.Column<int>(nullable: true),
                    Equipment3 = table.Column<int>(nullable: true),
                    Equipment4 = table.Column<int>(nullable: true),
                    Equipment5 = table.Column<int>(nullable: true),
                    Aircraft1 = table.Column<int>(nullable: true),
                    Aircraft2 = table.Column<int>(nullable: true),
                    Aircraft3 = table.Column<int>(nullable: true),
                    Aircraft4 = table.Column<int>(nullable: true),
                    Aircraft5 = table.Column<int>(nullable: true),
                    MessageGet = table.Column<string>(nullable: true),
                    MessageAlbum = table.Column<string>(nullable: true),
                    ResourceName = table.Column<string>(nullable: true),
                    ResourceGraphicVersion = table.Column<string>(nullable: true),
                    ResourceVoiceVersion = table.Column<string>(nullable: true),
                    ResourcePortVoiceVersion = table.Column<string>(nullable: true),
                    OriginalCostumeShipId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.ShipId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ShipId",
                table: "Ships",
                column: "ShipId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ships");
        }
    }
}
