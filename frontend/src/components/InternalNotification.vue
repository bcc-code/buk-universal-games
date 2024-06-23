<template>
  <dialog :style="state.style" ref="dialogue" @click="click()">
    <span class="close" @click="close()">×</span>
    <h2 :v-if="state.title" class="title">{{ state.title }}</h2>
    <img :v-if="state.icon" class="icon" :src="state.icon ?? undefined" />
    <p :v-if="state.body" class="body">{{ state.body }}</p>
  </dialog>
</template>
<script setup lang="ts">
import type NotificationService from '@/services/notification.service';
import { inject, reactive } from 'vue';

const notificationService = inject<NotificationService>('notificationService');
if(!notificationService) throw Error("notificationService not provided");

const state = reactive({
  action: () => {},
  body: null as string|null|undefined,
  icon: null as string|null|undefined,
  style: { visibility: 'hidden' as "hidden"|"visible" },
  title: null as string|null|undefined,
});

notificationService.registerInternalNotifier((title: string|null|undefined, options:{onClick:()=>void,body:string|undefined,icon:string|undefined}) => {
  if (options.onClick) {
    state.action = options.onClick;
  }
  state.body = options.body;
  state.icon = options.icon;
  state.style.visibility = 'visible' as const;
  state.title = title;
});

function click() {
  state.action();
  close();
}

function close() {
  state.action = () => {};
  state.body = null;
  state.icon = null;
  state.style.visibility = 'hidden' as const;
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
  padding: 1.5em;
  margin: auto;
  display: grid;
  grid-template:
    'icon title' auto
    'body body' auto / 70px auto;
  border: 4px solid var(--darkgreen);
  border-radius: 1em;
  box-shadow: 3px 3px 9px rgba(0, 0, 0, 0.3);
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
  font-size: 4em;
  line-height: 0.5em;
  position: absolute;
  padding: 0.1em;
  color: var(--darkgreen);
  top: 0;
  right: 0;
}
</style>
