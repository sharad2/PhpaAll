Feature :- Bank Freeze


===============================
Customer Requirement :-

The salary period already generated should be freezed. At present, when we change the bank account of an employee from BoB to DPNB and vice versa, data changes even for the salary period already generated. This is wrong. It should not change for earlier periods. It disturbs all our records.
=======================================================


===============================
Impacted Files:-
We are now keeping BankId, Designation and BankAccountNo in table EmployeePeriod so in place of seeing table employee we are now considering table EmployeePeriod for this.


Finance/Payroll/ManageEmployeePeriod.aspx                   	
Finance/Payroll/ManageEmployeePeriod.aspx.cs	
Finance/Payroll/Reports/AdjustmentRecovery.aspx.cs	
Finance/Payroll/Reports/GIS.aspx.cs	
Finance/Payroll/Reports/MiscRMT.aspx.cs	
Finance/Payroll/Reports/PaySlip.aspx.cs	
Finance/Payroll/Reports/Paybill1.aspx.cs	
Finance/Payroll/Reports/PaybillRegister.aspx.cs	
Finance/Payroll/Reports/RMTDReport.aspx.cs	
Finance/Payroll/Reports/RecoverySchedule.aspx.cs	
Finance/Payroll/Reports/SSS.aspx.cs	
Finance/Payroll/Reports/STHC.aspx.cs		
PhpaLibrary/Database/PayrollDataContext.cs                   	
PhpaLibrary/Database/PayrollDataContext.dbml	
PhpaLibrary/Database/PayrollDataContext.dbml.layout	
PhpaLibrary/Database/PayrollDataContext.designer.cs	

===================================================================

Database Modifications:-

Addded following three columns in the table EmployeePeriod  :-
1. BankId
2. BankAccountNo
3. Designation


Applied a new foreign key "Bank_EmployeePeriod"  on table EmployeePeriod referring to column Bank.BankId  