<template>
  <UserPageLayout>
    <section class="user-section">
      <PointsAndStickers :points="teamStatus?.status?.points" :stickers="teamStatus?.status?.stickers" :refresh="() => getTeamStatus()"/>
    </section>
    <section class="user-section" v-for="(status, i) in leagueStatus" :key="status.id">
      <LeageListItem :class="{'card-light' : i > 4}" :index="i+1" :team="status.team" :stickers="status.stickers" :points="status.points  " />
    </section>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from '../components/UserPageLayout.vue'
import PointsAndStickers from '../components/PointsAndStickers.vue'
import LeageListItem from '../components/LeageListItem.vue'

export default {
  name: 'LoginPage',
  props: {
    data: String,
  },
  components: { UserPageLayout, PointsAndStickers, LeageListItem },
  data() {
  },
  created() {
    if(Object.keys(this.$store.state.teamStatus).length === 0) {
       this.getTeamStatus() 
    }

    if(Object.keys(this.$store.state.leagueStatus).length === 0) {
       this.getLeagueStatus() 
    }
  },
  methods: {
    getTeamStatus() {
      this.$store.dispatch("getTeamStatus")
    },
    getLeagueStatus() {
      this.$store.dispatch("getLeagueStatus")
    }
  },
  computed: {
    teamStatus() {
      return this?.$store.state.teamStatus
    },
    leagueStatus() {
      return this?.$store.state.leagueStatus.status?.slice(0, 10)
    }
  }
}
</script>

<style>
  .user-section {
    padding: .25em 0;
  }

</style>
