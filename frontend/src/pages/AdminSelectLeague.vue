<template>
  <section
    class="bg"
    :style="{
      'background-color': '#a0e3be'
    }"
  >
    <img src="/image/ubg-logo.svg" alt="" class="logo" />
    <section class="leagues">
      <p>
        {{ $t('admin.select_league.intro') }}
      </p>
      <AdminLeagueSelector
        v-for="league in adminLeagues"
        class="league-card"
        :key="league.id"
        :name="league.name"
        @click="selectLeague(league.id)"
      />
    </section>
  </section>
</template>

<script>
import AdminLeagueSelector from '@/components/AdminLeagueSelector.vue'

export default {
  name: 'AdminSelectLeague',
  components: { AdminLeagueSelector },
  created() {
    if (!this.$store.state.adminLeagues.length) {
      this.getAdminLeagues()
    }
  },
  methods: {
    getAdminLeagues() {
      this.$store.dispatch('getAdminLeagues')
    },
    async selectLeague(id) {
      await this.$store.dispatch('setAdminLeagueSelected', id)
      this.$router.push({ name: 'AdminLeagueStatus' })
    }
  },
  computed: {
    adminLeagues() {
      return this?.$store.state.adminLeagues
    }
  }
}
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
