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
  return useQuery({
    queryKey: ['familyStatus'],
    queryFn: () =>
      api.get('/status/family', {
        headers: {
          'x-ubg-teamcode': getTeamCode(),
        },
      }),
    // don't re-run requests unless this amount of milliseconds have passed
    staleTime: 1000 * 10,
    gcTime: 1000 * 10,
  });
};

export const useSigninResponse = (teamCode?: () => Ref<string>) => {
  const getTeamCodeLocal = (): string => {
    if (teamCode === undefined) {
      return getTeamCode();
    } else return teamCode().value;
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
  });
};

export const useAdminMatches = (leagueId: number) => {
  return useQuery({
    queryKey: ['adminMatches', leagueId],
    queryFn: () =>
      api.get(`/admin/leagues/:leagueId/matches`, {
        params: {
          leagueId,
        },
        headers: {
          'x-ubg-teamcode': getTeamCode(),
        },
      }),
  });
};

export const useGames = () => {
  return useQuery({
    queryKey: ['games'],
    queryFn: () =>
      api.get('/games', {
        headers: {
          'x-ubg-teamcode': getTeamCode(),
        },
      }),
  });
};

export const useLeagues = () => {
  return useQuery({
    queryKey: ['leagues'],
    queryFn: () =>
      api.get('/admin/leagues', {
        headers: { 'x-ubg-teamcode': getTeamCode() },
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
      api.post(
        `/matches/:matchId/results`,
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
    },
  });
};
