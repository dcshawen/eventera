// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(() => {
    document.getElementById('toggleTheme').addEventListener('click', function (e) {
        e.preventDefault();
        var htmlTag = document.documentElement;
        var currentTheme = htmlTag.getAttribute('data-bs-theme');
        htmlTag.setAttribute('data-bs-theme', currentTheme === 'dark' ? 'light' : 'dark');
    });
})();