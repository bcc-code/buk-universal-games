<template>
  <AdminPageLayout>
    <h2>Admin League Status</h2>

    <LeageListItem
      v-for="i in [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]"
      :key="i"
      :class="{ 'card-light': i > 4 }"
      :index="i"
      :team="'Team'"
      :stickers="Math.ceil(150 / (i + 1))"
      :points="Math.ceil(200 / (i + 1))"
    />
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
import LeageListItem from "@/components/LeageListItem.vue";

export default {
  name: "AdminLeagueStatus",
  components: { AdminPageLayout, LeageListItem },
  data() {
    return {};
  },
  mounted() {},
  methods: {},
  computed: {},
};
</script>

<style scoped></style>
