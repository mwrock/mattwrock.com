function ShowStatus(status, msg) {
    $("[id$='_AdminStatus']").removeClass("warning");
    $("[id$='_AdminStatus']").removeClass("success");

    $("[id$='_AdminStatus']").addClass(status);
    $("[id$='_AdminStatus']").html(msg + '<a href="javascript:HideStatus()" style="width:20px;float:right">X</a>');
    $("[id$='_AdminStatus']").fadeIn(1000, function () { });
}

function HideStatus() {
    $("[id$='_AdminStatus']").fadeOut('slow', function () { });
}

function Hide(element) {
    $("[id$='" + element + "']").fadeOut('slow', function () { });
    return false;
}

function ValidatePasswordRetrieval() {
    if ($("[id$='txtEmail']").val().length == 0) {
        ShowStatus('warning', 'Email is required');
        return false;
    }
    if (ValidateEmail($("[id$='txtEmail']").val()) == false) {
        ShowStatus('warning', 'Email is invalid');
        return false;
    }
    return true;
}

function ValidateLogin() {
    if ($("[id$='UserName']").val().length == 0) {
        ShowStatus('warning', 'User name is required');
        return false;
    }
    if ($("[id$='Password']").val().length == 0) {
        ShowStatus('warning', 'Password is required');
        return false;
    }
    return true;
}

function ValidateChangePassword() {    
    if ($("[id$='CurrentPassword']").val().length == 0) {
        ShowStatus('warning', 'Old password is required');
        return false;
    }
    if ($("[id$='NewPassword']").val().length == 0) {
        ShowStatus('warning', 'New password is required');
        return false;
    }
    var minReq = $("[id$='_hdnPassLength']").val();
    var minPass = $("[id$='NewPassword']").val().length;

    if (minPass < minReq) {
        ShowStatus('warning', 'Minimum passwrod length is ' + minReq + ' characters');
        return false;
    }
    if ($("[id$='ConfirmNewPassword']").val().length == 0) {
        ShowStatus('warning', 'Confirm password is required');
        return false;
    }
    if ($("[id$='NewPassword']").val() != $("[id$='ConfirmNewPassword']").val()) {
        ShowStatus('warning', 'New and confirm passwords do not match');
        return false;
    }   
    return true;
}

function ValidateNewUser() {
    if ($("[id$='UserName']").val().length == 0) {
        ShowStatus('warning', 'User name is required');
        return false;
    }
    if ($("[id$='Email']").val().length == 0) {
        ShowStatus('warning', 'Email is required');
        return false;
    }
    if (ValidateEmail($("[id$='Email']").val()) == false) {
        ShowStatus('warning', 'Email is invalid');
        return false;
    }
    if ($("[id$='Password']").val().length == 0) {
        ShowStatus('warning', 'Password is required');
        return false;
    }
    var minReq = $("[id$='_hdnPassLength']").val();
    var minPass = $("[id$='Password']").val().length;

    if (minPass < minReq) {
        ShowStatus('warning', 'Minimum passwrod length is ' + minReq + ' characters');
        return false;
    }
    if ($("[id$='ConfirmPassword']").val().length == 0) {
        ShowStatus('warning', 'Confirm password is required');
        return false;
    }
    if ($("[id$='Password']").val() != $("[id$='ConfirmPassword']").val()) {
        ShowStatus('warning', 'Password and confirm password do not match');
        return false;
    }
    return true;
}

function ValidateEmail(email) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    return reg.test(email);
}