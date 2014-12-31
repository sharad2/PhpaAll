Feature :- Reset Password
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




