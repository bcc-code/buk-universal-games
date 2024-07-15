import { makeApi, Zodios, type ZodiosOptions } from '@zodios/core';
import { z } from 'zod';

const GameType = z.union([
  z.literal(1),
  z.literal(2),
  z.literal(3),
  z.literal(4),
  z.literal(5),
]);
const Game = z.object({
  id: z.number().int(),
  name: z.string().nullable(),
  type: GameType,
  gameType: z.string().nullable(),
  description: z.string().nullable(),
  participantsInfo: z.string().nullable(),
  safetyInfo: z.string().nullable(),
  winnerPoints: z.number().int(),
  looserPoints: z.number().int(),
});
const TeamStatus = z.object({
  teamId: z.number().int(),
  team: z.string().nullable(),
  points: z.number().int(),
  familyId: z.number().int().nullable(),
  score: z.number().int().nullable(),
});
const MatchListItem = z.object({
  matchId: z.number().int(),
  gameId: z.number().int(),
  addOn: z.string().nullable(),
  team1Id: z.number().int(),
  team1: z.string().nullable(),
  team2Id: z.number().int(),
  team2: z.string().nullable(),
  start: z.string().nullable(),
  winnerId: z.number().int(),
  winner: z.string().nullable(),
  team1Result: z.number().int().nullable(),
  team2Result: z.number().int().nullable(),
  position: z.string().nullable(),
});
const MatchResultDto = z.object({
  matchId: z.number().int(),
  teamId: z.number().int(),
  result: z.number().int(),
});
const League = z.object({
  id: z.number().int(),
  name: z.string().nullable(),
  color: z.string().nullable(),
});
const TeamType = z.union([z.literal(1), z.literal(2), z.literal(3)]);
const Team = z.object({
  id: z.number().int(),
  name: z.string().nullable(),
  code: z.string().nullable(),
  color: z.string().nullable(),
  type: TeamType,
  memberCount: z.number().int(),
  teamType: z.string().nullable(),
  leagueId: z.number().int().nullable(),
  familyId: z.number().int().nullable(),
});
const LeagueStatusReport = z.object({
  status: z.record(z.array(TeamStatus)).nullable(),
});
const SignInSuccessResponse = z.object({
  code: z.string().nullable(),
  team: z.string().nullable(),
  teamId: z.number().int(),
  access: z.string().nullable(),
  leagueId: z.number().int().nullable(),
  league: z.string().nullable(),
  coins: z.array(z.string()).nullable(),
  familyId: z.number().int().nullable(),
  familyName: z.string().nullable(),
});
const TeamFamilyStatus = z.object({
  teamId: z.number().int(),
  team: z.string().nullable(),
  points: z.number().int(),
});
const FamilyStatus = z.object({
  id: z.number().int(),
  name: z.string().nullable(),
  points: z.number().int(),
  teams: z.array(TeamFamilyStatus).nullable(),
});
const MyStatus = z.object({
  teamPoints: z.number().int(),
  familyPoints: z.number().int(),
});
const FamilyStatusReport = z.object({
  families: z.array(FamilyStatus).nullable(),
  myStatus: MyStatus,
});

export const schemas = {
  GameType,
  Game,
  TeamStatus,
  MatchListItem,
  MatchResultDto,
  League,
  TeamType,
  Team,
  LeagueStatusReport,
  SignInSuccessResponse,
  TeamFamilyStatus,
  FamilyStatus,
  MyStatus,
  FamilyStatusReport,
};

