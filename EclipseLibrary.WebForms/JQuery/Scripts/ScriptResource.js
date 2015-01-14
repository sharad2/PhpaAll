///#source 1 1 /JQuery/Scripts/CommonScripts.js
/* SS 14 Jul 2010: Touch this file to forice rebuild of all scripts. */

/* 
Utility function to create cookies  
*/

function createCookie(name, value, days) {
    var expires;
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    else {
        expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}


// Use this when you want to change the text within a container while keeping the old value visible.
// It Appends to the current text within the container.
// If the current text in the container is the same as passed text, it does nothing.
// Old Behavior: lbl -> <span>new value</span>
// $lbl.latestText('new value')
// New Behavior: current contents of td
// $td.latestText('new value') -> <span>new value</span>
// The whole container is pulsated to draw attention to it.
/*
latestText is used to update the value displayed within a container while preserving the previous value at the same time.
e.g. $('#mydiv').latestText('New Value');

How it works:
If mydiv does not contain any child elements as in <div>Value</div>, then the contents of the div are wrapped within
a span so that it now becomes <div><span>Value</span></div>.

Now the new value is inserted in the clone of the last element and the result becomes
<div><span>Value</span><span>New Value</span></div>

Finally, the container is pulsated to draw attention to it.

Special Cases:
If the container is a TD, we additionally enclose its contents in a div since pulsating a td is very ugly.
7 Dec 2009: If multiple elements are being updated, we avoid the pulsate effect because it takes too much time.
*/
(function ($) {
    $.fn.latestText = function (newValue) {
        if (newValue === undefined) {
            // Acts as getter
            var $span = $(':visible:last-child', this);
            if ($span.length == 0) {
                return $(this).text();
            } else {
                // Return the text of the last element
                return $span.eq($span.length - 1).text();
            }
        } else {
            var contElements = this.length;
            return this.each(function () {
                var $container;
                if (this.tagName == 'TD') {
                    // Wrap contents in a div for better pulsate effect
                    $container = $('> div', this);
                    if ($container.length == 0) {
                        $(this).wrapInner('<div></div>');
                        $container = $('> div', this);
                    }
                } else {
                    $container = $(this);
                }
                var $lastChild = $('> :visible:last-child', $container).removeClass('ui-state-highlight');
                if ($lastChild.length == 0) {
                    // Wrap contents in span
                    $container.wrapInner('<span></span>');
                    $lastChild = $('> :visible:last-child', $container);    // Now last child is span
                }
                if ($lastChild.length == 0) {
                    // Empty cell
                    $lastChild = $('<span></span>');
                }
                var addHtml = '<span class="ui-icon ui-icon-arrowthick-1-e" style="display:inline-block" >&nbsp;&nbsp;&nbsp;</span>';
                var $clone = $lastChild.clone().text(newValue).addClass('ui-state-highlight');
                $container.append(addHtml).append($clone);
                if (contElements == 1) {
                    $container.effect('pulsate');
                }
            });
        }
    }
})(jQuery);                                                                         // pass the jQuery object to this function

// This file contains methods which we developed at Eclipse

// http://www.dexign.net/post/2008/07/16/jQuery-To-Call-ASPNET-Page-Methods-and-Web-Services.aspx
// successFn(data, textStatus)
// errorFn(xhr, status, e)
// Example: CallPageMethod('GetCustomerDetails', {customer_id: '1234'}, function(data, textStatus) {
//   alert(data.d);
// });
/*
8 Dec 2009: Returns HttpRequest object which can be used to cancel the ajax call. The data returned by your web method
is passed to your success function as the first argument.
*/
function CallPageMethod(fn, data, successFn, errorFn) {
    var jsonData = JSON.stringify(data);

    var pagePath = window.location.pathname;
    //Call the page method
    return $.ajax({
        type: "POST",
        url: pagePath + "/" + fn,
        contentType: "application/json; charset=utf-8",
        data: jsonData,
        //dataType: "json",
        success: successFn,
        error: errorFn ? errorFn : function (xhr, status, e) {
            alert(xhr.responseText);
        }
    });
}



///#source 1 1 /JQuery/Dialog/AjaxDialog.js
/// <reference path="~/Doc/jquery-1.3.2-vsdoc.js" />
/*
Usage: $('#mydlg').dialog(...).ajaxDialog(options);

Sharad 15 Sep 2009: After loading, setting focus to first tabbable element.
<div>
<div>
... remote contents ...
</div>
<div>
... content template ...
</div>
</div>

22 Oct 2009: Only a single dialog will be open at any time.
Cancelling long running ajax calls when dialog closes.
Preventing submit if ajax call is in progress.

19 Nov 2009: loaded event raised even when cache is true and loading event has not been raised.

11 Dec 2009: Explicitly calling the dialong open handler if autoOpen is true

All events receive just the event object. There is no ui object. During the event you can access the
remote container: var $remote = $(this).ajaxDialog('remoteContainer');
ui.remote: JQuery object representing the div containing the remote contents
ui.content: JQuery object representing the div containing the content template
*/
(function(jQuery) {
    // We keep track of the currently open dialog here.
    // When another dialog is opened, we close this one. Thus only a single dialog will show at any time.
    var _currentlyOpenDialog = null;

    $.widget('ui.ajaxDialog', {

        // Default options of this widget. Options beginning with _ are private
        options: {
            // Dialog options
            _defButtonText: '',     // Text of the default button. It is displayed highleighted and is pressed when user presses enter
            _enablePostBack: false,     // Whether the dialog should be allowed to postback to the same page
            // Ajax options
            url: null,      // the remote url to invoke
            cache: false,       // Whether to avoid the load if content has already been loaded
            alreadyLoaded: false,
            _useDialog: true,    // Used internally to remember whether dialog is being used
            loaded: null,        // Function to call after the remote contents have been successfully loaded
            // When a submit button on the remote form is clicked, we temporarily store its name and value here so that
            // it can be posted as form data. e.g. 'btnCreate=value'
            _clickedSubmitButton: null,
            data: {},        // Data to pass along with query string
            submitting: null,   // this event is raised just before the form is submitted. Good time to update query string.
            // return false to prevent the submit
            loading: null,   // event is raised just before the remote contents are loaded. Return false to cancel load                    
            autoOpen: true,
            autoClose: true  // If true, close the currently open dialog when this dialog opens
        },

        widgetEventPrefix: 'ajaxdialog',

        _create: function() {
            // Perform widget initialization here.
            var widget = this;
            // If cache is false, we do not need the remote contents after the dialog closes
            if (widget.options._useDialog) {
                widget.element.dialog(widget.options)
                    .bind('dialogopen', function(event, ui) {
                        widget._onDialogOpen();
                    }).bind('dialogclose', function(event) {
                        if (widget.options.autoClose) {
                            _currentlyOpenDialog = null;
                        }
                        if (!widget.options.cache) {
                            // When the dialog closes, empty the remote contents.
                            // We expect to reload them when the dialog opens again. This prevents unnecessary events from firing
                            // even though the remote contents are no longer relevant.
                            widget.remoteContainer().empty();
                        }
                        // If an ajax call is still running, abort it when dialog closes
                        if (widget._ajaxCall != null) {
                            widget._ajaxCall.abort();
                            widget._ajaxCall = null;
                            widget.element.removeClass('ui-state-disabled');
                        }
                    });
                if (widget.options.autoOpen) {
                    // We have missed the open event. Call the event handler now
                    widget._onDialogOpen();
                }
            } else {
                if (widget.options.autoOpen) {
                    widget.load();
                }
            }
        },

        // Hook up default button handling
        _onDialogOpen: function() {
            var widget = this;
            if (widget.options.autoClose) {
                if (_currentlyOpenDialog) {
                    _currentlyOpenDialog.dialog('close');
                }
                _currentlyOpenDialog = widget.element;
            }
            var $defButton = widget.element.parent()
                .find('> .ui-dialog-buttonpane :button')
                .filter(function(index) {
                    return $(this).text() == widget.options._defButtonText;
                }).addClass('ui-priority-primary');
            if ($defButton.length > 0) {
                widget.element.bind('keydown.EclipseDialog', function(e) {
                    if (e.keyCode && e.keyCode == $.ui.keyCode.ENTER && !$(e.target).is('textarea')) {
                        $defButton.click();
                    }
                }).bind('dialogclose', function(event, ui) {
                    widget.element.unbind('keydown.EclipseDialog');
                });
            }
            if (widget.options._enablePostBack) {
                widget.element.parent().appendTo($('form:first'));
                // SS 14 Dec 2009: After dialog is repositioned, we need to manage focus again
                setTimeout(function() {
                    widget.element.find(':tabbable:first').focus();
                }, 0);
            }
        },

        // A jquery object representing the div in which remote contents have been loaded
        remoteContainer: function() {
            return $('div:first', this.element);
        },

        // Loads ajax contents even if they have been already loaded. Useful when cache=true
        load: function() {
            if (this.options._useDialog) {
                $(this.element).dialog('open');
            }
            if (this.options.cache && this.options.alreadyLoaded) {
                // If caching requested and already loaded, do not load again but do trigger the loaded event
                this._trigger('loaded', null, { cached: true });
                return;
            }
            this.reload();
        },

        // Current ajax request which is in progress. Type XmlHttpRequest.
        _ajaxCall: null,

        // Usage:  $('#mydlg').ajaxDialog('load')
        // loads the remote form using the url and the data specified in options.
        reload: function() {
            var widget = this;
            if (!widget._trigger('loading', null, null)) {
                return;
            }
            if (!widget.options.url) {
                // No url, no load
                return;
            }
            var queryData = widget._queryStringData();
            // Make the ajax call
            if (widget.options._useDialog) {
                $(widget.element).dialog('open');
            }
            widget.remoteContainer().html('Loading...')
            $.ajax({
                cache: false,       // Always call the remote page. Do not ever cache its contents.
                data: queryData,    // Query string to pass
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    // Something bad happened. Display error within the div
                    widget.remoteContainer().html(XMLHttpRequest.responseText)
                },
                success: function(data, status) {
                    $('> div:last', widget.element).removeClass('ui-helper-hidden');
                    widget.options.alreadyLoaded = true;
                    widget._loadData(data);
                },
                complete: function(XMLHttpRequest, textStatus) {
                    widget._onAjaxComplete(XMLHttpRequest);
                },
                type: 'GET',
                url: this.options.url
            });
        },

        // The form has been successfully loaded. bind submit and click events so that we can
        // prevent the browser from submitting the form without our consent.
        _loadData: function(data) {
            var widget = this;
            widget.remoteContainer()
                .html(data)
                .find('form')
                .submit(function(e) {
                    widget.submit();
                    return false;   // Important. Prevent the browser from submitting it.
                }).find(':submit')
                .click(function(e) {
                    widget.options._clickedSubmitButton = $(this).attr('name') + '=' + $(this).val();
                    // TODO: should we forget this task when the form is actually submitted?
                    setTimeout(function() {
                        // If the button does not actually cause a submit, we want to forget about it
                        widget.options._clickedSubmitButton = null;
                    }, 0);
                });
            if (widget.options._useDialog) {
                $(':input:not(:button):focusable:first', widget.remoteContainer()).focus();
            }
            // loaded event is triggered after the remote contents have been loaded.
            // ui.remote is a jquery selector which refers to the div in which the remote contents
            // were loaded.
            var ui = { remote: widget.remoteContainer(), cached: false };
            widget._trigger('loaded', null, ui);
        },

        // Usage: $('#mydlg').ajaxDialog('submit', 'btnInsert').
        // You can pass the name of a button which will be posted to the form. This is useful for simulating button clicks.
        submit: function(btnSubmitName, btnSubmitValue) {
            var widget = this;
            if (widget._ajaxCall) {
                //alert('Please hold on');
                return;
            }
            //TODO: Pass a reasonable ui object
            if (!widget._trigger('submitting', null)) {
                return;
            }

            var submitButtonValue;
            if (btnSubmitName) {
                submitButtonValue = '&' + btnSubmitName + '=' + (btnSubmitValue ? btnSubmitValue : 'dummy');
            } else if (widget.options._clickedSubmitButton) {
                submitButtonValue = '&' + widget.options._clickedSubmitButton;
            } else {
                submitButtonValue = '';
            }
            widget.options._clickedSubmitButton = null;     // forget the value now. We don't need it.
            var qdata = $.param(widget._queryStringData());
            var url = widget.options.url;
            if (qdata) {
                if (url.indexOf('?') == -1) {
                    url += '?';
                } else {
                    url += '&';
                }
                url += qdata;
            }
            var $form = $('form:first', widget.remoteContainer());
            widget.element.addClass('ui-state-disabled');
            widget._ajaxCall = $.ajax({
                cache: false,
                dataType: "html",
                data: $form.serialize() + submitButtonValue, // The submit button if any is part of the data
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    // In case of error, show the yellow page within the dialog.
                    $form.html(XMLHttpRequest.responseText)
                },
                success: function(data, status) {
                    widget._loadData(data);
                },
                complete: function(XMLHttpRequest, textStatus) {
                    widget._onAjaxComplete(XMLHttpRequest);
                },
                type: 'POST',
                // Always pass the initial query string when posting. This mimics ASP.NET behavior
                url: url
            });
        },

        // Triggers the closing event if appropriate
        _onAjaxComplete: function(XMLHttpRequest) {
            this._ajaxCall = null;
            this.element.removeClass('ui-state-disabled');

            if (XMLHttpRequest.status == 205) {
                // Status code 205 means that we should close the form
                // Assume that response is serialized json. Deserialize it.
                // rverma 3 Nov 2011:if there is no responseText then we are passing null as a data.
                var obj;
                if (XMLHttpRequest.responseText) {
                    obj = JSON.parse(XMLHttpRequest.responseText);
                    ui = { data: obj };
                } else {
                    obj = null;
                }

                var ui = { data: obj };
                var b = this._trigger('closing', null, ui);

                if (b && this.options._useDialog) {
                    this.element.dialog('close');
                } else {
                    // Reload dialog
                    this.reload();
                }
            }
        },
        // Returns the query string which will be passed along with the AJAX call in object format,
        // e.g. {pickslip_id: 123, customer_id:'234'}
        _queryStringData: function() {
            var widget = this;
            var data = $.extend({}, widget.options.data);
            return data;
        }
    });
})(jQuery);
///#source 1 1 /JQuery/GridViewEx/GridViewEx.js
/*

CSS classes interpreted by the gridViewEx widget.
gvex-sort-link. Apply to a link which is expected to sort the grid. The href of the link should be the sort expression.
gvex-edit-link. Apply to a link within a row which should put the grid in edit mode.
It is also applied to the row which is in edit or insert mode so that row
menu is not displayed for that row.
gvex-delete-link: Apply to a link within a row which should delete the row.
gvex-cancel-link. Apply to a link within a row which should cancel the editing of a row.
gvex-page-link. Apply to a link within the pages which should navigate to a specific page. href is the page#
gvex-masterrow. Apply to a tr element which functions as a master row. If ui-icon-folder-open icon is clicked within
this row, the child rows are collapsed and the icon is changed to ui-icon-folder-collapsed. The converse happens
when the collapsed icon is clicked.
Depends On: json2.js

2 Nov 2009: Popup menu is not displayed when multiple rows have been selected
5 Nov 2009: Selecting/unselecting checkbox in master row works.
18 Feb 2010: submitForm is now public because RowMenuPostBack calls it.
19 Jul 2010: Row menu now works with master detail
Added public function keyIndex.
*/
(function ($) {
    // options can contain any option supported by the selectable widget.
    // If the filter option is specified, the selectable widget is created.
    $.widget('ui.gridViewEx', {
        // Our prefix should be the same as the prefix of the selectable widget to ensure that we can fake
        // raising of selectable events.
        widgetEventPrefix: 'selectable',

        // Default options of this widget. Options beginning with _ are private
        options: {
            columnNames: null,      // Name by which each column can be referenced. String array
            form: 'form:first',          // The selector of the form to submit when sort link is clicked
            uniqueId: null,      // Server unique id of the grid
            _selections: null,    // The selector to a hidden field in which selected indexes should be stored when the grid is selectable
            // dataKeys are populated in _init by parsing the value of hidden field _dataKeysInputSelector
            dataKeys: [],          // json string representation of data key array [[key11, key12], [key21, key22], ...]
            menuItemActions: null,   // Array of functions to call when menu item clicked
            dataKeysCount: 0,        // Number of data keys specified for the grid
            _dataKeysInputSelector: null,  // selector of the input control in which data keys will be posted back. Optional.
            selectable: null        // Options to pass to the selectable widget.
        },

        _create: function () {
            var widget = this;
            if (widget.options._dataKeysInputSelector) {
                widget.options.dataKeys = JSON.parse($(widget.options._dataKeysInputSelector).val());
                //$(widget.options._dataKeysInputSelector).val(JSON.stringify(widget.options.dataKeys));
            }
            widget.element.click(function (e) {
                // Handle edit/cancel/page/expand/collapse etc clicks
                return widget._onClick(e);
            });
            if (widget.options.selectable) {
                // This is a selectable grid. Manage check boxes associted with the row.
                widget.element.selectable(widget.options.selectable);
            }

            // If hidden field for selections exists, update it whenever selections stop
            if (this.options._selections) {
                widget.element.bind('selectablestop', function (event, ui) {
                    // Update the hidden field after selections have been made
                    widget._updateClientState();
                });
            }
            if (widget.options.menuItemActions) {
                widget._rowMenu = $('#' + widget.element[0].id + '_menu');
                widget._hookRowMenu();
            }
        },
        /*************** Begin Row Menu code ********************/
        // The jquery object representing the menu. Set during init.
        _rowMenu: null,

        // DOM element representing the row in which the menu should be displayed
        // Set unset during mouse movement
        _curRow: null,

        // true when the user is in the process of selecting so that we can prevent display of row menu
        _selecting: false,

        // Non null when the menu has been queued up for display
        _showMenuTimer: null,

        // Called from _init only if the grid has row menu. Hooks up all the mouse events
        _hookRowMenu: function () {
            var widget = this;
            widget._rowMenu.click(function (e) {
                // Call the function associated with the clicked item
                $(this).addClass('ui-helper-hidden');
                var itemindex = $('div', this).index(e.target);
                // If no function has been specified for the item, do nothing
                if (itemindex != -1 && widget.options.menuItemActions[itemindex] != null) {
                    // Sharad 19 Jul 2010: Fixed bug when menu displayed within master-detail row
                    //var rowIndex = $('> tbody > tr', widget.element).index(widget._curRow);
                    var rowIndex = widget.rowIndex(widget._curRow);
                    var keys = widget.options.dataKeys[rowIndex];
                    // Introducing delay here to ensure that the menu gets hidden before
                    // a potentially long running function begins work.
                    widget._selecting = true; // Prevent row selections on hover
                    setTimeout(function () {
                        widget._selecting = false;
                        widget.options.menuItemActions[itemindex].apply(widget.element[0], [keys]);
                    }, 0);
                }
            }).find('div').hover(function (e) {
                $(this).addClass('ui-selected');
            }, function (e) {
                $(this).removeClass('ui-selected');
            });

            // If the grid is selectable, prevent interference with the
            // selection process by disabling hover during selection
            if (widget.options.selectable) {
                widget.element.bind('selectablestart', function (event, ui) {
                    widget._hideRowMenu();
                    widget._selecting = true;
                }).bind('selectablestop', function (event, ui) {
                    // If multiple selections have been made, we do not turn off the _selecting flag so that
                    // the display of popup menu is prevented.
                    widget._selecting = $('> tbody > tr.ui-selected', widget.element).length > 1;
                });
            }
            $('> tbody', widget.element).bind('mousemove', function (e) {
                if (widget._selecting) {
                    // Do nothing
                    return;
                }
                var $tr = $(e.target).closest('tr');
                // No row menu displayed for disabled rows
                if ($tr.length > 0 && $tr[0] != widget._curRow) {
                    widget._hideRowMenu();
                    // Sharad 19 Jul 2010: Row menu should not be displayed for master rows
                    //if ($tr.is(':not([disabled])') && !$tr.is('.gvex-edit-link')) {
                    if ($tr.is(':not([disabled], .gvex-masterrow, .gvex-subtotal-row, :has(th), .gvex-edit-link)')) {
                        widget._curRow = $tr[0];
                        // We want the menu to show only if the user sticks around at the row
                        widget._showMenuTimer = setTimeout(function () {
                            var offset = $(widget._curRow).position();
                            // Display menu to the left of the row. by subtracting menu width from left
                            widget._rowMenu.css('left', offset.left - widget._rowMenu.outerWidth())
                                            .css('top', offset.top)
                                            .removeClass('ui-helper-hidden');
                            $(widget._curRow).addClass('ui-selecting');
                        }, 500);
                    }
                }
            }).bind('mouseenter mouseleave', function (e) {
                if ($.inArray(e.relatedTarget, widget._rowMenu.find('*').andSelf()) != -1) {
                    // The user has moved on to the menu. Leave the row selected.
                } else {
                    widget._hideRowMenu();
                }
            }).keydown(function (e) {
                // Hide the menu on ESCAPE but do not change the current row.
                // This ensures that hovering on the current row will not display the menu again.
                if (e.keyCode == $.ui.keyCode.ESCAPE) {
                    widget._rowMenu.addClass('ui-helper-hidden');
                    if (widget._showMenuTimer != null) {
                        clearTimeout(widget._showMenuTimer);
                        widget._showMenuTimer = null;
                    }
                }
            }); ;
        },

        _hideRowMenu: function () {
            // Update current row book keeping
            if (this._showMenuTimer != null) {
                clearTimeout(this._showMenuTimer);
                this._showMenuTimer = null;
            }
            if (this._curRow != null) {
                $(this._curRow).removeClass('ui-selecting');
                this._curRow = null;
            }
            if (this._rowMenu) {
                this._rowMenu.addClass('ui-helper-hidden');
            }
        },
        /*************** End Row Menu code ********************/

        /**************** Begin row selection code ***************/
        // Store the selected indexes in the hidden field using JSON
        _updateClientState: function () {
            $(this.options._selections).val(JSON.stringify(this.selectedIndexes()));
        },
        /**************** End row selection code ***************/

        // Catches all sorts clicks to implement various features
        // If the clicked element has the class gvex-edit-link, the form is submitted for editing the row clicked
        // CommandFieldEx sets this class on the edit link
        _onClick: function (e) {
            var widget = this;
            widget._hideRowMenu();
            var $tr = $(e.target).closest('tr');
            if ($(e.target).is('.gvex-sort-link')) {
                // Sort link clicked. Postback the form along with the sort expression
                $(e.target).html('Sorting...').closest('th')
                    .addClass('ui-state-disabled');
                widget.submitForm('Sort$' + $(e.target).attr('href'));
                return false;   // prevent hyperlink navigation
            } else if ($(e.target).is('.gvex-edit-link')) {
                // Edit link clicked. Postback the form along with the row index
                widget.submitForm('Edit$' + widget.rowIndex($tr));
                return false;
            } else if ($(e.target).is('.gvex-delete-link')) {
                // Delete link clicked. Postback the form along with the row index
                widget.submitForm('Delete$' + widget.rowIndex($tr));
                return false;
            } else if ($(e.target).is('.gvex-cancel-link')) {
                // Cancel link clicked. Postback the form along with the row index
                widget.submitForm('Cancel$' + widget.rowIndex($tr));
                return false;
            } else if ($(e.target).is('tr.gvex-masterrow > td span.ui-icon')) {
                // Icon in the master row clicked. Expand or collapse
                if ($(e.target).hasClass('ui-icon-folder-open')) {
                    // Collapsing
                    widget._collapseMasterRow(e.target);
                } else if ($(e.target).hasClass('ui-icon-folder-collapsed')) {
                    // Expanding
                    widget._expandMasterRow(e.target);
                } else {
                    // We must be displaying the clock icon. Do nothing.
                }
                return false;
            } else if ($(e.target).is('.gvex-page-link')) {
                widget.submitForm('PageSort$' + $(e.target).attr('href'));
                return false;
            } else if (widget.options.selectable && widget.options.selectable.cancel == '*' && $tr.is('.ui-selectee')) {
                // Implement single row selection
                widget.unselectRows(e, $('tr.ui-selected', widget.element));
                widget.selectRows(e, $tr);
                return true;
            } else {
                return true;
            }
        },

        // Selects the passed rows and raises all the necessary events
        // Public because it is called by SelectCheckBoxField.
        // The start and stop events are always raised. Selecting and selected events are raised only for
        // those rows which are not already selected.
        selectRows: function (e, $tr) {
            // raise selecting event
            var widget = this;
            widget._trigger('start', e);
            $tr.filter(function (i) {
                // Ignore already selected rows
                return !$(this).is('.ui-selected');
            }).each(function (i) {
                $(this).addClass('ui-selecting');
                widget._trigger('selecting', e, { selecting: this });
                if ($(this).hasClass('ui-selecting')) {
                    // Selection was not cancelled
                    $(this).addClass('ui-selected').removeClass('ui-selecting');
                    widget._trigger('selected', e, { selected: this });
                } else {
                    // Selection was cancelled
                }
            });
            widget._trigger('stop', e);
        },

        // unselects the passed rows. Public because it is called by SelectCheckBoxField.
        unselectRows: function (e, $tr) {
            var widget = this;
            widget._trigger('start', e);
            $tr.each(function (i) {
                $(this).addClass('ui-unselecting');
                widget._trigger('unselecting', e, { unselecting: this });
                if ($(this).hasClass('ui-unselecting')) {
                    $(this).removeClass('ui-unselecting ui-selected');
                    widget._trigger('unselected', e, { unselected: this });
                }
            });
            widget._trigger('stop', e);
        },

        /*
        A variation on the built in __doPostBack(). Takes formId as an additional parameter so that we know which
        form needs to be submitted. Used by the sorting and paging click handlers of GridViewEx.
        */
        submitForm: function (eventArgument) {
            var widget = this;
            var $form = $(widget.options.form);
            var $target = $('input:hidden[name=__EVENTTARGET]', $form);
            if ($target.length == 0) {
                // __EVENTTARGET may not always exist so we create it if necessary
                $form.append('<input type="hidden" name="__EVENTTARGET" value="' + widget.options.uniqueId + '" />');
            } else {
                $target.val(widget.options.uniqueId);
            }
            var $arg = $('input:hidden[name=__EVENTARGUMENT]', $form);
            if ($arg.length == 0) {
                $form.append('<input type="hidden" name="__EVENTARGUMENT" value="' + eventArgument + '" />');
            } else {
                $arg.val(eventArgument);
            }
            // trigger the submit events attached to this form. This gives a chance to our AJAX handler to
            // submit the form properly
            $form.submit();
        },

        /********* Begin Public functions to access rows/columns/cells ****************/

        // Returns an array of data key values. The first array is the values of the first data key and so on.
        // The length of the returned array is always equal to the number of data keys.
        // If there are no data keys, the length of the array would be 0
        // If there are no rows, the length of each inner array would be 0.
        // Example: DataKeyNames=customer_id,po_id. Suppose three rows are selected. The return value would be
        // [[c1,c2,c3] [p1,p2,p3]]
        selectedKeys: function () {
            var widget = this;
            var keyArray = [];
            // Create one empty array per key within array
            for (var i = 0; i < widget.options.dataKeysCount; ++i) {
                keyArray.push([]);
            }
            var rows = $('> tbody > tr', widget.element).each(function (i) {
                if ($(this).is('.ui-selected')) {
                    for (var j = 0; j < keyArray.length; ++j) {
                        // Push the j'th key in the j'th array within keyArray
                        keyArray[j].push(widget.options.dataKeys[i][j]);
                    }
                }
            });
            return keyArray;
        },

        // Returns an array of keys corresponding to the passed rows
        // Example: DataKeyNames=customer_id,po_id. Suppose three rows are passed. The return value would be
        // [[c1,c2,c3] [p1,p2,p3]]
        /* Code example:
        function(event, ui) {
        var x = $(this).gridViewEx('keys', $(ui.selected));
        alert(x[0][0]);
        }
        */
        keys: function (rows) {
            var widget = this;
            var keyArray = [];
            // Create one empty array per key within array
            for (var i = 0; i < widget.options.dataKeysCount; ++i) {
                keyArray.push([]);
            }
            $(rows).each(function (i) {
                var rowIndex = widget.rowIndex($(this));
                for (var j = 0; j < keyArray.length; ++j) {
                    // Push the j'th key in the j'th array within keyArray
                    keyArray[j].push(widget.options.dataKeys[rowIndex][j]);
                }
            });
            return keyArray;
        },

        // Returns a jquery object containing all tr elements which are currently selected
        // Example usage:
        // $('#gvPickSlip').gridViewEx('selectedRows').attr('disabled','true');
        selectedRows: function () {
            return $('> tbody > tr.ui-selected', this.element);
        },

        // Returns an array of selected indexes
        selectedIndexes: function () {
            var widget = this;
            var indexes = [];
            $('> tbody > tr:not(.gvex-masterrow, .gvex-subtotal-row, :has(th))', this.element).map(function (i) {
                if ($(this).is('.ui-selected')) {
                    indexes.push(i);
                }
            });
            return indexes;
        },

        // Pass a jquery object containing the tr element, or a DOM element representing tr.
        // Get back the index of the row
        rowIndex: function (tr) {
            return $('> tbody > tr', this.element).not('.gvex-masterrow, .gvex-subtotal-row, :has(th)').index(tr);
        },

        // returns a jquery object containing the td of the row in the footer
        // col can be a number representing a column index or it can be
        // col can be a string representing a column name
        // col can also be a jquery object representing a single td.
        // Example usage:
        // $('input', $grid.gridViewEx('footerCell', colIndex)).val(sum);
        footerCell: function (col) {
            var colIndex;
            if (typeof col === "number") {
                colIndex = col;
            } else {
                colIndex = this.colIndex(col);
            }
            return $('> tfoot > tr > td', this.element).eq(colIndex);
        },

        // Returns the cell in the header row. Does not work in the presence of multi row headers.
        headerCell: function (col) {
            var colIndex;
            if (typeof col === "number") {
                colIndex = col;
            } else {
                colIndex = this.colIndex(col);
            }
            return $('> thead > tr > th', this.element).eq(colIndex);
        },

        // returns a jquery object containing all td of the rows in the passed column
        // Pass column name as string, or column index as integer.
        // Example usage
        //  var $grid = $('form#frmSolidSize #gvSolidSize'); // Some code to find the grid.
        //  $('input:text', $grid.gridViewEx('columnCells', ['Pieces', 'Cartons'])).change(function(e) {
        //     alert('You have just hooked the change event to the columns Pieces and Cartons of grid');
        //                });
        // SS 26 Feb 2010: Optionally pass a jquery object containing the rows whose cells you are intrested in
        columnCells: function (col, rows) {
            var widget = this;
            var colIndexes = [];
            if (typeof col === "number") {
                colIndexes.push(col);
            } else if ($.isArray(col)) {
                // For now assume string array
                colIndexes = $.map(col, function (val, i) {
                    if (typeof val === "number") {
                        return val;
                    } else if (typeof val === "string") {
                        return widget.colIndex(val);
                    } else {
                        alert('gridViewEx error 1');
                        return null;
                    }
                });
            } else {
                colIndexes.push(widget.colIndex(col));
            }
            if (rows === undefined) {
                rows = $('> tbody > tr', widget.element);
            }
            return $('> td', rows).filter(function (i) {
                return $.inArray(i % widget.options.columnNames.length, colIndexes) != -1;
            });
        },

        // Returns a jquery object containing a single tr element corresponding to the passed keys
        // keys is an array of values to match, e.g. ['customer_id1']
        // Example usage:
        // var $row=$('#gvLocationAudit').gridViewEx('rows',keys);
        rows: function (keys) {
            // SS 3 Aug 2010: Handled master detail case
            var rowIndex = this.keyIndex(keys);
            return $('> tbody > tr:not(.gvex-masterrow, .gvex-subtotal-row, :has(th))', this.element).eq(rowIndex);

        },

        // Pass an array of key values, get back the index of the single row
        // which corresponds to these key values.
        // keys is an array of values to match, e.g. ['customer_id1']
        keyIndex: function (keys) {
            var widget = this;
            var rowIndex = -1;
            $.each(widget.options.dataKeys, function (i, val) {
                var isMatch = true;     // Presume a match
                for (var j = 0; j < keys.length; ++j) {
                    if (val[j] != keys[j]) {
                        isMatch = false;
                        break;
                    }
                }
                if (isMatch) {
                    // This is the row we are looking for
                    rowIndex = i;
                    return false;   // break the each loop
                }
                return true;    // continue loop
            });
            return rowIndex;
        },

        // Pass a string representing column name or a jquery object representing td
        // Returns integer. -1 if index not found
        // Example usage
        // var colIndex = $grid.gridViewEx('colIndex', $(this).closest('td'));
        colIndex: function (col) {
            if (typeof col === "string") {
                return $.inArray(col, this.options.columnNames);
            } else {
                // Assume col represents a jquery object representing td
                return col.closest('tr').find('> td').index(col);
            }
        },

        /********* End Public functions to access rows/columns/cells ****************/

        /************ Begin expand collapse functions ****************/
        // Utility function to expand the passed master row
        // Pass the icon whose image will also be set to expanded
        _expandMasterRow: function (icon) {
            var $icon = $(icon);
            $icon.removeClass('ui-icon-folder-collapsed')
                .addClass('ui-icon-clock');
            setTimeout(function () {
                $icon.closest('tr')
                    .nextAll('tr').show();
                $icon.removeClass('ui-icon-clock')
                    .addClass('ui-icon-folder-open');
            }, 0);
        },

        // Utility function to collapse the passed master row
        // Pass the icon whose image will also be set to collapsed
        // Sharad 1 Aug 2009: Do not collapse subheaders and subfooters
        _collapseMasterRow: function (icon) {
            $(icon).removeClass('ui-icon-folder-open')
                .addClass('ui-icon-clock');
            setTimeout(function () {
                $(icon).removeClass('ui-icon-clock')
                    .addClass('ui-icon-folder-collapsed')
                    .closest('tr')
                    .nextAll('tr')      // Sharad 7 Sep 2010: Hiding the header row as well
                    .hide();
            }, 0);
        },

        expandAll: function () {
            var widget = this;
            $(' > tbody > tr > td span.ui-icon-folder-collapsed;', widget.element).each(function () {
                widget._expandMasterRow(this);
            });
        },

        collapseAll: function () {
            var widget = this;
            $(' > tbody > tr > td span.ui-icon-folder-open', widget.element).each(function () {
                widget._collapseMasterRow(this);
            });
        }
        /************ End expand collapse functions ****************/
    });

})(jQuery);


