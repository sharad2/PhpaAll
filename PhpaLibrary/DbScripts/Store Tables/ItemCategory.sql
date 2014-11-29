/****** Object:  Table [Store].[ItemCategory]    Script Date: 08/12/2009 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Store].[ItemCategory](
	[ItemCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[ItemCategoryCode] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NULL,
 CONSTRAINT [PK_ItemCategory] PRIMARY KEY CLUSTERED 
(
	[ItemCategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_ItemCategoryCode] UNIQUE NONCLUSTERED 
(
	[ItemCategoryCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [Store].[ItemCategory]    Script Date: 08/12/2009 1:03 PM ******/

Alter table Store.ItemCategory
Alter Column [Description] nvarchar(150) null;
