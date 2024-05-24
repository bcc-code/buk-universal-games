<template>
  <AdminPageLayout title="League status">
    <FamilyStatus></FamilyStatus>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from '@/components/AdminPageLayout.vue';
import FamilyStatus from '@/components/FamilyStatus.vue';

export default {
  name: 'AdminLeagueStatus',
  components: { AdminPageLayout, FamilyStatus },
};
</script>

<style scoped>
.league-title {
  padding: 0 1em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 10% 75% 15%;
}

.league-title-column {
  display: flex;
  flex-direction: column;
  width: 100%;
}

.league-title-text {
  font-size: 0.85em;
  color: var(--gray-2);
  margin: 1em 0 0;
}

.leagues {
  display: flex;
  width: 100%;
}

.green-font {
  color: var(--green);
}
</style>
