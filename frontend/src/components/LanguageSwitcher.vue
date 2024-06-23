<template>
  <div class="locale-changer">
    <div class="dropdown" :class="{ open: isOpen }">
      <div class="bg-white rounded-md p-2 btn shadow-md" @click="toggleDropdown(null)">
        {{ store.userLanguage.toUpperCase() }}
      </div>
      <div class="background" v-if="isOpen" @click="toggleDropdown(null)"></div>
      <div class="dropdown-menu" ref="languagePickerMenu">
        <div
          v-for="locale in sortedLocales"
          :key="`locale-${locale}`"
          class="dropdown-item"
          @click="changeLanguage(locale)"
        >
          <p>{{ SUPPORTED_LOCALES[locale] }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onUnmounted } from 'vue';
import { useI18n } from 'vue-i18n';

import { SUPPORTED_LOCALES, setI18nLanguage, type Locale} from "../libs/i18n"
import {usePiniaStore} from "../store/piniaStore"

const order:Locale[] = ['nb', 'en', 'de', 'es', 'nl', 'pl'];

const store = usePiniaStore()
const i18n = useI18n()

const isOpen = ref(false);
const languagePickerMenu = ref<HTMLDivElement | null>(null);

const toggleDropdown = (override: boolean | null = null) => {
  console.log(override, isOpen.value)
  isOpen.value = override ?? !isOpen.value;
  if (isOpen.value) {
    setTimeout(() => {
      document.addEventListener('click', closeDropdown);
    }, 100);
  } else {
    document.removeEventListener('click', closeDropdown);
  }
};


const changeLanguage = (locale: string) => {
  setI18nLanguage(i18n, locale);
  toggleDropdown(false);
};

const closeDropdown = (event: MouseEvent) => {
  event.stopPropagation();
  if (!languagePickerMenu.value) {
    document.removeEventListener('click', closeDropdown);
    return;
  }
  if (!languagePickerMenu.value.contains(event.target as Node)) {
    toggleDropdown(false);
  }
};

const sortedLocales = computed(() => {
  const remainingLocales = (Object.keys(SUPPORTED_LOCALES) as (keyof typeof SUPPORTED_LOCALES)[])
    .filter((locale) => !order.includes(locale)) 
    .sort((a, b) => SUPPORTED_LOCALES[a].localeCompare(SUPPORTED_LOCALES[b]));

  return [...order, ...remainingLocales];
});

onUnmounted(() => {
  document.removeEventListener('click', closeDropdown);
});
</script>

<style scoped>
.background {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(255, 255, 255, 0.849);
  z-index: 998;
}

.dropdown {
  position: relative;
  display: inline-block;
  cursor: pointer;
}

.selected-item {
  padding: 0.2rem 0.3em;
  border: 1px solid #ccc;
  border-radius: 4px;
  margin: 0em -0.5em 0.3em 0.5em;
}

.dropdown-menu {
  position: absolute;
  top: 70%;
  right: 0;
  z-index: 999;
  display: none;
  min-width: 100%;
  padding: 0.5rem;
  margin-top: 0.5rem;
  border: 1px solid var(--ice-blue);
  border-radius: 4px;
  background-color: var(--ice-blue);
  color: var(--dark-blue);
  box-shadow: 2px 2px 2px #ccc;
}

.dropdown.open .dropdown-menu {
  display: block;
}

.dropdown-item {
  padding: 0.5rem;
  cursor: pointer;
}

.dropdown-item:hover {
  background-color: #f2f2f2;
}
</style>
