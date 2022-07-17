<template>
  <section class="admin-page-layout">
    <div class="content-area">
      <h1 class="title">Admin</h1>
      <slot />
    </div>
    <AdminMenu />
  </section>
</template>

<script>
import AdminMenu from "../components/AdminMenu.vue";

export default {
  name: "AdminPageLayout",
  components: { AdminMenu },
  data() {
    return {
      title: "Tittel",
    };
  },
  mounted() {
    // if (Object.keys(this.$store.state.loginData).length === 0) {
    //   this.$store.dispatch("getLoginData");
    // }
  },
  methods: {},
};
</script>

<style scoped>
.admin-page-layout {
  display: flex;
  flex-direction: column;
  height: 100%;
  justify-content: space-between;
  padding-bottom: 5em;
}

.content-area {
  height: 100%;
  overflow: auto;
  padding: 2em;
}

.title {
  padding-bottom: 1em;
}
</style>
