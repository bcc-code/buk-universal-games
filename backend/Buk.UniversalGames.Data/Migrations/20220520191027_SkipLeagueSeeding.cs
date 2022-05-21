using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    public partial class SkipLeagueSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "leagues",
                keyColumn: "league_id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "location",
                table: "leagues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "leagues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "leagues",
                columns: new[] { "league_id", "location", "name" },
                values: new object[,]
                {
                    { 1, "Oslofjord Arena", "B-liga" },
                    { 2, "Mellom hotellbyggene", "U-liga" },
                    { 3, "Strandsonen", "K-liga" }
                });
        }
    }
}
