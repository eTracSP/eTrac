

//Show Ajax Progress Bar
function showAjaxProgress() {
    var docheight = $(document).height();
    $('body').append("<div id='ajaxProgress' class='ajaxProgress'><br/><br/><br/><img src='http://50.23.221.50/hydrant/Content/Images/UpdateProgress.gif' alt='' class='ajaxProgressContent'/></div>");
    $(".ajaxProgress").css("display", "block");
    $(".ajaxProgress").css("center", "0px");
    $(".ajaxProgress").css("top", "0px");
    $(".ajaxProgress").css("height", docheight);
} //End

//Hide Ajax Progress Bar
function closeAjaxProgress() {
    $("#ajaxProgress").remove();
} //End

var AJAXRequestURL = "/Common/AjaxRequest";

function ajaxProgressShow() {
    $('.ajaxProgress').show();
    $('.ajaxProgress').css("top", $(document).scrollTop() + "px");
}
function ajaxProgressHide() {
    $('.ajaxProgress').hide();
}

$(document).ready(function () {

    $("input[type='reset']").live("click", function () {
        $(".NotValid").removeAttr("errtitle");
        $(".NotValid").removeClass("NotValid");
    });

    $(document).ready(function () {
        $(".Fax").mask("(+1)(999)-999-9999");
        $(".Mobile").mask("(+1) 999-999-9999");
        $(".Phone").mask('(999) 999-9999');
        //$(".Postal").mask('99999');
    });

    ////Masking
    //$(".phMask").mask("99999-999999");
    //$(".Postcode").mask("a9a-9a9");

    // This is the jquery code for curving the button through css
    $(".button").wrap("<div class='buttonWrapper'></div>");
    $(".button").removeAttr("disabled");
    // This is the jquery code to disable auto complete for all textbox controls
    $("input[type=text]").each(function () {
        // $(this).attr('autocomplete', 'off');
    });

    $(document).scroll(function () {
        $('.ajaxProgress').css("top", $(document).scrollTop() + "px");
    });

    $("#ZipCode").live("keypress", function (e) {
        $(this).val($(this).val().replace(/[^0-9]+/g, ''));
    }); //End
    //$(".validdate").mask("99/99/9999");


    //--------- This is the jquery code that would be used to set all the textboxes with class AlphaNumericOnly as to accept Alpha-numerics only and Some Spaecial Characters like(.- )-------------//
    //    $(".AlphaNumericAndCharOnly").alphanumeric({ allow: ".,-, ,'," });

    //This function is used to show progress bar       
    $('.ajaxCall').live("click", function (e) {
        ajaxProgressShow();
    });

    $("[name^='ZipCode']").on("keypress", function (e) {
        $(this).val($(this).val().replace(/[^0-9]+/g, ''));
    }); //End   

    //Validation For the site -------------
    $(".isLetterOnly").keydown(function (e) {
        var reg = new RegExp("^[a-zA-Z]+$");
        return reg.test(e.key);
    })
    $(".LetterOnlyFr").keydown(function (e) {
        var reg = new RegExp("^[a-zA-Z ]+$");
        return reg.test(e.key);
    })
    $(".isValidUserNameOnly").keydown(function (e) {
        var reg = new RegExp("/^[a-zA-Z ]*$/");
        return reg.test(e.key);
    })
    //------Validation For the site 




    //This functionality is used show confirmation while deleting element
    $('.toggleStatus').live("click", function () {
        var control = $(this);
        ajaxProgressShow();

        $.post(AJAXRequestURL, {
            Method: "ChangeStatus",
            InputBoxValue: $(this).attr("rel"),
            InputBoxID: $(this).attr("id")
        },
         function (response) {
             eval(response);
             if (result) {
                 control.toggleClass('linkActive');
                 control.toggleClass('linkInactive');
                 if (control.hasClass('linkInactive')) {
                     control.attr('title', 'Inactive');
                 }
                 else {
                     control.attr('title', 'Active');
                 }
                 $(".ajaxProgress").css("display", "none");
             }
         });
    });

    /****************************************Stope Cut Copy Paste*****************************************************************/

    $('.cutcopypaste').live("cut copy paste", function (e) {
        e.preventDefault();
    }); //End

    /*****************************Nemeric Only**********************************************************/

    $('.integerOnly').live("cut copy paste", function (e) {
        e.preventDefault();
    });

    //End

    function validDecimal(element) {
        var RE = /^\d*\.?\d*$/;
        var value = element.val();
        if (RE.test(value)) {
            return true;
        } else {
            value = value.replace(/(\s+)?.$/, "");
            element.val(value)
            validDecimal(element);
            return false;
        }
    }

    /******************************decimal********************************************************************/
    $(".decimal").live('keyup', function (event) {
        validDecimal($(this));
    });

    $(".decimal").live('blur', function (event) {
        validDecimal($(this));
    });

    $(".decimal").live('keydown', function (event) {
        // Backspace, tab, enter, end, home, left, right,decimal(.)in number part, decimal(.) in alphabet 
        // We don't support the del key in Opera because del == . == 46. 
        var controlKeys = [8, 9, 13, 35, 36, 37, 39, 110, 190];
        // IE doesn't support indexOf 
        var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
        // Some browsers just don't raise events for control keys. Easy. 
        // e.g. Safari backspace. 
        if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0 
      (49 <= event.which && event.which <= 57) || // Always 1 through 9 
      (96 <= event.which && event.which <= 106) || // Always 1 through 9 from number section  
      (48 == event.which && $(this).attr("value")) || // No 0 first digit 
      (96 == event.which && $(this).attr("value")) || // No 0 first digit from number section 
      isControlKey) { // Opera assigns values for control keys. 
            return;
        } else {
            event.preventDefault();
        }

    });
    $('.decimal').live("cut copy paste", function (e) {
        e.preventDefault();
    });

    //End


    //--------- This is the jquery code that would be used to set all the textboxes with class Alphabeticonly as to accept Alphabets only -------------//
    //$(".AlphabeticOnly").live("keypress", function (e) {
    //    var key;
    //    key = e.which ? e.which : e.keyCode;
    //    if ((key >= 65 && key <= 91) || (key >= 97 && key < 123) || key == 8 || key == 9 || (key >= 37 && key <= 40) || key == 46 || /*key == 35 || key == 36 ||*/key == 116) {
    //        return true;
    //    }
    //    else {
    //        ReturnMessage = 'it only accepts alphabets';
    //        return false;

    //    }
    //}); //End

    $(".AlphabaticWithSpace").live("keypress", function (e) {
        var key;
        key = e.which ? e.which : e.keyCode;
        if ((key >= 65 && key <= 91) || (key >= 97 && key < 123) || key == 8 || key == 9 || key == 32 || (key >= 37 && key <= 40) || key == 46 || /*key == 35 || key == 36 ||*/key == 116) {
            return true;
        }
        else {
            return false;
        }
    }); //End

    //--------- This is the jquery code that would be used to set all the textboxes with class AlphaNumericOnly as to accept Alpha-numerics only -------------//
    $(".AlphaNumericOnly").live("keypress", function (e) {
        var key;
        key = e.which ? e.which : e.keyCode;
        if ((key >= 65 && key <= 90) || (key == 32) || (key >= 97 && key <= 122) || key == 8 || key == 9 || key == 32 || (key >= 37 && key <= 40) || key == 46 || /* key == 35 || key == 36 ||*/key == 116 || (key >= 48 && key <= 57)) {
            return true;
        }
        else {

            return false;
        }
    }); //End


    //--------- This is the jquery code that would be used to set all the textboxes with class NumericsOnly as to accept Numerics only -------------//
    $(".NumericOnly1").live("keypress", function (e) {
        var key;
        key = e.which ? e.which : e.keyCode;

        if (key == 8 || /*key == 32 */(key >= 37 && key <= 40) || key == 46 || (key >= 48 && key <= 57) || key == 9 || /*key == 35 || key == 36 ||*/key == 116) {
            showValidation($(this), "");
            return true;
        }
        else {
            showValidation($(this), "You can only enter numbers like 123");
            return false;
        }
    }); //End

    //--------- This is the jquery code to disable paste functionality -------------//
    $(".NumericOnly").bind('paste', function (e) {
        var el = $(this);
        var text;
        PrevText = $(this).val();

        setTimeout(function () {
            text = $(el).val();
            if (IsNumeric(text))
                $(el).val(text);
            else {
                $(el).val(PrevText);
            }
        }, 100);
    });

    $(".NumericWithoutDecimal").live("keypress", function (e) {
        var key;
        key = e.which ? e.which : e.keyCode;

        if (key == 8 || (key >= 48 && key <= 57) || key == 9 || (key >= 10 && key <= 12)) {
            return true;
        }
        else {
            return false;
        }
    });
    //--------- This is the jquery code to disable paste functionality -------------//
    $(".NumericWithoutDecimal").bind('paste', function (e) {
        var el = $(this);
        var text;
        PrevText = $(this).val();

        setTimeout(function () {
            text = $(el).val();
            if (IsNumeric(text))
                $(el).val(text);
            else {
                $(el).val(PrevText);
            }
        }, 100);
    });

    $(".AlphabeticOnly").bind('paste', function (e) {
        return false;
    });

    //--------- This is the jquery code to check validation in form default button -------------//
    $(".default").live('click', function (e) {
        var i = 0;
        $('.DateValidation').each(function () {
            var control = $(this);
            if (control.val() == "") {
                if (control.nextAll("div.validationMessage").html() != null) {
                    control.nextAll("div.validationMessage").remove();
                }
                control.after("<div class='invalid validationMessage' title='Required'>&nbsp;</div>");
                i = 1;
            }
        });
        if (i == 1) {
            //$(".default").attr("disabled", "disabled");
            return false;
        }
        if (TestValidation($(this).attr("id"), true)) {
            $('.ajaxProgress').show();
            return true;
        }
        else {
            return false;
        }
    });

    /*******************************************************************************************************************/
    /*******************************************************************************************************************/

    //$("input").live("keydown", function () {
    //    tooltip.hide();
    //});

    //$("input").click(function () {
    //    tooltip.hide();
    //});

    //$("select").live("change", function () {
    //    tooltip.hide();
    //});


    $(".requiredselect").live("change", function () {

        try {
            showValidation($(this), "");
            if (($.trim($(this).val()) == "") || ($.trim($(this).val()) == "0")) {
                showValidation($(this), "Required");
            }
        } catch (err) {
            return false;
        }
        //       $(".verror").remove();
        //        $(".oppverror").remove();
    });


    $(".NotValid").live("mouseover", function () {

        if ($(this).hasClass("blockToolTip")) {
            return;
        }
        var valerr = $(this).attr("errTitle");
        //tooltip.show(valerr);
    });


    $(".NotValid").live("mouseout", function () {

       // tooltip.hide();
    });

    $(".NotValid").live("click", function () {
       // tooltip.hide();
    });

    $(document).live("click", function () {
       // tooltip.hide();
    });

    /**********************************Check List Box***********************************/
    $(".cnCheckListBox").each(function () {
        var Idd = $(this).attr("id");
        var lstItem = '<div class="CheckListBox ' + $.trim($(this).attr("cmmnAssignment")) + '" id="checkListBx_' + Idd + '" style="margin-top: 5px; height: 100%; overflow: auto;" target_Id = "' + Idd + '">';
        $(this).find("option").each(function () {
            if ($(this).attr("selected") == "selected") {
                lstItem += '<p><input type="checkbox" checked="checked" tag="' + $(this).val() + '" id="opt' + Idd + $(this).val() + '">' + $.trim($(this).html()) + '</p>';
            }
            else {
                lstItem += '<p><input type="checkbox" tag="' + $(this).val() + '" id="opt' + Idd + $(this).val() + '">' + $.trim($(this).html()) + '</p>';
            }
        });
        lstItem += '</div>';
        $(this).after(lstItem);
        $(this).css("display", "none");
    });


    $(".CheckListBox").find("p").live("click", function (event) {
        var objTarget = $(event.target);
        var targetId = $.trim($(this).parent().first().attr("target_Id"));
        if (objTarget.is("input")) {
            ValidateCheckListBoxById($(this).parent().first());
            if ($(this).find("input").attr("checked") == "checked") {
                var tagId = $(this).find("input").attr("tag");
                $("#" + targetId).find("option[value='" + tagId + "']").attr("selected", "selected");
            }
            else {
                var tagId = $(this).find("input").attr("tag");
                $("#" + targetId).find("option[value='" + tagId + "']").removeAttr("selected");
            }
            return;
        }
        if ($(this).find("input").is(":checked")) {
            $(this).find("input").removeAttr("checked");
            var tagId = $(this).find("input").attr("tag");
            $("#" + targetId).find("option[value='" + tagId + "']").removeAttr("selected");
        }
        else {
            $(this).find("input").attr("checked", "checked");
            var tagId = $(this).find("input").attr("tag");
            $("#" + targetId).find("option[value='" + tagId + "']").removeAttr("selected");
            $("#" + targetId).find("option[value='" + tagId + "']").attr("selected", "selected");
        }
        ValidateCheckListBoxById($(this).parent().first());
    });
    /************************************************************************/
    /*******************************************************************************************************************/
    /*******************************************************************************************************************/
    /*******************************************************************************************************************/

    $("input[type='text']:not(.NoValidation), input[type='password']:not(.NoValidation)").live("keyup", function () {

        var ReturnMessage = '';
        var cls = "";
        var control = $(this);
        if ($(this).hasClass('required')) {
            cls += "#required";
            ReturnMessage = $(this).val() == '' ? 'Required' : '';
        }

        //NumericWithoutDecimal
        if ($(this).hasClass('NumericWithoutDecimal') && ReturnMessage == '') {
            cls += "#NumericWithoutDecimal";
            ReturnMessage = !isNumeric($(this).val()) ? 'only numbers like 1234' : '';
        }
        if ($(this).hasClass('DecimalOnly') && ReturnMessage == '') {
            cls += "#DecimalOnly";
            ReturnMessage = !isDecimal($(this).val()) ? 'only numbers/decimal values like 12.34' : '';
        }
        if ($(this).hasClass('AlphabeticOnly') && ReturnMessage == '') {
            cls += "#AlphabeticOnly";
            ReturnMessage = !isAlphabetic($(this).val()) ? 'only characters, like abcd' : '';
        }
        if ($(this).hasClass('AlphaNumericOnly') && ReturnMessage == '') {
            cls += "#AlphaNumericOnly";
            ReturnMessage = !isAlphaNumeric($(this).val()) ? 'only numbers and/or characters, like 123abc without space are allowed' : '';
        }
        if ($(this).hasClass('ValidEmail') && ReturnMessage == '') {
            cls += "#ValidEmail";
            ReturnMessage = !isValidEmail($(this).val()) ? 'email address, like user@domain.com' : '';
        }

        if ($(this).hasClass('ValidPassword') && ReturnMessage == '') {
            cls += "#ValidPassword";
            ReturnMessage = !isValidPassword($(this).val()) ? 'Password must be more than 6 characters. It cannot start with a number, underscore, or special character. It must contain at least one number , special character , one Lowercase and uppercase characters.' : '';
        }
        if ($(this).hasClass('ValidIPAddress') && ReturnMessage == '') {
            cls += "#ValidIPAddress";
            ReturnMessage = !isValidIPAddress($(this).val()) ? 'IP address, like "50.23.221.50"' : '';
        }

        if ($(this).hasClass('ValidPhone') && ReturnMessage == '') {
            cls += "#ValidPhone";
            ReturnMessage = !isValidPhone($(this).val()) ? 'Please enter phone number in +44 (0)9999 999999 format.' : '';
        }


        if ($(this).hasClass('ValidURL') && ReturnMessage == '') {
            cls += "#ValidURL";
            ReturnMessage = !isValidUrl($(this).val()) ? 'URL, like http://www.google.com' : '';
        }
        if ($(this).hasClass('AlphabaticWithSpace') && ReturnMessage == '') {
            cls += "#AlphabaticWithSpace";
            ReturnMessage = !isAlphabeticWithSpace($(this).val()) ? 'only alphabatic like abcd efg' : '';
        }
        if ($(this).hasClass('alphanumericandspace') && ReturnMessage == '') {
            cls += "#alphanumericandspace";
            ReturnMessage = !isAlphaNumericWithSpace($(this).val()) ? 'numbers and/or space, like - 112 new city' : '';
        }
        if ($(this).hasClass('alphanumericwithoutspace') && ReturnMessage == '') {
            cls += "#alphanumericwithoutspace";
            ReturnMessage = !isAlphaNumericWithoutSpace($(this).val()) ? 'numbers and character, like - aaa112 without space' : '';
        }
        if ($(this).hasClass('validdate') && ReturnMessage == '') {
            cls += "#validdate";
            ReturnMessage = !isValidDate($(this).val()) ? 'Date in dd/MM/yyyy Format' : '';
        }
        if ($.trim(cls) == "") {
            control.nextAll("div.validationMessage").remove();
        }
        else {
            showValidation(control, ReturnMessage);
        }
    });



    $("select:not(.NoValidation)").live("change", function () {
        if ($(this).parents("div.form").find("div.invalid").html() != null) {
            //$(this).parents("div.form").find(".default").attr("disabled", "disabled");
        }
        else {
            if (TestValidation($(this).attr("id"), false)) {
                //$(this).parents("div.form").find(".default").removeAttr("disabled");
            }
            else {// $(this).parents("div.form").find(".default").attr("disabled", "disabled"); 
            }
        }
    });


    $("input[type=text]:not(.NoValidation)").live("blur", function () {
        var ReturnMessage = '';
        var control = $(this);
        //code for unique only. 
        if ($(this).hasClass('UniqueOnly')) {
            if (control.nextAll("div.validationMessage").html() != null) {
                control.nextAll("div.validationMessage").remove();
            }
            $.post(AJAXRequestURL, {
                Method: "UniqueOnly",
                InputBoxValue: $(this).val(),
                InputBoxID: $(this).attr("id")
            },
                function (response) {
                    if (response == "Unique")
                    { response = ''; }
                    showValidation(control, response);
                }
                );
        }

    });
});
//--------- This is the javascript function that is used to check required fields for specified controls -------------//
function TestValidation(obj, showMessage) {
    var IsValid = true;
    var ReturnMessage = '';
    var controlWithError = null;
    $(".required", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (($(this).val().trim() == "" || $(this).val() == null) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = $(this).attr('title');
            controlWithError = $(this);
            IsValid = false;
        }
    });
    $(".NumericOnly", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isNumeric($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only numbers like 1234 are allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });
    $(".NumericWithoutDecimal", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isNumeric($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only numbers like 1234 are allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });

   
    //
    $(".DecimalOnly", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isDecimal($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only numbers/decimal values like 12.34 are allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });
    $(".AlphabeticOnly", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isAlphabetic($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only characters, like abcd are allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });
    $(".AlphaNumericOnly", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isAlphaNumeric($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only numbers and/or characters, like 123abc without space are allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });
    $(".ValidEmail", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isValidEmail($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only email address, like user@domain.com is allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });


    $(".ValidPassword", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isValidPassword($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'Password must be more than 6 characters. It cannot start with a number, underscore, or special character. It must contain at least one number , special character , one Lowercase and uppercase characters..';
            controlWithError = $(this);
            IsValid = false;
        }
    });


    $(".ValidIPAddress", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isValidIPAddress($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only IP address, like "50.23.221.50" is allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });


    $(".ValidPhone", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isValidPhone($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only phone number in +44 (0)9999 999999 format.';
            controlWithError = $(this);
            IsValid = false;
        }
    });


    $(".ValidURL", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isValidUrl($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only URL, like http://www.google.com is allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });

    $(".AlphabaticWithSpace", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isAlphabeticWithSpace($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only alphabatets like abcd efg are allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });
    $(".alphanumericandspace", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isAlphaNumericWithSpace($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'only numbers and/or space, like - 112 new city are allowed';
            controlWithError = $(this);
            IsValid = false;
        }
    });

    $(".alphanumericwithoutspace", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (!isAlphaNumericWithoutSpace($(this).val()) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = 'numbers and character, like - aaa112 without space';
            controlWithError = $(this);
            IsValid = false;
        }
    });
    $(".MustSelected", $("#" + obj.toString()).parents(".form:eq(0)")).each(function () {
        if (($(this).val().trim() == "" || $(this).val() == null || $(this).val().trim() == 0) && IsValid) {
            AddErrorTag($(this), "");
            ReturnMessage = $(this).attr('title');
            controlWithError = $(this);
            IsValid = false;
        }
    });


    if (IsValid) {

        return true;
    }
    else {
        if (showMessage) {
            //alert(controlWithError.attr("id"));
            //alert(ReturnMessage);
            if (controlWithError != null) {
                controlWithError.focus();
            }
        }
        return false;
    }
}
//--------- This is the javascript function to make default bottons in different forms-------------//
$(function () {
    $(".form input").keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            if (TestValidation($(this).attr("id")), true) {
                $('.default', $(this).parents('.form')).click();
                return false;
            }
            else {
                return false;
            }
        }
        else {
            return true;
        }
    });

    //************************************************** 23/09/2011 *****************************************//

    $("input[type=text]:not(.NoValidation), input[type='password']:not(.NoValidation)").blur(function () {
        var ReturnMessage = '';
        var cls = "";
        var control = $(this);
        if ($(this).hasClass('required')) {
            cls += "#required";
            ReturnMessage = $(this).val() == '' ? 'Required' : '';
        }

        //NumericWithoutDecimal
        if ($(this).hasClass('NumericWithoutDecimal') && ReturnMessage == '') {
            cls += "#NumericWithoutDecimal";
            ReturnMessage = !isNumeric($(this).val()) ? 'only numbers like 1234' : '';
        }
        if ($(this).hasClass('DecimalOnly') && ReturnMessage == '') {
            cls += "#DecimalOnly";
            ReturnMessage = !isDecimal($(this).val()) ? 'only numbers/decimal values like 12.34' : '';
        }
        if ($(this).hasClass('AlphabeticOnly') && ReturnMessage == '') {
            cls += "#AlphabeticOnly";
            ReturnMessage = !isAlphabetic($(this).val()) ? 'only characters, like abcd' : '';
        }
        if ($(this).hasClass('AlphaNumericOnly') && ReturnMessage == '') {
            cls += "#AlphaNumericOnly";
            ReturnMessage = !isAlphaNumeric($(this).val()) ? 'only numbers and/or characters, like 123abc without space are allowed' : '';
        }
        if ($(this).hasClass('ValidEmail') && ReturnMessage == '') {
            cls += "#ValidEmail";
            ReturnMessage = !isValidEmail($(this).val()) ? 'email address, like user@domain.com' : '';
        }
        if ($(this).hasClass('ValidPassword') && ReturnMessage == '') {
            cls += "#ValidPassword";
            ReturnMessage = !isValidPassword($(this).val()) ? 'Password must be more than 6 characters.It cannot start with a number, underscore, or special character. It must contain at least one number , special character , one Lowercase and uppercase characters.' : '';
        }

        if ($(this).hasClass('ValidIPAddress') && ReturnMessage == '') {
            cls += "#ValidIPAddress";
            ReturnMessage = !isValidIPAddress($(this).val()) ? 'IP address, like "50.23.221.50"' : '';
        }


        if ($(this).hasClass('ValidPhone') && ReturnMessage == '') {
            cls += "#ValidPhone";
            ReturnMessage = !isValidPhone($(this).val()) ? 'Please enter phone number in +44 (0)9999 999999 format.' : '';
        }


        if ($(this).hasClass('ValidURL') && ReturnMessage == '') {
            cls += "#ValidURL";
            ReturnMessage = !isValidUrl($(this).val()) ? 'URL, like http://www.google.com' : '';
        }
        if ($(this).hasClass('AlphabaticWithSpace') && ReturnMessage == '') {
            cls += "#AlphabaticWithSpace";
            if (!isAlphabeticWithSpace($(this).val())) {
                ReturnMessage = '';
            }
            else {
                ReturnMessage = 'only alphabatic like abcd efg';
            }

        }
        if ($(this).hasClass('alphanumericandspace') && ReturnMessage == '') {
            cls += "#alphanumericandspace";
            ReturnMessage = !isAlphaNumericWithSpace($(this).val()) ? 'numbers and/or space, like - 112 new city' : '';
        }
        if ($(this).hasClass('alphanumericwithoutspace') && ReturnMessage == '') {
            cls += "#alphanumericwithoutspace";
            ReturnMessage = !isAlphaNumericWithoutSpace($(this).val()) ? 'numbers and character, like - aaa112 without space' : '';
        }
        if ($(this).hasClass('validdate') && ReturnMessage == '') {
            cls += "#validdate";
            ReturnMessage = !isValidDate($(this).val()) ? 'Date in dd/MM/yyyy Format' : '';
        }
        if ($.trim(cls) == "") {
            control.nextAll("div.validationMessage").remove();
        }
        else {
            showValidation(control, ReturnMessage);
        }
    });


    $(".borderRed").live("keyup", function () {
        $(this).nextAll("div.validationMessage").remove();
        $(this).css("border", "");
        $(this).css("box-shadow", "");
        $(this).css("-moz-box-shadow", "");
        $(this).css("-webkit-box-shadow", "");
        $(this).css("background-color", "");
        $(this).css("border-style", "solid");
        $(this).css("border-width", "1px");
        $(this).removeAttr("Title");
    });

    $(".borderRed").live("blur", function () {
        $(this).nextAll("div.validationMessage").remove();
        $(this).css("border", "");
        $(this).css("box-shadow", "");
        $(this).css("-moz-box-shadow", "");
        $(this).css("-webkit-box-shadow", "");
        $(this).css("background-color", "");
        $(this).css("border-style", "solid");
        $(this).css("border-width", "1px");
        $(this).removeAttr("Title");
    });
    //**************************************************** END ***************************************************//
    $(".borderRed").hover(function () {

        /* <div id="notifica" class="errorNotifTip" style=""> </div>*/


        //                var tp = $.trim($(this).attr("Title"));
        //                if (tp.length > 0) {
        //                    var pssn = $(this).offset();
        //                    var ht = $(this).outerHeight();
        //                    ht = ht - 31;
        //                    var lft = pssn.left + $(this).outerWidth();
        //                    var tpo = pssn.top - 44 + ht;
        //                    $("#notifica").css("left", lft);
        //                    $("#notifica").css("top", tpo);

        //                    $("#notifica").html(tp);
        //                    $("#notifica").show();
        //                }

    });

    $(".borderRed").mouseout(function () {
        // $("#notifica").hide();
    });
});  //End of document.ready


