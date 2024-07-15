using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    /// <inheritdoc />
    public partial class Colors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make the 'color' column in the 'teams' table nullable
            migrationBuilder.Sql(@"
                ALTER TABLE teams ALTER COLUMN color DROP NOT NULL;
            ");

            // Add the 'color' column to the 'families' table if it does not exist
            migrationBuilder.Sql(@"
                ALTER TABLE families ADD COLUMN IF NOT EXISTS color text NOT NULL DEFAULT '';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the 'color' column from the 'families' table if it exists
            migrationBuilder.Sql(@"
                ALTER TABLE families DROP COLUMN IF EXISTS color;
            ");

            // Make the 'color' column in the 'teams' table not nullable
            migrationBuilder.Sql(@"
                ALTER TABLE teams ALTER COLUMN color SET NOT NULL;
            ");
        }
    }
}
