
$(document).ready(function () {
    // Becomes true when any value in a form view control changes
    var _fvChanged = false;
    // Whenever debit or credit changes, or if row selection changes, recompute totals
    // Save button is enabled when there is some modified row
    var $grid = $('#gvEditVoucherDetails').bind('selectablestop', function (event, ui) {
        UpdateAllTotals(this);
        if (_fvChanged || $(this).has('tr.ui-selected').length > 0) {
            $('#btnSave,#btnSave2').buttonEx('enable');
        } else {
            $('#btnSave,#btnSave2').buttonEx('disable');
        }
    }).bind('selectableselected', function (event, ui) {

        //Following code was commented in consultation with Mr Pal 

        // Propose discrepancy as the amount if nothing has been entered in this newly selected row
        //        var $debit = $('input:text[name$=tbDebit]', ui.selected);
        //        var $credit = $('input:text[name$=tbCredit]', ui.selected);
        //        if (!$debit.val() && !$credit.val()) {
        //            var discrep = Number($(this).gridViewEx('footerCell', "Head").text());
        //            if (!isNaN(discrep)) {
        //                if (discrep > 0) {
        //                    $credit.val(discrep);
        //                } else {
        //                    $debit.val(discrep);
        //                }
        //            }
        //        }
    }).change(function (e) {
        var $tr = $(e.target).closest('tr');
        var $ddlStatus = $('select', $tr);
        if (e.target != $ddlStatus[0]) {
            // Mark the row as modified
            $ddlStatus.val('S')
        }
        switch ($ddlStatus.val()) {
            case 'N':
            case 'U':
                // New or unchanged
                $(this).gridViewEx('unselectRows', e, $tr);
                break;

            case 'S':
                // Modified row, or needs to be inserted
                $(this).gridViewEx('selectRows', e, $tr);
                break;

            case 'D':
                // Row to be deleted
                $(this).gridViewEx('selectRows', e, $tr);
                break;
        }
    });

    UpdateAllTotals($grid[0]);

    // First Trigger the change handler so that payee/check can be hidden/shown
    $('#rblVoucherTypes').change();

    // Now If anything in formview changes, enable save
    $('#fvEditTemplate').change(function (e) {
        $('#btnSave,#btnSave2').buttonEx('enable');
        _fvChanged = true;
    });

});

// Show hide Check number and payee text boxes when voucher type changes
function rblVoucherTypes_Change(e) {
    switch ($(this).radioButtonListEx('val')) {
        case 'B':
            $('#tbPayee').closest('tr').show();
            $('#tbCheckNumber').closest('tr').show();
            break;

        case 'C':
            $('#tbPayee').closest('tr').show();
            $('#tbCheckNumber').closest('tr').hide();
            break;

        case 'J':
            $('#tbPayee').closest('tr').show();
            $('#tbCheckNumber').closest('tr').hide();
            break;
    }
}

// Show hide grid columns depending on user request
function ddlMoreColumns_Change(e) {

    var $grid = $('#gvEditVoucherDetails');
    switch ($(this).val()) {
        case '':
            $grid.gridViewEx('columnCells', 'Employee').hide();
            $grid.gridViewEx('headerCell', 'Employee').hide();
            $grid.gridViewEx('footerCell', 'Employee').hide();
            $grid.gridViewEx('columnCells', 'Job').hide();
            $grid.gridViewEx('headerCell', 'Job').hide();
            $grid.gridViewEx('footerCell', 'Job').hide();
            $grid.gridViewEx('columnCells', 'Contractor').hide();
            $grid.gridViewEx('headerCell', 'Contractor').hide();
            $grid.gridViewEx('footerCell', 'Contractor').hide();
            break;

        case 'E':
            $grid.gridViewEx('columnCells', 'Employee').show();
            $grid.gridViewEx('headerCell', 'Employee').show();
            $grid.gridViewEx('footerCell', 'Employee').show();
            $grid.gridViewEx('columnCells', 'Job').hide();
            $grid.gridViewEx('headerCell', 'Job').hide();
            $grid.gridViewEx('footerCell', 'Job').hide();
            $grid.gridViewEx('columnCells', 'Contractor').hide();
            $grid.gridViewEx('headerCell', 'Contractor').hide();
            $grid.gridViewEx('footerCell', 'Contractor').hide();
            break;

        case 'J':
            $grid.gridViewEx('columnCells', 'Employee').show();
            $grid.gridViewEx('headerCell', 'Employee').show();
            $grid.gridViewEx('footerCell', 'Employee').show();
            $grid.gridViewEx('columnCells', 'Job').show();
            $grid.gridViewEx('headerCell', 'Job').show();
            $grid.gridViewEx('footerCell', 'Job').show();
            $grid.gridViewEx('columnCells', 'Contractor').show();
            $grid.gridViewEx('headerCell', 'Contractor').show();
            $grid.gridViewEx('footerCell', 'Contractor').show();
            break;
    }
}

// Job auto complete requires division to be entered first. Additionally, it passes division id as a parameter to the web method.
function tbJob_Search(event, ui) {
    var divisionId = $('#tbDivisionCode').autocompleteEx('selectedValue');
    if (!divisionId) {
        alert('Please select division first');
        return false;
    }
    $(this).autocompleteEx('option', 'parameters', { divisionId: divisionId, term: $(this).val() });
    return true;
}
// HeadofAccount autocomplete requires station to be entered first.Passing extra parameter for station
function tbHead_Search(event, ui) {
    var station = $('#ddlStation').val();
    if (!station) {
        alert('Please select station first');
        return false;
    }
    $(this).autocompleteEx('option', 'parameters', {  station: station ,term: $(this).val()});
    return true;
}

