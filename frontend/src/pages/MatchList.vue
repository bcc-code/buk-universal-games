<template>
  <UserPageLayout> Match List </UserPageLayout>
</template>

<script>
import UserPageLayout from "../components/UserPageLayout.vue";

export default {
  name: "LoginPage",
  props: {
    data: String,
  },
  components: { UserPageLayout },
  data() {
    return {
      loginError: "Match List",
    };
  },
  mounted() {},
  methods: {},
};
</script>

<style scoped></style>