///#source 1 1 /JQuery/Input/Cascade.js
/*
$('#ddlColor').cascade(options);

this refers to the dropdown which will be filled with the contents returned by the web method.
this can also refer to a text box. In this situation, the value in the textbox is updated with the
first value returned by the method.
    
Required options:
outermostContainer: The outermost container within which all searches will be scoped
cascadeParentSelector: The selector of the parent whose change we will monitor, e.g. '#ddlStyle'.
webServicePath: The path to the web service or page which contains the method to call
webMethodName: The name of the web method to call
Order is important. The topmost parent should be listed first.

9 Nov 2009: Supported radio button as cascade parent
16 Nov: If web method not specified, pass parent keys to load function
17 Nov 2009: If option initializeAtStartup is true, call web method on startup to refresh the element's value
*/
(function ($) {
    $.widget('ui.cascade', {

        widgetEventPrefix: 'cascade',

        // Default options of this widget. Options beginning with _ are private
        options: {
            // If this is null, then the drop down is populated once when it is first clicked by calling
            // the WebMethod without any arguments
            cascadeParentSelector: null,
            webServicePath: null,
            webMethodName: null,
            preLoadData: null,  // function to call before making the ajax call to retrieve data.
            loadData: null,  // function responsible for loading the data returned from AJAX call. this is the DOM element
            parentChangeEventName: 'change',  // The event which the parent raises when its value changes
            interestEvent: 'click',  // When there is no parent, ajax method is called when this event occurs
            parentValue: null,   // function. this refers to the parent whose value is sought
            initializeAtStartup: false,  // Whether the web method should be invoked when document loads
            hideParentsFromChildren: false  // Whether my ancestors are visible to my children
        },

        _create: function () {
            var widget = this;
            this.refresh();
            if (widget.options.initializeAtStartup) {
                // Invoke the web method on ducument load
                widget._pendingCall = setTimeout(function () {
                    widget.callWebMethod();
                    widget._pendingCall = null;
                }, 0);
            }
        },

        // Non null when a tak to callWebMethod is pending
        _pendingCall: null,

        // Does the actual work of hooking up event. You should call it from your script only if your 
        // cascade parent element is created after initialization.
        refresh: function () {
            var widget = this;
            if (widget.options.cascadeParentSelector) {
                // Bind to change event of cascade parent
                widget._cascadeParentCached = $(widget.options.cascadeParentSelector);
                widget._cascadeParentCached.bind(widget.options.parentChangeEventName, function (e) {
                    widget.callWebMethod();
                });
            }
        },

        // The worker method which actually fires the ajax call. The call is bypassed if any parent key is empty.
        // Public because AjaxDropDown calls it to fill the list on click
        callWebMethod: function () {
            var widget = this;
            if (widget._pendingCall) {
                clearTimeout(widget._pendingCall);
                widget._pendingCall = null;
            }

            var parentKeys = widget._ancestorValues();
            // If any parent key is empty, avoid the ajax call
            if ($.inArray('', parentKeys) >= 0) {
                // At least one parent key is empty. Do not call the method.
                widget._loadAjaxData(null);
                return;
            }

            if (widget.options.preLoadData) {
                widget.options.preLoadData.call(this.element[0]);
            }
            if (widget.options.webMethodName) {
                // Add the disabled class to visually indicate that we are retrieving data.
                // The class will be removed when the ajax call completes.
                widget.element.addClass('ui-state-disabled');
                $.ajax({
                    type: 'POST',
                    url: widget.options.webServicePath + '/' + widget.options.webMethodName,
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ parentKeys: parentKeys }),
                    //dataType: 'json',
                    success: function (data, textstatus) {
                        widget._loadAjaxData(data.d);
                    },
                    error: function (xhr, status, e) {
                        alert('Cascade call failed. Info: ' + xhr.responseText);
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        widget.element.removeClass('ui-state-disabled');
                    }
                });
            } else {
                // No web method specified. Pass parent keys as data
                widget._loadAjaxData(parentKeys);
            }
        },

        // Caching the parent for performance reasons. null if we have no parent
        _cascadeParentCached: null,

        // Returns key value pair representing the cascade parent and its value.
        // Parent is null if cascadeParentSelector not specified.
        // Public because we internally call it for our cascade parents.
        cascadeParent: function (ignoreHideParents) {
            if (!ignoreHideParents && this.options.hideParentsFromChildren) {
                return null;
            } else {
                return {
                    parent: this._cascadeParentCached,
                    value: this.options.parentValue == null ? null : this.options.parentValue.call(this._cascadeParentCached)
                };
            }
        },

        // Returns the values of all cascade ancestors, including self. The topmost ancestor is first in the array
        _ancestorValues: function () {
            //var k = k.k;
            var parentKeys = [];
            var curAncestor = this.cascadeParent(true);
            // curAncestor.jquery is true for jquery objects. When the parent is not cascadable, we get back some
            // jquery object (in jquery 1.4.2). We break the loop in this case.
            while (curAncestor && curAncestor.parent) {
                parentKeys.push(curAncestor.value);
                // Hemant K. Singh 25 Apr 2014: jquery ui 1.8.12 requires us to test for ui-cascade, and not for cascade
                if ($(curAncestor.parent[0]).is(":data('ui-cascade')")) {
                    //if ($.data(curAncestor.parent[0], 'cascade')) {
                    curAncestor = curAncestor.parent.cascade('cascadeParent', false);
                } else {
                    // parent is not cascadable
                    curAncestor = null;
                }
            }
            return parentKeys.reverse();
        },

        // Called after ajax data has been retrieved from the web method. Loads this data in the associated element
        _loadAjaxData: function (data) {
            var $ctl = this.element;
            // loadData is responsible for firing the change event
            this.options.loadData.call(this.element[0], data);
        }

    });


})(jQuery);                                                                                                                                                                                                            // pass the jQuery object to this function

