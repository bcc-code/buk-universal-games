import { createApiClient } from '@/types/api';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import type { Ref } from 'vue';

const useProdDatabaseInDev = true;
export const rootUrl =
  location.hostname === 'universalgames.buk.no' || useProdDatabaseInDev
    ? 'https://universalgames.buk.no/api/'
    : `http://${location.hostname}:5125/`;

const api = createApiClient(rootUrl);

const getTeamCode = (): string => {
  const teamCode = window.localStorage.getItem('testTeamCode');
  if (!teamCode) throw Error('Team code is not defined. Should log out.');
  return teamCode;
};

export const useFamilyStatus = () => {
  const teamCode = getTeamCode();
  return useQuery({
    queryKey: ['familyStatus', teamCode],
    queryFn: () =>
      api.get('/status/family', {
        headers: {
          'x-ubg-teamcode': teamCode,
        },
      }),
    // don't re-run requests unless this amount of milliseconds have passed
    staleTime: 1000 * 60 * 3,
    gcTime: 1000 * 60 * 3,
    retry: 2,
  });
};

export const useSigninResponse = (teamCode?: () => Ref<string>) => {
  const getTeamCodeLocal = (): string => {
    if (teamCode === undefined) {
      return getTeamCode();
    } else return teamCode().value.toUpperCase();
  };

  return useQuery({
    queryKey: ['signinResponse', getTeamCodeLocal()],
    queryFn: () =>
      api.get('/start/:code', {
        params: { code: getTeamCodeLocal() },
      }),
    enabled: !!getTeamCodeLocal(),
    // don't re-run requests unless this amount of milliseconds have passed
    staleTime: 1000 * 60 * 60 * 2,
    gcTime: 1000 * 60 * 60 * 2,
    retry: 2,
  });
};

export const useAdminMatches = (leagueId: number) => {
  const teamCode = getTeamCode();
  return useQuery({
    queryKey: ['adminMatches', leagueId, teamCode],
    queryFn: () => {
      console.log('getting matches', leagueId);
      const responseMatches = api.get(`/admin/leagues/:leagueId/matches`, {
        params: {
          leagueId,
        },
        headers: {
          'x-ubg-teamcode': teamCode,
        },
      });
      return responseMatches;
    },
  });
};

export const useGames = () => {
  const teamCode = getTeamCode();
  return useQuery({
    queryKey: ['games', teamCode],
    queryFn: () =>
      api.get('/games', {
        headers: {
          'x-ubg-teamcode': teamCode,
        },
      }),
  });
};

export const useLeagues = () => {
  const teamCode = getTeamCode();
  return useQuery({
    queryKey: ['leagues', teamCode],
    queryFn: () =>
      api.get('/admin/leagues', {
        headers: { 'x-ubg-teamcode': teamCode },
      }),
  });
};

export const useConfirmTeamResult = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      matchId,
      teamId,
      result,
    }: {
      matchId: number;
      teamId: number;
      result: number;
    }) =>
      api.postMatchesMatchIdresults(
        { result, matchId: matchId, teamId: teamId },
        {
          params: {
            matchId: matchId.toString(),
          },
          headers: {
            'x-ubg-teamcode': getTeamCode(),
          },
        },
      ),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['adminMatches'] });
      queryClient.invalidateQueries({ queryKey: ['familyStatus'] });
    },
    retry: 5,
  });
};

export const useConfirmBothTeamResults = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      matchId,
      team1Result,
      team2Result,
    }: {
      matchId: number;
      team1Result: number;
      team2Result: number;
    }) =>
      api.postMatchesbothresults(
        { matchId, team1Result, team2Result },
        {
          headers: {
            'x-ubg-teamcode': getTeamCode(),
          },
        },
      ),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['adminMatches'] });
      queryClient.invalidateQueries({ queryKey: ['familyStatus'] });
    },
    retry: 5,
  });
};