//--------- This is the javascript function To read value of a querystring-------------//
function getQuerystring(key, default_) {
    if (default_ == null) default_ = "";
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null)
        return default_;
    else
        return qs[1];
}


//--------- This is the javascript function to replace all instances of a string from the source string-------------//
function ReplaceAll(Source, stringToFind, stringToReplace) {
    var temp = Source;
    var index = temp.indexOf(stringToFind);
    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }
    return temp;
}

function showValidation(control, ReturnMessage) {
    //check if there is any failure of validation and then attach message or class accordingly
    control.removeClass("NotValid");
    control.removeAttr("errTitle");
    if (ReturnMessage == '') {
        //valid
        result = { css: 'valid', ErrorMessage: 'Valid' };
        if (control.hasClass('borderRed')) {
            control.css("border", "");
            control.css("box-shadow", "");
            control.css("-moz-box-shadow", "");
            control.css("-webkit-box-shadow", "");
            control.css("background-color", "");
            control.css("border-style", "solid");
            control.css("border-width", "1px");
        }
    }
    else {
        //invalid
        control.addClass("NotValid");
        control.attr("errTitle", ReturnMessage);
        result = { css: 'invalid', ErrorMessage: ReturnMessage };
        if (control.hasClass('borderRed')) {
            control.css("border", "1px solid rgb(255,0,0)");
            control.css("box-shadow", "0 0 2px rgb(255,0,0)");
            control.css("-moz-box-shadow", "0 0 2px rgb(255,0,0)");
            control.css("-webkit-box-shadow", "0 0 2px rgb(255,0,0)");
            control.css("background-color", "rgb(255,250,250)");
        }
    }

    //now add a control at the end of control to show validation failure
    //    if (control.nextAll("div.validationMessage").html() != null) {
    //        control.nextAll("div.validationMessage").remove();
    //    }
    //    control.after("<div class='" + result.css + " validationMessage' title='" + result.ErrorMessage + "'>&nbsp;</div>");


}