///#source 1 1 /JQuery/Input/TextBoxEx.js
/*
Usage: $('#tb').textBoxEx(...).ajaxDialog(options);

The behavior you describe is the way IE works, i.e. onChange does not fire when AutoComplete is used to change the value.
Its unfortunate, because its obviously wrong.

In my Professional Validation And More product, I added a bunch of javascript to my textboxes to avoid this.
The general idea:
- Capture the original text on focus
- If onchange fires, abandon the captured text
- If onblur fires and the captured text is still around, compare it to the current value of the textbox.
If there is a difference, fire the onchange event.

21 Nov 2009: Setting tabindex = -1 for datepicker button

29 Jan 2010: Added watermark feature. Water mark added when textbox is initialized and whenever it loses focus.
Watermark removed whenever text box receives focus and when the form is submitted.

When watermark is being used, you cannot use the jQuery val() function to get or set the value since
the water mark text will appear to be the value. Therefore, you should use .textBoxEx('getval')
to get the value and .textBoxEx('setval', 'newval') to set the value.
*/
(function ($) {
    $.widget('ui.textBoxEx', {
        options: {
            fromSelector: null,  // Selector which selects the from date
            pickerOptions: null,     // Options to pass to date picker
            maxDateRange: null      // Used only for to dates
        },
        _create: function () {
            var oldText = null;
            var widget = this;
            widget._originalName = this.element.attr('name');
            if (!this.element.attr('readonly')) {
                $(this.element).focus(function (e) {
                    $(this).addClass('ui-selecting');
                    oldText = $(this).val();
                }).change(function (e) {
                    oldText = null;
                }).blur(function (e) {
                    $(this).removeClass('ui-selecting');
                    if (oldText != null && oldText != $(this).val()) {
                        $(this).change();
                    }
                });
            }
            if (this.options.pickerOptions) {
                if (this.options.fromSelector) {
                    this._fromElement = $(this.options.fromSelector);
                    var widget = this;
                    // Setting the beforeShow option later interferes with watermark functionality
                    $.extend(this.options.pickerOptions, { beforeShow: function (input, picker) {
                        return widget._beforeShow(input, picker);
                    }
                    });
                }
                this.element.datepicker(this.options.pickerOptions)
                    .nextAll('button.ui-datepicker-trigger:first')
                    .attr('tabindex', '-1')
                    .button({ icons: { primary: 'ui-icon-calendar' }, text: false, disabled: this.options.disabled });

            }
        },

        _originalName: null,

        getval: function () {
            return this.element.val();
        },

        setval: function (value) {
            this.element.val(value);
        },

        // If data is an array of values of cascade parent. Last value is loaded
        loadParentValues: function (data) {
            var countItems = (data == null ? 0 : data.length);
            if (countItems == 0) {
                this.element.val('');
            } else {
                this.element.val(data[countItems - 1].toString());
            }
            this.element.triggerHandler('change');
        },

        // Cached JQuery of from element
        _fromElement: null,

        // Called when the date picker is attached to a to date control
        // Sets min based on from date. Sets max based on date range. Never violates original min and max
        _beforeShow: function (input, picker) {
            var options = {};
            // Find out the date entered in the to box. Assume that the to box is the next input control
            var fromDate = this._fromElement.datepicker('getDate');
            // Find out the number of days that we are allowed to enter max.
            if (fromDate == null) {
                // Honor the minimum set for this control
                options = $.extend(options, { minDate: this.options.pickerOptions.minDate });
                //return { minDate: this.options.pickerOptions.minDays };
            } else {
                // From date has been entered. See whether it should serve as the minimum.
                var ONE_DAY = 86400000;    // Number of milliseconds in one day
                var today = new Date();
                if (this.options.pickerOptions.minDate == null) {
                    // Min days not specified so this becomes the min
                    options = $.extend(options, { minDate: fromDate });
                    //return { minDate: fromDate };
                } else {
                    // Min days has been specified. Compute the max of the two.
                    var origMinDate = new Date(today.getTime() + parseInt(this.options.pickerOptions.minDate * ONE_DAY));
                    options = $.extend(options, { minDate: origMinDate > fromDate ? origMinDate : fromDate });
                    //return { minDate: origMinDate > fromDate ? origMinDate : fromDate };
                }
                // Calculate max range constraint
                if (this.options.maxDateRange) {
                    var maxRangeDate = new Date(fromDate.getTime() + (this.options.maxDateRange * ONE_DAY));
                    var origMaxDate = new Date(today.getTime() + (this.options.pickerOptions.maxDate * ONE_DAY));
                    options = $.extend(options, { maxDate: origMaxDate < maxRangeDate ? origMaxDate : maxRangeDate });
                }
            }
            return options;
        }

    });
})(jQuery);
///#source 1 1 /JQuery/Input/RadioButtonListEx.js
// Fires the change event when radio selection changes. The change event can be cancelled by returning false
(function ($) {
    $.widget('ui.radioButtonListEx', {
        // Default options of this widget. Options beginning with _ are private
        options: {
            groupName: ''     // The name attribute of each radio in the group
        },
        _create: function () {
            // Constructor
            var widget = this;
            widget._allButtons = $('input:radio[name=\'' + widget.options.groupName + '\']', widget.element.closest('form'));
            // For radio buttons which are not directly inside the widget element,
            // Raise the change event handler of the widget element
            widget._allButtons.not(widget.element.children('input:radio')).change(function (e) {
                widget.element.triggerHandler('change');
            });
        },

        // A cached Jquery selector representing all buttons
        _allButtons: null,

//        setValue: function (value) {
//            alert('setValue superseded by radioButtonListEx("val")');
//            var $curChecked = this._allButtons.filter(':checked');
//            if ($curChecked.val() != value) {
//                this._allButtons.filter('[value=' + value + ']').attr('checked', 'checked');
//                this.element.change();
//            }
//        },


//        getValue: function () {
//            alert('getValue superseded by radioButtonListEx("val")');
//            return this._allButtons.filter(':checked').val();
//        },

        // If no value is passed, returns the currently selected value.
        // If string value is passed, sets the value to value.
        val: function (value) {
            if (value === undefined) {
                return this._allButtons.filter(':checked').val();
            } else {
                var $curChecked = this._allButtons.filter(':checked');
                if (value != this.val()) {
                    this._allButtons.filter('[value=' + value + ']').attr('checked', 'checked');
                    this.element.change();
                }
            }
        },

        // If i is not passed, returns the index of the currently selected button
        // If i is passed, selects the button at the passed index.
        // If i is negative, or greater than the index of the last button, this function does nothing.
        // Thus selecting the button after last is equivalent to selecting the first button.
        // Ignores invisible buttons while setting or getting index.
        index: function (i) {
            var $visibleButtons = this._allButtons.filter(':visible');
            if (i === undefined) {
                return $visibleButtons.index($visibleButtons.filter(':checked'));
            } else {
                if (i < $visibleButtons.length && i != this.index()) {
                    $visibleButtons.eq(i).attr('checked', 'checked');
                    if (this.element.has($visibleButtons[i]).length == 0) {
                        // Raise the change event only if the button is not within the radio button list.
                        // When the button is within the list, change event bubbles automatically.
                        this.element.change();
                    }
                }
            }
        }
    });
})(jQuery);

