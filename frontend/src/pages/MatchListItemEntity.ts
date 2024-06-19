import { schemas } from '@/types/api';
import type { z } from 'zod';

export type MatchListItemEntity = z.infer<typeof schemas.MatchListItem>;