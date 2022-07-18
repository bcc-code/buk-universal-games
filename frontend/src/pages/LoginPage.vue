<template>
  <section
    class="bg"
    :style="{
      'background-image': `url(${image})`,
    }"
  >
    <img src="../assets/logo.png" alt="" class="logo" />
    <input type="text" placeholder="Team code" v-model="teamCode" />
    <router-link v-if="teamCode.length > 3" class="btn-primary" :to="loginUrl">Login</router-link>
    <p v-if="loginError" class="login-msg">{{ loginError }}</p>
  </section>
</template>

<script>
export default {
  name: "LoginPage",
  data() {
    return {
      image: require("@/assets/bg.png"),
      teamCode: "8PB3P5N",
      loginError: null,
    };
  },
  computed: {
    loginUrl() {
      let url = "/";

      if (this.teamCode) {
        url = "/" + this.teamCode + "/league-list";
      }

      return url;
    },
  },
};
</script>

<style scoped>
.bg {
  height: 100%;
  background-size: cover;
  background-position: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 1em;
}

.btn-primary {
  margin: 1em auto;
  width: 100%;
  text-decoration: none;
}

.login-msg {
  color: white;
  text-align: center;
}
</style>
