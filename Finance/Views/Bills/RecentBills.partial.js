$(document).ready(function () {
    "use strict";

    $("#billDatePickerTo,#billDatePickerFrom,#dueDatePickerFrom,#dueDatePickerTo,#billApproveDatePicker").datetimepicker({
        format: 'D/M/YYYY'
    });

    // Code to clean up the URL by not posting unnecessary filters
    $('#frmFilters').on('submit', function (e) {
        // If all checkboxes in a group are checked, uncheck all so that they do not get posted
        $('#fsDivisions,#fsContractors,#fsApprovers,#fsStations,#fsprocessingDivision').each(function (index, elem) {
            if ($('input:not(:checked)', elem).length == 0) {
                //alert('Hi');
                $('input:checked', elem).prop('checked', false);
            }
        });
        //alert($('input:not(value),input[value=""]', '#fsBillDates,#fsAmounts').length);

        // Disable empty textboxes so that they do not get posted
        $('input', '#fsBillDates,#fsAmounts').filter(function (index, elem) {
            return $(elem).val() == '';
        }).prop('disabled', true);
        //return false;
    }).on('click', 'a.uncheck', function (e) {
        // Uncheck all checkboxes within panel body
        $(e.target).closest('.panel-body').find('input:checkbox:checked').prop('checked', false);
        return false;
    });

    // Handle approve and unapprove buttons
    $('#billGroup').on('click', '.approve', function (e) {
        // Approve button was clicked
        var billId = $(e.target).closest('div.row').find('input:checkbox').attr('value');
        var url = $(e.delegateTarget).attr('data-approve-url');
        //alert(url);
        var data = new Object();
        data[$(e.delegateTarget).attr('data-approve-paramname')] = billId;
        $.post(url, data, function (data, textStatus, jqXHR) {
            var y = 0;
        }, 'json').done(function (data, textStatus, jqXHR) {
            $(e.target).addClass('btn-success  btn-xs disabled').removeClass('btn-default approve');
        }).fail(function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 500) {
                // url was found but server returned error
                alert(jqXHR.responseText || errorThrown);
            } else {
                // Url was bad
                alert(jqXHR.status + ': ' + errorThrown + ' ' + this.url);
            }
        });
    });
});
