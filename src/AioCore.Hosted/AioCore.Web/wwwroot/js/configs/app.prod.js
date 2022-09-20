import autoprefixer from 'autoprefixer';

module.exports = {
    plugins: [
        require('cssnano')({
            preset: 'default',
            discardComments: true,
            plugins: [
                autoprefixer
            ]
        }),
    ],
    content: [
        "./src/AioCore.Hosted/AioCore.Web/Pages/**/*.{razor}"
    ],
    theme: {
        extend: {},
    },
}