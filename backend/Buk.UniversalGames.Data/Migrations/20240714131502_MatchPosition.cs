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
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='matches' AND column_name='position') THEN
                        ALTER TABLE matches ADD COLUMN position text NOT NULL DEFAULT '';
                    END IF;
                END
                $$;
            ");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'game_type') THEN
                        CREATE TYPE game_type AS ENUM ('land_water_beach', 'labyrinth', 'hamster_wheel', 'mastermind', 'iron_grip');
                    END IF;
                END
                $$;

                ALTER TYPE game_type ADD VALUE IF NOT EXISTS 'hamster_wheel';
            ");

            migrationBuilder.Sql(@"
                -- Remove enum value in a safe manner using a custom function
                CREATE OR REPLACE FUNCTION remove_enum_value(enum_name text, value text) RETURNS void AS $$
                DECLARE
                    old_val text;
                BEGIN
                    EXECUTE format('ALTER TYPE %I RENAME VALUE %L TO %L', enum_name, value, 'old_' || value);
                    EXECUTE format('ALTER TYPE %I RENAME TO %I_old', enum_name, enum_name);
                    EXECUTE format('CREATE TYPE %I AS ENUM', enum_name) || (SELECT string_agg(quote_literal(enumlabel), ',') FROM pg_enum WHERE enumtypid = (SELECT oid FROM pg_type WHERE typname = enum_name || '_old') AND enumlabel != 'old_' || value);
                    EXECUTE format('ALTER TABLE matches ALTER COLUMN game_type TYPE %I USING game_type::text::%I', enum_name, enum_name);
                    EXECUTE format('DROP TYPE %I_old', enum_name);
                END;
                $$ LANGUAGE plpgsql;
                
                SELECT remove_enum_value('game_type', 'human_shuffle_board');

                DROP FUNCTION remove_enum_value;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE matches DROP COLUMN IF EXISTS position;
            ");

            migrationBuilder.Sql(@"
                ALTER TYPE game_type ADD VALUE IF NOT EXISTS 'human_shuffle_board';
            ");
        }
    }
}
