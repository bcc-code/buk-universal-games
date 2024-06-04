<template>
  <section
    :class="[
      'whitespace-nowrap text-dark-brown rounded-md p-2 m-1 flex flex-col justify-center items-center btn shadow-md',
      additionalClass,
    ]"
  >
    {{ name }}
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  name: string;
  selectedClass: string;
  unselectedClass: string;
  isSelected: boolean;
}>();

const additionalClass = computed(() =>
  props.isSelected ? props.selectedClass : props.unselectedClass,
);
</script>

<style scoped>
.card {
  padding: 1em;
  margin: 0 0.25em;
  border-radius: 0.75em;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.card-dark {
  background-color: var(--dark);
  color: white;
}
</style>
