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
      value: String
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
    display: flex;
    flex-direction: column;
    width: 100%;
    align-items: center;
  }
  
  input {
    padding: 10px;
    border: none;
    border-radius: 5px;
    background-color: #f2f2f2;
    box-sizing: border-box;
    width: 80%;
  }
  </style>