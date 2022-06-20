SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[kullanici](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ad] [varchar](50) NULL,
	[soyad] [varchar](50) NULL,
	[eposta] [varchar](250) NULL,
	[sifre] [varchar](50) NULL,
	[kayittipi] [smallint] NULL,
	[tarih] [datetime] NULL,
	[kod] [varchar](50) NULL,
	[durum] [smallint] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[kullanici] ADD  CONSTRAINT [DF_kullanici_tarih]  DEFAULT (getdate()) FOR [tarih]
GO

ALTER TABLE [dbo].[kullanici] ADD  CONSTRAINT [DF_kullanici_kod]  DEFAULT (newid()) FOR [kod]
GO

ALTER TABLE [dbo].[kullanici] ADD  CONSTRAINT [DF_kullanici_durum]  DEFAULT ((1)) FOR [durum]
GO

