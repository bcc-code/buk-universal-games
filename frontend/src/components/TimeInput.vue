<template>
  <div>
    <input type="number" v-model="minutes" @input="updateTotal" :placeholder="'Minutes'" />
    <span>&nbsp;:&nbsp;</span> 
    <input type="number" v-model="seconds" @input="updateTotal" :placeholder="'Seconds'" />
  </div>
</template>

<script>
export default {
  name: "TimeInput",
  props: {
    modelValue: Number,
  },
  emits: ['update:model-value'],
  data() {
    return {
      minutes: null,
      seconds: null,
      totalSeconds: this.value,
    };
  },
  methods: {
    updateTotal() {
      this.totalSeconds = this.minutes * 60 + this.seconds;
      this.$emit("update:model-value", this.totalSeconds);
    },
  },
};
</script>

<style scoped>
input {
  margin: 0.5em;
}
</style>
```