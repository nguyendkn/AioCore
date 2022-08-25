module.exports = {
    plugins: [
        require('@tailwindcss/forms')
    ],
    content: [
        "./Pages/**/*.razor",
        "./Pages/**/*.cshtml",
        "./Shared/**/*.razor"
    ],
    theme: {
        extend: {},
    },
}