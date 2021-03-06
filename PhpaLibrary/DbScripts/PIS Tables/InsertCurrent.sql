set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER TRIGGER [PIS].[InsertCurrent]
ON [PIS].[ServicePeriod]
After INSERT
AS
--Get the range of level for this job type from the jobs table.
DECLARE 
@ServicePeriodId int,
@EmployeeId int,
@IsCurrent bit,
@Designation nvarchar(50),
@Grade int,
@BasicSalary money,
@DateOfIncrement datetime,
@IncrementAmount money,
@PostedAt nvarchar(50),
@MinPayScaleAmount money,
@MaxPayScaleAmount money
SELECT 
@ServicePeriodId = i.ServicePeriodId,
@EmployeeId = i.EmployeeId,
@IsCurrent = i.IsCurrent
FROM ServicePeriod sp, inserted i
WHERE sp.ServicePeriodId = i.ServicePeriodId 
AND sp.EmployeeId = i.EmployeeId

-- Update employee when IsCurrent is true, and set the IsCurrent
-- of other ServicePeriods to false
IF(@IsCurrent = 'True')
BEGIN
BEGIN TRAN
 Update ServicePeriod SET IsCurrent = 'False'
 WHERE ServicePeriodId != @ServicePeriodId
 AND EmployeeId = @EmployeeId

SELECT @Designation = i.Designation,
@Grade = i.Grade,
@BasicSalary = i.BasicSalary,
@DateOfIncrement = i.DateOfIncrement,
@IncrementAmount = i.IncrementAmount,
@PostedAt = i.PostedAt,
@MinPayScaleAmount = i.MinPayScaleAmount,
@MaxPayScaleAmount = i.MaxPayScaleAmount
FROM ServicePeriod sp, inserted i
WHERE sp.ServicePeriodId = i.ServicePeriodId
AND sp.EmployeeId = i.EmployeeId

IF(@MinPayScaleAmount is null)
BEGIN
SELECT @MinPayScaleAmount = '0000' 
FROM ServicePeriod sp, inserted i
WHERE sp.ServicePeriodId = i.ServicePeriodId
AND sp.EmployeeId = i.EmployeeId
END

IF(@MaxPayScaleAmount is null)
BEGIN
SELECT @MaxPayScaleAmount = '0000' 
FROM ServicePeriod sp, inserted i
WHERE sp.ServicePeriodId = i.ServicePeriodId
AND sp.EmployeeId = i.EmployeeId
END

IF(@IncrementAmount is null)
BEGIN
SELECT @IncrementAmount = '0000' 
FROM ServicePeriod sp, inserted i
WHERE sp.ServicePeriodId = i.ServicePeriodId
AND sp.EmployeeId = i.EmployeeId
END

UPDATE payroll.Employee
SET Designation = @Designation,
Grade = @Grade,
BasicSalary = @BasicSalary,
DateOfIncrement =@DateOfIncrement,
IncrementAmount = @IncrementAmount,
PostedAt = @PostedAt,
PayScale = CAST(@MinPayScaleAmount AS VARCHAR(10)) + '-' + 
 CAST(@IncrementAmount AS VARCHAR(10)) 
+ '-' + CAST(@MaxPayScaleAmount AS VARCHAR(10))
WHERE EmployeeId =@EmployeeId

IF @@error > 0
ROLLBACK TRAN
ELSE
COMMIT TRAN
END

