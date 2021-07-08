(function () {
    var button = document.querySelector("#cookieConsent button[data-cookie-string]");
    if (button != null) {
        button.addEventListener("click", function (event) {
            document.cookie = button.dataset.cookieString;
        }, false);
    }
})();