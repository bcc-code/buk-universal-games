<template>
    <div class="single-text-input">
      <input
        type="text"
        :value="selectedValue"
        @input="updateValue($event.target.value)"
      />
    </div>
  </template>
  
  <script>
  export default {
    name: "SimpleGuessInput",
    props: {
      value: {
        type: String,
        required: true,
      },
    },
    data() {
      return {
        selectedValue: this.value,
      };
    },
    methods: {
      updateValue(newValue) {
        this.selectedValue = newValue;
        this.$emit("update:value", newValue);
      },
    },
  };
  </script>
  
  <style scoped>
  .single-text-input {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-gap: 10px;
  }
  
  input {
    padding: 10px;
    border: none;
    border-radius: 5px;
    background-color: #f2f2f2;
    box-sizing: border-box;
    width: 100%;
  }
  </style>