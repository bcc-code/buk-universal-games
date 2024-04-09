# buk-universal-games
This is a default readme.

```sh
npm install
# start dev server
npm run dev
# type check, compile and minify for production
npm run build
# linting and formatting with eslint and prettier
npm run lint
npm run format
```

### Not implemented yet

Tests with vitest:

```sh
npm run test:unit
```

End-to-End Tests with [Playwright](https://playwright.dev):

```sh
# Install browsers for the first run
npx playwright install

# When testing on CI, must build the project first
npm run build

# Runs the end-to-end tests
npm run test:e2e
# Runs the tests only on Chromium
npm run test:e2e -- --project=chromium
# Runs the tests of a specific file
npm run test:e2e -- tests/example.spec.ts
# Runs the tests in debug mode
npm run test:e2e -- --debug
```

### Config references
[Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar)  
[Vite Configuration Reference](https://vitejs.dev/config/)  
[Vitest](https://vitest.dev/)  
[Playwright](https://playwright.dev)  
[ESLint](https://eslint.org/)  