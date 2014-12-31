Feature :- Job Payment Register


===============================
Customer Requirement :-

While feeding vouchers with Job code especially for Job Codes  44/45/and 60 special provisions are required. For the recoveries made under the Heads from 100 to 400 (except 200.03 and 200.04), all other recoveries is to be shown separately in column “Others”. At present, it is getting reduced from column 3 in job payment report. This is to be corrected. Only 200.03 and 200.04 should be netted and not others Heads.

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

This feature has been approved by the end user. We are ready to merge this change in the master.




