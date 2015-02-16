///#source 1 1 Create.partial.js

// Disable auto discover for all elements:
//Dropzone.autoDiscover = false;

$(document).ready(function () {
	"use strict";
	//var myDropzone = new Dropzone("div#mydropzone", { url: "/file/post", autoProcessQueue: false });

	$('#tbDivision,#tbContractor').each(function (index, elem) {
		$(elem).typeahead({
			highlight: true
		}, {
			//name: 'Division',
			displayKey: 'label',
			source: function (query, cb) {
				var url = $(elem).attr('data-url').replace('~', query);
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
				empty: 'No matching results found'
			}
		}).on('typeahead:selected typeahead:autocompleted', function (e, sug, ds) {
			// Store the id of the selected division in the hdden field
			//$('#hfDivisionId').val(sug.value);
			$(this).closest('div.form-group').find('input:not(".typeahead")').val(sug.value);
		}).on('input', function (e) {
			// When user changes the divisionId, empty the hidden field
			$(this).closest('div.form-group').find('input:not(".typeahead")').val('');
		});

	});


	$("#billDatePicker,#divSubDatePicker,#dueDatePicker").datetimepicker({
		format: 'D/M/YYYY'
	});

});

