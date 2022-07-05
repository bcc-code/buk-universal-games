<template>
  <div class="bg" :style="{
    'background-image': `url(${image})`,
    }">
    <img src="../assets/logo.png" alt="" class="logo">
    <input type="text" placeholder="Team code" v-model="teamCode">
    <button class="btn-primary" @click="login">Login</button>
  </div>
</template>

<script>
export default {
  name: 'LoginPage',
  data() {
    return {
        image: require("@/assets/bg.png"),
        teamCode: null,
    }
  },
  methods: {
    login() {
        console.log("teamcode", this.teamCode)
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
</style>
