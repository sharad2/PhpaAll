USE [Lobesa5Jan]
GO
/****** Object:  Table [PIS].[Office]    Script Date: 04/12/2010 16:55:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [PIS].[Office](
	[OfficeId] [int] IDENTITY(1,1) NOT NULL,
	[OfficeName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SubDivisionId] [int] NOT NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NULL,
 CONSTRAINT [PK_Office] PRIMARY KEY CLUSTERED 
(
	[OfficeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_OfficeName] UNIQUE NONCLUSTERED 
(
	[OfficeName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [PIS].[Office]  WITH CHECK ADD  CONSTRAINT [FK_Office_SubDivision] FOREIGN KEY([SubDivisionId])
REFERENCES [PIS].[SubDivision] ([SubDivisionId])
GO
ALTER TABLE [PIS].[Office] CHECK CONSTRAINT [FK_Office_SubDivision]