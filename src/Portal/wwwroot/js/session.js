// Session inactivity tracking for authenticated pages.
// After (session - warning) milliseconds without user activity, a warning dialog
// appears with a countdown; extending calls the portal, which refreshes the API
// token. If the countdown reaches zero the session is closed and the user is
// sent back to the login page with the "expired" notice.
(function () {
    var $body = $("body.app-page");
    if (!$body.length) return;

    var sessionSeconds = (parseInt($body.data("session-ms"), 10) || 1200000) / 1000;
    var warnSeconds = (parseInt($body.data("session-warn-ms"), 10) || 60000) / 1000;

    var $overlay = $("#session-overlay");
    var $countdown = $("#session-countdown");
    var inactivityTimer = null;
    var countdownTimer = null;

    function scheduleWarning() {
        clearTimeout(inactivityTimer);
        var delaySeconds = Math.max(sessionSeconds - warnSeconds, 5);
        inactivityTimer = setTimeout(showWarning, delaySeconds * 1000);
    }

    function showWarning() {
        var remaining = warnSeconds;
        $countdown.text(remaining);
        $overlay.removeClass("d-none");

        countdownTimer = setInterval(function () {
            remaining -= 1;
            $countdown.text(remaining);

            if (remaining <= 0) {
                clearInterval(countdownTimer);
                expireSession();
            }
        }, 1000);
    }

    function hideWarning() {
        clearInterval(countdownTimer);
        $overlay.addClass("d-none");
    }

    function extendSession() {
        fetch("/session/extend", { method: "POST" })
            .then(function (response) {
                if (!response.ok) throw new Error("extend failed");
                return response.json();
            })
            .then(function () {
                hideWarning();
                scheduleWarning();
            })
            .catch(expireSession);
    }

    function expireSession() {
        fetch("/session/logout", { method: "POST" }).finally(function () {
            window.location.href = "/Login?expired=true";
        });
    }

    $("#session-extend").on("click", extendSession);

    ["mousemove", "keydown", "click", "scroll", "touchstart"].forEach(function (eventName) {
        document.addEventListener(eventName, function () {
            if ($overlay.hasClass("d-none")) scheduleWarning();
        }, { passive: true });
    });

    scheduleWarning();
})();
