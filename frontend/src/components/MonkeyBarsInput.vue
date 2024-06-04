<template>
  <div>
    <input
      type="number"
      v-model="totalNrOfBars"
      @input="updateTotal"
      :placeholder="'Total nr of bars'"
    />
    <input
      type="number"
      v-model="nrOfParticipants"
      @input="updateTotal"
      :placeholder="'Nr of participants'"
    />
  </div>
</template>

<script>
export default {
  name: 'MonkeyBarsInput',
  props: {
    modelValue: Number,
  },
  emits: ['update:model-value'],
  data() {
    return {
      totalNrOfBars: null,
      nrOfParticipants: null,
      averageBars: this.value,
    };
  },
  methods: {
    updateTotal() {
      if (this.nrOfParticipants > 0) {
        this.averageBars = this.totalNrOfBars / this.nrOfParticipants;
      } else {
        this.averageBars = 0;
      }
      this.$emit('update:model-value', Math.ceil(this.averageBars * 10));
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
