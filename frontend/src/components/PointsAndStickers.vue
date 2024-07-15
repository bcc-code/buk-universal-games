<template>
  <section
    class="bg-white p-6 border border-gray-200 flex items-center justify-between w-full rounded-md shadow-sm"
  >
    <div class="flex items-center space-x-4">
      <img src="/image/logo_icon.png" class="h-16 w-16 rounded-md" />
      <div>
        <p class="text-xs uppercase text-gray-500">{{ $t('team') }}</p>
        <h2 class="text-xl font-bold text-gray-800">{{ teamName ?? '-' }}</h2>
        <p class="text-lg font-semibold text-gray-600">
          {{ formatPoints(teamPoints) ?? '-' }}
        </p>
      </div>
      <div class="ml-8">
        <p class="text-xs uppercase text-gray-500">{{ $t('family') }}</p>
        <h2 class="text-xl font-bold text-gray-800">{{ familyName ?? '-' }}</h2>
        <p class="text-lg font-semibold text-gray-600">
          {{ formatPoints(familyPoints) ?? '-' }}
        </p>
      </div>
    </div>
    <div class="flex flex-col items-center space-y-2">
      <LanguageSwitcher />
      <LogOutButton />
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import LanguageSwitcher from './LanguageSwitcher.vue';
import LogOutButton from './LogOutButton.vue';
import { useFamilyStatus, useSigninResponse } from '@/hooks/hooks';
import { formatPoints } from './formatPoints';

const { data } = useSigninResponse();
const { data: familyStatus } = useFamilyStatus();

const teamName = computed(() => data.value?.team);
const familyName = computed(() => data.value?.familyName);
const familyPoints = computed(() => familyStatus.value?.myStatus?.familyPoints);
const teamPoints = computed(() => familyStatus.value?.myStatus?.teamPoints);
</script>
