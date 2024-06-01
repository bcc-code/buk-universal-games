<template>
  <section class="px-5 flex justify-center items-center h-screen w-full">
    <form class="flex flex-col py-10 space-y-10 justify-center align-middle w-full">
      <img src="/image/logo_icon.svg" alt="" class="logo shadow-md" />
      <div class="w-full justify-center flex">
        <p class="text-xl">
          {{ $t('admin.select_game.intro') }}
        </p>
      </div>
      <div class="flex flex-wrap space-x-5">
        <AdminLeagueSelector v-for="game in games" class="min-w-min" :class="[
          store.state.adminFilterGameSelected === game.id
            ? 'bg-dark-blue text-white'
            : 'bg-ice-blue',
        ]" :key="game.id" :name="game.name ?? ''" @click="() => selectGame(game.id)" />
      </div>
    </form>
  </section>
</template>

<script setup lang="ts">
import { useStore } from 'vuex';
import { useRouter } from 'vue-router';
import AdminLeagueSelector from '@/components/AdminLeagueSelector.vue';
import { useGames } from '@/hooks/hooks';

const store = useStore();
const router = useRouter();

const { data: games } = useGames();

async function selectGame(id: number) {
  await store.commit('setAdminFilterGameSelected', id);
  router.push({ name: 'AdminMatchListGame' });
}
</script>

<style scoped>
.bg {
  height: 100%;
  background-size: cover;
  background-position: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 1em;
}

.logo {
  width: 60%;
  max-width: 400px;
  margin: 0 auto 2em auto;
  display: block;
  border: 5px solid var(--peach-50);
  border-radius: 40px;
}

.game-card {
  margin: 0.25em 0;
}

.flex {
  display: flex;
  flex-wrap: wrap;
  /* Allows items to wrap onto the next line as needed */
  gap: 5px;
  /* Spacing between items, equivalent to your space-x-5 */
}
</style>
