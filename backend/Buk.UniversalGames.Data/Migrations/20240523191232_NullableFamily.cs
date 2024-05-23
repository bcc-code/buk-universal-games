using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    /// <inheritdoc />
    public partial class NullableFamily : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_teams_families_family_id",
                table: "teams");

            migrationBuilder.AlterColumn<int>(
                name: "family_id",
                table: "teams",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_teams_families_family_id",
                table: "teams",
                column: "family_id",
                principalTable: "families",
                principalColumn: "family_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_teams_families_family_id",
                table: "teams");

            migrationBuilder.AlterColumn<int>(
                name: "family_id",
                table: "teams",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_teams_families_family_id",
                table: "teams",
                column: "family_id",
                principalTable: "families",
                principalColumn: "family_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
