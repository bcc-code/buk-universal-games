/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
		extend: {
			fontFamily: {
				inter: ['Inter', 'sans-serif'],
				HelveticaNeueCyr: ['HelveticaNeueCyr', 'sans-serif'],
			},
			fontSize: {
				xs: ['0.75rem', { lineHeight: '1.5' }],
				sm: ['0.875rem', { lineHeight: '1.5715' }],
				base: ['1rem', { lineHeight: '1.5', letterSpacing: '-0.017em' }],
				lg: ['1.125rem', { lineHeight: '1.5', letterSpacing: '-0.017em' }],
				xl: ['1.25rem', { lineHeight: '1.5', letterSpacing: '-0.017em' }],
				'2xl': ['1.5rem', { lineHeight: '1.415', letterSpacing: '-0.017em' }],
				'3xl': ['1.875rem', { lineHeight: '1.333', letterSpacing: '-0.017em' }],
				'4xl': ['2.25rem', { lineHeight: '1.277', letterSpacing: '-0.017em' }],
				'5xl': ['3rem', { lineHeight: '1.2', letterSpacing: '-0.017em' }],
				'6xl': ['3.75rem', { lineHeight: '1', letterSpacing: '-0.017em' }],
				'7xl': ['5rem', { lineHeight: '0.9', letterSpacing: '-0.017em' }],
			},
			letterSpacing: {
				tighter: '-0.02em',
				tight: '-0.01em',
				normal: '0',
				wide: '0.01em',
				wider: '0.02em',
				widest: '0.4em',
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
        'label-1': '#323538',
        'label-2': '#7c7878',
        'label-3': '#e5e5e5',
        'grey': '#A4A4A4',
      },
		},
	},
  plugins: [],
}