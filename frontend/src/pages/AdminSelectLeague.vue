<template>
  <section class="px-5 flex justify-center items-center h-screen w-full">
    <form class="flex flex-col py-10 space-y-10 justify-center align-middle w-full" @submit="tryLogin">
      <img src="/image/logo_icon.svg" alt="" class="logo" />
      <div class="w-full justify-center flex">
        <p class="text-xl">
          {{ $t('admin.select_league.intro') }}
        </p>
        
      </div>
      <div class="flex space-x-5">
        <AdminLeagueSelector v-for="league in adminLeagues" class="bg-vanilla" :key="league.id" :name="league.name"
          @click="selectLeague(league.id)" />
      </div>

      <p v-if="loginMessage" class="login-msg">{{ loginMessage }}</p>
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
      this.$router.push({ name: 'AdminSelectGame' });
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
