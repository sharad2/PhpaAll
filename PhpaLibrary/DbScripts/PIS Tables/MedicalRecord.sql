Begin Tran
Use Lobesa5Jan

Alter Table PIS.MedicalRecord
Add CashMemoNo nvarchar(30) null

Alter Table PIS.MedicalRecord
Alter column HospitalAddress nvarchar(256) 

IF @@error > 0
ROLLBACK TRAN
ELSE
COMMIT TRAN
