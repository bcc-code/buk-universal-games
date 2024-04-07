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



html {
  box-sizing: border-box;
  height: 100%;
}

body {
  margin: 0;
  font-size: 16px;
  font-family: "ClashGrotesk-Medium", sans-serif;
  height: 100%;
}

main {
  height: 100%;
  max-width: 960px;
  margin: 0 auto;
}

#app {
  height: 100%;
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
