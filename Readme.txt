Sharad 11 Feb 2015:
---------------------
Field order in create should be similar to field order in Edit. Edit field ordering is good.


Sharad 8 Feb 2015
-----------------
TODO: Remove amount filter from recent pages

Bill Module 6 Feb 2015
-------------------------------
A bill is for a division/station. Hereinafter whenever we say division, we mean division/station.

 It is either raised by a contractor, or it is raised internally.  The payment made by finance will be on behalf of this division.
Then the bill is routed to other divisions until it finally reaches the finance division. For each bill we capture the division it currently is in. This division is called the
ActionDivision implying that this division is responsible for acting on the division.


When a bill is received, the action division is the same as the bill division. The UI provides the ability to change the action division of a bill.
 In real life, the people of the responsible division can 
be asked to change the action division at some point (push). Or the people in the finance division can choose to change the action divison to finance when they desire (pull).
The software itself does not care whether the push or the pull method is used.

A bill needs to be approved before it can be paid. A bill can be approved at any time after it is received. Only managers can approve bills. A bill cannot be edited after it has been approved.
A bill can be unapproved at any time as long as it has not already been paid.

A voucher can be created against an approved bill. Now the bill is deemed to have been paid. After a bill is paid, it is completely frozen and no changes can be made to it.


Security: Visitor rights on the Bills package will allow the user readonly access to all sreens of the bills module except reports.
BillsExecutive role is required to view the bill reports.
BillsOperator role is required to create, edit and delete bills.
BillsManager role is needed to approve bills.


Changes
Rename SubmittedToDivision to Division
Rename CurrentDivion to ProcessingDivision

Update create screen

SB - Create edit, view nomenclature and layout
HKV - Outstanding bills
Sanjeev - feedback
MB - recent bill. Make collapsible, div and station adjacent. Division nomenclature. Unapprove on recent page
Anil - Image.  Create voucher link on view page. Audit in view
Dinesh - Security
DB - Search
SS - Voucher entry, login links, logo on layout


TODO:
Approve/Unapprove on view page
Voucher date entry modifications for bill
Fund requirement on home page







Feature :- Job Payment Register


===============================
Customer Requirement :-

While feeding vouchers with Job code especially for Job Codes  44/45/and 60 special provisions are required. For the recoveries made under the Heads from 100 to 400 (except 200.03 and 200.04), all other recoveries is to be shown separately in column �Others�. At present, it is getting reduced from column 3 in job payment report. This is to be corrected. Only 200.03 and 200.04 should be netted and not others Heads.

=======================================================


===============================
Impacted Files:-

Finance/Reports/ContractorPayment.aspx.cs	

If JobType of the passed Job is MainCivil then

1. While calculating Amount column, considering only those recoveries of those heads where RecoveryType is set to "NoWork."
2. While calculating Others Recovery column, Adding all recoveries of Expenditure Heads except where RecoveryType is "NoWork"





===================================================================

Database Modifications:-

1. Added 2 new tables
  a. RecoveryType : Stores possible values for RecoveryType
  b. JobType : Stores possible values for JobType

2. Added 2 new columns :
  a. RecoveryType in HeadOfAccount table : will store the Recovery type of each Head of Account
  b. Jobtype in Job table : will store th type of job

3. Added RecoveryType column in HeadHierarchy view.


============================================================

This feature has been approved by the end user. We have to give feature of populating new job type and head of account type in the 
interfaces where we are creating a new job or new head.




Following Features have been incorporated in the master:-
1.  1% Health Contribution 
2.  Reset Password
3.  Assign M.R. Number and Date
4.  Bank Freeze
5.  Reset end date
6.  display_bank_name


#1 Feature :- auto_assign_mr_number
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


#################################################################################################

#2 Feature :- Reset_End_date
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

################################################################################################


# 3 Feature :- Bank Freeze
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

#########################################################################################


# 4 Feature :- Health Contribution 
==================================

Customer Requirement :-

The recovery of Health Contribution should always be 1% of total sanction.  At present whenever the total sanction changes, we are calculating this manually and feeding. Please make this 1 % at back end itself for all Contract employees n secondment employees.  NOT FOR DEPUTATIONISTS

=======================================================

Proposed Solution:-

While creating an adjustment you can specify whether is % of gross. This is in addition to current possible values flat amount and % of basic.
You will still be able to override percentages for specific employees.
All reports which show adjustments will show this new type of adjustment properly.


===============================
Impacted Files:-
We have implemented this feature in the same way as the feature currently avaliable for % Basic

