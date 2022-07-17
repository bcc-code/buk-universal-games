<template>
  <AdminPageLayout>
    <h2>Admin Games</h2>

    <router-link to="/admin/AdminGame">Single game</router-link>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";

export default {
  name: "AdminGames",
  components: { AdminPageLayout },
  data() {
    return {};
  },
  mounted() {},
  methods: {},
  computed: {},
};
</script>

<style scoped></style>
