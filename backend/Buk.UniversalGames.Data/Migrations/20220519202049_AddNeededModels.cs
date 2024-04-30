using Buk.UniversalGames.Library.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    public partial class AddNeededModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:game_type", "unknown,water,wood,fire,earth,metal")
                .Annotation("Npgsql:Enum:team_type", "unknown,participant,admin");

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    game_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<GameType>(type: "game_type", nullable: false),
                    winner_points = table.Column<int>(type: "integer", nullable: false),
                    looser_points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.game_id);
                });

            migrationBuilder.CreateTable(
                name: "leagues",
                columns: table => new
                {
                    league_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_leagues", x => x.league_id);
                });

            migrationBuilder.CreateTable(
                name: "stickers",
                columns: table => new
                {
                    sticker_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    league_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stickers", x => x.sticker_id);
                    table.ForeignKey(
                        name: "fk_stickers_leagues_league_id",
                        column: x => x.league_id,
                        principalTable: "leagues",
                        principalColumn: "league_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    team_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    league_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<TeamType>(type: "team_type", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teams", x => x.team_id);
                    table.ForeignKey(
                        name: "fk_teams_leagues_league_id",
                        column: x => x.league_id,
                        principalTable: "leagues",
                        principalColumn: "league_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "matches",
                columns: table => new
                {
                    match_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team1id = table.Column<int>(type: "integer", nullable: false),
                    team2id = table.Column<int>(type: "integer", nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: false),
                    start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    winner_id = table.Column<int>(type: "integer", nullable: false),
                    team1team_team_id = table.Column<int>(type: "integer", nullable: false),
                    team2team_team_id = table.Column<int>(type: "integer", nullable: false),
                    winner_team_team_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_matches", x => x.match_id);
                    table.ForeignKey(
                        name: "fk_matches_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_matches_teams_team1team_team_id",
                        column: x => x.team1team_team_id,
                        principalTable: "teams",
                        principalColumn: "team_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_matches_teams_team2team_team_id",
                        column: x => x.team2team_team_id,
                        principalTable: "teams",
                        principalColumn: "team_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_matches_teams_winner_team_team_id",
                        column: x => x.winner_team_team_id,
                        principalTable: "teams",
                        principalColumn: "team_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sticker_scans",
                columns: table => new
                {
                    sticker_scan_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sticker_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sticker_scans", x => x.sticker_scan_id);
                    table.ForeignKey(
                        name: "fk_sticker_scans_stickers_sticker_id",
                        column: x => x.sticker_id,
                        principalTable: "stickers",
                        principalColumn: "sticker_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sticker_scans_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "team_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "points",
                columns: table => new
                {
                    point_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: true),
                    match_id = table.Column<int>(type: "integer", nullable: true),
                    sticker_id = table.Column<int>(type: "integer", nullable: true),
                    points = table.Column<int>(type: "integer", nullable: false),
                    added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_points", x => x.point_id);
                    table.ForeignKey(
                        name: "fk_points_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "game_id");
                    table.ForeignKey(
                        name: "fk_points_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "matches",
                        principalColumn: "match_id");
                    table.ForeignKey(
                        name: "fk_points_stickers_sticker_id",
                        column: x => x.sticker_id,
                        principalTable: "stickers",
                        principalColumn: "sticker_id");
                    table.ForeignKey(
                        name: "fk_points_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "team_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_matches_game_id",
                table: "matches",
                column: "game_id");

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
                name: "ix_points_game_id",
                table: "points",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_points_match_id",
                table: "points",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "ix_points_sticker_id",
                table: "points",
                column: "sticker_id");

            migrationBuilder.CreateIndex(
                name: "ix_points_team_id",
                table: "points",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_sticker_scans_sticker_id",
                table: "sticker_scans",
                column: "sticker_id");

            migrationBuilder.CreateIndex(
                name: "ix_sticker_scans_team_id",
                table: "sticker_scans",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_stickers_league_id",
                table: "stickers",
                column: "league_id");

            migrationBuilder.CreateIndex(
                name: "ix_teams_league_id",
                table: "teams",
                column: "league_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "points");

            migrationBuilder.DropTable(
                name: "sticker_scans");

            migrationBuilder.DropTable(
                name: "matches");

            migrationBuilder.DropTable(
                name: "stickers");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "leagues");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:game_type", "unknown,water,wood,fire,earth,metal")
                .OldAnnotation("Npgsql:Enum:team_type", "unknown,participant,admin");
        }
    }
}
