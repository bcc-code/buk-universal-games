<template>
  <section class="user-page-layout">
    <h1 class="login-msg">{{title}}</h1>
    <slot />
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
</style>
