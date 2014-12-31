Feature :- Job Payment Register


===============================
Customer Requirement :-

The recovery of Health Contribution should always be 1% of total sanction.  At present whenever the total sanction changes, we r calculating this manually n feeding. Please make this 1 % at back end itself for all Contract employees n secondment employees.  NOT FOR DEPUTATIONISTS

=======================================================


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

1. PaybillRegister (/Payroll/Reports/PaybillRegister.aspx)
2. PaySlip (/Payroll/Reports/PaySlip.aspx)
3. TdsCertificate (/Payroll/Reports/TdsCertificate.aspx)
4. Paybill1 (/Payroll/Reports/Paybill1.aspx)