function isNumeric(strValue) {
    if ((strValue + "").length > 0) {
        var objRegExp = /(^-?\d\d*\.\d*$)|(^-?\d\d*$)|(^-?\.\d\d*$)/;
        return objRegExp.test(strValue);
    }
    else {
        return true;
    }
}
function isDecimal(strVal) {
    if ((strVal + "").length > 0) {
        var objRegExp = /^\d+(?:\.\d+)?$/;
        return objRegExp.test(strVal);
    }
    else {
        return true;
    }
    //    return (checkValidation(strVal, "^[-+]?\d*\.?\d{1,2}$"));
}
function isAlphabetic(strVal) {
    return checkValidation(strVal, "^[a-zA-Z ]*$");
}
function isAlphaNumeric(strVal) {
    return checkValidation(strVal, "^[A-Za-z0-9 _.-]+$");
}
function isAlphabeticWithSpace(strVal) {
    return checkValidation(strVal, "^[a-zA-Z ]*$");
}
function isAlphaNumericWithSpace(strVal) {
    return checkValidation(strVal, "^([a-zA-Z0-9 ])*$");
}

function isAlphaNumericWithoutSpace(strVal) {
    return checkValidation(strVal, "^([a-zA-Z0-9])*$");
}

function isValidEmail(strVal) {
    return checkValidation(strVal, "^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$");
}
//Check for the valid password not working
function isValidPassword(strVal) {
    return checkValidation(strVal, "((?=.*\\d)(?=.*[a-z])(?=.*[@#$%])(?=.*[A-Z]).{6,20})");
}

