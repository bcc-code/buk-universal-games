<template>
  <section class="px-5 flex justify-center items-center h-screen">
    <form class="flex flex-col py-10 space-y-10 justify-center align-middle" @submit="tryLogin">
      <img src="/image/logo_icon.svg" alt="" class="logo" />
      <input
        type="text"
        class="text-center text-vanilla p-3 w-full uppercase tracking-wide bg-ice-blue/20"
        :placeholder="$t('login.teamcode')"
        v-model="teamCode"
      />
      <button v-if="teamCode.length > 3" class="bg-vanilla text-lg text-dark-brown py-3 px-2">
        {{ $t('login.login_button') }}
      </button>

      <p v-if="loginMessage" class="login-msg">{{ loginMessage }}</p>
      <div class="mx-auto text-center bg-hazy-green/10 rounded-md p-3">
        <p class="text-label-3 text-xs">{{ $t('install_hint') }}</p>
      </div>
    </form>
  </section>
</template>

<script>
import { inject } from 'vue'
export default {
  name: 'LoginPage',
  props: {
    code: String
  },
  data() {
    return {
      teamCode: '',
      notificationService: null
    }
  },
  mounted() {
    this.$store.commit('setLoginMessage', '')
    this.notificationService = inject('notificationService')
    if (this.code) {
      this.teamCode = this.code
      this.tryLogin()
    }
  },
  methods: {
    async tryLogin(ev) {
      // Do not perform normal HTML form submit.
      if (ev) {
        ev.preventDefault()
      }
      try {
        this.notificationService.requestExternal()
      } catch (e) {
        console.log(e)
      }

      window.localStorage.setItem('testTeamCode', this.teamCode.toUpperCase())
      const loginData = await this.$store.dispatch('signIn')
      this.$store.commit('setLoginMessage', '')
      if (!loginData || loginData.error) {
        window.localStorage.removeItem('testTeamCode')
        if (loginData.error) {
          this.$store.commit('setLoginMessage', loginData.error)
        } else {
          this.$store.commit(
            'setLoginMessage',
            'Something went wrong, we could not log you in. Please try again'
          )
        }
      } else {
        await this.$store.dispatch('getGames')

        if (loginData.access.toLowerCase() === 'admin') {
          await this.$store.dispatch('getAdminLeagues')
          this.$store.dispatch('getAdminLeagueStatus')
          this.$router.push({ name: 'AdminSelectLeague' })
        } else {
          this.$store.dispatch('getMatches')
          this.$router.push({ name: 'LeagueList' })
        }
      }
    }
  },
  computed: {
    loginMessage() {
      return this.$store.state.loginMessage ?? ''
    }
  }
}
</script>

<style scoped>
.logo {
  width: 80%;
  max-width: 400px;
  margin: 0 auto;
  display: block;
  border: 10px solid white;
  border-radius: 80px;
}

.codeInput {
  text-transform: uppercase;
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
