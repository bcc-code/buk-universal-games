<template>
  <section>
    <div class="items-end flex flex-col mr-10 mt-4"></div>
    <div class="content-area">
      <h1 v-if="showTitle" class="title">
        BUK {{ $store.state.loginData.team }}
      </h1>
      <slot />
    </div>
    <AdminMenu v-if="$store.state.loginData.access === 'Admin'" />
    <UserMenu v-else />
  </section>
</template>

<script>
import UserMenu from '../components/UserMenu.vue';
import AdminMenu from '../components/AdminMenu.vue';

export default {
  name: 'UserPageLayout',
  components: {
    AdminMenu,
    UserMenu,
  },
  props: {
    showTitle: Boolean,
  },
  data() {
    return {
      title: 'Tittel',
    };
  },
  mounted() {
    if (Object.keys(this.$store.state.loginData).length === 0) {
      this.$store.dispatch('signIn');
    }
  },
  methods: {},
};
</script>

<style scoped>
.user-page-layout {
  display: flex;
  flex-direction: column;
  width: 100%;
  /* height: 100%; */
  justify-content: space-between;
  padding-bottom: 5em;
}

.content-area {
  width: 100%;
  height: auto;
  padding: 1em 1em 10em;
}

.title {
  padding-bottom: 0.5em;
}
</style>
