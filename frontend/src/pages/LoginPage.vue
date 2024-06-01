<template>
  <section class="px-5 flex justify-center items-center h-screen w-full">
    <form
      class="flex flex-col py-10 space-y-10 justify-center align-middle w-full"
      @submit.prevent="tryLogin"
    >
      <BigLogo/>
      <input
        type="text"
        class="text-center text-label-1 p-3 w-full shadow-md uppercase tracking-wide bg-white"
        :placeholder="$t('login.teamcode')"
        v-model="teamCode"
      />
      <button
        class="bg-peach-50 text-lg shadow-lg border-2-peach-200 text-peach-200 py-3 px-2"
        :class="[teamCode.length < 3 ? 'opacity-0' : 'opacity-100']"
        type="submit"
      >
        {{ $t('login.login_button') }}
      </button>

      <p v-if="loginMessage" class="login-msg">{{ loginMessage }}</p>
    </form>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import { useStore } from 'vuex';
import { useRouter, useRoute } from 'vue-router';
import { useSigninResponse } from '../hooks/hooks';
import BigLogo from '@/components/BigLogo.vue';

const route = useRoute();
const code = route.params.code as string | undefined;

const teamCode = ref(code ? code.toUpperCase() : '');
const store = useStore();
const router = useRouter();

watch(
  teamCode,
  (newVal) => {
    console.log(newVal);
    if (newVal) {
      teamCode.value = newVal.toUpperCase();
      window.localStorage.setItem('testTeamCode', teamCode.value);
    }
  },
  { immediate: true },
);

const {
  data: signInResponse,
  isLoading,
  refetch,
  error,
} = useSigninResponse(() => teamCode);

const loginMessage = computed(() => {
  if (isLoading.value) return 'Logging you in, please wait ...';
  if (error.value)
    return (
      'Something went wrong, we could not log you in. Please try again.' +
      error.value.message
    );
  return '';
});

const tryLogin = async () => {
  if (!teamCode.value) return;
  const response = await refetch();
  console.log(error, signInResponse);
  if (signInResponse.value && !error.value && !response.isError) {
    await store.dispatch('getGames');
    if (signInResponse.value.access?.toLowerCase() === 'admin') {
      await store.dispatch('getAdminLeagues');
      store.dispatch('getAdminLeagueStatus');
      router.push({ name: 'AdminSelectGame' });
    } else {
      store.dispatch('getMatches');
      router.push({ name: 'LeagueList' });
    }
  } else {
    window.localStorage.removeItem('testTeamCode');
  }
};

onMounted(() => {
  if (code) {
    window.localStorage.clear();
    tryLogin();
  }
});
</script>

<style scoped>
.logo {
  width: 60%;
  max-width: 400px;
  margin: 0 auto 2em auto;
  display: block;
  border: 5px solid var(--peach-50);
  border-radius: 40px;
}
</style>
