function SetInnerHtml(id, html) {
    document.getElementById(id).innerHTML = html;
}

function RemoveElement(id) {
    document.getElementById(id).remove();
}

function AppendCss(url) {
    const head = document.getElementsByTagName('head')[0];
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = url;
    link.media = 'all';
    head.appendChild(link);
}

function AppendJs(url) {
    const head = document.getElementsByTagName('head')[0];
    const script = document.createElement('script');
    script.url = url;
    head.appendChild(script);
}