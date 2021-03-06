USE [Lobesa5Jan]
GO
/****** Object:  Table [PIS].[SubDivision]    Script Date: 04/12/2010 16:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [PIS].[SubDivision](
	[SubDivisionId] [int] IDENTITY(1,1) NOT NULL,
	[SubDivisionName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DivisionId] [int] NOT NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NULL,
 CONSTRAINT [PK_SubDivision] PRIMARY KEY CLUSTERED 
(
	[SubDivisionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_SubDivision] UNIQUE NONCLUSTERED 
(
	[SubDivisionName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [PIS].[SubDivision]  WITH CHECK ADD  CONSTRAINT [FK_SubDivision_Division] FOREIGN KEY([DivisionId])
REFERENCES [dbo].[Division] ([DivisionId])
GO
ALTER TABLE [PIS].[SubDivision] CHECK CONSTRAINT [FK_SubDivision_Division]