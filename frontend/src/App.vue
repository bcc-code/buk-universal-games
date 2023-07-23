<template>
  <main>
    <router-view :key="$route.fullPath"></router-view>
    <internal-notification></internal-notification>
  </main>
</template>

<script setup>
import InternalNotification from '@/components/InternalNotification.vue';
</script>

<style>
*,
*:before,
*:after {
  box-sizing: inherit;
}

:root {
  --dark: #A4709D;
  --gray-1: #e5e5e5;
  --gray-2: #999;
  --green: #9FE2BD;
  --red: #DE6050;
  --darkgreen:#22C16A;
  --yellow: #FACC18;
  /* These values yield colours extremely similar to the colours above, through applied CSS filters.
  This means they can be used to colourise SVG elements inserted through <img /> tags. */
  --active-green-button: brightness(0) saturate(100%) invert(48%) sepia(43%) saturate(1337%) hue-rotate(114deg) brightness(96%) contrast(101%);
  --inactive-yellow-button: brightness(0) saturate(100%) invert(81%) sepia(58%) saturate(4717%) hue-rotate(11deg) brightness(107%) contrast(112%);
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
  font-family: "ClashGrotesk-Medium", sans-serif;
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
