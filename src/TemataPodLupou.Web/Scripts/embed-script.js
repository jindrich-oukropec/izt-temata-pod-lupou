function embed(target, url) {

	var renderIframe = function () {
		const id = 'temata-' + Date.now();
        const iframe = document.createElement('iframe');
		iframe.id = id;
		iframe.setAttribute('src', url);
		iframe.style.width = '100%';
		iframe.style.border = '0';
        iframe.style.margin = '15px 0';
		iframe.setAttribute('scrolling', 'no');
		const targetElement = document.getElementById(target);
		targetElement.prepend(iframe);
        iFrameResize(iframe);
    };

	if (
		document.readyState === "complete" ||
		(document.readyState !== "loading" && !document.documentElement.doScroll)
	) {
		renderIframe();
	} else {
		document.addEventListener("DOMContentLoaded", renderIframe);
	}

    function parseUrl(url) {
        var m = url.match(/^(([^:\/?#]+:)?(?:\/\/((?:([^\/?#:]*):([^\/?#:]*)@)?([^\/?#:]*)(?::([^\/?#:]*))?)))?([^?#]*)(\?[^#]*)?(#.*)?$/),
            r = {
                hash: m[10] || "",                   // #asd
                host: m[3] || "",                    // localhost:257
                hostname: m[6] || "",                // localhost
                href: m[0] || "",                    // http://username:password@localhost:257/deploy/?asd=asd#asd
                origin: m[1] || "",                  // http://username:password@localhost:257
                pathname: m[8] || (m[1] ? "/" : ""), // /deploy/
                port: m[7] || "",                    // 257
                protocol: m[2] || "",                // http:
                search: m[9] || "",                  // ?asd=asd
                username: m[4] || "",                // username
                password: m[5] || ""                 // password
            };
        if (r.protocol.length == 2) {
            r.protocol = "file:///" + r.protocol.toUpperCase();
            r.origin = r.protocol + "//" + r.host;
        }
        r.href = r.origin + r.pathname + r.search + r.hash;
        return r;
    };

    var origin = parseUrl(url).origin;

	window.addEventListener("message", (event) => {
        if (event.origin !== origin || !event.data.redirect)
			return;

        window.location.href = event.data.redirect;
	}, false);

}