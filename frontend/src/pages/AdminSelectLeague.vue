<template>
  <section class="px-5 flex justify-center items-center h-screen">
    <form
      class="flex flex-col py-10 space-y-10 justify-center align-middle"
      @submit="tryLogin"
    >
      <img src="/image/logo_icon.svg" alt="" class="logo" />
      <div class="w-full justify-center flex">
        <p class="text-white">
          {{ $t('admin.select_league.intro') }}
        </p>
      </div>
      <div class="flex space-x-5">
        <AdminLeagueSelector
          v-for="league in adminLeagues"
          class="bg-vanilla"
          :key="league.id"
          :name="league.name"
          @click="selectLeague(league.id)"
        />
      </div>

      <p v-if="loginMessage" class="login-msg">{{ loginMessage }}</p>
      <div class="mx-auto text-center bg-hazy-green/10 rounded-md p-3">
        <p class="text-label-3 text-xs">{{ $t('install_hint') }}</p>
      </div>
    </form>
  </section>
</template>

<script>
import AdminLeagueSelector from '@/components/AdminLeagueSelector.vue';

export default {
  name: 'AdminSelectLeague',
  components: { AdminLeagueSelector },
  created() {
    if (!this.$store.state.adminLeagues.length) {
      this.getAdminLeagues();
    }
  },
  methods: {
    getAdminLeagues() {
      this.$store.dispatch('getAdminLeagues');
    },
    async selectLeague(id) {
      await this.$store.dispatch('setAdminLeagueSelected', id);
      this.$router.push({ name: 'AdminLeagueStatus' });
    },
  },
  computed: {
    adminLeagues() {
      return this?.$store.state.adminLeagues;
    },
  },
};
</script>

<style scoped>
.bg {
  height: 100%;
  background-size: cover;
  background-position: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 1em;
}

.logo {
  width: 80%;
  max-width: 400px;
  margin: 0 auto 2em auto;
  display: block;
  border: 10px solid white;
  border-radius: 80px;
}

.league-card {
  margin: 0.25em 0;
}
</style>
