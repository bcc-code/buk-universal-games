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
      <div class = 'mb-4' :v-if="typeof calculatedResult ==='number'">

        Beregnet score: {{calculatedResult}}
      </div>
      
      <button type="submit" class="btn btn-success btn-blank h-14 p-4 shadow-md"
      :disabled="isPending"
      
      >Lagre</button>
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
import { ref, watch ,computed} from 'vue';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import z from 'zod'
import { toRaw } from 'vue';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const date = ref<unknown>();
const cheats= ref<""|number>("");

const timeSchema = z.object({
  hours: z.number(),
  minutes: z.number(),
  seconds: z.number() ,
}).optional();

const validatedDate = computed(()=>{
  return timeSchema.parse(date.value)
})

watch(validatedDate,(val)=>console.log(toRaw(val) ),{immediate:true})
watch(cheats,(val)=>console.log(toRaw(val),typeof val ),{immediate:true})

const calculatedResult = computed<number|undefined>(()=>{
  if(typeof cheats.value !== 'number') return;
  if(!validatedDate.value) return;
  cheats.value
  validatedDate.value
let score;
// ...
return score;

})

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const { mutate: confirmResult, isPending, error } = useConfirmTeamResult();

const submitForm = () => {
  const matchId:number =props.match.matchId
  const result :number= calculatedResult.value
  const teamId :number= props.match.team1Id;
   confirmResult({matchId:matchId, result:result, teamId:teamId},{onSuccess(){
    showSuccessToast()
  }})
  
}

</script>
<style>
.dp__theme_light {
    --dp-disabled-color-text: #00000000;
}
</style>