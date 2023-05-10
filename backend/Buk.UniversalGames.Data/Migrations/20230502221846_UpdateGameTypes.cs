using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    ALTER TYPE game_type RENAME VALUE 'water' TO 'nerve_spiral';
                    ALTER TYPE game_type RENAME VALUE 'wood' TO 'ticket_twist';
                    ALTER TYPE game_type RENAME VALUE 'fire' TO 'monkey_bars';
                    ALTER TYPE game_type RENAME VALUE 'earth' TO 'mine_field';
                    ALTER TYPE game_type RENAME VALUE 'metal' TO 'table_surfing';

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    ALTER TYPE game_type RENAME VALUE 'nerve_spiral' TO 'water';
                    ALTER TYPE game_type RENAME VALUE 'ticket_twist' TO 'wood';
                    ALTER TYPE game_type RENAME VALUE 'monkey_bars' TO 'fire';
                    ALTER TYPE game_type RENAME VALUE 'mine_field' TO 'earth';
                    ALTER TYPE game_type RENAME VALUE 'table_surfing' TO 'metal';
            ");
        }
    }
}
