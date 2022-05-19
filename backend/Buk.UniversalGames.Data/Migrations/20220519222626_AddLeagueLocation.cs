using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    public partial class AddLeagueLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "leagues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 1,
                columns: new[] { "location", "name" },
                values: new object[] { "Oslofjord Arena", "B-liga" });

            migrationBuilder.UpdateData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 2,
                columns: new[] { "location", "name" },
                values: new object[] { "Mellom hotellbyggene", "U-liga" });

            migrationBuilder.UpdateData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 3,
                columns: new[] { "location", "name" },
                values: new object[] { "Strandsonen", "K-liga" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "leagues");

            migrationBuilder.UpdateData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 1,
                column: "name",
                value: "B");

            migrationBuilder.UpdateData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 2,
                column: "name",
                value: "U");

            migrationBuilder.UpdateData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 3,
                column: "name",
                value: "K");
        }
    }
}
