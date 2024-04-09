<template>
  <UserPageLayout>
    <PointsAndStickers
      :points="teamStatus?.points"
      :stickers="coins.length"
      :refresh="refresh"
      :loading="$store.state.gamesLoading"
    />

    <h2>{{ $t('menu.games') }}</h2>

    <GamesList
      :games="$store.state.games"
      :loading="$store.state.gamesLoading"
      @clicked="gameClicked"
    ></GamesList>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from '@/components/UserPageLayout.vue'
import PointsAndStickers from '@/components/PointsAndStickers.vue'
import GamesList from '@/components/GamesList.vue'

export default {
  name: 'GameInfo',
  components: { UserPageLayout, PointsAndStickers, GamesList },
  created() {
    this.$store.dispatch('getGames')

    if (Object.keys(this.$store.state.leagueStatus).length === 0) {
      this.refresh()
    }
  },
  methods: {
    async refresh() {
      this.$store.commit('setGamesLoading', true)
      await this.$store.dispatch('getLeagueStatus', true)
      this.$store.commit('setGamesLoading', false)
    },
    gameClicked(game) {
      this.$router.push({
        name: 'GameInfoDetail',
        params: {
          game: game.id
        }
      })
    }
  },
  computed: {
    coins() {
      return this.$store.state.coins
    },
    teamStatus() {
      return this?.leagueStatus?.status?.total?.find(
        (score) => score.team == this.$store.state.loginData?.team
      )
    },
    leagueStatus() {
      return this?.$store.state.leagueStatus
    }
  }
}
</script>
