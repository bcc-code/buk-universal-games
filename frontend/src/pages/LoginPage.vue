<template>
  <section
    class="bg"
    :style="{
      'background-image': `url(${image})`,
    }"
  >
    <div class="content">
      <img src="../assets/logo.png" alt="" class="logo" />
      <input type="text" placeholder="Team code" v-model="teamCode" />
      <router-link v-if="teamCode.length > 3" class="btn-primary" :to="loginUrl">Login</router-link>
      <p v-if="loginMessage" class="login-msg">{{ loginMessage }}</p>
    </div>
  </section>
</template>

<script>
export default {
  name: "LoginPage",
  data() {
    return {
      image: require("@/assets/bg.png"),
      teamCode: "8PB3P5N",
    };
  },
  mounted() {
    this.$store.commit("setLoginMessage", "");
  },
  computed: {
    loginMessage() {
      return this.$store.state.loginMessage ?? "";
    },
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
  background-size: cover;
  background-position: center;
  padding: 1em 2em;
  min-height: 100%;
}

.content {
  padding-top: 10em;
  padding-bottom: 10em;
  display: flex;
  flex-direction: column;
  justify-content: center;
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
