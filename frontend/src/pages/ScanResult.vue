<template>
  <UserPageLayout>
    <div v-if="resultParsed">
      <img v-if="resultParsed.error" src="/images/sticker-error.png" alt="" />
      <img v-if="!resultParsed.error" src="/images/sticker-success.png" alt="" />
    </div>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "../components/UserPageLayout.vue";

export default {
  name: "ScanResult",
  props: {
    result: String,
  },
  components: { UserPageLayout },
  data() {
    return {
      resultParsed: null,
    };
  },
  mounted() {
    console.log("result", this.result);

    if (this.result) {
      try {
        this.resultParsed = JSON.parse(this.result);
      } catch (error) {
        console.error(error);
        this.$router.push({ name: "Login" });
      }
    } else {
      this.$router.push({ name: "Login" });
    }
  },
  methods: {},
  computed: {},
};
</script>

<style scoped></style>
