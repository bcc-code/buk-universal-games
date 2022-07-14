<template>
  <div class="root">
    <h1>Processing your scan, hang on ...</h1>
  </div>
</template>

<script>
import { postStickerCode, myGetTeamCodeFunction } from "@/libs/apiHelper";

export default {
  name: "ScanProcessing",
  data() {
    return {
      a: "b",
    };
  },
  mounted() {
    const stickerCode = this.$store.state.scanning.stickerCode;
    const teamCode = myGetTeamCodeFunction();

    console.log(JSON.stringify(this.$store.state.loginData));
    // const handlingURL = this.$store.state.scanning.handlingURL;

    if (stickerCode && teamCode) {
      postStickerCode(stickerCode).then((r) => {
        this.$store.commit("setScanning", { handlingURL: false, stickerCode: null });

        this.$router.push({
          name: "ScanResult",
          params: { code: teamCode, result: JSON.stringify(r) },
        });
      });
    } else {
      console.log("Something went wrong, going back to login");
      console.log("stickerCode", stickerCode);
      console.log("teamCode", teamCode);
      this.$router.push({ name: "Login" });
    }
  },
  methods: {},
  computed: {},
};
</script>

<style scoped>
.root {
  padding: 1em;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
  width: 100%;
  height: 100%;
}
</style>
