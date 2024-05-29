<template>
  <section class="px-5 flex justify-center items-center h-screen">
    <form class="flex flex-col py-10 space-y-10 justify-center align-middle" @submit="tryLogin">
      <img src="/image/logo_icon.svg" alt="" class="logo" />
      <div class="w-full justify-center flex">
        <p>
          {{ $t('admin.select_game.intro') }}
        </p>
      </div>
      <div class="flex space-x-5">
        <AdminLeagueSelector v-for="game in adminGames" class="bg-vanilla" :key="game.id" :name="game.name"
          @click="selectGame(game.id)" />
      </div>

      <p v-if="loginMessage" class="login-msg">{{ loginMessage }}</p>
    </form>
  </section>
</template>

<script>
import AdminLeagueSelector from '@/components/AdminLeagueSelector.vue';

export default {
  name: 'AdminSelectGame',
  components: { AdminLeagueSelector },
  methods: {
    getAdminGames() {
      this.$store.dispatch('getAdminGames');
    },
    async selectGame(id) {
      console.log(id)
      await this.$store.commit('setAdminFilterGameSelected', id);
      
      this.$router.push({ name: 'AdminMatchListGame' });
    },
  },
  computed: {
    adminGames() {
      return this?.$store.state.games;
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

.game-card {
  margin: 0.25em 0;
}
</style>
