using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    /// <inheritdoc />
    public partial class RankingChangesAndFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "league_id",
                table: "matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_matches_league_id",
                table: "matches",
                column: "league_id");

            migrationBuilder.AddForeignKey(
                name: "fk_matches_leagues_league_id",
                table: "matches",
                column: "league_id",
                principalTable: "leagues",
                principalColumn: "league_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_matches_leagues_league_id",
                table: "matches");

            migrationBuilder.DropIndex(
                name: "ix_matches_league_id",
                table: "matches");

            migrationBuilder.DropColumn(
                name: "league_id",
                table: "matches");
        }
    }
}
