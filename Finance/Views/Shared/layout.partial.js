$(document).ready(function () {
    $('#tbLayoutSearch').typeahead({
        highlight: false,
        hint: false
    }, {
        //name: 'layout-bill',
        //displayKey: 'billnumber',
        source: function (query, cb) {
            var url = $('#tbLayoutSearch').attr('data-list-url').replace('~', query);
            $.get(url).done(function (data, textStatus, jqXHR) {
                this.cb(data);
            }.bind({ cb: cb })).fail(function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == 500) {
                    this.cb([{ label: 'Error ' + (jqXHR.responseText || errorThrown), value: '' }]);
                } else {
                    this.cb([{ label: 'Http Error ' + jqXHR.status + ': ' + errorThrown + ' ' + this.url, value: '' }]);
                }
            }.bind({ cb: cb, url: url }));
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
