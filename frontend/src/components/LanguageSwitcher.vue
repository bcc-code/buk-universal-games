<template>
    <div class="locale-changer">
      <select v-model="selectedLanguage" @change="changeLanguage">
        <option v-for="locale in ['en', 'no']" :key="`locale-${locale}`" :value="locale">{{ locale.toUpperCase() }}</option>
      </select>
    </div>
</template>

<script>
import { setI18nLanguage } from '@/libs/i18n';


export default {
    name: "LanguageSwitcher",
    data() {
        return {
            selectedLanguage: this.$i18n.locale,
        };
    },
    methods: {
        changeLanguage() {
            this.$store.commit("setUserLanguage", this.selectedLanguage);
            setI18nLanguage(this.$i18n, this.selectedLanguage);
            localStorage.setItem("userLanguage", this.selectedLanguage);
        },
    },
};

</script>