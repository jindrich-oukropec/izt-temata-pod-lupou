function embed(target, url) {

	var callback = function () {
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
		callback();
	} else {
		document.addEventListener("DOMContentLoaded", callback);
	}

}