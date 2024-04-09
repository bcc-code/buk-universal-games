<template>
    <div class="locale-changer">
        
        <div class="dropdown" :class="{ open: isOpen }">
            <div class="selected-item" @click="toggleDropdown(null)">{{ selectedLanguage.toUpperCase() }}</div>
            <div class="background" v-if="isOpen" @click="toggleDropdown(null)"></div>
            <div class="dropdown-menu" ref="languagePickerMenu">
                <div v-for="locale in Object.keys(locales)" :key="`locale-${locale}`" class="dropdown-item" @click="changeLanguage(locale)">
               <p> {{ locales[locale] }}</p>
                </div>
            </div>
        </div>
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
                fi: "Suomi",
                fr: "Français",
                hu: "Magyar",
                it: "Italiano",
                nb: "Norsk",
                nl: "Nederlands",
                pl: "Polski",
                ro: "Română",
                ru: "Русский",
                tr: "Türkçe",
                ua: "українська",
            },
            isOpen: false
        };
    },
    methods: {
        toggleDropdown(override = null) {
            this.isOpen = override || !this.isOpen;
            if (this.isOpen) {
                setTimeout(() => {
                    document.addEventListener('click', this.closeDropdown);
                }, 100);
            } else {
                document.removeEventListener('click', this.closeDropdown);
            }
        },
        changeLanguage(locale) {
            this.$store.commit("setUserLanguage", locale);
            setI18nLanguage(this.$i18n, locale);
            this.selectedLanguage = locale;
            localStorage.setItem("userLanguage", locale);
            this.toggleDropdown(false);
        },
        closeDropdown(event) {
            event.stopPropagation();
            if(!this.$refs.languagePickerMenu)
            {
                document.removeEventListener('click', this.closeDropdown);
                return;
            }
            if (!this.$refs.languagePickerMenu.contains(event.target)) {
                this.toggleDropdown(false);
            }
        }
    }
};

</script>

<style scoped>
.background {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(240, 6, 166, 0.849);
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
  margin:0em -0.5em 0.3em 0.5em;
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
  border: 1px solid #9d0101;
  border-radius: 4px;
  background-color: #ba1414;
  color:var(--dark);
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
```