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
Feature :- Bank Freeze
=======================


Customer Requirement :-
===========================

The salary period already generated should be freezed. At present, when we change the bank account of an employee from BoB to DPNB and vice versa, data changes even for the salary period already generated. This is wrong. It should not change for earlier periods. It disturbs all our records.

=======================================================



Impacted Files:-
====================
We are now keeping BankId, Designation and BankAccountNo in table EmployeePeriod so in place of seeing table employee we are now considering table EmployeePeriod for this.


# Finance/Payroll/ManageEmployeePeriod.aspx.                    	
# Finance/Payroll/ManageEmployeePeriod.aspx.cs
Displaying Designation and Account Number from table employee period in place of employee. 

Stuck with showing bank name in the same way.

	
# Finance/Payroll/Reports/AdjustmentRecovery.aspx.cs	
Displays loan recoveries made from employee salaries. It provides two filters for the bank and account number from which recoveries should be listed. These filters are now applies to the bank and account in employee period.
Displaying Designation and Account Number from table employee period in place of employee.


# Finance/Payroll/Reports/PaySlip.aspx.cs	
It provides a filter for the bank from which salaries are paid. This filter now applies to the bank in employee period.
Displaying Bank name, Account Number and Designation from table employee period in place of table employee.

# Finance/Payroll/Reports/GIS.aspx.cs	
# Finance/Payroll/Reports/MiscRMT.aspx.cs	
# Finance/Payroll/Reports/Paybill1.aspx.cs	
It provides a filter for the bank from which bills are paid. This filter now applies to the bank in employee period.
Displaying Designation from table employee period in place of employee

# Finance/Payroll/Reports/PaybillRegister.aspx.cs
Displaying BankName, Designation and account number from table employee period in place of employee

# Finance\Payroll\Reports\GPF.aspx.cs
It provides a filter for the bank from which GPF are paid. This filter now applies to the bank in employee period.
Displaying Designation from table employee period in place of employee

# Finance/Payroll/Reports/RMTDReport.aspx.cs	
# Finance/Payroll/Reports/RecoverySchedule.aspx.cs	
# Finance/Payroll/Reports/SSS.aspx.cs	
# Finance/Payroll/Reports/STHC.aspx.cs.
Displays recoveries made from employee salaries. It provides a filter for the bank from which recoveries should be listed. This filter now applies to the bank in employee period. The designation listed on report now comes from employee period.

# PhpaLibrary/Database/PayrollDataContext.cs
When a new employee period is created, copy bank information from employee table.   
             	
# PhpaLibrary/Database/PayrollDataContext.dbml
Added columns bank id, designation and bank account number in employee period table.	

===================================================================

Database Modifications:-
=========================

Addded following three columns in the table EmployeePeriod  :-
1. BankId
2. BankAccountNo
3. Designation


Applied a new foreign key "Bank_EmployeePeriod"  on table EmployeePeriod referring to column Bank.BankId  



==========================================================================

This change has been demonstrated to the end users and they have given their approval.

As we are now displaying BankName, Designation and account number from table employee in the case where these information are not avaliable in the table employeeperiod. This is creating confusions to end users.




