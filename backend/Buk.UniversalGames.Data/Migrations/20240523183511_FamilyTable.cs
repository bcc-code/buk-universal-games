using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    /// <inheritdoc />
    public partial class FamilyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "families",
                columns: table => new
                {
                    family_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_families", x => x.family_id);
                });

            // Insert a default family with family_id = 0
            migrationBuilder.Sql("INSERT INTO families (family_id, name) VALUES (0, 'Default Family');");

            migrationBuilder.CreateIndex(
                name: "ix_teams_family_id",
                table: "teams",
                column: "family_id");

            migrationBuilder.AddForeignKey(
                name: "fk_teams_families_family_id",
                table: "teams",
                column: "family_id",
                principalTable: "families",
                principalColumn: "family_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_teams_families_family_id",
                table: "teams");

            migrationBuilder.DropTable(
                name: "families");

            migrationBuilder.DropIndex(
                name: "ix_teams_family_id",
                table: "teams");
        }
    }
}
