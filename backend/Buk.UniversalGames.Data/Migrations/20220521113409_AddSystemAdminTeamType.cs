using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    public partial class AddSystemAdminTeamType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:game_type", "unknown,water,wood,fire,earth,metal")
                .Annotation("Npgsql:Enum:team_type", "unknown,participant,admin,system_admin")
                .OldAnnotation("Npgsql:Enum:game_type", "unknown,water,wood,fire,earth,metal")
                .OldAnnotation("Npgsql:Enum:team_type", "unknown,participant,admin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:game_type", "unknown,water,wood,fire,earth,metal")
                .Annotation("Npgsql:Enum:team_type", "unknown,participant,admin")
                .OldAnnotation("Npgsql:Enum:game_type", "unknown,water,wood,fire,earth,metal")
                .OldAnnotation("Npgsql:Enum:team_type", "unknown,participant,admin,system_admin");
        }
    }
}
