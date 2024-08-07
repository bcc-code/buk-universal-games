import { z } from 'zod';

export const gameTypeSchema = z.union([
  z.literal('iron_grip'),
  z.literal('land_water_beach'),
  z.literal('labyrinth'),
  z.literal('hamster_wheel'),
  z.literal('mastermind'),
]);

export type GameType = z.infer<typeof gameTypeSchema>;
