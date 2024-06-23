import z from 'zod'

const timeSchemaRequired = z.object({
  hours: z.number(),
  minutes: z.number(),
  seconds: z.number(),
});

export const timeSchema = timeSchemaRequired.optional();

export type TimeType = z.infer<typeof timeSchema>;

