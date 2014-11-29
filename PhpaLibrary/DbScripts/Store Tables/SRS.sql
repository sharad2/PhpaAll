/****** Object:  Table [Store].[SRS]    Script Date: 08/12/2009 11:46:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Store].[SRS](
	[SRSId] [int] IDENTITY(1,1) NOT NULL,
	[SRSCode] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SRSCreateDate] [datetime] NOT NULL,
	[SRSFrom] [int] NULL,
	[SRSTo] [int] NULL,
	[IssuedTo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[VehicleNumber] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ChargeableTo] [int] NULL,
	[ApplyingOfficer] [int] NULL,
	[ReceivingOfficer] [int] NULL,
	[IntendingOfficer] [int] NULL,
	[Remarks] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IssuingOfficer] [int] NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_SRS] PRIMARY KEY CLUSTERED 
(
	[SRSId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_SRSCode] UNIQUE NONCLUSTERED 
(
	[SRSCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Store].[SRS]  WITH CHECK ADD  CONSTRAINT [FK_SRSApplyingOfficer_Employee] FOREIGN KEY([ApplyingOfficer])
REFERENCES [payroll].[Employee] ([EmployeeId])
GO
ALTER TABLE [Store].[SRS] CHECK CONSTRAINT [FK_SRSApplyingOfficer_Employee]
GO
ALTER TABLE [Store].[SRS]  WITH CHECK ADD  CONSTRAINT [FK_SRSChargeableTo_HeadOfAccount] FOREIGN KEY([ChargeableTo])
REFERENCES [dbo].[HeadOfAccount] ([HeadOfAccountId])
GO
ALTER TABLE [Store].[SRS] CHECK CONSTRAINT [FK_SRSChargeableTo_HeadOfAccount]
GO
ALTER TABLE [Store].[SRS]  WITH CHECK ADD  CONSTRAINT [FK_SRSFrom_Division] FOREIGN KEY([SRSFrom])
REFERENCES [dbo].[Division] ([DivisionId])
GO
ALTER TABLE [Store].[SRS] CHECK CONSTRAINT [FK_SRSFrom_Division]
GO
ALTER TABLE [Store].[SRS]  WITH CHECK ADD  CONSTRAINT [FK_SRSIntendingofficer_Employee] FOREIGN KEY([IntendingOfficer])
REFERENCES [payroll].[Employee] ([EmployeeId])
GO
ALTER TABLE [Store].[SRS] CHECK CONSTRAINT [FK_SRSIntendingofficer_Employee]
GO
ALTER TABLE [Store].[SRS]  WITH CHECK ADD  CONSTRAINT [FK_SRSIssuingOfficer_Employee] FOREIGN KEY([IssuingOfficer])
REFERENCES [payroll].[Employee] ([EmployeeId])
GO
ALTER TABLE [Store].[SRS] CHECK CONSTRAINT [FK_SRSIssuingOfficer_Employee]
GO
ALTER TABLE [Store].[SRS]  WITH CHECK ADD  CONSTRAINT [FK_SRSReceivingOfficer_Employee] FOREIGN KEY([ReceivingOfficer])
REFERENCES [payroll].[Employee] ([EmployeeId])
GO
ALTER TABLE [Store].[SRS] CHECK CONSTRAINT [FK_SRSReceivingOfficer_Employee]
GO
ALTER TABLE [Store].[SRS]  WITH CHECK ADD  CONSTRAINT [FK_SRSTo_Division] FOREIGN KEY([SRSTo])
REFERENCES [dbo].[Division] ([DivisionId])
GO
ALTER TABLE [Store].[SRS] CHECK CONSTRAINT [FK_SRSTo_Division]