<template>
  <div class="flex">
    <nav
      class="text-dark mr-4 mb-5 bg-ice-blue cursor-pointer rounded-md hover:bg-vanilla text-dark-blue btn flex items-center justify-center"
      @click="router.back()"
    >
      <div class=" ">
        <ArrowLeftIcon class="h-5" />
      </div>
    </nav>
  </div>
</template>

<script setup lang="ts">
import { ArrowLeftIcon } from '@heroicons/vue/24/solid';
import { useRouter } from 'vue-router';

const router = useRouter();
</script>