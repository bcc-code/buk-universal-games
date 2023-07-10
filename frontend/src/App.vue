<template>
  <main>
    <div v-if="notificationService.canAskForPermission" class="external-notification-bell"
      @click="notificationService.requestExternal()">🔔</div>
    <router-view :key="$route.fullPath"></router-view>
    <internal-notification></internal-notification>
  </main>
</template>

<script setup>
import InternalNotification from '@/components/InternalNotification.vue';
import { inject } from 'vue'
const notificationService = inject('notificationService');
</script>

<style>
@font-face {
  font-family: "ClashGrotesk-Semibold";
  src: url("./assets/fonts/ClashGrotesk-Semibold.otf") format("truetype");
  font-weight: 600;
  font-display: swap;
  font-style: normal;
}

@font-face {
  font-family: "ClashGrotesk-Medium";
  src: url("./assets/fonts/ClashGrotesk-Medium.otf") format("truetype");
  font-display: swap;
  font-style: normal;
}

*,
*:before,
*:after {
  box-sizing: inherit;
}

:root {
  --dark: #A4709D;
  --gray-1: #e5e5e5;
  --gray-2: #b6b6b6;
  --green: #9FE2BD;
  --red: #DE6050;
}

html {
  box-sizing: border-box;
  height: 100%;
  background-color: var(--gray-1);
}

body {
  margin: 0;
  font-size: 16px;
  background-color: var(--gray-1);
  font-family: "ClashGrotesk-Medium";
  height: 100%;
}

main {
  height: 100%;
  max-width: 960px;
  margin: 0 auto;
  background-color: var(--gray-1);
}

#app {
  height: 100%;
  background-color: var(--gray-1);
}

img {
  width: 100%;
  max-width: 100%;
}

button,
input,
.btn-primary,
.btn {
  border-radius: 0.5em;
  padding: 0.75em;
  text-align: center;
  border: none;
  font-size: 1em;
  font: inherit;
}

h1,
p {
  margin: 0;
}

h1 {
  font-family: "ClashGrotesk-Semibold", sans-serif;
  font-size: 1.5em;
}

.btn-success {
  background-color: var(--green);
}

.btn-blank {
  border: 2px solid var(--dark);
}

.btn-primary {
  background-color: var(--dark);
  color: white;
}

.user-section {
  padding: 0.2em 0;
}

.external-notification-bell {
  position: fixed;
  top: 1em;
  right: 1em;
}
</style>
