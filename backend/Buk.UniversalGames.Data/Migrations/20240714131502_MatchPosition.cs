using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    /// <inheritdoc />
    public partial class MatchPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:game_type", "land_water_beach,labyrinth,hamster_wheel,mastermind,iron_grip")
                .Annotation("Npgsql:Enum:team_type", "participant,admin,system_admin")
                .OldAnnotation("Npgsql:Enum:game_type", "land_water_beach,labyrinth,human_shuffle_board,mastermind,iron_grip")
                .OldAnnotation("Npgsql:Enum:team_type", "participant,admin,system_admin");

            migrationBuilder.AddColumn<string>(
                name: "position",
                table: "matches",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "position",
                table: "matches");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:game_type", "land_water_beach,labyrinth,human_shuffle_board,mastermind,iron_grip")
                .Annotation("Npgsql:Enum:team_type", "participant,admin,system_admin")
                .OldAnnotation("Npgsql:Enum:game_type", "land_water_beach,labyrinth,hamster_wheel,mastermind,iron_grip")
                .OldAnnotation("Npgsql:Enum:team_type", "participant,admin,system_admin");
        }
    }
}
