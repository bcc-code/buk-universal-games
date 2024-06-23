import { z } from 'zod';

export const gameTypeSchema = z.union([
  z.literal('iron_grip'),
  z.literal('land_water_beach'),
  z.literal('labyrinth'),
  // 🧹rename to hamster_wheel
  z.literal('human_shuffleboard'),
  z.literal('mastermind'),
]);

export type GameType = z.infer<typeof gameTypeSchema>;
