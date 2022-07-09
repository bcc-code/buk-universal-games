<template>
  <UserPageLayout>
    <PointsAndStickers :points="teamStatus?.status?.points" :stickers="teamStatus?.status?.stickers" :refresh="() => refreshStatus()"/>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from '../components/UserPageLayout.vue'
import PointsAndStickers from '../components/PointsAndStickers.vue'

export default {
  name: 'LoginPage',
  props: {
    data: String,
  },
  components: { UserPageLayout, PointsAndStickers },
  data() {
  },
  created() {
    if(Object.keys(this.$store.state.teamStatus).length === 0) {
       this.refreshStatus() 
    }
  },
  methods: {
    refreshStatus() {
      this.$store.dispatch("getTeamStatus")
    }
  },
  computed: {
    teamStatus() {
      return this?.$store.state.teamStatus
    }
  }
}
</script>

<style scoped>
</style>
