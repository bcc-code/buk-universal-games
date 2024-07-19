<template>
  <div class="p-4 bg-white rounded-md">
    <form @submit.prevent="submitForm">
      <div class="flex justify-evenly">
        <div class="mb-4">
          {{ $t(`point`) }} {{ match.team1 }}:
          <br />

          <input
            class="shadow-md border-solid border-2 border-slate-200 rounded-md w-36"
            type="number"
            v-model="pointsTeam1"
            min="0"
            max="9"
            :placeholder="$t(`point`)"
            required
          />
          <div v-if="calculatedResult">
            {{ $t(`calculated-score`) }} {{ calculatedResult.team1Result }}
          </div>
        </div>
        <div class="mb-4">
          {{ $t(`point`) }} {{ match.team2 }}:
          <br />
          <input
            class="shadow-md border-solid border-2 border-slate-200 rounded-md w-36"
            type="number"
            v-model="pointsTeam2"
            min="0"
            max="9"
            :placeholder="$t(`point`)"
            required
          />
          <div v-if="calculatedResult">
            {{ $t(`calculated-score`) }} {{ calculatedResult.team2Result }}
          </div>
        </div>
      </div>

      <LoadingButton
        type="submit"
        class="btn-success btn-blank h-14 p-4"
        :is-loading="isPending"
      >
        {{ $t(`save`) }}
      </LoadingButton>
    </form>
  </div>
  <!-- 🧹move to parent where we save. -->
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="error">
    <div class="alert alert-error block">{{ error.message }}</div>
  </div>
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="showSuccess">
    <div class="alert alert-success block">{{ $t(`saved`) }}</div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useConfirmBothTeamResults } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
import { floatToInt, lerp } from './mathHelpers';
import LoadingButton from '@/components/LoadingButton.vue';
import { useI18n } from 'vue-i18n';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const pointsTeam1 = ref<number | ''>('');
const pointsTeam2 = ref<number | ''>('');

const calculatedResult = computed<
  { team1Result: number; team2Result: number } | undefined
>(() => {
  const validatedPointsTeam1 = pointsTeam1.value;
  const validatedPointsTeam2 = pointsTeam2.value;
  if (!(typeof validatedPointsTeam1 === 'number')) return;
  if (!(typeof validatedPointsTeam2 === 'number')) return;

  const minScore = 1;
  const maxScore = 20;
  const minPoints = 0;
  const maxPoints = 9;

  const team1Result = lerp(
    minPoints,
    maxPoints,
    minScore,
    maxScore,
    validatedPointsTeam1,
    'clamp',
  );
  const team2Result = lerp(
    minPoints,
    maxPoints,
    minScore,
    maxScore,
    validatedPointsTeam2,
    'clamp',
  );

  return {
    team1Result: floatToInt(team1Result),
    team2Result: floatToInt(team2Result),
  };
});

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const {
  mutate: confirmBothTeamResults,
  isPending,
  error,
} = useConfirmBothTeamResults();
const { t } = useI18n();
const submitForm = () => {
  if (!calculatedResult.value) throw Error(t('points-registration-error'));

  const matchId = props.match.matchId;

  const resultTeam1 = calculatedResult.value.team1Result;
  const resultTeam2 = calculatedResult.value.team2Result;

  const variables = {
    matchId: matchId,
    team1Result: resultTeam1,
    team2Result: resultTeam2,
  };

  confirmBothTeamResults(variables, {
    onSuccess() {
      showSuccessToast();
    },
  });
};
</script>
