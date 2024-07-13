<template>
  <video controls playsinline>
    <source :src="`/videos/${gameType}.mp4`" type="video/mp4" />
    Couldn't load the video
  </video>
</template>

<script setup lang="ts">
defineProps({
  gameType: {
    type: String,
    required: true,
  },
});
</script>
