Feature :- Job Payment Register


===============================
Customer Requirement :-

The recovery of Health Contribution should always be 1% of total sanction.  At present whenever the total sanction changes, we r calculating this manually n feeding. Please make this 1 % at back end itself for all Contract employees n secondment employees.  NOT FOR DEPUTATIONISTS

=======================================================


===============================
Impacted Files:-
We have implemented this feature in the same way as the feature currently avaliable for % Basic

Finance/Controls/EmployeeAdjustmentEditor.ascx	
Finance/Controls/EmployeeAdjustmentEditor.ascx.cs	
Finance/Payroll/Adjustment.aspx	
Finance/Payroll/Adjustment.aspx.cs	
Finance/Payroll/EmployeeAdjustments.aspx.cs	
PhpaLibrary/Database/PayrollDataContext.cs	
PhpaLibrary/Database/PayrollDataContext.designer.cs	

===================================================================

Database Modifications:-

We have added following new columns :-

A. Table (Adjustment) 

	1. FractionOfGross  (Float)

B. Table (EmployeeAdjustment)

        1. IsFractionGrossOverridden   (bit)
        2. FractionOfGross             (float)
