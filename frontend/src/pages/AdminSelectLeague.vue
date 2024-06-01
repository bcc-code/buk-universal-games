<template>
  <section class="px-5 flex justify-center items-center h-screen w-full">
    <form class="flex flex-col py-10 space-y-10 justify-center align-middle w-full">
      <img src="/image/logo_icon.svg" alt="" class="logo shadow-md" />
      <div class="w-full justify-center flex">
        <p class="text-xl">
          {{ $t('admin.select_league.intro') }}
        </p>
      </div>
      <div class="flex flex-wrap gap-1 justify-center">
        <AdminLeagueSelector v-for="league in leagues" :isSelected="store.state.adminLeagueSelected === league.id"
          selectedClass="bg-dark-brown text-white" unselected-class="bg-vanilla" :key="league.id"
          :name="league.name ?? ''" @click="() => selectLeague(league.id)" />
      </div>
    </form>
  </section>
</template>

<script setup lang="ts">
import { useStore } from 'vuex';
import { useRouter } from 'vue-router';
import AdminLeagueSelector from '@/components/AdminLeagueSelector.vue';
import { useLeagues } from '@/hooks/hooks';

const store = useStore();
const router = useRouter();

const { data: leagues } = useLeagues();

async function selectLeague(id: number) {
  await store.commit('setAdminLeagueSelected', id);
  router.push({ name: 'AdminSelectGame' });
}
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
