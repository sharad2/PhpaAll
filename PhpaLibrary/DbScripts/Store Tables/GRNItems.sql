/****** Object:  Table [Store].[GRNItems]    Script Date: 08/12/2009 11:44:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Store].[GRNItems](
	[GRNItemId] [int] IDENTITY(1,1) NOT NULL,
	[GRNId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[OrderedQty] [int] NULL,
	[AcceptedQty] [int] NULL,
	[ReceivedQty] [int] NULL,
	[Price] [money] NULL,
	[LandedPrice] [money] NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_GRNItem] PRIMARY KEY CLUSTERED 
(
	[GRNItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Store].[GRNItems]  WITH CHECK ADD  CONSTRAINT [FK_GRNItems_GRN] FOREIGN KEY([GRNId])
REFERENCES [Store].[GRN] ([GRNId])
GO
ALTER TABLE [Store].[GRNItems] CHECK CONSTRAINT [FK_GRNItems_GRN]
GO
ALTER TABLE [Store].[GRNItems]  WITH CHECK ADD  CONSTRAINT [FK_GRNItems_Item] FOREIGN KEY([ItemId])
REFERENCES [Store].[Item] ([ItemId])
GO
ALTER TABLE [Store].[GRNItems] CHECK CONSTRAINT [FK_GRNItems_Item]