if not exists(select * from sys.databases where name='Logs')
         create database Logs;
Go
Use Logs;
Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Logs](
         [Id] [int] IDENTITY(1,1) NOT NULL,
         [Message] [nvarchar](max) NULL,
         [MessageTemplate] [nvarchar](max) NULL,
         [Level] [nvarchar](128) NULL,
         [TimeStamp] [datetime] NOT NULL,
         [Exception] [nvarchar](max) NULL,
         [Properties] [xml] NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED
(
         [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]
GO