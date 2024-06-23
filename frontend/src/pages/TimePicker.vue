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
      <button type="submit" class="btn btn-success btn-blank h-14 p-4 shadow-md"
      :disabled="isPending"
      
      >Lagre</button>
  </form>
</div>
</template>

<script setup>
import { ref, watch ,computed} from 'vue';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import z from 'zod'
import { toRaw } from 'vue';
import { useConfirmTeamResult } from '@/hooks/hooks';

const date = ref();
const cheats= ref("");

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

const submitForm = () => {
  alert('Form submitted');
  console.log(validatedDate.value)
  confirmResult({matchId:matchId, result:result, teamId:teamId},{})
}

const { mutate: confirmResult, isPending } = useConfirmTeamResult();

</script>

<style>
.dp__theme_light {
    --dp-disabled-color-text: #00000000;
}
</style>