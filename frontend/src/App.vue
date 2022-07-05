<template>
  <main>
    <LoginPage />
  </main>
</template>

<script>
import LoginPage from './components/LoginPage.vue'

export default {
  name: 'App',
  components: {
    LoginPage
  }
}
</script>

<style>
  *, *:before, *:after {
    box-sizing: inherit;
  }

  :root {
    --dark: #2D001C;
  }

  html {
    box-sizing: border-box;
  }

  body {
    margin: 0;
    background-color: var(--dark);
  }

  main {
    max-width: 480px;
    margin: 0 auto;
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
  }

  .btn-primary {
    background-color: var(--dark);
    color: white;
  }
</style>
