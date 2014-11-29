/****** Object:  Table [Store].[SRSItems]    Script Date: 08/12/2009 11:48:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Store].[SRSItems](
	[SRSItemId] [int] IDENTITY(1,1) NOT NULL,
	[SRSId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[HeadOfAccountId] [int] NOT NULL,
	[QtyRequired] [int] NULL,
	[Remarks] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_SRSItems] PRIMARY KEY CLUSTERED 
(
	[SRSItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Store].[SRSItems]  WITH CHECK ADD  CONSTRAINT [FK_SRSItems_HeadOfAccount] FOREIGN KEY([HeadOfAccountId])
REFERENCES [dbo].[HeadOfAccount] ([HeadOfAccountId])
GO
ALTER TABLE [Store].[SRSItems] CHECK CONSTRAINT [FK_SRSItems_HeadOfAccount]
GO
ALTER TABLE [Store].[SRSItems]  WITH CHECK ADD  CONSTRAINT [FK_SRSItems_Item] FOREIGN KEY([ItemId])
REFERENCES [Store].[Item] ([ItemId])
GO
ALTER TABLE [Store].[SRSItems] CHECK CONSTRAINT [FK_SRSItems_Item]
GO
ALTER TABLE [Store].[SRSItems]  WITH CHECK ADD  CONSTRAINT [FK_SRSItems_SRS] FOREIGN KEY([SRSId])
REFERENCES [Store].[SRS] ([SRSId])
GO
ALTER TABLE [Store].[SRSItems] CHECK CONSTRAINT [FK_SRSItems_SRS]