function isValidIPAddress(strVal) {
    var patt1 = /^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]|[.*]$)(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]|[.*]$)){3}$/g;
    return checkValidation(strVal, patt1);
}

function isValidPhone(strVal) {
    // alert("phone");
    // return checkValidation(strVal, "^[01]?[- .]?\(?[2-9]\d{2}\)?[- .]?\d{3}[- .]?\d{4}$");
    var patt1 = /^((\+)?[1-9]{1,2})?([-\s\.])?((\(\d{1,4}\))|\d{1,4})(([-\s\.])?[0-9]{1,12}){1,2}$/;
    return checkValidation(strVal, patt1);
}

function isValidDate(strVal) {
    //return checkValidation(strVal, "^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$");
    if (strVal < Date()) {
        return checkValidation(strVal, "^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$");
    }
}
function isValidUrl(strVal) {


    return checkValidation(strVal, "/(http(s)?:\\)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?/");
    //  return checkValidation(strVal, "^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$");
}
function checkValidation(strVal, strExpression) {
    if ((strVal + "").length > 0) {
        var reg = new RegExp(strExpression);
        return reg.test(strVal);
    }
    else {
        return true;
    }
}

function AddErrorTag(control, ReturnMessage) {
    result = { css: 'invalid', ErrorMessage: ReturnMessage };
    control.after("<div class='" + result.css + " validationMessage' title='" + result.ErrorMessage + "'>&nbsp;</div>");
}



