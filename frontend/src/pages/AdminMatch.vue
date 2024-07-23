<template>
  <AdminPageLayout>
    <div class="absolute top-10 right-10 flex space-x-1">
      <LanguageSwitcher />
    </div>
    <BackButton />

    <AdminMatchInfo
      v-if="match && gameType"
      :match="match"
      :gameType="gameType"
    />

    <AdminRegisterPoints
      v-if="match && gameType"
      :match="match"
      :gameType="gameType"
    />

    <MatchListItem
      class="mt-5"
      v-if="match && match.team1 && match.team2 && gameType"
      :key="match.matchId"
      :team1="match.team1"
      :team2="match.team2"
      :team1result="match.team1Result ?? null"
      :team2result="match.team2Result ?? null"
      :start="match.start ?? ''"
      :winner="match.winner"
      :gameType="gameType"
      addOn=""
      gameAddOn=""
      :position="match.position ?? ''"
      
    />
  </AdminPageLayout>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useAdminMatches, useGames } from '@/hooks/hooks';
import AdminPageLayout from '@/components/AdminPageLayout.vue';
import { useStore } from 'vuex';
import AdminMatchInfo from './AdminMatchInfo.vue';
import { gameTypeSchema } from './GameType';
import BackButton from './BackButton.vue';
import { z } from 'zod';
import AdminRegisterPoints from './AdminRegisterPoints.vue';
import MatchListItem from '@/components/MatchListItem.vue';
import LanguageSwitcher from '@/components/LanguageSwitcher.vue';

const route = useRoute();

const matchId = z.coerce.number().parse(route.params.matchId);
const leagueId = useStore().state.adminLeagueSelected;

const { data: matches } = useAdminMatches(leagueId);
const match = computed(() =>
  matches.value?.find((m: any) => m.matchId == matchId),
);

const { data: games } = useGames();

const gameType = computed(() => {
  const game = games.value?.find((g) => g.id == match.value?.gameId);
  if (!game) return null;
  return gameTypeSchema.parse(game?.gameType);
});

const router = useRouter();

if (typeof matchId !== 'number') {
  router.back();
}
</script>