///#source 1 1 /JQuery/Input/buttonEx.js
/*
buttonEx class
*/
(function ($) {
    // Global variables
    $.buttonEx = {
        // JQuery. Button ex sets this global before valdiating elements. showErrors option set by JQueryScriptManager
        // displays errors within this validation summary
        validationSummary: null
    };

    // If causesValidation is true, validates all controls within the closest validation container
    // Hidden fields are not validated. Only controls within the same form as the button are validated.
    $.widget('ui.buttonEx', $.ui.button, {
        // Default options of this widget. Options beginning with _ are private
        options: {
            causesValidation: false,     // Whether validation should be performed
            click: null,           // function to call on click. If causesValidation is true, called only if validation passes
            disableAfterClick: false    // Whether button should be disabled after it is successfully clicked
        },

        _create: function () {
            var widget = this;
            // On click handler will be called only if validation succeeds
            widget.element.click(function (e) {
                var b = true;
                if (widget.options.causesValidation) {
                    b = widget.validate();
                }
                if (b && widget.options.click) {
                    b = widget.options.click.call(this, e);
                }
                if (b && widget.options.disableAfterClick) {
                    $(this).buttonEx('option', 'icons', { primary: 'ui-icon-clock' });
                    // Use timeout to allow other actions, such as form submission, to complete
                    setTimeout(function () {
                        widget.element.buttonEx('disable');
                    }, 0);
                }
                return b;
            });
            // Call base class
            $.ui.button.prototype._create.apply(this, arguments);
        },

        // Use this function to programmatically cause the validation on the client side
        // returns true if validation succeeds. Inputs which are within the same validation container as the button
        // are valdiated. Inputs in nested validation containers are excluded.
        // The validation summary must be within the same validation container as the button or it must be in one of the
        // enclosing validation containers. The form is considered to be the outermost valiadtion container.
        validate: function () {
            var $form = this.element.closest('form');
            var validator = $form.validate();
            var $containerElement = this.element.closest('.val-container,form');
            // Inputs within container elements which have a rule defined. Also, the element must be directly within the
            // button's validation container
            var $inputs = $('input,select', $containerElement)
                .not(validator.settings.ignore)
                .not('[disabled]')
                .filter(function (i) {
                    return validator.settings.rules[$(this).attr('name')] !== undefined &&
                        $(this).closest('.val-container,form')[0] == $containerElement[0];
                });
            // All parent validation containers of the button's validation container.
            // Top most parent is first in the list.
            var $parents = $containerElement.parents('.val-container,form').andSelf();
            var best = -1;
            $('ol.val-summary').addClass('ui-helper-hidden').empty().each(function (i) {
                var index = $parents.index($(this).closest('.val-container,form'));
                if (index > best) {
                    // This validation summary is so far closest to the button's container
                    $.buttonEx.validationSummary = $(this);
                }
            });
            if (!$.buttonEx.validationSummary) {
                alert('Internal error: Validation summary not found');
                return false;   // Assume validation has failed
            }
            var bValid = true;
            var namecache = [];   // Validate only one element per name. Important for radio buttons.
            $inputs.each(function (i) {
                if ($.inArray($(this).attr('name'), namecache) < 0) {
                    var b = validator.element(this);
                    if (b !== undefined && !b) {
                        bValid = false;
                    }
                    namecache.push($(this).attr('name'));
                }
            });
            return bValid;
        }
    });

})(jQuery);


