<template>
  <section
    class="text-dark-brown rounded-md w-full p-1 m-1 flex flex-col justify-center items-center"
  >
    {{ name }}
  </section>
</template>

<script>
export default {
  name: 'AdminLeagueSelector',
  props: {
    name: String,
  },
  mounted() {},
  methods: {},
};
</script>

<style scoped>
.card {
  padding: 1em;
  margin: 0 0.25em;
  border-radius: 0.75em;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.card-dark {
  background-color: var(--dark);
  color: white;
}
</style>
