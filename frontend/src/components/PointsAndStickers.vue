<template>
  <section
    class="bg-white border border-gray-200 items-center justify-between w-full rounded-md shadow-sm"
  >
    <div class="flex w-full justify-end space-y-2">
      <div class="mx-auto justify-center">
        <img src="/icon/ubg_logo.svg" class="h-16 w-16 rounded-md" />
      </div>
      <div class="absolute top-10 right-10 flex space-x-1">
        <LanguageSwitcher />
        <!-- <LogOutButton /> -->
      </div>
    </div>
    <div class="flex mx-auto justify-evenly w-full pb-3">
      <div class="flex space-x-4 items-start justify-evenly w-full">
        <div>
          <p class="text-xs uppercase text-red-500">{{ $t('team') }}</p>
          <h2 class="text-xl font-bold text-gray-800">{{ teamName ?? '-' }}</h2>
          <p class="text-lg font-semibold text-gray-600">
            {{ formatPoints(teamPoints) ?? '-' }}
          </p>
        </div>
        <div class="ml-8">
          <p class="text-xs uppercase text-red-500">{{ $t('family') }}</p>
          <h2 class="text-xl font-bold text-gray-800">
            {{ familyName ?? '-' }}
          </h2>
          <p class="text-lg font-semibold text-gray-600">
            {{ formatPoints(familyPoints) ?? '-' }}
          </p>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useFamilyStatus, useSigninResponse } from '@/hooks/hooks';
import LanguageSwitcher from './LanguageSwitcher.vue';
import { formatPoints } from './formatPoints';

const { data } = useSigninResponse();
const { data: familyStatus } = useFamilyStatus();

const teamName = computed(() => data.value?.team);
const familyName = computed(() => data.value?.familyName);
const familyPoints = computed(() => familyStatus.value?.myStatus?.familyPoints);
const teamPoints = computed(() => familyStatus.value?.myStatus?.teamPoints);
</script>