const endpoints = makeApi([
  {
    method: 'get',
    path: '/:code/cache/clear',
    alias: 'getCodecacheclear',
    requestFormat: 'json',
    parameters: [
      {
        name: 'code',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.void(),
  },
  {
    method: 'get',
    path: '/:code/cache/warmup',
    alias: 'getCodecachewarmup',
    requestFormat: 'json',
    parameters: [
      {
        name: 'code',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.void(),
  },
  {
    method: 'get',
    path: '/:code/systemadmin/games/matches',
    alias: 'getCodesystemadmingamesmatches',
    requestFormat: 'json',
    parameters: [
      {
        name: 'code',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.void(),
  },
  {
    method: 'get',
    path: '/:code/systemadmin/leagues/status',
    alias: 'getCodesystemadminleaguesstatus',
    requestFormat: 'json',
    parameters: [
      {
        name: 'code',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.void(),
  },
  {
    method: 'get',
    path: '/:code/systemadmin/leagues/teams',
    alias: 'getCodesystemadminleaguesteams',
    requestFormat: 'json',
    parameters: [
      {
        name: 'code',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.void(),
  },
  {
    method: 'get',
    path: '/:code/systemadmin/status/clearstatusandmatches',
    alias: 'getCodesystemadminstatusclearstatusandmatches',
    requestFormat: 'json',
    parameters: [
      {
        name: 'code',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.void(),
  },
  {
    method: 'get',
    path: '/:code/systemadmin/status/export',
    alias: 'getCodesystemadminstatusexport',
    requestFormat: 'json',
    parameters: [
      {
        name: 'code',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.void(),
  },
  {
    method: 'get',
    path: '/admin/leagues',
    alias: 'getAdminleagues',
    requestFormat: 'json',
    parameters: [
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.array(League),
  },
  {
    method: 'get',
    path: '/admin/leagues/:leagueId/game/:gameId/matches',
    alias: 'getAdminleaguesLeagueIdgameGameIdmatches',
    requestFormat: 'json',
    parameters: [
      {
        name: 'leagueId',
        type: 'Path',
        schema: z.number().int(),
      },
      {
        name: 'gameId',
        type: 'Path',
        schema: z.number().int(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.array(MatchListItem),
  },
  {
    method: 'get',
    path: '/admin/leagues/:leagueId/matches',
    alias: 'getAdminleaguesLeagueIdmatches',
    requestFormat: 'json',
    parameters: [
      {
        name: 'leagueId',
        type: 'Path',
        schema: z.number().int(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.array(MatchListItem),
  },
  {
    method: 'get',
    path: '/admin/leagues/:leagueId/status',
    alias: 'getAdminleaguesLeagueIdstatus',
    requestFormat: 'json',
    parameters: [
      {
        name: 'leagueId',
        type: 'Path',
        schema: z.number().int(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: LeagueStatusReport,
  },
  {
    method: 'get',
    path: '/admin/leagues/:leagueId/teams',
    alias: 'getAdminleaguesLeagueIdteams',
    requestFormat: 'json',
    parameters: [
      {
        name: 'leagueId',
        type: 'Path',
        schema: z.number().int(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.array(Team),
  },
  {
    method: 'get',
    path: '/games',
    alias: 'getGames',
    requestFormat: 'json',
    parameters: [
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.array(Game),
  },
  {
    method: 'get',
    path: '/games/:gameId/ranking/:leagueId',
    alias: 'getGamesGameIdrankingLeagueId',
    requestFormat: 'json',
    parameters: [
      {
        name: 'gameId',
        type: 'Path',
        schema: z.number().int(),
      },
      {
        name: 'leagueId',
        type: 'Path',
        schema: z.number().int(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.array(TeamStatus),
  },
  {
    method: 'get',
    path: '/matches',
    alias: 'getMatches',
    requestFormat: 'json',
    parameters: [
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: z.array(MatchListItem),
  },
  {
    method: 'post',
    path: '/matches/:matchId/results',
    alias: 'postMatchesMatchIdresults',
    requestFormat: 'json',
    parameters: [
      {
        name: 'body',
        type: 'Body',
        schema: MatchResultDto,
      },
      {
        name: 'matchId',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: MatchListItem,
  },
  {
    method: 'get',
    path: '/start/:code',
    alias: 'getStartCode',
    requestFormat: 'json',
    parameters: [
      {
        name: 'code',
        type: 'Path',
        schema: z.string(),
      },
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: SignInSuccessResponse,
  },
  {
    method: 'get',
    path: '/status/family',
    alias: 'getStatusfamily',
    requestFormat: 'json',
    parameters: [
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: FamilyStatusReport,
  },
  {
    method: 'get',
    path: '/status/league',
    alias: 'getStatusleague',
    requestFormat: 'json',
    parameters: [
      {
        name: 'x-ubg-teamcode',
        type: 'Header',
        schema: z.unknown(),
      },
    ],
    response: LeagueStatusReport,
  },
]);

export const api = new Zodios(endpoints);

export function createApiClient(baseUrl: string, options?: ZodiosOptions) {
  return new Zodios(baseUrl, endpoints, options);
}
