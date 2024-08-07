<template>
  <div class="p-4 bg-white rounded-md">
    <form @submit.prevent="submitForm">
      <div class="mb-4">
        Henge-tid:
        <TimePicker v-model="date" placeholder="Henge-tid"></TimePicker>
      </div>
      <div class="mb-4">
        Juks:
        <input
          class="shadow-md border-solid border-2 border-slate-200 rounded-md"
          type="number"
          v-model="cheats"
          :placeholder="'Antall juks'"
          required
          name="reg"
          id="reg"
          min="0"
        />
      </div>

      <div class="mb-4" v-if="typeof calculatedResult === 'number'">
        Beregnet score: {{ calculatedResult }}
      </div>

      <LoadingButton
        type="submit"
        class="btn-success btn-blank h-14 p-4"
        :is-loading="isPending"
      >
        Lagre
      </LoadingButton>
    </form>
  </div>
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="error">
    <div class="alert alert-error block">{{ error.message }}</div>
  </div>
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="showSuccess">
    <div class="alert alert-success block">Lagret</div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import '@vuepic/vue-datepicker/dist/main.css';
import { toRaw } from 'vue';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
import { timeToNumber, type TimeType } from './TimeType';
import TimePicker from './TimePicker.vue';
import { clamp, floatToInt, lerp } from './mathHelpers';
import LoadingButton from '@/components/LoadingButton.vue';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const date = ref<TimeType>();
const cheats = ref<'' | number>('');

// 🧹 pass to parent which handles saving
const calculatedResult = computed<number | undefined>(() => {
  const timePenaltyPerCheat = timeToNumber({
    hours: 0,
    minutes: 0,
    seconds: 10,
  });
  const lowerBoundEffectiveHengeTid = timeToNumber({
    hours: 0,
    minutes: 2,
    seconds: 0,
  });
  const upperBoundEffectiveHengeTid = timeToNumber({
    hours: 0,
    minutes: 7,
    seconds: 0,
  });
  const minScore = 1;
  const maxScore = 20;
  const minCheats = 0;
  const maxCheats = 10;

  if (typeof cheats.value !== 'number') return;
  if (!date.value) return;

  const totalHengeTid = timeToNumber(date.value);
  const timePenalty =
    clamp(minCheats, cheats.value, maxCheats) * timePenaltyPerCheat;
  const effectiveHengeTid = totalHengeTid - timePenalty;

  const score = lerp(
    lowerBoundEffectiveHengeTid,
    upperBoundEffectiveHengeTid,
    minScore,
    maxScore,
    effectiveHengeTid,
    'clamp',
  );

  return floatToInt(score);
});

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

// 🧹 move form and its submission to parent component. it should still include button, mutation, toasts, wrapper div.
const { mutate: confirmResult, isPending, error } = useConfirmTeamResult();
// 🧹remove
watch(
  error,
  (val) => console.log('error', toRaw(val), JSON.stringify(toRaw(val))),
  { immediate: true },
);

const submitForm = () => {
  const matchId: number = props.match.matchId;

  if (!calculatedResult.value)
    throw Error(
      'CalculatedResult returned undefined. The form is involid even if it can be submitted. You might be missing some validation on the form fields.',
    );

  const result = calculatedResult.value;

  const teamId: number = props.match.team1Id;

  const variables = { matchId: matchId, result: result, teamId: teamId };
  console.log(variables);
  confirmResult(variables, {
    onSuccess() {
      showSuccessToast();
    },
  });
};
// 🧹 if the component is rendered and the score are already set, then show a confirmation before registering scores. We press it first to allow overwriting scores.
</script>
