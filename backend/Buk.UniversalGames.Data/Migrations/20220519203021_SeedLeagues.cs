using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    public partial class SeedLeagues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_teams_leagues_league_id",
                table: "teams");

            migrationBuilder.AlterColumn<int>(
                name: "league_id",
                table: "teams",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.InsertData(
                table: "leagues",
                columns: new[] { "league_id", "name" },
                values: new object[,]
                {
                    { 1, "B" },
                    { 2, "U" },
                    { 3, "K" }
                });

            migrationBuilder.AddForeignKey(
                name: "fk_teams_leagues_league_id",
                table: "teams",
                column: "league_id",
                principalTable: "leagues",
                principalColumn: "league_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_teams_leagues_league_id",
                table: "teams");

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

            migrationBuilder.AlterColumn<int>(
                name: "league_id",
                table: "teams",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_teams_leagues_league_id",
                table: "teams",
                column: "league_id",
                principalTable: "leagues",
                principalColumn: "league_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
