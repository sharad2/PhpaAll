Feature :- Reset_End_date


===============================
Customer Requirement :-

In Receipt n payments report and Fund position report, during the month, previous day report is not displaying correctly. It always shows updated figures. This needs o be corrected and should display as it is being displayed for Balance sheet and Trial Balance.
=======================================================


===============================
Impacted Files:-
In place of calcuating month end of the passed date and passing it further we are now pasing the chosen date.

Finance/Reports/FundPositionReport.aspx.cs	
Finance/Reports/ReceiptandPayment.aspx	
Finance/Reports/ReceiptandPayment.aspx.cs	

There is a single date filter in these reports which contols transactions being displayed in the report. 
In place of calculating month end date of the selected date in this filter we are directly passing the selected date for retreiving the report.

===================================================================

Database Modifications:-

NA


====================================================================
This feature has been tested and approved by the end user. Currently we have given this feature in the PHPA test server so that users can regress test this change.

Feature is Done and ready to merge in the master.