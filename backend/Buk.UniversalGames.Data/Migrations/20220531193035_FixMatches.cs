using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    public partial class FixMatches : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "winner_id",
                table: "matches",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "leagues",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_matches_team1id",
                table: "matches",
                column: "team1id");

            migrationBuilder.CreateIndex(
                name: "ix_matches_team2id",
                table: "matches",
                column: "team2id");

            migrationBuilder.CreateIndex(
                name: "ix_matches_winner_id",
                table: "matches",
                column: "winner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_matches_teams_team1id",
                table: "matches",
                column: "team1id",
                principalTable: "teams",
                principalColumn: "team_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_matches_teams_team2id",
                table: "matches",
                column: "team2id",
                principalTable: "teams",
                principalColumn: "team_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_matches_teams_winner_id",
                table: "matches",
                column: "winner_id",
                principalTable: "teams",
                principalColumn: "team_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_matches_teams_team1id",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "fk_matches_teams_team2id",
                table: "matches");

            migrationBuilder.DropForeignKey(
                name: "fk_matches_teams_winner_id",
                table: "matches");

            migrationBuilder.DropIndex(
                name: "ix_matches_team1id",
                table: "matches");

            migrationBuilder.DropIndex(
                name: "ix_matches_team2id",
                table: "matches");

            migrationBuilder.DropIndex(
                name: "ix_matches_winner_id",
                table: "matches");

            migrationBuilder.AlterColumn<int>(
                name: "winner_id",
                table: "matches",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "leagues",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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
        }
    }
}
