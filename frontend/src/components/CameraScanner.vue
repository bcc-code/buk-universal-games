<template>
  <section :class="{ root: true, hidden: !show, scanning }">
    <!-- <input type="file" name="" id="" v-on:change="loadFile" />
    <img ref="imgElement" :src="image" alt="" /> -->
    <div v-if="!scanning">
      <div>Scan res: {{ results }}</div>
    </div>
    <button class="btn-primary" v-on:click="close">Close</button>
    <div id="reader"></div>
  </section>
</template>

<script>
export default {
  name: "CameraScanner",
  data() {
    return {
      image: "/favicon.ico",
      results: "",
      scanner: null,
      show: false,
      scanning: false,
    };
  },
  methods: {
    // loadFile(event) {
    //   this.image = URL.createObjectURL(event.target.files[0]);
    // },

    start() {
      this.show = true;
      this.scanning = true;

      this.scanner = new window.Html5QrcodeScanner("reader", { fps: 10, qrbox: 250, facingMode: "environment" });
      this.scanner.render(this.onScanSuccess);
    },

    close() {
      this.show = false;
      this.scanner.clear();
    },

    onScanSuccess(decodedText, decodedResult) {
      console.log(`Scan result: ${decodedText}`, decodedResult);

      this.results = decodedText + "\n";
      this.scanner.clear(); // Stop scanning

      this.scanning = false;
    },
  },
};
</script>

<style scoped>
.root {
  position: fixed;
  inset: 0;
  color: #000;
  background-color: #fff;
  z-index: 10;
  max-width: 100%;
  width: 480px;
  margin: auto;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
}

.hidden {
  display: none !important;
}

#reader {
  background-color: var(--gray-1);
  width: 100%;
  height: 1px;
  opacity: 0;
}

.scanning #reader {
  height: 100%;
  opacity: 1;
}
</style>
