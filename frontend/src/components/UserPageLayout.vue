<template>
  <section class="user-page-layout">
    <div class="content-area">
      <h1 v-if="showTitle" class="title">BUK {{ $store.state.loginData.team }}</h1>
      <slot />
    </div>
    <AdminMenu v-if="$store.state.loginData.access === 'Admin'" />
    <UserMenu v-else />
  </section>
</template>

<script>
import UserMenu from "../components/UserMenu.vue";
import AdminMenu from "../components/AdminMenu.vue";

export default {
  name: "UserPageLayout",
  components: {
    AdminMenu,
    UserMenu,
  },
  props: {
    showTitle: Boolean,
  },
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
  padding-bottom: 5em;
}

.content-area {
  height: 100%;
  overflow: auto;
  padding: 2em 1em;
}

.title {
  padding-bottom: 0.5em;
}
</style>
