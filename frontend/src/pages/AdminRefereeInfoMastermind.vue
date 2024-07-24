<template>
  <!-- This is a workaround. When the html is added through the translation file, the css doesn't get added to the built css file, but if we add them directly to vue they will be added to the css. So keep this line. -->
  <div
    class="bg-amber-50 bg-blue-50 bg-red-50 font-bold list-disc list-inside rounded-md"
  ></div>
  <div v-html="$t('admin-referee-info.mastermind')"></div>
</template>