# Finance/Payroll/Adjustment.aspx	
In the create new adjustment process we have now introduced the option of configuring % of Gross. 
# Finance/Payroll/Adjustment.aspx.cs	
A new column FractionOfGross is being added in the table Adjustment. Configured value will get store in this column.

# Finance/Controls/EmployeeAdjustmentEditor.ascx
Addded a new column % Gross in the EmploymentAdjustment Editor dialog.
Provided option to override the set default for % Gross. A checkbox Fraction Of Gross is given for this. Added a jQuery function
cbFractionGrossOverrriden_Click which is for making Fractoin Of Gross check box read only or selectable.
Added new columns IsFractionGrossOverridden and FractionOfGross in the table EmployeeAdjustment. Populating and updating these new columns. 

# Finance/Controls/EmployeeAdjustmentEditor.ascx.cs	
Added function tbFractionOfGross_DataBinding for data binding in the column FractionOfGross

# Finance/Payroll/EmployeeAdjustments.aspx.cs
Changed formula for adding the set % of Gross with the other deductions. In the same way formula has been changed for adding % of 


#PhpaLibrary/Database/PayrollDataContext.cs	
Populating columns FractionOfGros and FractionGrossOverridden


===================================================================

Database Modifications:-

We have added following new columns :-

A. Table (Adjustment) 

	1. FractionOfGross  (Float)

B. Table (EmployeeAdjustment)

        1. IsFractionGrossOverridden   (bit)
        2. FractionOfGross             (float)



====================================================================

Identified changes :-

We have noticed that we will have to change some more pages for incorporating % of Gross feature.
Following are the pages:-

1. SalaryRemittancesToBank (/PHPA2/Payroll/SalaryPeriods.aspx)
   In the "PayrollDataContext.cs" we need to correct the fucntion AddDefaultAdjustments and add gross rule here so that it can be inserted in the  table payroll.PeriodEmployeeAdjustment
   in column Amount.    => Done
2. PaybillRegister (/Payroll/Reports/PaybillRegister.aspx) => Done
3. PaySlip (/Payroll/Reports/PaySlip.aspx) => Done
4. TdsCertificate (/Payroll/Reports/TdsCertificate.aspx) => Done
5. Paybill1 (/Payroll/Reports/Paybill1.aspx) => Done
6. % of gross should be configurable for Deductions. In case of Allounces this option should not come.  Adjustment screen and Emp Adjustment screens should be impacted.
   ISSUES =>
   A. While creating an adjustment if user toggles between Deduction and Adjustment then we are not able to hide the % Gross option.
   B. In EmployeeAdustment page when user is trying to add a new adjustment at that time we are not able to hide the Override % Gross. This is 
   because we do not able to know whether the selected adjustment type is Allounce or Deduction.

   In rest scenarios we are able to fix this. We have fixed this issue through .aspx file in the following files 
   # Finance/Payroll/Adjustment.aspx
   # Finance/Controls/EmployeeAdjustmentEditor.ascx
    

####################################################################################################


#5 Feature :- Reset Password
=======================


Customer Requirement :-
===========================

Every user must be able to change his/her password.

=======================================================



Impacted Files:-
====================
We are now keeping BankId, Designation and BankAccountNo in table EmployeePeriod so in place of seeing table employee we are now considering table EmployeePeriod for this.


# /Login.aspx
We have given an interface of changing password in this page. Following controls are given in reagard to this:-

User Name
Current Password
New Password
Confirm New Password  

A button named as Save Password is also given in the same page.                 	

# /finance/login.aspx.cs
Introduced a protected method btnSavePassword_Click which handles the click event of button Save Password. 
This method is validating the current password, new password with its confirmation and on succesfull validation this method updates the new password in the table PhpaUser.

===================================================================

Database Modifications:-
=========================

NA

==========================================================================




Feature :- display_bank_name
=======================


Customer Requirement :-
===========================

Currently when user ran the report Loan Recovery for a particular head of account at that time there is no provision to print the loan bank name in the printouts. They are entering the loan bank name manually. So they have asked us to add the loan bank name in the report header comment so that it would get printed automatically with the report output.
=======================================================



Impacted Files:-
====================
AdjustmentRecovery.aspx
Showing Loan bank name in the report heading 
===================================================================

Database Modifications:-
=========================
NA

==========================================================================



Year Closing:-

Database changes:-

Added new table dbo.FinancialYear

Added unique constraint on column Name

USE [PHPA2151114]
GO

ALTER TABLE dbo.FinancialYear ADD  CONSTRAINT [UK_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


