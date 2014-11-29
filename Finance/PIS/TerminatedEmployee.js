$(document).ready(function () {
    //Fixed disabled button problem for terminated employees.
    var terminated = $.trim($('#spTerminated').html());
    if (terminated != '') {
    // Hide RowMenuItem edit and delete button.
        $('.ui-widget.gvex-rowmenu').hide();
        // Disable insert buttons in remote forms
        $('form').find('.ui-button').attr('disabled', 'disabled').addClass('ui-state-disabled');
    }
});