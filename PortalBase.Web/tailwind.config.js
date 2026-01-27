/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",
    "./wwwroot/js/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        // ALDA TRAVELED Brand Colors
        'alda-dark': '#1C1C1C',
        'alda-gold': '#E1AD01',
        'alda-light': '#F8F9FA',
        'alda-gray': '#6C757D',
        // Legacy support
        'brand-primary': '#E1AD01',
        'brand-secondary': '#1C1C1C',
        'brand-accent': '#E1AD01',
        'brand-dark': '#1C1C1C'
      },
      fontFamily: {
        'montserrat': ['Montserrat', 'sans-serif'],
        'roboto': ['Roboto', 'sans-serif'],
        'sans': ['Roboto', 'system-ui', 'sans-serif'],
        'display': ['Montserrat', 'sans-serif']
      }
    }
  },
  plugins: []
}

