<template>
  <section class="px-5 flex justify-center items-center h-screen w-full">
    <form class="flex flex-col py-10 space-y-10 justify-center align-middle w-full">
      <BigLogo />
      <div class="w-full justify-center flex">
        <p class="text-xl">
          {{ $t('admin.select_game.intro') }}
        </p>
      </div>
      <div class="flex flex-wrap gap-1 justify-center">
        <AdminLeagueSelector v-for="game in games" :isSelected="false/*store.state.adminFilterGameSelected === game.id*/"
          selectedClass="bg-dark-blue text-white" unselectedClass="bg-ice-blue" :key="game.id" :name="game.name ?? ''"
          @click="() => selectGame(game.id)" />
      </div>
    </form>
  </section>
</template>

<script setup lang="ts">
import { useStore } from 'vuex';
import { useRouter } from 'vue-router';
import AdminLeagueSelector from '@/components/AdminLeagueSelector.vue';
import BigLogo from '@/components/BigLogo.vue';
import { useGames } from '@/hooks/hooks';

const store = useStore();
const router = useRouter();

const { data: games } = useGames();

async function selectGame(id: number) {
  await store.commit('setAdminFilterGameSelected', id);
  router.push({ name: 'AdminSelectLeague' });
}
</script>
