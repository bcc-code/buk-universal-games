<template>
  <section :class="{ root: true, hidden: !show, scanning }">
    <button class="close" v-on:click="close">X</button>

    <input type="file" ref="fileInput" v-on:change="loadFile" capture="environment" accept="image/*" />
    <button class="btn-primary scan" v-on:click="$refs.fileInput.click">Scan QR code</button>
    <img v-if="image" ref="imgElement" :src="image" alt="" />
    <div v-if="!scanning" class="scan-result">
      <a v-if="resultIsLink" :href="result">{{ result }}</a>
      <p v-else>{{ result }}</p>
    </div>
    <div id="reader"></div>
  </section>
</template>

<script>
import { Html5Qrcode } from "html5-qrcode";

export default {
  name: "CameraScanner",
  data() {
    return {
      image: null,
      result: "",
      resultIsLink: false,
      scanner: null,
      show: false,
      scanning: false,
    };
  },
  methods: {
    loadFile(event) {
      this.image = URL.createObjectURL(event.target.files[0]);

      // const scanner = new window.Html5QrcodeScanner("reader", { fps: 10, qrbox: 250, facingMode: "environment" });

      const html5QrCode = new Html5Qrcode(/* element id */ "reader");
      this.scanner = html5QrCode;

      const image = event.target.files[0];
      html5QrCode
        .scanFile(image, true)
        .then((decodedText) => {
          // success, use decodedText
          console.log(decodedText);
          this.result = decodedText;
          this.resultIsLink = this.result.includes("http");

          if (this.resultIsLink) {
            setTimeout(() => {
              window.open(this.result, "_blank").focus();
            }, 250);
          }
        })
        .catch((err) => {
          // failure, handle it.
          this.result = "Please try scanning the QR code again, we couldn't read it.";
          this.resultIsLink = false;
          console.log(`Error scanning file. Reason: ${err}`);
        });
    },

    start() {
      this.show = true;

      if (this.$refs.fileInput) {
        this.$refs.fileInput.click();
      }
    },

    close() {
      this.show = false;

      if (this.scanner) {
        this.scanner.clear();
      }
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
  margin-bottom: auto;
}

.scanning #reader {
  /* height: 100%; */
  height: 100px;
  opacity: 1;
}

img {
  width: 128px;
  height: auto;
  max-width: 100%;
}

.close {
  margin-top: 1em;
  margin-right: 1em;
  margin-bottom: auto;
  margin-left: auto;
  width: 3em;
  height: 3em;
  line-height: 1;
}

.scan {
  margin-bottom: 2em;
}

.scan-result {
  margin-top: 2em;
  margin-bottom: 2em;
}

input[type="file"] {
  width: 1px;
  height: 1px;
  opacity: 0;
}
</style>
