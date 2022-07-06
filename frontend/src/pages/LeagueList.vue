<template>
  <UserPageLayout>
    <p>Hello</p>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from '../components/UserPageLayout.vue'

export default {
  name: 'LoginPage',
  components: { UserPageLayout },
  data() {
    return {
        loginError: "Hello"
    }
  },
  methods: {

  }
}
</script>

<style scoped>
</style>
