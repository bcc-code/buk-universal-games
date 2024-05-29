import { createApiClient } from '@/types/api';
import { useQuery } from '@tanstack/vue-query';
import type { Ref } from 'vue';

const useProdDatabaseInDev = false;
export const rootUrl =
  location.hostname === 'universalgames.buk.no' || useProdDatabaseInDev
    ? 'https://universalgames.buk.no/api/'
    : `http://${location.hostname}:5125/`;

const api = createApiClient(rootUrl);

export const useFamilyStatus = () => {
  const teamCode = getTeamCode();

  return useQuery({
    queryKey: ['familyStatus'],
    queryFn: () =>
      api.get('/status/family', {
        headers: {
          'x-ubg-teamcode': teamCode,
        },
      }),
    // don't re-run requests unless this amount of milliseconds have passed
    staleTime: 1000 * 10,
    gcTime: 1000 * 10,
  });
};

const getTeamCode = (): string => {
  const teamCode = window.localStorage.getItem('testTeamCode');
  if (!teamCode) throw Error('Team code is not defined. Should log out.');
  return teamCode;
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
