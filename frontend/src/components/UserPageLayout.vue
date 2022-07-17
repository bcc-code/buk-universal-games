<template>
  <section class="user-page-layout">
    <div class="content-area">
      <h1 class="title">BUK {{ $store.state.loginData.team }}</h1>
      <slot />
    </div>
    <UserMenu />
  </section>
</template>

<script>
import UserMenu from "../components/UserMenu.vue";

export default {
  name: "UserPageLayout",
  components: { UserMenu },
  data() {
    return {
      title: "Tittel",
    };
  },
  mounted() {
    if (Object.keys(this.$store.state.loginData).length === 0) {
      this.$store.dispatch("getLoginData");
    }
  },
  methods: {},
};
</script>

<style scoped>
.user-page-layout {
  display: flex;
  flex-direction: column;
  height: 100%;
  justify-content: space-between;
  padding-bottom: 6em;
}

.content-area {
  height: 100%;
  overflow: auto;
  padding: 2em 1em;
}

.title {
  padding-bottom: .5em;
}
</style>
