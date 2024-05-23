<template>
    <div class="pt-6">
        <div v-if="isLoading" class="text-center text-gray-500">Loading...</div>
        <div v-else-if="error" class="text-center text-red-500">Error: {{ error.message }}</div>
        <div v-else>
            <div v-for="family in familyStatus?.families" :key="family.id" class="mb-8">
                <div class="bg-white p-6 rounded-lg shadow-md">
                    <div class="flex items-center justify-between">
                        <div class="flex items-center space-x-4">
                            <img src="/icon/family-icon.svg" alt="Family Icon" class="h-12 w-12">
                            <h2 class="text-xl font-bold">{{ family.name }}</h2>
                        </div>
                        <div class="text-xl font-bold text-gray-700">{{ (formatPoints(family.points)) }}</div>
                    </div>
                    <div class="mt-4">
                        <div v-for="team in family.teams" :key="team.teamId"
                            class="flex items-center justify-between p-4 border-b border-gray-200 last:border-0">
                            <div class="flex items-center space-x-4">
                                <img src="/icon/buk-icon.svg" alt="Team Icon" class="h-6 w-6">
                                <span class="text-lg">{{ team.team }}</span>
                            </div>
                            <div class="text-lg text-gray-600">{{ (formatPoints(team.points)) }}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<script setup lang="ts">
import { toRaw, watch } from 'vue';
import { useFamilyStatus } from '../hooks/useFamilyStatus';

const { data: familyStatus, isLoading, error } = useFamilyStatus();

watch(familyStatus, (familyStatus) => {
    console.log(toRaw(familyStatus))
})

function formatPoints(points: number | undefined): string {
    if (points === undefined) {
        return '£ ?';
    }

    if (points < 1000) {
        return `£ ${points.toFixed(0)}k`;
    } else {
        return `£ ${(points / 1000).toFixed(2)}M`;
    }
}

</script>