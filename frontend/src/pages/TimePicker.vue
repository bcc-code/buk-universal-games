<template>
<!-- 🧹formatting   -->
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
    :min-time="{hours:0, minutes:0, seconds:0}"
    :max-time="{hours:0, minutes:10, seconds:0}"
    :start-time="{hours:0, minutes:0, seconds:1}"
    minutes-grid-increment="1" 
    format="mm:ss"
    :clearable="false"
    :ui="{input:'h-14 inline'}"
    :placeholder="placeholder"
    @update:model-value="(newDate:unknown)=>timeSchema.parse(newDate)"
    ></VueDatePicker>
  </div>
</template>

<script setup lang="ts">
/** This component should be used inside a form with submit. That way, when the onSubmit on the form is called, then it will show an error message on this component if the date is undefined. Only this way, the date is actually defined when onSubmit is being run. */
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import { timeSchema, type TimeType } from './TimeType';

const date = defineModel<TimeType>()
const {placeholder} = defineProps<{placeholder:string}>()

</script>
<style>
.dp__theme_light {
    --dp-disabled-color-text: #00000000;
}
</style>