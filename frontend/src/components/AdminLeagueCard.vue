<template>
    <section class="card card-dark">
        {{name}}
    </section>
</template>

<script>

export default {
  name: "AdminLeagueCard",
  props: {
    name: String,
  },
  mounted() {
  },
  methods: {},
};
</script>

<style scoped>
    .card {
        padding: 1em;
        margin: 0 .25em;
        border-radius: .75em;
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
