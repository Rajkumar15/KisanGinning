var sendarray = [];
$(document).ready(function () {
   
})
$.ajaxPrefilter(function (options, original_Options, jqXHR) {
    options.async = true;
});
$('.js-basic-example').DataTable({
    "processing": true, // for show progress bar
    "serverSide": true, // for process server side
    "filter": true, // this is for disable filter (search box)
    "orderMulti": false, // for disable multiple column at once

    "ajax": {
        "url": "/PooldataMachine/PaymentReadyList",
        "type": "POST",
        "datatype": "json"
    },
    "columns": [
            { "data": "pkid", "name": "pkid", "orderable": false, "autoWidth": true },
                { "data": "paymenttype", "name": "paymenttype", "autoWidth": true },
                 { "data": "paidto", "name": "paidto", "autoWidth": true },
                   { "data": "farmername", "name": "farmername", "autoWidth": true },
                  { "data": "details", "name": "details", "autoWidth": true },
                   {
                       "data": "paymentdate", "render": function (data, type, full, meta) {
                           return '' + formatJsonDate(data) + '';
                       }
                   },
                    {
                        "data": "clearingdate", "render": function (data, type, full, meta) {
                            return '' + formatJsonDate(data) + '';
                        }
                    },
                    { "data": "amountpaid", "name": "amountpaid", "autoWidth": true },
    ],
    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
        $("td:first", nRow).html("<p>" + parseFloat(iDisplayIndex + 1) + "</p><input type='checkbox' name=" + aData.pkid + " class='check form-control' onchange='saas(this)'/>")
        return nRow;
    }
})
function saas(cname)
{
    if (cname.checked) {
        sendarray.push(cname.name);
    } else {
        sendarray.pop(cname.name);
    }
}
function formatJsonDate(jsonDate) {
    if (jsonDate == null) {
        return "";
    }
    else {
        var dateString = jsonDate.substr(6);
        var currentTime = new Date(parseInt(dateString));
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var date = day + "/" + month + "/" + year;
        return date;
    }
};
function SendData()
{
    if (sendarray.length == 0) {
        jAlert("Select Checkboxes to Send bank", "System Alert");
    }
    else {
        $.ajax(
{
    url: '/PooldataMachine/Senddata/',
    type: 'POST',
    contentType: "application/json",
    data: JSON.stringify({ 'poopupdatasend': sendarray }),
    dataType: 'json',
    async: false,
    success: function (data) {
        debugger;
        if (data == "success") {
            jAlert("Data Send Successfully", "SystemAlert");
            window.location.reload();
        } else {
            jAlert("Data Save Failed enter required field", "SystemAlert")
        }
    }
});


    }
}
