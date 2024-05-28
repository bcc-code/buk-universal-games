<template>
  <div class="bg-white rounded-md p-2" @click="logout">
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
      this.$store.dispatch('logout'); // Dispatch the logout action
      this.$router.push('/'); // Redirect to login page after logout
    },
  },
};
</script>
