<template>
  
  <div class="p-4 bg-white rounded-md">

    <form @submit.prevent="submitForm">
      <div class="mb-4">

        Henge-tid:
        <div
        class="w-64  inline-block   shadow-md"
        >
        <VueDatePicker 
        v-model="date"
        required
        auto-apply
        enable-seconds
        time-picker
        no-hours-overlay
        :min-time="{hours:0, minutes:0, seconds:1}"
        :max-time="{hours:0, minutes:10, seconds:0}"
        :start-time="{hours:0, minutes:0, seconds:1}"
        minutes-grid-increment="1" 
        format="mm:ss"
        :clearable="false"
        :ui="{input:'h-14 inline'}"
        placeholder="Henge-tid"
        ></VueDatePicker>
      </div>
      </div>
      <div class="mb-4">
        
        Juks:
        <input
              class="shadow-md border-solid border-2 border-slate-200 rounded-md"
              type="number"
              v-model="cheats"
              :placeholder="'Antall juks'"
              required
              name="reg" id="reg" 
            />

      </div>

      <div class = 'mb-4' v-if="typeof calculatedResult ==='number'">
        Beregnet score: {{calculatedResult}}
      </div>
      
      <button type="submit" class="btn btn-success btn-blank h-14 p-4 shadow-md"
      :disabled="isPending"
      
      >Lagre</button>
  </form>
</div>
<div class="toast toast-center toast-bottom pb-24 z-20" v-if="error">
    <!-- 🧹test error from backend, it should show a good message. maybe do 0-20 validation. -->
    <div class="alert alert-error block">{{ error.message }}</div>
  </div>
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="showSuccess">
    <div class="alert alert-success block">Lagret</div>
  </div>


</template>

<script setup lang="ts">
import { ref, watch ,computed} from 'vue';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import z from 'zod'
// 🧹 throw err on unused imports or variables
import { toRaw } from 'vue';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
import { timeSchema, type TimeType } from './TimeType';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const date = ref<TimeType>();
const cheats= ref<""|number>("");


// 🧹 move to time picker component. it should be used inside a form. Create that as a comment. 
const validatedDate = computed(()=>{
  return timeSchema.parse(date.value)
})

// 🧹remove
// watch(validatedDate,(val)=>console.log(toRaw(val) ),{immediate:true})
// watch(cheats,(val)=>console.log(toRaw(val),typeof val ),{immediate:true})

// 🧹 the number should be returned from each registration component.
// 🧹 remove comments
const calculatedResult = computed<number | undefined>(() => {

  // 🧹handle time to number conversion centrally? a date should just be a number, and the size of the date object should never be relevant.
  const timePenaltyPerCheat = 10 / (24 * 60 * 60); // Convert 10 seconds to fraction of a day
  const lowerBoundEffectiveTime = 2 / (24 * 60); // Convert 2 minutes to fraction of a day
  const upperBoundEffectiveTime = 7 / (24 * 60); // Convert 7 minutes to fraction of a day
  const minScore = 1;
  const maxScore = 20;
  const minCheats = 0;
  const maxCheats = 10;

  if (typeof cheats.value !== 'number') return;
  if (!validatedDate.value) return;

  // 🧹use lerp
  // Convert validatedDate to fractional minutes
  const totalHengeTid =
    (validatedDate.value.hours * 60 + validatedDate.value.minutes + validatedDate.value.seconds / 60) / (24 * 60);

    // 🧹use clamp
  // Calculate the time penalty
  const timePenalty = Math.max(minCheats,Math.min(cheats.value,maxCheats)) * timePenaltyPerCheat;

  // Calculate the effective henge-tid
  const effectiveHengeTid = totalHengeTid - timePenalty;

  // 🧹 create lerp func
  // Calculate the unclamped score using LERP
  const unclampedScore =
    ((effectiveHengeTid - lowerBoundEffectiveTime) * (maxScore - minScore)) /
      (upperBoundEffectiveTime - lowerBoundEffectiveTime) +
    minScore;

    // 🧹 create clamp func
  // Clamp the score between minScore and maxScore
  const clampedScore = Math.max(minScore, Math.min(unclampedScore, maxScore));

  return floatToInt(clampedScore);
});

// 🧹 we should be able to take decimals on the backend, and save it. 
function floatToInt(num:number):number{
  return Math.round(num)
}

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
watch(error,(val)=>console.log("error",toRaw(val),JSON.stringify(toRaw(val)) ),{immediate:true})

const submitForm = () => {
  const matchId:number =props.match.matchId;
  
  if(! calculatedResult.value) throw Error('CalculatedResult returned undefined. The form is involid even if it can be submitted. You might be missing some validation on the form fields.')

  const result = calculatedResult.value;

  const teamId :number= props.match.team1Id;

  const variables = { matchId: matchId, result: result, teamId: teamId };
  console.log(variables)
   confirmResult(variables,{
    onSuccess(){
    showSuccessToast()
  }})
  
}
// 🧹 if the component is rendered and the score are already set, then show a confirmation before registering scores. We press it first to allow overwriting scores.
</script>
<style>
/* 🧹move to time picker component */
.dp__theme_light {
    --dp-disabled-color-text: #00000000;
}
</style>