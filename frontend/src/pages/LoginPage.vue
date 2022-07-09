<template>
  <section class="bg" :style="{
    'background-image': `url(${image})`,
    }">
    <img src="../assets/logo.png" alt="" class="logo">
    <input type="text" placeholder="Team code" v-model="teamCode">
    <button class="btn-primary" @click="login">Login</button>
    <p v-if="loginError" class="login-msg">{{loginError}}</p>
  </section>
</template>

<script>

export default {
  name: 'LoginPage',
  data() {
    return {
        image: require("@/assets/bg.png"),
        teamCode: "6EZ1FOV",
        loginError: null
    }
  },
  methods: {
    async login() {
        window.localStorage.setItem('teamCode', this.teamCode);

        let loginData = await this.$store.dispatch("getLoginData")

        console.log("login data", loginData)

        if(loginData) {
          this.$router.push({name: 'LeagueList'})
        } else 
          this.loginError = "Sorry..."
    }
  }
}
</script>

<style scoped>
    .bg {
        height: 100vh;
        background-size: cover;
        background-position: center;
        display: flex;
        flex-direction: column;
        justify-content: center;
        padding: 1em;
    }

    button {
        margin: 1em 0;
    }

    .login-msg {
      color: white;
      text-align: center;
    }
</style>