function tbJob_KeyPress() {
    var divisionId = $('#tbDivisionCode').autocompleteEx('selectedValue');
    if (!divisionId) {
        alert('Please select division first');
        return false;
    }
}

function tbJob_Change() {
    var divisionId = $('#tbDivisionCode').autocompleteEx('selectedValue');
    if (!divisionId) {
        alert('Please select division first');
        return false;
    }
    $(this).autocompleteEx('option', 'parameters', { divisionId: divisionId, term: $(this).val() });
    var badJob = $(this).autocompleteEx('validate');
    var job = $(this).autocompleteEx('selectedValue');
    if (!job) {
        alert('Job is invalid or it does not belongs division. Please re-enter valid job');
        $(this).val('');
        $(this).focus();
    }
}

function tbJob_Select(event, ui) {
    $(this).autocompleteEx('select', ui.item.Value, ui.item.Text);
    var tr = $(this).closest('tr');
    var tbContractor = tr.find('input:text[name$=tbContractor]');
    var tbHead = tr.find('input:text[name$=tbHead]');

    $(tbContractor).autocompleteEx('select', ui.item.ContractorValue, ui.item.ContractorText);
    $(tbHead).autocompleteEx('select', ui.item.HeadOfAccountValue, ui.item.HeadOfAccountText);
    return false;
}

// Dependency custom function. Returns true if the element is part of a selected row.
function IsRowSelected(element) {
    return $(element).closest('tr').is('.ui-selected');
}

// Ensure that exactly one of debit or credit must be entered in each row
$.validator.addMethod('DebitOrCredit', function (value, element, params) {
    var $tr = $(element).closest('tr');
    var debitAmount = $('input:text[name$=tbDebit]', $tr).val();
    var creditAmount = $(element).val();
    return (debitAmount || creditAmount) && !(debitAmount && creditAmount);
}, function (params, element) {
    var rowIndex = $('#gvEditVoucherDetails').gridViewEx('rowIndex', $(element).closest('tr'));
    return $.validator.format('Row {0}: Debit or credit, but not both, must be specified', rowIndex + 1);
});

// Recompute all debit and credit totals. Called when any debit or credit amount changes, or when row status changes
function UpdateAllTotals(grid) {
    var colindexDebit = $(grid).gridViewEx('colIndex', "Debit");
    RecomputeColumnTotal(grid, colindexDebit);
    var colindexCredit = $(grid).gridViewEx('colIndex', "Credit");
    RecomputeColumnTotal(grid, colindexCredit);
    UpdateDiscrepancy(grid, colindexDebit, colindexCredit);
}

// Recompute the totals of the passed column. Only selected rows are considered
function RecomputeColumnTotal(grid, colindex) {
    var sum = 0;
    $('input', $(grid).gridViewEx('columnCells', colindex)).filter(function (i) {
        var val = $('select', $(this).closest('tr')).val();
        return val == 'S' || val == 'U';
    }).each(function (i) {
        var val = $(this).val();
        if (val) {
            val = val.replace(/,/g, '');
            sum += Number(val);
        }
    });
    $(grid).gridViewEx('footerCell', colindex).html(sum.toFixed(2));
}

// Display the debit/credit discrepancy in red
function UpdateDiscrepancy(grid, colindexDebit, colindexCredit) {
    var debit = $(grid).gridViewEx('footerCell', colindexDebit).html();
    var credit = $(grid).gridViewEx('footerCell', colindexCredit).html();
    var $tdhead = $(grid).gridViewEx('footerCell', "Head");
    if (debit == '' || debit == '&nbsp;') {
        debit = 0;
    } else {
        debit = Number(debit);
    }
    if (credit == '' || credit == '&nbsp;') {
        credit = 0;
    } else {
        credit = Number(credit);
    }
    if (isNaN(debit) || isNaN(credit)) {
        $tdhead.html('???').addClass('ui-state-error');
    } else if (debit == credit) {
        $tdhead.html('Balanced').removeClass('ui-state-error');
    } else {
        $tdhead.html(debit - credit).addClass('ui-state-error');
    }
}

function btnSave_Click(e) {
    if ($('#gvEditVoucherDetails').gridViewEx('footerCell', "Head").hasClass('ui-state-error')) {
        alert('The voucher must be balanced before it can be saved.');
        return false;
    }
}

function msg_HeadRequired(params, element) {
    var rowIndex = $('#gvEditVoucherDetails').gridViewEx('rowIndex', $(element).closest('tr'));
    return $.validator.format('Row {0}: Head of account is required', rowIndex + 1);
}

function msg_NumericAmount(params, element) {
    var rowIndex = $('#gvEditVoucherDetails').gridViewEx('rowIndex', $(element).closest('tr'));
    return $.validator.format('Row {0}: {1} must be numeric', rowIndex + 1, $(element).attr('title'));
}

$.validator.addMethod('ValidatePayee', function (value, element, params) {
    var rblVoucherTypes = $('#rblVoucherTypes').radioButtonListEx('val');
    if (rblVoucherTypes == 'B' || rblVoucherTypes == 'C') {
        var payeeText = $('#tbPayee').val();
        if (!payeeText) {
            return false;
        }
        else {
            $('#tbPayee').autocompleteEx('select', payeeText, payeeText);
            return true;
        }
    }
    else {
        return true;
    }
});