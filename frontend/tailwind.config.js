/** @type {import('tailwindcss').Config} */
module.exports = {
  purge: ['./public/index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  content: [],
  theme: {
    screens: {
      sm: '480px',
      md: '768px',
      lg: '976px',
      xl: '1440px',
    },
    colors: {
      'dark-blue': '#0B1725',
      'vanilla': '#F7EAD0',
      'blue-grey': '#72898B',
      'dark-brown': '#816668',
      'hazy-green': '#A0ACA7',
      'warm-brown': '#CEAB9D',
      'dusty-blue': '#426083',
      'ice-blue': '#DCE9EA',
      'grey': '#A4A4A4',
    },
    fontFamily: {
      sans: ['Times', 'sans-serif'],
      serif: ['Times New Roman', 'serif'],
    },
    extend: {
      spacing: {
        '128': '32rem',
        '144': '36rem',
      },
      borderRadius: {
        '4xl': '2rem',
      }
    }
  },
  plugins: [],
}

