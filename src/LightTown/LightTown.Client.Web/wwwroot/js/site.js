var theme = 0;

function switchTheme() {
    var style = document.documentElement.style;
    if (theme === 0) {
        style.setProperty("--primary-bg-color", getComputedStyle(document.body).getPropertyValue("--primary-bg-color-light"));
        style.setProperty("--secondary-bg-color", getComputedStyle(document.body).getPropertyValue("--secondary-bg-color-light"));
        style.setProperty("--blending-bg-color", getComputedStyle(document.body).getPropertyValue("--blending-bg-color-light"));
        style.setProperty("--primary-text-color", getComputedStyle(document.body).getPropertyValue("--primary-text-color-light"));
        style.setProperty("--secondary-text-color", getComputedStyle(document.body).getPropertyValue("--secondary-text-color-light"));
        theme = 1;
    } else {
        style.setProperty("--primary-bg-color", getComputedStyle(document.body).getPropertyValue("--primary-bg-color-dark"));
        style.setProperty("--secondary-bg-color", getComputedStyle(document.body).getPropertyValue("--secondary-bg-color-dark"));
        style.setProperty("--blending-bg-color", getComputedStyle(document.body).getPropertyValue("--blending-bg-color-dark"));
        style.setProperty("--primary-text-color", getComputedStyle(document.body).getPropertyValue("--primary-text-color-dark"));
        style.setProperty("--secondary-text-color", getComputedStyle(document.body).getPropertyValue("--secondary-text-color-dark"));
        theme = 0;
    }
}

function getCookies() {
    return document.cookie;
}

function unsetCookies() {
    var cookies = document.cookie.split(";");

    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }
}