<template>
  <main>
    <router-view :key="$route.fullPath"></router-view>
  </main>
</template>

<script>

export default {
  name: 'App',
}
</script>

<style>

  @font-face {
    font-family: 'ClashGrotesk-Semibold';
    src: url('./assets/fonts/ClashGrotesk-Semibold.otf') format('truetype');
        font-weight: 600;
        font-display: swap;
        font-style: normal;
  }

  *, *:before, *:after {
    box-sizing: inherit;
  }

  :root {
    --dark: #2D001C;
    --gray-1: #E5E5E5;
    --gray-2: #B6B6B6;
    --green: #21F7AA;
  }

  html {
    box-sizing: border-box;
  }

  body {
    margin: 0;
    font-size: 16px;
    background-color: var(--dark);
    font-family: sans-serif;
  }

  main {
    height: 100vh;
    max-width: 480px;
    margin: 0 auto;
    background-color: var(--gray-1);
  }

  img {
    width: 100%;
    max-width: 100%;
  }

  button,
  input {
    border-radius: .5em;
    padding: .75em;
    text-align: center;
    border: none;
    font-size: 1em;
  }

  h1,
  p {
    margin: 0;
  }

  h1 {
    font-family: 'ClashGrotesk-Semibold', sans-serif;
    font-size: 1.5em;
  }


  .btn-primary {
    background-color: var(--dark);
    color: white;
  }
</style>
