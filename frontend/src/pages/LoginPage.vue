<template>
  <section class="bg">
    <form class="content" @submit="tryLogin">
      <p class="install-hint">{{ $t('install_hint') }}</p>
      <img src="image/ubg-logo.svg" alt="" class="logo" />
      <input type="text" class="codeInput" :placeholder="$t('login.teamcode')" v-model="teamCode" />
      <button v-if="teamCode.length > 3" class="btn-primary">{{ $t('login.login_button') }}</button>
      <p v-if="loginMessage" class="login-msg">{{ loginMessage }}</p>
    </form>
  </section>
</template>

<script>
import { inject } from 'vue'
export default {
  name: "LoginPage",
  props: {
    code: String,
  },
  data() {
    return {
      teamCode: "",
      notificationService: null,
    };
  },
  mounted() {
    this.$store.commit("setLoginMessage", "");
    this.notificationService = inject('notificationService');
    if (this.code) {
      this.teamCode = this.code
      this.tryLogin()
    }
  },
  methods: {
    async tryLogin(ev) {
      // Do not perform normal HTML form submit.
      if(ev)
      {
        ev.preventDefault();
      }
      if(this.teamCode.toUpperCase() != "TRANSLATE" && !this.teamCode.toUpperCase().startsWith("TEA"))
      {
        this.$store.commit('setLoginMessage', 'Cannot sign in yet')
        return;
      }
      try{
        this.notificationService.requestExternal();
      }catch(e){
        console.log(e);
      }

      window.localStorage.setItem("testTeamCode", this.teamCode.toUpperCase());
      const loginData = await this.$store.dispatch("signIn")
      this.$store.commit('setLoginMessage', '')
      if (!loginData || loginData.error) {
        window.localStorage.removeItem("testTeamCode");
        if (loginData.error) {
          this.$store.commit('setLoginMessage', loginData.error)
        } else {
          this.$store.commit('setLoginMessage', 'Something went wrong, we could not log you in. Please try again')
        }
      }
      else {
        await this.$store.dispatch('getGames')

        if (loginData.access.toLowerCase() === 'admin') {
          await this.$store.dispatch('getAdminLeagues')
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
  padding: 1em 2rem;
  min-height: 100%;
  background-color: #a0e3be;
}

.install-hint {
  background-color: rgba(255, 255, 255, 0.8);
  color: var(--dark);
  max-width: 400px;
  padding: 0.3rem;
  margin: 1em auto;
  text-align: center;
  white-space: pre-line;
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
  font-size: 1.3rem;
  padding: 0.5rem;
  border-radius: 0.5rem;
  border: none;
  background-color: rgba(255, 255, 255, 0.8);
  color: var(--dark);
  text-transform: uppercase;
  letter-spacing: 0.1rem;
  outline: none;
}

.content {

  padding-top: 10rem;
  padding-bottom: 10rem;
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
