<template>
  <main>
    <router-view :key="$route.fullPath" />
    <InternalNotification></InternalNotification>
  </main>
</template>
<script setup lang="ts">
import InternalNotification from '@/components/InternalNotification.vue';
</script>

<style>
*,
*:before,
*:after {
  box-sizing: inherit;
}

html,
:root {
  box-sizing: border-box;
  height: 100%;
  /* background-color: white; */
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
/* 
.btn-success {
  background-color: var(--green);
} */

.btn-blank {
  border: 2px solid var(--dark-blue);
}

/* .btn-primary {
  background-color: var(--vanilla);
  color: white;
} */

.user-section {
  padding: 0.2em 0;
}

.external-notification-bell {
  position: fixed;
  top: 1em;
  right: 1em;
}
</style>
