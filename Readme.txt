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

Added a new column MRNumber in the table Payroll.SalaryPeriod of type nvarchar length 50.

==========================================================================




