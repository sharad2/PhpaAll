/****** Object:  Table [Store].[Item]    Script Date: 08/12/2009 11:45:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Store].[Item](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[ItemCode] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ItemCategoryId] [int] NULL,
	[Description] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UOMId] [int] NULL,
	[HeadOfAccountId] [int] NOT NULL,
	[ReorderingLevel] [int] NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_ItemCode] UNIQUE NONCLUSTERED 
(
	[ItemCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Store].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_HeadOfAccount] FOREIGN KEY([HeadOfAccountId])
REFERENCES [dbo].[HeadOfAccount] ([HeadOfAccountId])
GO
ALTER TABLE [Store].[Item] CHECK CONSTRAINT [FK_Item_HeadOfAccount]
GO
ALTER TABLE [Store].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemCategory] FOREIGN KEY([ItemCategoryId])
REFERENCES [Store].[ItemCategory] ([ItemCategoryId])
GO
ALTER TABLE [Store].[Item] CHECK CONSTRAINT [FK_Item_ItemCategory]
GO
ALTER TABLE [Store].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_UOM] FOREIGN KEY([UOMId])
REFERENCES [Store].[UOM] ([UOMId])
GO
ALTER TABLE [Store].[Item] CHECK CONSTRAINT [FK_Item_UOM]



/****** Object:  Table [Store].[Item]    Script Date: 08/12/2009 12:15PM 
New Column added ******/

Alter table Store.Item
Add [Brand] nvarchar(50) null;

Alter table Store.Item
Add [Color] nvarchar(50) null;

Alter table Store.Item
Add [Dimension] nvarchar(50) null;

Alter table Store.Item
Add [Identifier] nvarchar(50) null;

Alter table Store.Item
Add [Size] nvarchar(50) null;

Alter table Store.Item
Alter Column [HeadOfAccountId] int null;

Alter table Store.Item
Alter Column [Description] nvarchar(150) null;