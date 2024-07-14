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
            // Add the new column 'position' if it does not already exist
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='matches' AND column_name='position') THEN
                        ALTER TABLE matches ADD COLUMN position text NOT NULL DEFAULT '';
                    END IF;
                END
                $$;
            ");

            // Drop the old game_type enum if it exists
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_type WHERE typname = 'game_type_old') THEN
                        DROP TYPE game_type_old;
                    END IF;
                END
                $$;
            ");
            // Rename the existing game_type enum if it exists
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_type WHERE typname = 'game_type') THEN
                        ALTER TYPE game_type RENAME TO game_type_old;
                    END IF;
                END
                $$;
            ");

            // Create the new game_type enum with the required values
            migrationBuilder.Sql(@"
                CREATE TYPE game_type AS ENUM ('land_water_beach', 'labyrinth', 'hamster_wheel', 'mastermind', 'iron_grip');
            ");

            // Drop the old game_type enum if it exists
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_type WHERE typname = 'game_type_old') THEN
                        DROP TYPE game_type_old;
                    END IF;
                END
                $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate the old game_type enum if needed
            migrationBuilder.Sql(@"
                CREATE TYPE game_type_old AS ENUM ('unknown', 'nerve_spiral', 'ticket_twist', 'monkey_bars', 'mine_field', 'table_surfing', 'land_water_beach', 'labyrinth', 'mastermind', 'iron_grip', 'hamster_wheel');
            ");

            // Drop the new game_type enum
            migrationBuilder.Sql(@"
                DROP TYPE game_type;
            ");

            // Rename the old game_type enum back to its original name
            migrationBuilder.Sql(@"
                ALTER TYPE game_type_old RENAME TO game_type;
            ");

            // Remove the column 'position' if it exists
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='matches' AND column_name='position') THEN
                        ALTER TABLE matches DROP COLUMN position;
                    END IF;
                END
                $$;
            ");
        }
    }
}
