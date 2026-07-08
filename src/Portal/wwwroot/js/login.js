// Client-side interactions for the login screen:
// DNI/CE toggle, password visibility, and submit button enablement.
(function () {
    var $form = $("#login-form");
    if (!$form.length) return;

    var $docType = $("#document-type");
    var $user = $("#login-user");
    var $password = $("#login-password");
    var $submit = $("#login-submit");

    function selectDocType(type) {
        $docType.val(type);
        $(".doc-toggle-option").each(function () {
            $(this).toggleClass("active", $(this).data("doc-type") === type);
        });
        $user.attr("inputmode", type === "DNI" ? "numeric" : "text");
    }

    $(".doc-toggle-option").on("click", function () {
        selectDocType($(this).data("doc-type"));
        $user.trigger("focus");
    });

    selectDocType($docType.val() || "DNI");

    $("#toggle-password").on("click", function () {
        var reveal = $password.attr("type") === "password";
        $password.attr("type", reveal ? "text" : "password");
        $(this).attr("title", reveal ? "Ocultar contraseña" : "Mostrar contraseña");
    });

    function refreshSubmit() {
        var ready = $user.val().trim() !== "" && $password.val() !== "";
        $submit.prop("disabled", !ready);
    }

    $user.add($password).on("input", refreshSubmit);
    refreshSubmit();

    var $toast = $("#expired-toast");
    if ($toast.length) {
        setTimeout(function () {
            $toast.fadeOut(400);
        }, 8000);
    }
})();
