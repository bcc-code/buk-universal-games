<template>
  <MatchListItem
    class="mb-5"
    v-if="match && match.team1 && match.team2"
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

  <div class="rounded-md bg-white p-4 mb-4 bg-green-50 italic">
    {{ $t('admin.intro') }}
    
  </div>
  <AdminRefereeInfo />
</template>

<script setup lang="ts">
import MatchListItem from '@/components/MatchListItem.vue';
import type { GameType } from './GameType';
import type { MatchListItemEntity } from './MatchListItemEntity';
import AdminRefereeInfoIronGrip from './AdminRefereeInfoIronGrip.vue';
import AdminRefereeInfoLandWaterBeach from './AdminRefereeInfoLandWaterBeach.vue';
import AdminRefereeInfoLabyrinth from './AdminRefereeInfoLabyrinth.vue';
import AdminRefereeInfoHamsterWheel from './AdminRefereeInfoHamsterWheel.vue';
import AdminRefereeInfoMastermind from './AdminRefereeInfoMastermind.vue';

const props = defineProps<{
  match: MatchListItemEntity;
  gameType: GameType;
}>();

const AdminRefereeInfo = {
  iron_grip: AdminRefereeInfoIronGrip,
  land_water_beach: AdminRefereeInfoLandWaterBeach,
  labyrinth: AdminRefereeInfoLabyrinth,
  hamster_wheel: AdminRefereeInfoHamsterWheel,
  mastermind: AdminRefereeInfoMastermind,
}[props.gameType];
</script>
