<template>
  <div class="bg-vanilla rounded-md p-2 align-middle justify-center shadow-md" @click="logout">
    <ArrowRightStartOnRectangleIcon class="h-4 text-label-1" />
  </div>
</template>

<script>
import { ArrowRightStartOnRectangleIcon } from '@heroicons/vue/24/solid';

export default {
  name: 'LogOutButton',
  components: { ArrowRightStartOnRectangleIcon },
  methods: {
    logout() {
      window.localStorage.clear();
      this.$router.push('/');
    },
  },
};
</script>
