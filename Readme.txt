Feature :- auto_assign_mr_number
=======================


Customer Requirement :-
===========================

Currently there is now way of assigning a MR Number in the TDS certificate. They peoples are manually enter this number to the TDS certificates in the printouts.
They need this MR number to be entered one time while creating a salary period. One salary period must have same MR Number so all employes which are being added in a salary period must have contain the same MR Number for that salary period.  

=======================================================



Impacted Files:-
====================
#TdsCertificate.aspx
Populating MR Number column from salaryperiod table.

#TdsCertificate.aspx.cs
Retreiving MRNumber

#SalaryPeriods.aspx
Added option to enter MR Number for the salary periods. 
Made MRNumber option hidden in the case when a new salaryperiod is being created. MRNumber will never get inserted it will always get updated.

#PayrollDataContext.dbml
Added new column MRNumber in the table SalaryPeriod

===================================================================

Database Modifications:-
=========================

Added a new columns MRNumber and MRNumberDate in the table Payroll.SalaryPeriod of type nvarchar length 50.

==========================================================================


Meeting Notes:-
Display MR Number in the main screen of SalaryPeriod. => Done
Explain MR Number in the UI. => Done




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