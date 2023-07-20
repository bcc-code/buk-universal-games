<template>
    <div class="locale-changer">
        <select v-model="selectedLanguage" @change="changeLanguage">
            <option v-for="locale in Object.keys(locales)" :key="`locale-${locale}`" :value="locale">
                {{ locales[locale] }}</option>
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
            locales: {
                cn: "中文",
                de: "Deutsch",
                en: "English",
                es: "Español",
                fr: "Français",
                nb: "Norsk Bokmål",
                nl: "Nederlands",
                pl: "Polski",
                ro: "Română",
                ru: "Русский",
                tr: "Türkçe"
            }
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
