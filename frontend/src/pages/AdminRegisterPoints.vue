<template>
  <RegisterPointsIronGrip       v-if=     "gameType==='iron_grip'"        :match="match"></RegisterPointsIronGrip>
  <RegisterPointsLandWaterBeach v-else-if="gameType==='land_water_beach'" :match="match"></RegisterPointsLandWaterBeach>
  <RegisterPointsLabyrinth      v-else-if="gameType==='labyrinth'"        :match="match"></RegisterPointsLabyrinth>
  <RegisterPointsHamsterWheel   v-else-if="gameType==='hamster_wheel'"    :match="match"></RegisterPointsHamsterWheel>
  <RegisterPointsMastermind     v-else-if="gameType==='mastermind'"       :match="match"></RegisterPointsMastermind>
  <div v-else>
    <div class="alert alert-error block">Noe gikk galt</div>
  </div>
</template>

<script setup lang="ts">
import type { MatchListItemEntity } from './MatchListItemEntity';
import type { GameType } from './GameType';
import RegisterPointsIronGrip from "./RegisterPointsIronGrip.vue"
import RegisterPointsLandWaterBeach from './RegisterPointsLandWaterBeach.vue';
import RegisterPointsLabyrinth from './RegisterPointsLabyrinth.vue';
import RegisterPointsHamsterWheel from './RegisterPointsHamsterWheel.vue';
import RegisterPointsMastermind from './RegisterPointsMastermind.vue';

defineProps<{
  match: MatchListItemEntity;
  gameType: GameType;
}>();
</script>
