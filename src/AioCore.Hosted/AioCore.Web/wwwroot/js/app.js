function loadStylesheet(sourceUrl) {
    if (sourceUrl === undefined) {
        console.error("Invalid stylesheet url");
        return;
    }

    const tag = document.createElement('link');
    tag.href = sourceUrl;

    tag.onload = function () {
        console.log("Script loaded successfully");
    }

    tag.onerror = function () {
        console.error("Failed to load script");
    }

    document.body.appendChild(tag);
}

function loadJs(sourceUrl) {
    if (sourceUrl === undefined) {
        console.error("Invalid source script url");
        return;
    }

    const tag = document.createElement('script');
    tag.src = sourceUrl;
    tag.type = "text/javascript";

    tag.onload = function () {
        console.log("Script loaded successfully");
    }

    tag.onerror = function () {
        console.error("Failed to load script");
    }

    document.body.appendChild(tag);
}