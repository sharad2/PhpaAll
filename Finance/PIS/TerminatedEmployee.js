$(document).ready(function() {
    var terminated = $('#spTerminated').html();
    if (terminated != '') {
    // Hide RowMenuItem edit and delete button.
        $('.ui-widget.gvex-rowmenu').hide();
        // Disable insert buttons in remote forms
        $('form').find('.ui-button').attr('disabled', 'disabled');       
    }
});