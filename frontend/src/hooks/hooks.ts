import { createApiClient } from '@/types/api';
import { useQuery } from '@tanstack/vue-query';
import type { Ref } from 'vue';

const useProdDatabaseInDev = false;
export const rootUrl =
  location.hostname === 'universalgames.buk.no' || useProdDatabaseInDev
    ? 'https://universalgames.buk.no/api/'
    : `http://${location.hostname}:5125/`;

const api = createApiClient(rootUrl);

// don't re-run requests unless this amount of seconds has passed
const cacheRequestForSeconds = 10;

const getTeamCode = (): string => {
  const teamCode = window.localStorage.getItem('testTeamCode');
  if (!teamCode) throw Error('Team code is not defined. Should log out.');
  return teamCode;
};

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
    staleTime: 1000 * cacheRequestForSeconds,
  });
};

export const useSigninResponse = (teamCode: Ref<string>) => {
  console.log(teamCode.value);
  return useQuery({
    queryKey: ['signinResponse', teamCode.value],
    queryFn: () =>
      api.get('/start/:code', {
        params: { code: teamCode.value },
      }),
    enabled: !!teamCode.value, // Only run if teamCode is truthy
    staleTime: 1000 * 60 * 60 * 2, // 2 hours in milliseconds
    gcTime: 1000 * 60 * 60 * 2, // Optional, defaults to 5 minutes if not specified
  });
};
