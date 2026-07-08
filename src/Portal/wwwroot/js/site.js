// Shared site-wide scripts.

// "Work in progress" modal for links and buttons that are not functional yet.
$(document).on("click", "[data-wip]", function (event) {
    event.preventDefault();
    event.stopPropagation();
    bootstrap.Modal.getOrCreateInstance(document.getElementById("wip-modal")).show();
});

// Hamburger toggle: expands the side menu to show a text label per option.
$("#sidebar-toggle").on("click", function () {
    $("#sidebar").toggleClass("expanded");
});

// User dropdown: end the session and go back to the login screen.
$("#logout-button").on("click", function () {
    fetch("/session/logout", { method: "POST" }).finally(function () {
        window.location.href = "/Login";
    });
});
