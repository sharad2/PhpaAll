/****** Object:  Table [Store].[SRSIssueItems]    Script Date: 08/12/2009 11:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Store].[SRSIssueItems](
	[SRSIssueItemId] [int] IDENTITY(1,1) NOT NULL,
	[GRNItemId] [int] NULL,
	[SRSItemId] [int] NOT NULL,
	[QtyIssued] [int] NULL,
	[IssueDate] [datetime] NULL,
	[Remarks] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NULL,
 CONSTRAINT [PK_SRSIssues] PRIMARY KEY CLUSTERED 
(
	[SRSIssueItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Store].[SRSIssueItems]  WITH CHECK ADD  CONSTRAINT [FK_SRSIssues_GRNItems] FOREIGN KEY([GRNItemId])
REFERENCES [Store].[GRNItems] ([GRNItemId])
GO
ALTER TABLE [Store].[SRSIssueItems] CHECK CONSTRAINT [FK_SRSIssues_GRNItems]
GO
ALTER TABLE [Store].[SRSIssueItems]  WITH CHECK ADD  CONSTRAINT [FK_SRSIssues_SRSItem] FOREIGN KEY([SRSItemId])
REFERENCES [Store].[SRSItems] ([SRSItemId])
GO
ALTER TABLE [Store].[SRSIssueItems] CHECK CONSTRAINT [FK_SRSIssues_SRSItem]