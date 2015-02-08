$(document).ready(function () {

    var _timer;

    // Autocomplete query is in progress
    var _isExecuting;

    function GetAutocompleteData(url, cb) {
        //alert(url);
        _isExecuting = true;
        _timer = null;
        $.get(url).done(function (data, textStatus, jqXHR) {
            this.cb(data);
        }.bind({ cb: cb })).fail(function (jqXHR, textStatus, errorThrown) {
            //alert(jqXHR.responseText);
            if (jqXHR.status == 500) {
                this.cb([{ particulars: 'Error ' + (jqXHR.responseText || errorThrown), value: '' }]);
            } else {
                this.cb([{ particulars: 'Http Error ' + jqXHR.status + ': ' + errorThrown + ' ' + this.url, value: '' }]);
            }
        }.bind({ cb: cb, url: url })).complete(function () {
           // alert('done');
            _isExecuting = false;
        });
    }

    $('#tbLayoutSearch').typeahead({
        highlight: false,
        hint: false
    }, {
        //name: 'layout-bill',
        //displayKey: 'billnumber',
        source: function (query, cb) {
            if (_isExecuting) {
                // Do nothing
                return;
            }
            var url = $('#tbLayoutSearch').attr('data-list-url').replace('~', query);
            if (_timer) {
                clearTimeout(_timer);
            }
            // Provide 500ms delay before executing the query
            _timer = setTimeout(GetAutocompleteData, 500, url, cb);
        },
        templates: {
            suggestion: function (sugg) {
                var x = $('<span>').append($('<span>').addClass('sugg-date').html(sugg.date));
                if (sugg.billNumber) {
                    x = x.append($('<span>').addClass('sugg-number').html(sugg.billNumber));
                }
                if (sugg.amount) {
                    x = x.append($('<span>').addClass('sugg-amount').html(sugg.amount));
                }
                if (sugg.particulars) {
                    x = x.append($('<span>').addClass('sugg-particulars text-muted').html(sugg.particulars));
                }
                if (sugg.text) {
                    x = x.append($('<span>').addClass('sugg-text').html(sugg.text));
                }
                //var y = x.html();
                return x.html();
            },
            empty: 'No matching results found'
        }
    }).on('typeahead:selected typeahead:autocompleted', function (e, sug, ds) {
        // Redirect to bill details
        window.location = $(this).attr('data-bill-url').replace('0', sug.billId);
    });
});
