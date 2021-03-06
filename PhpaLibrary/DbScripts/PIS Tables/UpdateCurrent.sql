
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [PIS].[UpdateCurrent]
ON [PIS].[ServicePeriod]
After Update
AS
--Get the range of level for this job type from the jobs table.
DECLARE 
@EmployeeIdnew int,
@iscurrentold bit,
@iscurrentnew bit,
@Designationnew nvarchar(50),
@Gradenew int,
@BasicSalarynew money,
@DateOfIncrementnew datetime,
@IncrementAmountnew money,
@PostedAtnew nvarchar(50),
@MinPayScaleAmountnew money,
@MaxPayScaleAmountnew money,
@ServicePeriodIdnew int

--selecting old values
select @iscurrentold = d.iscurrent
from deleted d

--selecting new values
select @iscurrentnew = i.iscurrent,
@EmployeeIdnew = i.EmployeeId,
@Designationnew = i.Designation,
@Gradenew = i.Grade,
@BasicSalarynew = i.BasicSalary,
@DateOfIncrementnew = i.DateOfIncrement,
@IncrementAmountnew = i.IncrementAmount,
@PostedAtnew = i.PostedAt,
@MinPayScaleAmountnew = i.MinPayScaleAmount,
@IncrementAmountnew = i.IncrementAmount, 
@MaxPayScaleAmountnew = i.MaxPayScaleAmount,
@ServicePeriodIdnew = i.ServicePeriodId 
from inserted i

--Updating when employee table when isCurrent is true
IF(@iscurrentnew = 1)
BEGIN
BEGIN TRAN

IF(@MinPayScaleAmountnew is null)
BEGIN
SELECT @MinPayScaleAmountnew = '0000'
FROM inserted i
END

IF(@MaxPayScaleAmountnew is null)
BEGIN
SELECT @MaxPayScaleAmountnew = '0000'
FROM inserted i
END

IF(@IncrementAmountnew is null)
BEGIN
SELECT @IncrementAmountnew = '0000'
FROM inserted i
END

Update ServicePeriod SET IsCurrent = 'False'
 WHERE ServicePeriodId != @ServicePeriodIdnew
 AND EmployeeId = @EmployeeIdnew 

UPDATE payroll.Employee
SET Designation = @Designationnew,
Grade = @Gradenew,
BasicSalary = @BasicSalarynew,
DateOfIncrement = @DateOfIncrementnew,
IncrementAmount = @IncrementAmountnew,
PostedAt = @PostedAtnew,
PayScale = CAST(@MinPayScaleAmountnew AS VARCHAR(10)) + '-' + 
 CAST(@IncrementAmountnew AS VARCHAR(10)) 
+ '-' + CAST(@MaxPayScaleAmountnew AS VARCHAR(10))
WHERE EmployeeId = @EmployeeIdnew
IF @@error > 0
ROLLBACK TRAN
ELSE
COMMIT TRAN
END

-- Updating denormalized columns of employee with null values
-- when IsCurrent is uncheked
ELSE IF (@iscurrentold = 1 and @iscurrentnew = 0)
BEGIN
BEGIN TRAN
 
UPDATE payroll.Employee
SET Designation = null,
Grade = null,
BasicSalary = null,
DateOfIncrement = null,
IncrementAmount = null,
PostedAt = null,
PayScale = null
WHERE EmployeeId = @EmployeeIdnew

IF @@error > 0
ROLLBACK TRAN
ELSE
COMMIT TRAN

END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

