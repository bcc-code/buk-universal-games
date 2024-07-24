<template>
  <video playsinline autoplay muted controls>
    <source :src="`/videos/${gameType}.mp4`" type="video/mp4" />
    {{ $t('couldnt-load-the-video') }}
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