/***********************************Himanshu***************************************/

function ValidateValue(control) {

    var cls = "";
    var ReturnMessage = '';
    // var control = $(this);
    if (control.hasClass('required')) {
        cls += "#required";
        ReturnMessage = control.val() == '' ? 'Required' : '';
    }
    if (control.hasClass('NumericOnly') && ReturnMessage == '') {
        cls += "#NumericOnly";
        ReturnMessage = !isNumeric(control.val()) ? 'only numbers like 1234' : '';
    }
    if (control.hasClass('NumericWithoutDecimal') && ReturnMessage == '') {
        cls += "#NumericWithoutDecimal";
        ReturnMessage = !isNumeric(control.val()) ? 'only numbers like 1234' : '';
    }
    //
    if (control.hasClass('DecimalOnly') && ReturnMessage == '') {
        cls += "#DecimalOnly";
        ReturnMessage = !isDecimal(control.val()) ? 'only numbers/decimal values like 12.34' : '';
    }
    if (control.hasClass('AlphabeticOnly') && ReturnMessage == '') {
        cls += "#AlphabeticOnly";
        ReturnMessage = !isAlphabetic(control.val()) ? 'only characters, like abcd' : '';
    }
    if (control.hasClass('AlphaNumericOnly') && ReturnMessage == '') {
        cls += "#AlphaNumericOnly";
        ReturnMessage = !isAlphaNumeric(control.val()) ? 'only numbers and/or characters, like 123abc without space are allowed' : '';
    }
    if (control.hasClass('ValidEmail') && ReturnMessage == '') {
        cls += "#ValidEmail";
        ReturnMessage = !isValidEmail(control.val()) ? 'email address, like user@domain.com' : '';
    }
    if (control.hasClass('ValidPassword') && ReturnMessage == '') {
        cls += "#ValidPassword";
        ReturnMessage = !isValidPassword(control.val()) ? 'Password must be more than 6 characters.It cannot start with a number, underscore, or special character. It must contain at least one number , special character , one Lowercase and uppercase characters.' : '';
    }
    if (control.hasClass('ValidIPAddress') && ReturnMessage == '') {
        cls += "#ValidIPAddress";
        ReturnMessage = !isValidIPAddress(control.val()) ? 'IP address, like "50.23.221.50"' : '';
    }

    if (control.hasClass('ValidPhone') && ReturnMessage == '') {
        cls += "#ValidPhone";
        ReturnMessage = !isValidPhone(control.val()) ? 'Please enter phone number in +44 (0)9999 999999 format.' : '';
    }

    if (control.hasClass('ValidURL') && ReturnMessage == '') {
        cls += "#ValidURL";
        ReturnMessage = !isValidUrl(control.val()) ? 'URL, like http://www.google.com' : '';
    }
    if (control.hasClass('AlphabaticWithSpace') && ReturnMessage == '') {
        cls += "#AlphabaticWithSpace";
        ReturnMessage = !isAlphabeticWithSpace(control.val()) ? 'only alphabatic like abcd efg' : '';
    }
    if (control.hasClass('alphanumericandspace') && ReturnMessage == '') {
        cls += "#alphanumericandspace";
        ReturnMessage = !isAlphaNumericWithSpace(control.val()) ? 'numbers and/or space, like - 112 new city' : '';
    }
    if (control.hasClass('alphanumericwithoutspace') && ReturnMessage == '') {
        cls += "#alphanumericwithoutspace";
        ReturnMessage = !isAlphaNumericWithoutSpace(control.val()) ? 'numbers and character, like - aaa112 without space' : '';
    }
    if (control.hasClass('validdate') && ReturnMessage == '') {
        cls += "#validdate";
        ReturnMessage = !isValidDate(control.val()) ? 'Date in dd/MM/yyyy Format' : '';
    }
    if ($.trim(cls) == "") {
        control.nextAll("div.validationMessage").remove();
    }
    else {
        showValidation(control, ReturnMessage);
    }

}
/***************************************************************************************/



