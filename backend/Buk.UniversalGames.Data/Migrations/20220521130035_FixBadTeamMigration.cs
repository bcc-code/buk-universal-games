using Buk.UniversalGames.Library.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    public partial class FixBadTeamMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_matches_teams_team1team_team_id",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "fk_matches_teams_team2team_team_id",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "fk_matches_teams_winner_team_team_id",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "fk_points_teams_team_id",
                table: "points");

            migrationBuilder.DropForeignKey(
                name: "fk_sticker_scans_teams_team_id",
                table: "sticker_scans");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropIndex(
                name: "ix_sticker_scans_team_id",
                table: "sticker_scans");

            migrationBuilder.DropIndex(
                name: "ix_points_team_id",
                table: "points");

            migrationBuilder.DropIndex(
                name: "ix_matches_team1team_team_id",
                table: "matches");

            migrationBuilder.DropIndex(
                name: "ix_matches_team2team_team_id",
                table: "matches");

            migrationBuilder.DropIndex(
                name: "ix_matches_winner_team_team_id",
                table: "matches");

            migrationBuilder.DropColumn(
                name: "team1team_team_id",
                table: "matches");

            migrationBuilder.DropColumn(
                name: "team2team_team_id",
                table: "matches");

            migrationBuilder.DropColumn(
                name: "winner_team_team_id",
                table: "matches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "team1team_team_id",
                table: "matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "team2team_team_id",
                table: "matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "winner_team_team_id",
                table: "matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    team_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    league_id = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<TeamType>(type: "team_type", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teams", x => x.team_id);
                    table.ForeignKey(
                        name: "fk_teams_leagues_league_id",
                        column: x => x.league_id,
                        principalTable: "leagues",
                        principalColumn: "league_id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_sticker_scans_team_id",
                table: "sticker_scans",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_points_team_id",
                table: "points",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_matches_team1team_team_id",
                table: "matches",
                column: "team1team_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_matches_team2team_team_id",
                table: "matches",
                column: "team2team_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_matches_winner_team_team_id",
                table: "matches",
                column: "winner_team_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_teams_league_id",
                table: "teams",
                column: "league_id");

            migrationBuilder.AddForeignKey(
                name: "fk_matches_teams_team1team_team_id",
                table: "matches",
                column: "team1team_team_id",
                principalTable: "teams",
                principalColumn: "team_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_matches_teams_team2team_team_id",
                table: "matches",
                column: "team2team_team_id",
                principalTable: "teams",
                principalColumn: "team_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_matches_teams_winner_team_team_id",
                table: "matches",
                column: "winner_team_team_id",
                principalTable: "teams",
                principalColumn: "team_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_points_teams_team_id",
                table: "points",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "team_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sticker_scans_teams_team_id",
                table: "sticker_scans",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "team_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
