Begin Tran
Use Lobesa5Jan

Alter Table PIS.ServicePeriod
Drop column DateOfRelieve

Alter Table PIS.ServicePeriod
Alter Column Grade int

Alter Table PIS.ServicePeriod
Drop column LeavingReason

Alter Table PIS.ServicePeriod
Drop column ChangeReason

Alter Table PIS.ServicePeriod
Drop column IsCurrent

Alter Table PIS.ServicePeriod
Add  DateOfNextIncrement datetime null

Alter Table PIS.ServicePeriod
Add  NextPromotionDate datetime null

IF @@error > 0
ROLLBACK TRAN
ELSE
COMMIT TRAN