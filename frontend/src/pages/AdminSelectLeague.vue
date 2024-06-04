<template>
  <section class="px-5 flex justify-center items-center h-screen w-full">
    <form
      class="flex flex-col py-10 space-y-10 justify-center align-middle w-full"
    >
      <BigLogo />
      <div class="w-full justify-center flex">
        <p class="text-xl">
          {{ $t('admin.select_league.intro') }}
        </p>
      </div>
      <div class="flex flex-wrap gap-1 justify-center">
        <AdminLeagueSelector
          v-for="league in leagues"
          :isSelected="false /*store.state.adminLeagueSelected === league.id*/"
          selectedClass="bg-dark-brown text-white"
          unselected-class="bg-vanilla"
          :key="league.id"
          :name="league.name ?? ''"
          @click="() => selectLeague(league.id)"
        />
      </div>
    </form>
  </section>
</template>

<script setup lang="ts">
import { useStore } from 'vuex';
import { useRouter } from 'vue-router';
import AdminLeagueSelector from '@/components/AdminLeagueSelector.vue';
import BigLogo from '@/components/BigLogo.vue';
import { useLeagues } from '@/hooks/hooks';

const store = useStore();
const router = useRouter();

const { data: leagues } = useLeagues();

async function selectLeague(id: number) {
  await store.commit('setAdminLeagueSelected', id);
  router.push({ name: 'AdminMatchListGame' });
}
</script>
