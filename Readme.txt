Feature :- Job Payment Register


===============================
Customer Requirement :-

While feeding vouchers with Job code especially for Job Codes  44/45/and 60 special provisions are required. For the recoveries made under the Heads from 100 to 400 (except 200.03 and 200.04), all other recoveries is to be shown separately in column “Others”. At present, it is getting reduced from column 3 in job payment report. This is to be corrected. Only 200.03 and 200.04 should be netted and not others Heads.

=======================================================


===============================
Impacted Files:-

In case of job code 44 45 and 60 we have to calculate recoveries seperately. Currently we have achieved this by hard wiring job codes. We are now getting rid of these hard wirings.

WIP

Finance/Reports/ContractorPayment.aspx.cs	



===================================================================

Database Modifications:-

NA