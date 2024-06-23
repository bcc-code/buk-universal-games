<template>
  <AdminPageLayout>
    <BackButton />

    <AdminMatchInfo v-if="match && gameType" :match="match" :gameType="gameType" />

    <AdminRegisterPoints v-if="match && gameType" :match="match" :gameType="gameType" />
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
  if(!game) return null;
  return gameTypeSchema.parse(game?.gameType);
});

const router = useRouter();

if (!matchId) {
  router.back();
}
</script>