function ClearErrorNotifications() {
    $("input").nextAll("div.validationMessage").remove();
    $(".borderRed").css("border", "");
    $(".borderRed").css("box-shadow", "");
    $(".borderRed").css("-moz-box-shadow", "");
    $(".borderRed").css("-webkit-box-shadow", "");
    $(".borderRed").css("background-color", "");
    $(".borderRed").css("border-style", "solid");
    $(".borderRed").css("border-width", "1px");
    $(".borderRed").attr("Title", "");
    $(".borderRed").removeAttr("Title");
}



/*******************************************************Form Validate***************************/

function ValidateForm() {
    $("input[type=text]:not(.NoValidation),input[type='password']:not(.NoValidation)").each(function () {
        var ReturnMessage = '';
        var cls = "";
        var control = $(this);
        if ($(this).hasClass('required')) {
            cls += "#required";
            ReturnMessage = $(this).val() == '' ? 'Required' : '';
        }
        if ($(this).hasClass('NumericOnly') && ReturnMessage == '') {
            cls += "#NumericOnly";
            ReturnMessage = !isNumeric($(this).val()) ? 'only numbers like 1234' : '';
        }
        if ($(this).hasClass('NumericWithoutDecimal') && ReturnMessage == '') {
            cls += "#NumericWithoutDecimal";
            ReturnMessage = !isNumeric($(this).val()) ? 'only numbers like 1234' : '';
        }
        if ($(this).hasClass('DecimalOnly') && ReturnMessage == '') {
            cls += "#DecimalOnly";
            ReturnMessage = !isDecimal($(this).val()) ? 'only numbers/decimal values like 12.34' : '';
        }
        if ($(this).hasClass('AlphabeticOnly') && ReturnMessage == '') {
            cls += "#AlphabeticOnly";
            ReturnMessage = !isAlphabetic($(this).val()) ? 'only characters, like abcd' : '';
        }
        if ($(this).hasClass('AlphaNumericOnly') && ReturnMessage == '') {
            cls += "#AlphaNumericOnly";
            ReturnMessage = !isAlphaNumeric($(this).val()) ? 'only numbers and/or characters, like 123abc without space are allowed' : '';
        }
        if ($(this).hasClass('ValidEmail') && ReturnMessage == '') {
            cls += "#ValidEmail";
            ReturnMessage = !isValidEmail($(this).val()) ? 'email address, like user@domain.com' : '';
        }

        if ($(this).hasClass('ValidPassword') && ReturnMessage == '') {
            cls += "#ValidPassword";
            ReturnMessage = !isValidPassword($(this).val()) ? 'Password must be more than 6 characters. It cannot start with a number, underscore, or special character. It must contain at least one number , special character , one Lowercase and uppercase characters.' : '';
        }

        if ($(this).hasClass('ValidIPAddress') && ReturnMessage == '') {
            cls += "#ValidIPAddress";
            ReturnMessage = !isValidIPAddress($(this).val()) ? 'IP address, like "50.23.221.50"' : '';
        }

        if ($(this).hasClass('ValidPhone') && ReturnMessage == '') {
            cls += "#ValidPhone";
            ReturnMessage = !isValidPhone($(this).val()) ? 'Please enter phone number in +44 (0)9999 999999 format.' : '';
        }

        if ($(this).hasClass('ValidURL') && ReturnMessage == '') {
            cls += "#ValidURL";
            ReturnMessage = !isValidUrl($(this).val()) ? 'URL, like http://www.google.com' : '';
        }
        if ($(this).hasClass('AlphabaticWithSpace') && ReturnMessage == '') {
            cls += "#AlphabaticWithSpace";
            ReturnMessage = !isAlphabeticWithSpace($(this).val()) ? 'only alphabatic like abcd efg' : '';
        }
        if ($(this).hasClass('alphanumericandspace') && ReturnMessage == '') {
            cls += "#alphanumericandspace";
            ReturnMessage = !isAlphaNumericWithSpace($(this).val()) ? 'numbers and/or space, like - 112 new city' : '';
        }
        if ($(this).hasClass('alphanumericwithoutspace') && ReturnMessage == '') {
            cls += "#alphanumericwithoutspace";
            ReturnMessage = !isAlphaNumericWithoutSpace($(this).val()) ? 'numbers and character, like - aaa112 without space' : '';
        }
        if ($(this).hasClass('validdate') && ReturnMessage == '') {
            cls += "#validdate";
            ReturnMessage = !isValidDate($(this).val()) ? 'Date in dd/MM/yyyy Format' : '';
        }
        if ($.trim(cls) == "") {
            control.nextAll("div.validationMessage").remove();
        }
        else {
            showValidation(control, ReturnMessage);
        }
    });
}

