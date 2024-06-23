<template>

  <div class="p-4 bg-white rounded-md">

    <form @submit.prevent="submitForm">
       <div class="flex justify-evenly">

         <div class="mb-4">
           Poeng {{ match.team1 }}:
          <br/>

           <input
           class="shadow-md border-solid border-2 border-slate-200 rounded-md w-36"
           type="number"
           v-model="pointsTeam1"
          min="0"
          max="9"
          :placeholder="'Poeng'"
          required
          />
          <div v-if="calculatedResult">
          Beregnet score: {{calculatedResult.team1Result}}
        </div>
        </div>
        <div class="mb-4">
          Poeng {{ match.team2 }}:
          <br/>
          <input
          class="shadow-md border-solid border-2 border-slate-200 rounded-md w-36"
          type="number"
          v-model="pointsTeam2"
          min="0"
          max="9"
          :placeholder="'Poeng'"
          required
          />
          <div v-if="calculatedResult">
            Beregnet score: {{calculatedResult.team2Result}}
          </div>
        </div>
      </div>
        
      <button type="submit" class="btn btn-success btn-blank h-14 p-4 shadow-md" :disabled="isPending">
        Lagre
      </button>
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
import { ref, computed } from 'vue';
import z from 'zod';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const pointsTeam1 = ref<number | ''>('');
const pointsTeam2 = ref<number | ''>('');

const scoreSchema = z.object({
  pointsTeam1: z.number().min(0).max(9),
  pointsTeam2: z.number().min(0).max(9),
}).optional();

const validatedScores = computed(() => {
  const newLocal = scoreSchema.safeParse({ pointsTeam1: pointsTeam1.value, pointsTeam2: pointsTeam2.value });
  return newLocal.success ? newLocal.data : undefined;
});

const calculatedResult = computed<{ team1Result: number; team2Result: number } | undefined>(() => {
  if (!validatedScores.value) return;

  const minScore = 1;
  const maxScore = 20;
  const minPoints = 0;
  const maxPoints = 9;

  const lerp = (min: number, max: number, t: number) => min + t * (max - min);

  const team1Result = lerp(minScore, maxScore, (validatedScores.value.pointsTeam1 - minPoints) / (maxPoints - minPoints));
  const team2Result = lerp(minScore, maxScore, (validatedScores.value.pointsTeam2 - minPoints) / (maxPoints - minPoints));

  return {
    team1Result: Math.round(team1Result),
    team2Result: Math.round(team2Result),
  };
});

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

// 🧹 create a new endpoint which both of these, or at least create a hook so we know what interface we want.
const { mutate: confirmResultTeam1, isPending: isPendingTeam1, error: errorTeam1 } = useConfirmTeamResult();
const { mutate: confirmResultTeam2, isPending: isPendingTeam2, error: errorTeam2 } = useConfirmTeamResult();

const error = computed(() => errorTeam1.value || errorTeam2.value);
const isPending = computed(() => isPendingTeam1.value || isPendingTeam2.value);

const submitForm = () => {
  if (!calculatedResult.value) throw Error('CalculatedResult returned undefined. The form is invalid even if it can be submitted. You might be missing some validation on the form fields.');

  const matchId = props.match.matchId;

  const resultTeam1 = calculatedResult.value.team1Result;
  const resultTeam2 = calculatedResult.value.team2Result;

  const team1Id = props.match.team1Id;
  const team2Id = props.match.team2Id;

  const variablesTeam1 = { matchId: matchId, result: resultTeam1, teamId: team1Id };
  const variablesTeam2 = { matchId: matchId, result: resultTeam2, teamId: team2Id };

  confirmResultTeam1(variablesTeam1, {
    onSuccess() {
      confirmResultTeam2(variablesTeam2, {
        onSuccess() {
          showSuccessToast();
        },
      });
    },
  });
};
</script>