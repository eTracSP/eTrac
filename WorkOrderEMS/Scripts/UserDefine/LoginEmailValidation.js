$(document).ready(function () {
    $('#RecoveryEmail').bind('blur focus', function (event) {
        if (event.type === 'blur') {
            //cache jquery objects
            var $invalidEmailError = $('#invalidEmailError'),
                $submitButton = $('#submitButton'),
                $this = $(this);

            var v = $this.val();

            //trim spaces
            v = v.replace(/^\s+|\s+$/g, "");

            //check email against regex
            if (v.match(/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i)) {
                $invalidEmailError.hide();
                $submitButton.removeAttr('disabled').removeClass('disabled');
                $this.addClass('email-good').removeClass('email-bad');
            }
            else {
                $invalidEmailError.show();
                $submitButton.attr('disabled', 'disabled').addClass('disabled');
                $this.addClass('email-bad').removeClass('email-good');
            }
            //replace email with trimmed version
            $this.val(v);
        }
        //remove status styles while editing
        if (event.type === 'focus') {
            $(this).removeClass('email-bad email-good');
        }
    });

});