///#source 1 1 /JQuery/Input/CheckBoxListEx.js
/*
Raises custom event cblitemclick when any of the checkboxes within the list are clicked. This event
is used as our InterestEvent.

cblitemclick can be raised for two reasons:
- user clicks on a check box. In this case e.originalEvent.target refers to the input control which was clicked.
- check box list was loaded via ajax. In this case e.originalEvent is undefined.

Example:
function(e) {
if (e.originalEvent === undefined) {
// Read all the checked values and do something with them
var values = $(this).checkBoxListEx('values');
// Or initialize the checkboxes
$(this).checkBoxListEx('select', 'P,U');
} else {
if ($(e.originalEvent.target).is(':checked')) {
// This value has just been selected
var newval = $(e.originalEvent.target).val();
} else {
// This value has just been unselected
var oldval = $(e.originalEvent.target).val();    
}
}
}
*/
(function($) {
    $.widget('ui.checkBoxListEx', {
        widgetEventPrefix: 'cbl',

        // Default options of this widget.
        options: {
            widthItem: '15em'       // Width of each item in the list. Used when filling from AJAX
        },

        _create: function() {
            // Constructor
            this._refresh();
        },

        // Binds click handlers to checkboxes within list.
        _refresh: function() {
            var widget = this;
            $('input:checkbox', this.element).click(function(e) {
                //widget.element.triggerHandler('itemclick');       // raise custom click
                widget._trigger('itemclick', e);
            });
        },

        // Pass an array of values to enable. Remaining values will be disabled and unchecked.
        enableValues: function(values) {
            var widget = this;
            $(':checkbox', this.element).each(function(i) {
                if ($.inArray($(this).val(), values) >= 0) {
                    $(this).removeAttr('disabled');
                } else {
                    $(this).attr('disabled', 'true').removeAttr('checked');
                }
            });
        },

        // Pass a comma seperated list of values to select.
        // We do not raise the itemclick event to avoid the possibility of recursion.
        select: function(values) {
            values = values.split(',');
            $('input:checkbox', this.element).each(function(i) {
                if ($.inArray($(this).val(), values) >= 0) {
                    $(this).attr('checked', 'checked');
                } else {
                    $(this).removeAttr('checked');
                }
            });
        },

        // Returns array of all selected values. Equivalent to server side SelectedValues
        values: function() {
            var widget = this;
            var values = [];
            $('input:checkbox:checked', this.element).each(function(i) {
                values.push($(this).val());
            });
            return values;
        }

        // data is an array of DropDownItem. Removes all checkboxes which currently exist and
        // creates new ones based on data. Called when checkbox list has a cascade parent.
//        load: function(data) {
//            var curval = this.values();
//            this.element.empty();
//            if (data == null || data.length == 0) {
//                // If no available choices, say so
//                //this.element.append($('<option></option>').val('').html('(None available)'));
//            } else {
//                for (var i = 0; i < data.length; ++i) {
//                    var id = this.element[0].id + '_' + i;
//                    var cb = $('<span><input type=\'checkbox\' value=\'' +
//                        data[i].Value + '\' id=\'' + id + '\' /><label for=\'' + id + '\' style=\'width:20em;\'>' +
//                        data[i].Text + '</label></span>');
//                    this.element.append(cb);
//                }
//            }
//            this._refresh();
//            // Must fire the change event to let everyone know that our contents have changed
//            this.element.removeClass('ui-state-disabled');
//            this._trigger('itemclick', null);
//        }
    });

})(jQuery);

