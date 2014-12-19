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