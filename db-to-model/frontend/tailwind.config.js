function withOpacity(variableName) {
  return ({ opacityValue }) => {
    if (opacityValue !== undefined) {
      return `rgba(var(${variableName}), ${opacityValue})`
    }
    return `rgb(var(${variableName}))`
  }
}
/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      textColor: {
        skin: {
          base: 'rgb(var(--color-text-base) / <alpha-value>)',
          muted: 'rgb(var(--color-text-muted) / <alpha-value>)',
          inverted: 'rgb(var(--color-text-inverted) / <alpha-value>)',
          hover: 'rgb(var(--color-text-hover) / <alpha-value>)',
        },
      },
      backgroundColor: {
        skin: {
          fill: 'rgb(var(--color-bg-fill) / <alpha-value>)',
          accent: 'rgb(var(--color-bg-accent) / <alpha-value>)',
          primary: 'rgb(var(--color-bg-primary) / <alpha-value>)',
        },
      },
      borderColor: {
        skin: {
          fill: 'rgb(var(--color-border-fill) / <alpha-value>)',
          accent: 'rgb(var(--color-border-accent) / <alpha-value>)',
        },
      },
      ringColor: {
        skin: {
          base: 'rgb(var(--color-ring-base) / <alpha-value>)',
          focus: 'rgb(var(--color-ring-focus) / <alpha-value>)',
          text: 'rgb(var(--color-ring-text) / <alpha-value>)',
        },
      },
    },
  },
  plugins: [require('@tailwindcss/forms')],
}
