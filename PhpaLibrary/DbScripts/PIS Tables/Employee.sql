Begin Tran
Use [Lobesa5Jan]

Alter Table payroll.Employee
Alter column HomeTown nvarchar(256) 

Alter Table payroll.Employee
Drop Column PayScale

Alter Table payroll.Employee
Drop Column IncrementAmount

Alter Table payroll.Employee
Drop Column PostedAt

Alter Table payroll.Employee
Drop Column DateOfIncrement

Alter Table payroll.Employee
Drop Column Grade

Alter Table payroll.Employee
Add LeavingReason nvarchar(50) null 

Alter Table payroll.Employee
Add RelieveOrderNo nvarchar (30) null 

Alter Table payroll.Employee
Add  RelieveOrderDate datetime null

Alter Table payroll.Employee
Add  DateOfRelieve datetime null

Alter Table payroll.Employee
Add  ProbationEndDate datetime null

Alter Table payroll.Employee
Add  ACRDate datetime null

Alter Table payroll.Employee
Add OfficeId int null

ALTER TABLE [payroll].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Office] FOREIGN KEY([OfficeId])
REFERENCES [PIS].[Office] ([OfficeId])

ALTER TABLE [payroll].[Employee] CHECK CONSTRAINT [FK_Employee_Office]

Alter Table payroll.Employee
Add SubDivisionId int null

ALTER TABLE [payroll].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_SubDivision] FOREIGN KEY([SubDivisionId])
REFERENCES [PIS].[SubDivision] ([SubDivisionId])

ALTER TABLE [payroll].[Employee] CHECK CONSTRAINT [FK_Employee_SubDivision]


IF @@error > 0
ROLLBACK TRAN
ELSE
COMMIT TRAN
