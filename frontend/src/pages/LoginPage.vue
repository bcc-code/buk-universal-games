<template>
  <section class="px-5 flex justify-center items-center h-screen w-full">
    <form
      class="flex flex-col py-10 space-y-10 justify-center align-middle w-full"
      @submit.prevent="tryLogin"
    >
      <BigLogo />
      <input
        type="text"
        class="text-center text-label-1 p-3 w-full shadow-md uppercase tracking-wide bg-white"
        :placeholder="$t('login.teamcode')"
        v-model="teamCode"
      />
      <LoadingButton
        :isLoading="isSigningIn"
        :class="[
          teamCode.length < 3 ? 'opacity-0 btn-disabled' : 'opacity-100',
        ]"
        class="btn-primary"
      >
        {{ $t('login.login_button') }}
      </LoadingButton>

      <div v-if="errorMessage" class="alert alert-error">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="stroke-current shrink-0 h-6 w-6"
          fill="none"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
          />
        </svg>

        <span>
          {{ errorMessage }}
        </span>
      </div>
    </form>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useSigninResponse } from '../hooks/hooks';
import BigLogo from '@/components/BigLogo.vue';
import LoadingButton from '@/components/LoadingButton.vue';

const route = useRoute();
const code = route.params.code as string | undefined;

const teamCode = ref(code ?? '');
const router = useRouter();
const isSigningIn = computed(() => isLoading.value || isRefetching.value);

const {
  data: signInResponse,
  isLoading,
  refetch,
  error,
  isRefetching,
} = useSigninResponse(() => teamCode);

const errorMessage = computed(() => {
  if (error.value) {
    const errorMessage = (error?.value as any)?.response?.data?.error;
    return (
      errorMessage ??
       "$t('something-went-wrong-we-could-not-log-you-in-please-try-again') "
    );
  }
  return null;
});

const tryLogin = async () => {
  if (!teamCode.value.toUpperCase()) return;

  window.localStorage.setItem('testTeamCode', teamCode.value.toUpperCase());

  const response = await refetch();

  if (signInResponse.value && !error.value && !response.isError) {
    if (signInResponse.value.access?.toLowerCase() === 'admin') {
      router.push({ name: 'AdminSelectGame' });
    } else {
      router.push({ name: 'LeagueList' });
    }
  } else {
    window.localStorage.removeItem('testTeamCode');
  }
};

onMounted(() => {
  tryLogin();
});
</script>