///#source 1 1 /JQuery/Input/DropDownListEx.js

(function ($) {
    $.widget('ui.dropDownListEx', {
        // Default options of this widget. Options beginning with _ are private
        options: {
            // Items which will be added to the beginnng of the list before items from the web method are added
            // e.g. [ {Text='(Please Select)', Value=''}, Persitent='WhenEmpty' ]
            persistentItems: [],
            clientPopulate: false, // true means that the web method will be called on first click
            cookieExpiryDays: 7,         // Used if we are asked to set the cookie
            cookieName: null                // Used by setCookie
        },
        _create: function () {
            this.element.change(function (e) {
                // Populate hidden field
                $(this).nextAll(':input:first').val($('option[value="' + $(this).val() + '"]', this).text());
            });
            if (this.options.clientPopulate) {
                this.element.one('click.dropDownListEx', function (e) {
                    $(this).cascade('callWebMethod');
                });
            }
        },

        // Save the current value so that we can reselect it after loading
        preFill: function () {
            var curVal = this.element.val();
            this.element.empty()
                .append($('<option></option>').val(curVal).html('Loading...'))
                .addClass('ui-state-disabled');
        },

        // Data is an array of drop down items to be used for filling the combo
        // data will be null in case of an ajax error
        // TODO: Support optgroup element
        fill: function (data) {
            // Make an attempt to keep the current value selected
            var curval = this.element.val();
            this.element.empty();
            var widget = this;
            if (data && data.length > 0) {
                // Some items found. Add only AlwaysVisible persistent items
                $.each(this.options.persistentItems, function (i) {
                    if (this.Persistent == 'Always') {
                        widget.element.append($('<option></option>').val(this.Value).html(this.Text));
                    }
                });
                for (var i = 0; i < data.length; ++i) {
                    var newOption = $('<option></option>').val(data[i].Value).html(data[i].Text);
                    if (data[i].Value == curval) {
                        newOption.attr('selected', 'true');
                    }
                    this.element.append(newOption);
                }
            } else {
                // No items found. Add all persistent items
                $.each(this.options.persistentItems, function (i) {
                    widget.element.append($('<option></option>').val(this.Value).html(this.Text));
                });
            }
            this.element.removeClass('ui-state-disabled');
            // Must fire the change event to let everyone know that our contents have changed
            if (curval != this.element.val()) {
                this.element.change();
            }
            // Don't attempt to fill when control is clicked
            this.element.unbind('click.dropDownListEx');
        },

        // Called when UseCookie is true. Sets the value of the cookie. The cookie is read on server side by SetValueFromCookie
        setCookie: function () {
            var data = { Text: $('option:selected', this.element).text(), Value: this.element.val() };
            createCookie(this.options.cookieName, escape(JSON.stringify(data)), this.options.cookieExpiryDays)
        }
    });
})(jQuery);

