Following Features have been incorporated in the master:-
1.  1% Health Contribution 
2.  Reset Password
3.  Assign M.R. Number and Date
4.  Bank Freeze
5.  Reset end date


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

===================================================================

Database Modifications:-
=========================


==========================================================================