function ValidateDropDown() {
    //$(".")
    $(".requiredselect").each(function () {
        try {
            if (($.trim($(this).val()) == "") || ($.trim($(this).val()) == "0")) {
                showValidation($(this), "Required");
            }
            else {
                showValidation($(this), "");
            }
        } catch (err) {
            return false;
        }
    });
}


function IsFormValid() {
    ValidateForm();
    ValidateDropDown();
    ValidateCheckListBox();

    if ($(".NotValid").length > 0) {
        return false;
    }
    else {
        return true;
    }
}

//if (checkAlExist()) { \\commented on sample testing -20-02-2013
//            return true;
//        }
//        else {
//            return false;
//        }


function ValidateCheckListBoxById(Cnt_rl) {
    Cnt_rl.removeClass("NotValid");
    if (Cnt_rl.hasClass("required")) {
        var cntr = 0;
        Cnt_rl.find("input").each(function () {
            if ($(this).is(":checked")) {
                cntr = cntr + 1;
            }
        });
        if (cntr <= 0) {
            Cnt_rl.addClass("NotValid");
            Cnt_rl.attr("errtitle", "Required");
        }
        else {
            Cnt_rl.removeClass("NotValid");
            Cnt_rl.removeAttr("errtitle");
        }
    }
}


function GetSelectedLstValuesArray(cntrl_Id) {
    return GetCheckListBoxValuesArray($("#checkListBx_" + cntrl_Id));
}

function GetSelectedLstValuesCS(cntrl_Id) {
    return GetCheckListBoxValuesComma($("#checkListBx_" + cntrl_Id));
}

function GetCheckListBoxValuesArray(Cnt_rl) {
    var Arr = new Array();
    var i = 0;
    Cnt_rl.find("input").each(function () {
        if ($(this).attr("checked") == "checked") {
            Arr[i] = $(this).attr("tag");
            i++;
        }
    });
    return Arr;
}

function GetCheckListBoxValuesComma(Cnt_rl) {
    var Arr = "";
    Cnt_rl.find("input").each(function () {
        if ($(this).attr("checked") == "checked") {
            Arr = Arr + $(this).attr("tag") + ",";
        }
    });
    return Arr;
}

function ValidateCheckListBox() {
    $(".CheckListBox").removeClass("NotValid");
    $(".CheckListBox").each(function () {
        if ($(this).hasClass("required")) {
            var cntr = 0;
            $(this).find("input").each(function () {
                if ($(this).attr("checked") == "checked") {
                    cntr = cntr + 1;
                }
            });
            if (cntr <= 0) {
                $(this).addClass("NotValid");
                $(this).attr("errtitle", "Required");
            }
            else {
                $(this).removeClass("NotValid");
                $(this).removeAttr("errtitle");
            }
        }
    });
}

/*******************************************************Form Group Validate***************************/

