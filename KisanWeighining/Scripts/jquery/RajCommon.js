function isNumber(evt, element) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function isIntNumber(e, element) {   
    k = e.which;
    debugger;
    if (element.value.length > 9) {
        if (k == 9) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        if ((k >= 96 && k <= 105) || k == 8) {
            if ($(this).val().length == 10) {
                if (k == 8) {
                    return true;
                } else {
                    if (k == 9) {
                        return true;
                    }
                    else {
                        event.preventDefault();
                        return false;
                    }
                }
            }
        } else {
            if (k == 9) {
                return true;
            }
            else {
                event.preventDefault();
                return false;
            }
        }
    }
}
var a = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ', 'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
var b = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];

function inWords(num) {
    debugger;
    if ((num = num.toString()).length > 9) return 'overflow';
    n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
    if (!n) return; var str = '';
    str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'crore ' : '';
    str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'lakh ' : '';
    str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'thousand ' : '';
    str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'hundred ' : '';
    str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'only ' : '';
    return str;
}

function onlyAlpha(e) {

    var t = event || e;
    var n = t.which || t.keyCode;
    if (n > 31 && (n < 48 || n > 57)) return true;
    return false
}

function onlyAlphaNumeric(e) {

    var t = event || e;
    var n = t.which || t.keyCode;
    if ((n < 65 || n > 90) && (n < 48 || n > 57) && (n < 97 || n > 122)) return false;
    return true
}

$(".Rajstring").keypress(function (e) {
    if (!onlyAlpha("Rajstring")) {
        e.preventDefault();
    }
});

$(".Rajalph").keypress(function (e) {
    if (!onlyAlphaNumeric("Rajalph")) {
        e.preventDefault();
    }
});


$('.Rajnumber').keypress(function (e) {
    if (!isNumber("Rajnumber")) {
        e.preventDefault();
    }
});