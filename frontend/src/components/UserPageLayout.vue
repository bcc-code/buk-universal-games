<template>
  <section class="user-page-layout">
    <section class="user-content-wrapper">
      <h1 class="login-msg"> BUK {{ $store.state.loginData.team }} </h1>
      <slot />
    </section>
    <UserMenu />
  </section>
</template>

<script>
import UserMenu from '../components/UserMenu.vue'

export default {
  name: 'UserPageLayout',
  components: { UserMenu },
  data() {
    return {
        title: "Tittel"
    }
  },
  beforeCreate() {
    if(Object.keys(this.$store.state.loginData).length === 0) {
       this.$store.dispatch("getLoginData")
    }
  },
  methods: {

  }
}
</script>

<style scoped>
    .user-page-layout {
        display: flex;
        flex-direction: column;
        height: 100%;
        justify-content: space-between;
    }

    .user-content-wrapper {
      padding: 2em 1em;
    }
</style>