function ValidateGroupForm(group) {
    $("input[type=text]:not(.NoValidation)").each(function () {
        if ($.trim($(this).attr("group")) == group) {
            var ReturnMessage = '';
            var cls = "";
            var control = $(this);
            if ($(this).hasClass('required')) {
                cls += "#required";
                ReturnMessage = $(this).val() == '' ? 'Required' : '';
            }
            if ($(this).hasClass('NumericOnly') && ReturnMessage == '') {
                cls += "#NumericOnly";
                ReturnMessage = !isNumeric($(this).val()) ? 'only numbers like 1234' : '';
            }
            if ($(this).hasClass('NumericWithoutDecimal') && ReturnMessage == '') {
                cls += "#NumericWithoutDecimal";
                ReturnMessage = !isNumeric($(this).val()) ? 'only numbers like 1234' : '';
            }
            if ($(this).hasClass('DecimalOnly') && ReturnMessage == '') {
                cls += "#DecimalOnly";
                ReturnMessage = !isDecimal($(this).val()) ? 'only numbers/decimal values like 12.34' : '';
            }
            if ($(this).hasClass('AlphabeticOnly') && ReturnMessage == '') {
                cls += "#AlphabeticOnly";
                ReturnMessage = !isAlphabetic($(this).val()) ? 'only characters, like abcd' : '';
            }
            if ($(this).hasClass('AlphaNumericOnly') && ReturnMessage == '') {
                cls += "#AlphaNumericOnly";
                ReturnMessage = !isAlphaNumeric($(this).val()) ? 'only numbers and/or characters, like 123abc without space are allowed' : '';
            }
            if ($(this).hasClass('ValidEmail') && ReturnMessage == '') {
                cls += "#ValidEmail";
                ReturnMessage = !isValidEmail($(this).val()) ? 'email address, like user@domain.com' : '';
            }

            if ($(this).hasClass('ValidPassword') && ReturnMessage == '') {
                cls += "#ValidPassword";
                ReturnMessage = !isValidPassword($(this).val()) ? 'Password must be more than 6 characters. It cannot start with a number, underscore, or special character. It must contain at least one number , special character , one Lowercase and uppercase characters.' : '';
            }
            if ($(this).hasClass('ValidIPAddress') && ReturnMessage == '') {
                cls += "#ValidIPAddress";
                ReturnMessage = !isValidIPAddress($(this).val()) ? 'IP address, like "50.23.221.50"' : '';
            }


            if ($(this).hasClass('ValidPhone') && ReturnMessage == '') {
                cls += "#ValidPhone";
                ReturnMessage = !isValidPhone($(this).val()) ? 'Please enter phone number in +44 (0)9999 999999 format.' : '';
            }

            if ($(this).hasClass('ValidURL') && ReturnMessage == '') {
                cls += "#ValidURL";
                ReturnMessage = !isValidUrl($(this).val()) ? 'URL, like http://www.google.com' : '';
            }
            if ($(this).hasClass('AlphabaticWithSpace') && ReturnMessage == '') {
                cls += "#AlphabaticWithSpace";
                ReturnMessage = !isAlphabeticWithSpace($(this).val()) ? 'only alphabatic like abcd efg' : '';
            }
            if ($(this).hasClass('alphanumericandspace') && ReturnMessage == '') {
                cls += "#alphanumericandspace";
                ReturnMessage = !isAlphaNumericWithSpace($(this).val()) ? 'numbers and/or space, like - 112 new city' : '';
            }
            if ($(this).hasClass('alphanumericwithoutspace') && ReturnMessage == '') {
                cls += "#alphanumericwithoutspace";
                ReturnMessage = !isAlphaNumericWithoutSpace($(this).val()) ? 'numbers and character, like - aaa112 without space' : '';
            }
            if ($(this).hasClass('validdate') && ReturnMessage == '') {
                cls += "#validdate";
                ReturnMessage = !isValidDate($(this).val()) ? 'Date in dd/MM/yyyy Format' : '';
            }
            if ($.trim(cls) == "") {
                control.nextAll("div.validationMessage").remove();
            }
            else {
                showValidation(control, ReturnMessage);
            }
        }
    });
}

function IsGroupValid(group) {
    ValidateForm();
    if ($('.NotValid[value="' + group + '"]').length > 0) {
        return false;
    }
    else {
        return true;
    }
}


function PopulateCheckListbox() {
    $(".CheckListBox").remove();
    $(".cnCheckListBox").each(function () {
        var Idd = $(this).attr("id");
        var lstItem = '<div class="CheckListBox ' + $.trim($(this).attr("cmmnAssignment")) + '" id="checkListBx_' + Idd + '" style="margin-top: 5px; height: 100%; overflow: auto;" target_Id = "' + Idd + '">';
        $(this).find("option").each(function () {
            if ($(this).attr("selected") == "selected") {
                lstItem += '<p><input type="checkbox" checked="checked" tag="' + $(this).val() + '" id="opt' + Idd + $(this).val() + '">' + $.trim($(this).html()) + '</p>';
            }
            else {
                lstItem += '<p><input type="checkbox" tag="' + $(this).val() + '" id="opt' + Idd + $(this).val() + '">' + $.trim($(this).html()) + '</p>';
            }
        });
        lstItem += '</div>';
        $(this).after(lstItem);
        $(this).css("display", "none");
    });
}



function PopulateCheckListboxById(control) {
    control.nextAll("div.CheckListBox").remove();
    var Idd = control.attr("id");
    var lstItem = '<div class="CheckListBox ' + $.trim(control.attr("cmmnAssignment")) + '" id="checkListBx_' + Idd + '" style="margin-top: 5px; height: 100%; overflow: auto;; float: left;" target_Id = "' + Idd + '">';
    control.find("option").each(function () {
        if ($(this).attr("selected") == "selected") {
            lstItem += '<p><input type="checkbox" checked="checked" tag="' + $(this).val() + '" id="opt' + Idd + $(this).val() + '">' + $.trim($(this).html()) + '</p>';
        }
        else {
            lstItem += '<p><input type="checkbox" tag="' + $(this).val() + '" id="opt' + Idd + $(this).val() + '">' + $.trim($(this).html()) + '</p>';
        }
    });
    lstItem += '</div>';
    control.after(lstItem);
    control.css("display", "none");
}
// 

window.onload = test;
function test() {
    $("#Email").val("");
    $("#Password").val("");
    $("#Users_Email").val("");
    $("#Users_Password").val("");
    $("#UserModel_Email").val("");
    $("#UserModel_Password").val("");
}

function ValidateTimeNew() {

    var endFirst = $('.endTime').val().substring(0, $('.endTime').val().indexOf(':'));
    var endLast = $('.endTime').val().substring($('.endTime').val().indexOf(':') + 1, $('.endTime').val().indexOf(' '));
    var startfirst = $('.startTime').val().substring(0, $('.startTime').val().indexOf(':'));
    var startLast = $('.startTime').val().substring($('.startTime').val().indexOf(':') + 1, $('.startTime').val().indexOf(' '));

    if ($('.endTime').val().toLowerCase().indexOf("pm") > 0) {
        if ($('.startTime').val().toLowerCase().indexOf("pm") > 0) {
            if (parseInt(startfirst) > parseInt(endFirst)) {
                //AlertMessage("Opening Time Should Not be Greater Than Closing Time");
                return false;
            }
            else if (parseInt(startfirst) == parseInt(endFirst) && parseInt(startLast) > parseInt(endLast)) {
                //AlertMessage("Opening Time Should Not be Greater Than Closing Time");
                return false;
            }
        }
    }
    else if ($('.endTime').val().toLowerCase().indexOf("am") > 0) {
        if ($('.startTime').val().toLowerCase().indexOf("am") > 0) {

            ////if (parseInt(endFirst) == 12) {

            ////}
            //else {
            if (parseInt(startfirst) > parseInt(endFirst)) {
                //AlertMessage("Opening Time Should Not be Greater Than Closing Time");
                return false;
            }
            else if (parseInt(startfirst) == parseInt(endFirst) && parseInt(startLast) > parseInt(endLast)) {
                //  AlertMessage("Opening Time Should Not be Greater Than Closing Time");
                return false;

            }
            //}
        }
    }

    else {

    }
    return true;
}