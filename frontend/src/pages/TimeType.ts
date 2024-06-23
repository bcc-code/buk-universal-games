import z from 'zod'

const timeSchemaRequired = z.object({
  hours: z.number(),
  minutes: z.number(),
  seconds: z.number(),
});

export const timeSchema = timeSchemaRequired.optional();

export type TimeType = z.infer<typeof timeSchema>;

const daySchema = z.number().brand<"Day">()
type Day = z.infer<typeof daySchema>


export function timeToNumber(time: NonNullable<TimeType>): Day {
  const hours = time.hours + time.minutes / 60 + time.seconds / (60*60);
  return daySchema.parse(hours / 24);
}
