<template>
  <UserPageLayout> Game Info </UserPageLayout>
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
      loginError: "Game Info",
    };
  },
  mounted() {},
  methods: {},
};
</script>

<style scoped></style>
