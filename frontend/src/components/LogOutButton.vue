<template>
  <div class="bg-white rounded-md p-2 btn" @click="logout">
    <ArrowRightStartOnRectangleIcon class="h-5 text-label-1" />
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
