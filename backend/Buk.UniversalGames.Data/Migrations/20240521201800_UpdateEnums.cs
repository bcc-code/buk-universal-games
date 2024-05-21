using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    public partial class UpdateEnums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'game_type') THEN
                        CREATE TYPE game_type AS ENUM ('land_water_beach', 'labyrinth', 'human_shuffle_board', 'mastermind', 'iron_grip');
                    ELSE
                        BEGIN
                            ALTER TYPE game_type ADD VALUE IF NOT EXISTS 'land_water_beach';
                            ALTER TYPE game_type ADD VALUE IF NOT EXISTS 'labyrinth';
                            ALTER TYPE game_type ADD VALUE IF NOT EXISTS 'human_shuffle_board';
                            ALTER TYPE game_type ADD VALUE IF NOT EXISTS 'mastermind';
                            ALTER TYPE game_type ADD VALUE IF NOT EXISTS 'iron_grip';
                        END;
                    END IF;
                END
                $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                END
                $$;
            ");
        }
    }
}
