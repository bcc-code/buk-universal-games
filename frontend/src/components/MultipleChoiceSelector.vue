<template>
    <div class="multiple-choice">
      <button
        v-for="(option, index) in options"
        :key="index"
        :class="{ active: selectedOption === option.value }"
        @click="selectOption(option.value)"
      >
        {{ option.label }}
      </button>
    </div>
  </template>
  
  <script>
  export default {
    name: "MultipleChoiceQuestion",
    props: {
      options: {
        type: Array,
        required: true,
      },
      value: {
        type: String,
        default: "",
      },
    },
    data() {
      return {
        selectedOption: this.value,
      };
    },
    methods: {
      selectOption(optionValue) {
        this.selectedOption = optionValue;
        this.$emit("input", optionValue);
        this.$emit("option-selected", optionValue);
      },
    },
  };
  </script>
  
  <style scoped>
  .multiple-choice {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-gap: 10px;
  }
  
  button {
    padding: 10px;
    border: none;
    border-radius: 5px;
    background-color: #f2f2f2;
    cursor: pointer;
  }
  
  button.active {
    background-color: #4caf50;
    color: white;
  }
  </style>