///#source 1 1 /JQuery/Input/AutoComplete.js
(function ($) {
    // Static variable which keeps track of the number of autocomplete widgets created.
    //var __count = 0;
    $.widget("ui.autocompleteEx", $.ui.autocomplete, {

        widgetEventPrefix: 'autocomplete',

        options: {
            webServicePath: null,
            webMethodName: null,        // required
            delay: 4000,
            focus: function (event, ui) {
                // Do not change the value in the text box
                return false;
            },
            // The text for which a valid id has been saved. When the value in tb does not match this,
            // we clear the stored id
            _validText: '',

            // Set these in the search event to pass custom parameters to your web method
            // They are cleared after the ajax call is initiated.
            parameters: null,

            // If true, then the value entered is validated when focus lost. Invalid values are cleared.
            autoValidate: false,

            // Web method to invoke to validate the entered text. Defaults to webMethodName
            validateWebMethodName: null
        },

        _relevanceGood: -1000000,        // some huge negative number

        // Fires the ajax query to retrieve the list
        _fire: function (request, response) {
            var self = this;
            $.ajax({
                type: 'POST',
                url: (this.options.webServicePath || window.location.pathname) + '/' + this.options.webMethodName,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(this.options.parameters || { term: request.term }),
                //dataType: 'json',  -- jquery 1.5 does not like us to specify the data type
                success: function (data, textStatus) {
                    var items;
                    var relevancePerfect = -1000000;     // some huge negative number
                    self._relevanceGood = -1000000;
                    if (!data || !data.d) {
                        items = null;
                    } else if ($.isArray(data.d)) {
                        items = data.d;
                    } else if (data.d.RelevancePerfect || data.d.RelevanceGood) {
                        items = data.d.Items;
                        if (items && items.length > 1) {
                            // Relevance is relevant only if we have multiple items
                            if (typeof data.d.RelevancePerfect === "number") {
                                relevancePerfect = data.d.RelevancePerfect;
                            }
                            if (typeof data.d.RelevanceGood === "number" && items[0].Relevance <= data.d.RelevanceGood) {
                                // Only the best choices should be highlighted
                                self._relevanceGood = items[0].Relevance;
                            }
                        }
                    } else {
                        // Unexpected return object
                        items = null;
                    }
                    if (items == null || items.length == 0) {
                        // No match
                        response([{ Text: 'No Match', Value: ''}]);
                    } else if (items.length == 1) {
                        // If there is only a single match, auto select it regardless of relevance
                        //self.select(items[0].Value, items[0].Text);
                        response(items);
                        // trigger the select handler so that the item gets selected
                        self._trigger('select', null, { item: items[0] });
                    } else if (items[0].Relevance <= relevancePerfect) {
                        // Auto select the first perfectly relevant item but still show the menu
                        self.select(items[0].Value, items[0].Text);
                        response(items);
                    } else {
                        response(items);
                    }
                },
                error: function (xhr, status, e) {
                    response([{ Text: xhr.responseText, value: ''}]);
                }
            });
            this.options.parameters = null;
        },

        _create: function () {
            var self = this;
            this.options.source = function (request, response) {
                self._fire(request, response);
            };

            // Call base class
            $.ui.autocomplete.prototype._create.apply(this, arguments);

            this.element.dblclick(function (e) {
                // Double clicking will unconditionally open the drop down
                var oldMinLength = self.options.minLength;
                self.options.minLength = 0;
                self.search();
                self.options.minLength = oldMinLength;
            }).change(function (e) {
                // If value in text box changes, mark it as invalid
                if ($(this).is('.ac-valid') && self.options._validText != $(this).val()) {
                    self.clearItem();
                }
            });

            this.options.autoValidate && this.element.change(function (e) {
                if (!self._noAutoValidate) {
                    self.validate(function (item) {
                        item || self.element.addClass('ui-state-error').effect('pulsate', {}, 500, function () {
                            self.element.removeClass('ui-state-error').val('');
                        });
                    });
                }
            });
        },

        // While autocomplete menu is open, autovalidation should not happen
        _noAutoValidate: false,

        /* Special handle the select event. */
        _trigger: function (type, event, ui) {
            var b = $.ui.autocomplete.prototype._trigger.apply(this, arguments);
            switch (type) {
                case 'select':
                    if (b) {
                        // developer did not cancel the selection
                        this.select(ui.item.Value, ui.item.Text);
                        b = false;
                    }
                    break;

                case 'open':
                    this._noAutoValidate = true;
                    break;

                case 'close':
                    this._noAutoValidate = false;
                    break;
            }
            return b;
        },

        // Display the text property instead of the label property
        // Items with the most negative relevance are displayed in bold. It is assumed that items are sorted 
        // ascending by relevance.
        _renderItem: function (ul, item) {
            var menuText = item.Text;
            if (item.Detail) {
                menuText += ' <em>(' + item.Detail + ')</em>';
            }
            var $li = $("<li></li>")
			    .data("item.autocomplete", item)
			    .append("<a>" + menuText + "</a>")
			    .appendTo(ul);
            if (!item.Value) {
                // Indicate that this is unselectable
                $li.addClass('ui-state-error');
            } else if (item.Relevance && item.Relevance == this._relevanceGood) {
                $li.addClass('ui-priority-primary');
            }
            return $li;
        },

        /****************** Public functions *********************/
        // Utility to select an item and underline it
        // Pass empty value to clear the item
        select: function (value, text) {
            if (value) {
                this.element.val(text)
                    .addClass('ac-valid')
                    .attr('title', text)
                    .prev('input').val(value);
                this.options._validText = text;
            } else {
                this.clearItem();
            }
        },

        // Clears the value in the control. The hidden field is cleared but the text box is not.
        // You should clear the textbox seperately using $(this).autocompleteEx('clearItem').val('');
        clearItem: function () {
            this.element.removeClass('ac-valid').removeAttr('title')
                        .prev('input').val('');
        },

        // Return the currently selected value. The currently selected text is retrieved from val().
        selectedValue: function () {
            var ret = this.element.prev('input').val();
            return ret;
        },

        // Synchronously calls the web method to validate the entry in the text box.
        // The entry is passed as the id parameter to the web method.
        // To pass custom parameters, set the parameters option before calling this method
        // It updates text and value in case of success. If validation fails, text box is cleared.
        // Example: var x = $(this).autocompleteEx('validate' callback);
        // A call back function can be passed which will be called only after successful validation.
        // function MyCallBack(item) {
        //   // this is the autocomplete element
        //   // item is the AutoCompleteItem returned by the web method or null if no matching item
        //}
        // This function is chainable
        validate: function (callback) {
            this.options.validateWebMethodName || alert('Autocomplete Error: validateWebMethodName is needed');
            if (this.element.is('.ac-valid')) {
                // Already valid or empty. do nothing
                return;
            }
            var ret = this.element.val();
            if (!ret) {
                // Textbox is empty. Validate the value. This scenario
                // occurs when value is set via quaery string on the server side and this function is called
                // from ready handler.
                ret = this.selectedValue();
            }
            // Now we have text but no value
            this.element.addClass("ui-autocomplete-loading");
            var self = this;
            $.ajax({
                async: false,
                timeout: 1000,   // Since the request is synchronous, do not wait for more than a second
                type: 'POST',
                url: (this.options.webServicePath || window.location.pathname) + '/' + this.options.validateWebMethodName,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(this.options.parameters || { term: ret }),
                //dataType: 'json',  -- jquery 1.5 does not like us to specify the data type
                success: function (data, textStatus) {
                    if (data.d) {
                        if (!data.d.Value) {
                            alert('Autocomplete Error: Empty value returned for text *' + data.d.Text + '*');
                        }
                        self._trigger('select', null, { item: data.d });
                        ret = null;     // to indicate success
                    } else {
                        self.clearItem();
                    }
                    if (callback) {
                        callback.call(self.element[0], data.d);
                    }
                },
                error: function (xhr, status, e) {
                    // AJAX error
                    alert(xhr.responseText);
                },
                complete: function (XMLHttpRequest, textStatus) {
                    self.element.removeClass("ui-autocomplete-loading");
                }
            });
            this.options.parameters = null;
            //return ret;
        }
    })
})(jQuery);
///#source 1 1 /UI/WebControls/AppliedFilters.js
/*
You can determine the filters visible to this Applied Filters
var filters = $('#ctlAPpliedFilters').appliedFilters('option', 'filters');
alert(filters.vwh_id);
*/
(function($) {
    $.widget('ui.appliedFilters', {
        options: {
            filters: {}  // key value pair representing all filters
        },

        _create: function() {
            // Constructor
        }
    });

})(jQuery);
