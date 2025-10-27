// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(() => {
    const THEME_COOKIE = 'site_theme';

    function setCookie(name, value, days) {
        let expires = '';
        if (days) {
            const date = new Date();
            date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
            expires = '; expires=' + date.toUTCString();
        }
        document.cookie = name + '=' + encodeURIComponent(value) + expires + '; path=/';
    }

    function getCookie(name) {
        const match = document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)');
        return match ? decodeURIComponent(match.pop()) : null;
    }

    // Apply saved theme from cookie if present
    const savedTheme = getCookie(THEME_COOKIE);
    if (savedTheme) {
        document.documentElement.setAttribute('data-bs-theme', savedTheme);
    }

    document.getElementById('toggleTheme').addEventListener('click', function (e) {
        e.preventDefault();
        var htmlTag = document.documentElement;
        var currentTheme = htmlTag.getAttribute('data-bs-theme');
        var nextTheme = currentTheme === 'dark' ? 'light' : 'dark';
        htmlTag.setAttribute('data-bs-theme', nextTheme);
        setCookie(THEME_COOKIE, nextTheme, 365);
    });
})();