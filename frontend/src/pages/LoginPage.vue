<template>
  <section class="bg">
    <form class="content" @submit="tryLogin">
      <img src="image/ubg-logo.png" alt="" class="logo" />
      <input type="text" class="codeInput" :placeholder="$t('login.teamcode')" v-model="teamCode" />
      <button v-if="teamCode.length > 3" class="btn-primary">{{ $t('login.login_button') }}</button>
      <p v-if="loginMessage" class="login-msg">{{ loginMessage }}</p>
    </form>
  </section>
</template>

<script>
export default {
  name: "LoginPage",
  props: {
    code: String,
  },
  data() {
    return {
      teamCode: "",
    };
  },
  mounted() {
    this.$store.commit("setLoginMessage", "");
    if (this.code) {
      this.teamCode = this.code
      this.tryLogin()
    }
  },
  methods: {
    async tryLogin(ev) {
      // Do not perform normal HTML form submit.
      ev?.preventDefault();
      window.localStorage.setItem("teamCode", this.teamCode.toUpperCase());
      const loginData = await this.$store.dispatch("signIn")
      this.$store.commit('setLoginMessage', '')


      if (!loginData || loginData.error) {
        if (loginData.error) {
          this.$store.commit('setLoginMessage', loginData.error)
        } else {
          this.$store.commit('setLoginMessage', 'Something went wrong, we could not log you in. Please try again')
        }
      }
      else {
        this.$store.dispatch('getGames')
        if (loginData && loginData.access === 'Admin') {
          this.$store.dispatch('getAdminLeagues')
          this.$store.dispatch('getAdminLeagueStatus')
          this.$router.push({ name: 'AdminSelectLeague' })
        }
        else {
          this.$store.dispatch('getMatches')
          this.$router.push({ name: 'LeagueList' })
        }
      }
    }
  },
  computed: {
    loginMessage() {
      return this.$store.state.loginMessage ?? "";
    }
  },
};
</script>

<style scoped>
.bg {
  background-size: cover;
  background-position: center;
  padding: 1em 2em;
  min-height: 100%;
  background-color: #a0e3be;
}

.logo {
  width: 80%;
  max-width: 400px;
  margin: 0 auto;
  display: block;
  border: 10px solid white;
  border-radius: 80px;
}

.codeInput {
  margin: 1em auto;
  width: 100%;
  max-width: 600px;
  text-align: center;
  font-size: 1.3em;
  padding: 0.5em;
  border-radius: 0.5em;
  border: none;
  background-color: rgba(255, 255, 255, 0.8);
  color: var(--dark);
  text-transform: uppercase;
  letter-spacing: 0.1em;
  outline: none;
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
