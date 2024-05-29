import { createApiClient } from '@/types/api';
import { useQuery } from '@tanstack/vue-query';

const useProdDatabaseInDev = false;
export const rootUrl =
  location.hostname === 'universalgames.buk.no' || useProdDatabaseInDev
    ? 'https://universalgames.buk.no/api/'
    : `http://${location.hostname}:5125/`;

const api = createApiClient(rootUrl);

const getTeamCode = () => {
  return window.localStorage.getItem('testTeamCode');
};

// don't re-run requests unless this amount of seconds has passed
const cacheRequestForSeconds = 10;

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

export const useSigninResponse = ()=>{
  
}