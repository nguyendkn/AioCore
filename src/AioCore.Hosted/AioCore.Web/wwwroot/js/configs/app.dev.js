module.exports = {
    plugins: [
        require('@tailwindcss/forms')
    ],
    content: [
        "./Areas/**/*.cshtml",
        "./Pages/**/*.razor",
        "./Pages/**/*.cshtml",
        "./Shared/**/*.razor",
        "./Shared/**/*.cshtml"
    ],
    theme: {
        extend: {},
    },
}