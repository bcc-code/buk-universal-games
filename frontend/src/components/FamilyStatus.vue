<template>
  <div class="pt-6">
    update 4
    <div v-if="isLoading" class="text-center text-gray-500">Loading...</div>
    <div v-else-if="error" class="text-center text-red-500">
      Scream in panic! <br />
      An error happened. {{ error.message }}
    </div>
    <div v-else>
      <div
        v-for="(family, familyIndex) in familyStatus?.families"
        :key="family.id"
        class="mb-8"
      >
        <div class="bg-white p-6 rounded-lg border border-slate">
          <div class="flex items-center justify-between mb-4">
            <div class="flex items-center space-x-4">
              <div>
                <img
                  v-if="familyIndex === 0"
                  src="/image/1_place.png"
                  alt="Gold Medal"
                  class="h-8 w-8"
                />
                <img
                  v-else-if="familyIndex === 1"
                  src="/image/2_place.png"
                  alt="Silver Medal"
                  class="h-8 w-8"
                />
                <img
                  v-else-if="familyIndex === 2"
                  src="/image/3_place.png"
                  alt="Bronze Medal"
                  class="h-8 w-8"
                />
                <div v-else class="text-2xl font-bold text-gray-500 h-8 w-8">
                  {{ familyIndex + 1 }}
                </div>
              </div>
              <img
                :src="'/image/mafia_family.png'"
                alt="Family Icon"
                class="w-12"
              />
              <h2
                class="text-2xl font-bold"
                :style="{ color: family.color ?? '#000000' }"
              >
                {{ family.name }}
              </h2>
            </div>
            <div class="flex items-center space-x-2">
              <div class="text-2xl font-bold text-gray-700">
                {{ formatPoints(family.points) }}
              </div>
            </div>
          </div>
          <hr class="border-t border-gray-200" />
          <div
            v-for="team in family.teams"
            :key="team.teamId"
            :class="{
              'bg-yellow-100 -mx-6 px-10': team.teamId === currentTeamId(),
            }"
            class="flex items-center justify-between p-4 border-b border-gray-200 last:border-0"
          >
            <div class="flex items-center space-x-4">
              <img src="/icon/buk-icon.svg" alt="Team Icon" class="h-6 w-6" />
              <span class="text-lg">{{ team.team }}</span>
            </div>
            <div class="text-lg text-gray-600">
              {{ formatPoints(team.points) }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useFamilyStatus, useSigninResponse } from '../hooks/hooks';
import { formatPoints } from './formatPoints';

const { data: familyStatus, isLoading, error } = useFamilyStatus();
const { data: signinData } = useSigninResponse();

const currentTeamId = () => signinData.value?.teamId;
</script>

<style scoped>
.bg-yellow-100 {
  background-color: #fefcbf;
}
</style>
