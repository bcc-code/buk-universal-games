<template>
  <div>
    <input type="number" v-model="minutes" @input="updateTotal" :placeholder="'Minutes'" />
    <input type="number" v-model="seconds" @input="updateTotal" :placeholder="'Seconds'" />
    <input type="number" v-model="penaltyPoints" @input="updateTotal" :placeholder="'Penalty Points'" />
  </div>
</template>

<script>
export default {
  name: "TableSurfingInput",
  props: {
    modelValue: Number,
  },
  emits: ['update:model-value'],
  data() {
    return {
      minutes: null,
      seconds: null,
      penaltyPoints: null,
      totalSeconds: this.value,
    };
  },
  methods: {
    updateTotal() {
      this.totalSeconds = this.minutes * 60 + this.seconds + this.penaltyPoints * 10;
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