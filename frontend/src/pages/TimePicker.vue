<template>
  <form @submit.prevent="submitForm">
    <div
    class="w-64"
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
      ></VueDatePicker>
    </div>
      <button type="submit">Submit</button>
  </form>
</template>

<script setup>
import { ref, watch ,computed} from 'vue';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import z from 'zod'
import { toRaw } from 'vue';

const date = ref();

const timeSchema = z.object({
  hours: z.number(),
  minutes: z.number(),
  seconds: z.number() ,
}).optional();

const validatedDate = computed(()=>{
  return timeSchema.parse(date.value)
})

watch(validatedDate,(val)=>console.log(toRaw(val) ),{immediate:true})

const submitForm = () => {
  alert('Form submitted');
  console.log(validatedDate.value)
}
</script>

<style>
.dp__theme_light {
    --dp-disabled-color-text: #00000000;
}
</style>