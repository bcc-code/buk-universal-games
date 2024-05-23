<template>
    <div v-if="isLoading">Loading...</div>
    <div v-else-if="error">Error: {{ error.message }}</div>
    <div v-else>
        <div v-for="family in familyStatus?.families" :key="family.id">
            <h2>{{ family.name }} (Points: {{ family.points }})</h2>
            <ul>
                <li v-for="team in family.teams" :key="team.teamId">
                    {{ team.team }} (Points: {{ team.points }})
                </li>
            </ul>
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
</script>