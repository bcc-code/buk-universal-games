<template>
  <dialog :style="state.style" ref="dialogue" @click="click()">
    <span class="close" @click="close()">×</span>
    <h2 :v-if="state.title" class="title">{{ state.title }}</h2>
    <img :v-if="state.icon" class="icon" :src="state.icon" />
    <p :v-if="state.body" class="body">{{ state.body }}</p>
  </dialog>
</template>
<script setup>
import { inject, reactive } from 'vue';
const notificationService = inject('notificationService');
const state = reactive({
  action: () => { },
  body: null,
  icon: null,
  style: { visibility: 'hidden' },
  title: null,
});
notificationService.registerInternalNotifier((title, options) => {
  if (options.onClick) {
    state.action = options.onClick;
  }
  state.body = options.body;
  state.icon = options.icon;
  state.style.visibility = 'visible';
  state.title = title;
});

function click() {
  state.action();
  close();
}

function close() {
  state.action = () => { };
  state.body = null;
  state.icon = null;
  state.style.visibility = 'hidden'
  state.title = null;
}
</script>
<style scoped>
dialog {
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
  display: grid;
  grid-template: 'icon title' auto
    'body body' auto / auto auto;
}

dialog .title {
  grid-area: title;
}

dialog .icon {
  grid-area: icon;
  max-width: 64px;
}

dialog .body {
  grid-area: body;
}

dialog .close {
  cursor: pointer;
  font-size: 5em;
  line-height: 0.5em;
  position: absolute;
  top: 0;
  right: 0;
}
</style>
