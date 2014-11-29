/****** Object:  Table [Store].[GRN]    Script Date: 08/12/2009 11:42:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Store].[GRN](
	[GRNId] [int] IDENTITY(1,1) NOT NULL,
	[GRNCode] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[GRNReceiveDate] [datetime] NULL,
	[GRNCreateDate] [datetime] NULL,
	[TransportationMode] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ConveyenceReceiptNo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PONumber] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PODate] [datetime] NULL,
	[DeliveryChallanNumber] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DeliveryChallanDate] [datetime] NULL,
	[InvoiceNo] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[InvoiceDate] [datetime] NULL,
	[OtherReferenceNumber] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NameofCarrier] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AddressOfCarrier] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OrderPlaced] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ContractorId] [int] NULL,
	[Created] [smalldatetime] NULL,
	[CreatedBy] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Modified] [smalldatetime] NULL,
	[ModifiedBy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedWorkstation] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_GRN] PRIMARY KEY CLUSTERED 
(
	[GRNId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_GRNCode] UNIQUE NONCLUSTERED 
(
	[GRNCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
