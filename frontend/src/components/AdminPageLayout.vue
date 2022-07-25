<template>
  <section class="admin-page-layout">
    <div class="content-area">
      <h1 class="title">{{ title }}</h1>
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
  props: { title: String },
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
  width: 100%;
  justify-content: space-between;
  padding-bottom: 5em;
}

.content-area {
  width: 100%;
  padding: 1em 1em 10em;
  background-color: var(--gray-1);
}

.title {
  padding-bottom: 1em;
}
</style>
