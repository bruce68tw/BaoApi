USE [Bao]
GO
/****** Object:  Table [dbo].[Bao]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bao](
	[Id] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[IsMove] [bit] NOT NULL,
	[ReplyType] [char](1) NOT NULL,
	[PrizeType] [varchar](3) NOT NULL,
	[PrizeNote] [nvarchar](100) NOT NULL,
	[Note] [nvarchar](500) NULL,
	[StageCount] [tinyint] NOT NULL,
	[StageMaxError] [smallint] NOT NULL,
	[LaunchStatus] [char](1) NOT NULL,
	[Status] [bit] NOT NULL,
	[Creator] [varchar](10) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Reviser] [varchar](10) NULL,
	[Revised] [datetime] NULL,
 CONSTRAINT [PK_Bao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BaoAttend]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoAttend](
	[UserId] [varchar](10) NOT NULL,
	[BaoId] [varchar](10) NOT NULL,
	[AttendStatus] [char](1) NOT NULL,
	[NowLevel] [smallint] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_BaoAttend] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[BaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BaoStage]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoStage](
	[Id] [varchar](10) NOT NULL,
	[BaoId] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[AppHint] [nvarchar](100) NULL,
	[CustHint] [nvarchar](100) NULL,
	[Answer] [varchar](22) NOT NULL,
	[Sort] [smallint] NOT NULL,
 CONSTRAINT [PK_BaoStage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cms]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cms](
	[Id] [varchar](10) NOT NULL,
	[CmsType] [varchar](10) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Text] [nvarchar](max) NULL,
	[Html] [nvarchar](max) NULL,
	[Note] [nvarchar](255) NULL,
	[FileName] [nvarchar](100) NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
	[Creator] [varchar](10) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Reviser] [varchar](10) NULL,
	[Revised] [datetime] NULL,
 CONSTRAINT [PK_Cms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StageReplyLog]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StageReplyLog](
	[Id] [varchar](10) NOT NULL,
	[UserId] [varchar](10) NOT NULL,
	[StageId] [varchar](10) NOT NULL,
	[Reply] [varchar](255) NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_BaoReply] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StageReplyStatus]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StageReplyStatus](
	[UserId] [varchar](10) NOT NULL,
	[StageId] [varchar](10) NOT NULL,
	[ReplyStatus] [char](1) NOT NULL,
	[ErrorCount] [smallint] NOT NULL,
 CONSTRAINT [PK_UserAppStage] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[StageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Account] [varchar](20) NOT NULL,
	[Pwd] [varchar](22) NOT NULL,
	[Status] [bit] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserApp]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserApp](
	[Id] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NULL,
	[Phone] [varchar](15) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Ip] [varchar](20) NULL,
	[Address] [nvarchar](255) NULL,
	[AuthCode] [varchar](10) NULL,
	[Status] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Revised] [datetime] NOT NULL,
 CONSTRAINT [PK_UserApp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCust]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCust](
	[Id] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Account] [varchar](30) NOT NULL,
	[Pwd] [varchar](22) NOT NULL,
	[Phone] [varchar](15) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[IsCorp] [bit] NOT NULL,
	[Status] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_UserCust] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[XpCode]    Script Date: 2024/12/4 下午 06:06:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[XpCode](
	[Type] [varchar](20) NOT NULL,
	[Value] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Sort] [int] NOT NULL,
	[Ext] [varchar](30) NULL,
	[Note] [nvarchar](255) NULL,
 CONSTRAINT [PK_XpCode] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsMove], [ReplyType], [PrizeType], [PrizeNote], [Note], [StageCount], [StageMaxError], [LaunchStatus], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D5V46GGVKA', N'尋寶測試-Any', CAST(N'2024-12-01T13:00:00.000' AS DateTime), CAST(N'2024-12-31T13:00:00.000' AS DateTime), 1, N'A', N'MG', N'獎金100元+小禮物', N'測試尋寶-隨意單題解答-類型。', 3, 3, N'A', 1, N'A01', CAST(N'2022-02-10T13:00:00.000' AS DateTime), N'A01', CAST(N'2024-12-04T10:32:50.000' AS DateTime))
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsMove], [ReplyType], [PrizeType], [PrizeNote], [Note], [StageCount], [StageMaxError], [LaunchStatus], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D5XA7DPSUA', N'尋寶測試-Step', CAST(N'2024-12-01T13:00:00.000' AS DateTime), CAST(N'2024-12-31T13:00:00.000' AS DateTime), 1, N'S', N'M', N'獎金100元', N'小泰山救美(部件1) ... 公主可不容易，必須由小朋友為小泰山指出正確路徑才行！小朋友，請利用部件組合出正確國字，幫助小泰山救出公主', 3, 0, N'A', 0, N'A01', CAST(N'2022-02-10T13:00:00.000' AS DateTime), NULL, CAST(N'2022-02-18T06:53:16.173' AS DateTime))
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsMove], [ReplyType], [PrizeType], [PrizeNote], [Note], [StageCount], [StageMaxError], [LaunchStatus], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'msjEOq8ES1', N'資管愛宴尋寶', CAST(N'2024-12-01T13:00:00.000' AS DateTime), CAST(N'2024-12-31T13:00:00.000' AS DateTime), 1, N'A', N'MG', N'獎金若干+神祕小禮物', N'回答謎題不必依造順序, 由最後答對題目數量決定勝負, 若題數相同則由答題時間較少者勝出。每個題目可以答錯5次。', 8, 5, N'A', 1, N'pz52vT8ijG', CAST(N'2024-11-29T11:55:50.000' AS DateTime), N'pz52vT8ijG', CAST(N'2024-11-29T13:48:27.000' AS DateTime))
GO
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'BYiQbBF6s6', N'D5V46GGVKA', N'A', 1, CAST(N'2024-11-23T12:06:48.700' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'BYiQbBF6s6', N'D5XA7DPSUA', N'A', 1, CAST(N'2024-11-23T12:21:58.413' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'D90L80DBYA', N'D5V46GGVKA', N'A', 1, CAST(N'2022-02-01T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'D90L80DBYA', N'D5XA7DPSUA', N'A', 1, CAST(N'2022-02-02T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'Em19Tes6HZ', N'D5V46GGVKA', N'A', 1, CAST(N'2024-12-04T11:38:56.063' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'Em19Tes6HZ', N'D5XA7DPSUA', N'A', 1, CAST(N'2024-12-02T12:27:41.080' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'Em19Tes6HZ', N'msjEOq8ES1', N'A', 1, CAST(N'2024-12-03T10:07:07.060' AS DateTime))
GO
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'2zsgqFGink', N'msjEOq8ES1', N'伊甸之歌', N'伊甸之歌.png', N'猜一個2位數的數字', N'數字', N'odDG6D8CcyfYRhBj9KxYpg', 5)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5VJR9D85A', N'D5V46GGVKA', N'貓熊食物', N'panda.jpg', N'答案 bb', N'熊貓2', N'Ia0L2Da5DQj0z2QLTCmOfA', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5VJT4ZGMA', N'D5V46GGVKA', N'小浣熊方向', N'map.png', N'答案 aa', N'地圖2', N'QSS8CpM1wn8IbyS6IHpJEg', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5XA7DQ7UA', N'D5XA7DPSUA', NULL, N'car.jpg', N'捷運', N'捷運2', N'fpcw2DJb1cyUbx0SnqYxTw', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5XA7DQPEA', N'D5XA7DPSUA', NULL, N'food.png', N'小籠包', N'小籠包2', N'ZNZRfpkUV_c1Yv1ohcw33g', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5XB16D7HA', N'D5V46GGVKA', N'大象海報', N'zoo.jpg', N'答案 cc', N'動物園2', N'4DI6kDmt0peL9bSVUFcsfA', 2)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5XBJERMBA', N'D5XA7DPSUA', NULL, N'night.jpg', N'夜店', N'夜店2', N'pac41t5DOnRKSLIMesHwBA', 2)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'fsHakPpREX', N'msjEOq8ES1', N'70歲的禮物', N'70歲禮物.png', N'猜8個字, 70歲生日禮物', N'70歲生日禮物', N'Ek-rxTkPhEvIsQVelGTs-A', 3)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'MiPvC4Ggp7', N'msjEOq8ES1', N'伊甸宗旨', N'伊甸宗旨.png', N'猜4個字OOO悟, 摩斯不只賣漢堡', N'解碼', N'auC_rNEUYDJpCNglBm2i_A', 2)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'mTwYWqgU9E', N'msjEOq8ES1', N'小倩', N'竹俗分不清楚.png', N'猜人名3個字, 阿竹、阿俗分不清楚', N'人名', N'NR1R5BLzcI4uF0VFzjghkw', 6)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'PukQw5E66A', N'msjEOq8ES1', N'七年之癢', N'七年之癢.png', N'猜人名3個字, 七年之癢', N'年資七年', N'mVXzJ9jSH5F59HBAJ7eQiw', 4)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'SvMfssXeYN', N'msjEOq8ES1', N'花貓在睡覺', N'花貓在睡覺.png', N'猜5個字, 電話不是電話', N'電話不是電話', N'GAJY8lfDJYx0gKySd4EAxg', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'Z1BSZIL9vB', N'msjEOq8ES1', N'巫婆納莉', N'巫婆納莉.png', N'猜10個字', N'10個數字', N'JoXUE3vZXov_-7-eKUtLiA', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [Name], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'Zf8aaW6Lze', N'msjEOq8ES1', N'2張圖', N'猜2張圖.png', N'猜8個字, 由左到右', N'2幅畫', N'UkzOtFYxe1DoPoamwz1oRw', 7)
GO
INSERT [dbo].[Cms] ([Id], [CmsType], [Title], [Text], [Html], [Note], [FileName], [StartTime], [EndTime], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D655G2BSJA', N'Msg', N'系統維護公告', N'因系統升級，本系統將於2022/02/20 00:00 ~ 01:00 進行停機維護。', NULL, NULL, NULL, CAST(N'2022-02-10T00:00:00.000' AS DateTime), CAST(N'2022-02-22T00:00:00.000' AS DateTime), 1, N'U001', CAST(N'2021-12-08T19:27:48.000' AS DateTime), NULL, NULL)
INSERT [dbo].[Cms] ([Id], [CmsType], [Title], [Text], [Html], [Note], [FileName], [StartTime], [EndTime], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D65626P4TA', N'Msg', N'系統功能異動', N'即日起，個人資料維護Email欄位改為唯讀。', NULL, NULL, NULL, CAST(N'2021-12-20T00:00:00.000' AS DateTime), CAST(N'2021-12-31T00:00:00.000' AS DateTime), 1, N'U001', CAST(N'2021-12-08T19:40:59.000' AS DateTime), NULL, NULL)
INSERT [dbo].[Cms] ([Id], [CmsType], [Title], [Text], [Html], [Note], [FileName], [StartTime], [EndTime], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D65KUEVM4A', N'Card', N'新年賀卡', NULL, N'<p>恭喜新年好!!</p><p><img src="/image/CmsCard/D65KU9XSXA.png"><br></p>', NULL, NULL, CAST(N'2021-12-30T00:00:00.000' AS DateTime), CAST(N'2021-12-31T00:00:00.000' AS DateTime), 1, N'U001', CAST(N'2021-12-09T00:47:43.000' AS DateTime), NULL, NULL)
INSERT [dbo].[Cms] ([Id], [CmsType], [Title], [Text], [Html], [Note], [FileName], [StartTime], [EndTime], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D65KWJ19RA', N'Card', N'春節賀卡', NULL, N'<p>春節快樂!!</p><p><img src="/image/CmsCard/D65KW4Y59A.jpg"><br></p>', NULL, N'賀卡.png', CAST(N'2022-02-11T00:00:00.000' AS DateTime), CAST(N'2022-02-28T00:00:00.000' AS DateTime), 1, N'U001', CAST(N'2021-12-09T00:49:05.000' AS DateTime), N'U001', CAST(N'2022-02-11T22:29:48.000' AS DateTime))
GO
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'84aespVkTB', N'Em19Tes6HZ', N'MiPvC4Ggp7', N'Y41wTrY6QipZExH7v8KmhA==', CAST(N'2024-12-03T18:35:58.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'bVCLfUsDto', N'Em19Tes6HZ', N'Z1BSZIL9vB', N'+WsML9eYfY1vi4BkOleuhA==', CAST(N'2024-12-03T18:59:22.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'cdL2pmTphn', N'Em19Tes6HZ', N'D5VJT4ZGMA', N'n7mL7GNTkW8tFrssx7Y8Pw==', CAST(N'2024-12-04T11:39:05.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'DEuWkY0IDE', N'BYiQbBF6s6', N'D5VJT4ZGMA', N'98olVhxvJ2R7GHDxG7y2HQ', CAST(N'2024-11-28T14:28:52.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'DmqkOdfLyc', N'Em19Tes6HZ', N'Z1BSZIL9vB', N'+WsML9eYfY1vi4BkOleuhA==', CAST(N'2024-12-03T18:59:19.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'f0xTQkgDEX', N'Em19Tes6HZ', N'D5VJT4ZGMA', N'n7mL7GNTkW8tFrssx7Y8Pw==', CAST(N'2024-12-04T11:46:00.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'foaUxLMaJG', N'Em19Tes6HZ', N'D5VJR9D85A', N'n61OBaxDMlD5iJwVh1pE/w==', CAST(N'2024-12-04T11:47:40.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'g1ZVqdkYHH', N'BYiQbBF6s6', N'D5XB16D7HA', N'YKbv7AuPxyJwiRYsZmobFA==', CAST(N'2024-11-28T17:39:10.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'gNlH5Jxe3a', N'Em19Tes6HZ', N'D5VJT4ZGMA', N'n7mL7GNTkW8tFrssx7Y8Pw==', CAST(N'2024-12-04T11:42:09.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'K0EVLvFRon', N'Em19Tes6HZ', N'D5VJT4ZGMA', N'n7mL7GNTkW8tFrssx7Y8Pw==', CAST(N'2024-12-04T11:39:07.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'KIsDF5hGCx', N'Em19Tes6HZ', N'Z1BSZIL9vB', N'+WsML9eYfY1vi4BkOleuhA==', CAST(N'2024-12-03T18:59:26.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'MGOAByZB0N', N'Em19Tes6HZ', N'SvMfssXeYN', N'JG8WxqnLdfkccg7QEhHCIA==', CAST(N'2024-12-03T18:36:36.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'Qgu3Wjd5Ze', N'Em19Tes6HZ', N'Z1BSZIL9vB', N'+WsML9eYfY1vi4BkOleuhA==', CAST(N'2024-12-03T18:59:15.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'SEMG3PnnKQ', N'Em19Tes6HZ', N'Z1BSZIL9vB', N'JG8WxqnLdfkccg7QEhHCIA==', CAST(N'2024-12-03T18:53:19.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N't65y8sj8O0', N'Em19Tes6HZ', N'Z1BSZIL9vB', N'+WsML9eYfY1vi4BkOleuhA==', CAST(N'2024-12-03T18:59:24.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'Vdwz5OYLwx', N'Em19Tes6HZ', N'Z1BSZIL9vB', N'+WsML9eYfY1vi4BkOleuhA==', CAST(N'2024-12-03T18:59:17.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'xkdoYq9c04', N'Em19Tes6HZ', N'D5VJT4ZGMA', N'n7mL7GNTkW8tFrssx7Y8Pw==', CAST(N'2024-12-04T11:39:44.000' AS DateTime))
INSERT [dbo].[StageReplyLog] ([Id], [UserId], [StageId], [Reply], [Created]) VALUES (N'zvshMHrvJy', N'Em19Tes6HZ', N'Z1BSZIL9vB', N'+WsML9eYfY1vi4BkOleuhA==', CAST(N'2024-12-03T18:59:20.000' AS DateTime))
GO
INSERT [dbo].[StageReplyStatus] ([UserId], [StageId], [ReplyStatus], [ErrorCount]) VALUES (N'BYiQbBF6s6', N'D5VJR9D85A', N'1', 2)
INSERT [dbo].[StageReplyStatus] ([UserId], [StageId], [ReplyStatus], [ErrorCount]) VALUES (N'Em19Tes6HZ', N'D5VJR9D85A', N'1', 0)
INSERT [dbo].[StageReplyStatus] ([UserId], [StageId], [ReplyStatus], [ErrorCount]) VALUES (N'Em19Tes6HZ', N'D5VJT4ZGMA', N'L', 3)
INSERT [dbo].[StageReplyStatus] ([UserId], [StageId], [ReplyStatus], [ErrorCount]) VALUES (N'Em19Tes6HZ', N'MiPvC4Ggp7', N'1', 0)
INSERT [dbo].[StageReplyStatus] ([UserId], [StageId], [ReplyStatus], [ErrorCount]) VALUES (N'Em19Tes6HZ', N'SvMfssXeYN', N'0', 1)
INSERT [dbo].[StageReplyStatus] ([UserId], [StageId], [ReplyStatus], [ErrorCount]) VALUES (N'Em19Tes6HZ', N'Z1BSZIL9vB', N'0', 8)
GO
INSERT [dbo].[User] ([Id], [Name], [Account], [Pwd], [Status], [IsAdmin]) VALUES (N'D656GB3BFA', N'伊甸-資管處', N'peter', N'xIP2zoUcns2fuDX_dVFzfA', 1, 1)
INSERT [dbo].[User] ([Id], [Name], [Account], [Pwd], [Status], [IsAdmin]) VALUES (N'U001', N'測試人員', N'aa', N'QSS8CpM1wn8IbyS6IHpJEg', 1, 1)
GO
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Ip], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'BYiQbBF6s6', N'a1', N'091111111', N'a1@aa.bb', N'192.168.43.1', N'a2', N'32561', 1, CAST(N'2024-11-23T11:56:47.883' AS DateTime), CAST(N'2024-11-27T15:48:36.013' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Ip], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'Em19Tes6HZ', N'aa', N'091111111', N'aa@aa.bb', N'192.168.43.1', N'aa', N'78949', 1, CAST(N'2024-11-28T17:59:49.887' AS DateTime), CAST(N'2024-12-03T10:33:11.497' AS DateTime))
GO
INSERT [dbo].[UserCust] ([Id], [Name], [Account], [Pwd], [Phone], [Email], [Address], [IsCorp], [Status], [Created]) VALUES (N'A01', N'測試單位', N'aa', N'QSS8CpM1wn8IbyS6IHpJEg', N'091234567', N'a1@aa.bb', N'Taipei', 1, 1, CAST(N'2021-01-01T12:00:00.000' AS DateTime))
INSERT [dbo].[UserCust] ([Id], [Name], [Account], [Pwd], [Phone], [Email], [Address], [IsCorp], [Status], [Created]) VALUES (N'pz52vT8ijG', N'伊甸-資管處', N'peter', N'Udww3cRz1DpgEenrumyncA', N'091111111', N'a2@aa.bb', N'Taipei', 1, 1, CAST(N'2024-11-29T11:52:08.000' AS DateTime))
GO
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'AttendStatus', N'A', N'已參加', 1, NULL, N'Attend')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'AttendStatus', N'F', N'已完成', 2, NULL, N'Finish')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'LaunchStatus', N'A', N'已上架', 2, NULL, N'Already')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'LaunchStatus', N'N', N'未上架', 1, NULL, N'上架狀態 NotReady')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'LaunchStatus', N'R', N'準備上架', 1, NULL, N'Ready')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'LaunchStatus', N'X', N'下架', 3, NULL, N'Cancel')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'PrizeType', N'0', N'無', 1, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'PrizeType', N'G', N'獎品', 3, NULL, N'Gift')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'PrizeType', N'M', N'獎金', 2, NULL, N'Money')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'PrizeType', N'MG', N'獎金+獎品', 4, NULL, N'Money+Gift')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyBaoStatus', N'A', N'已參加', 1, NULL, N'Attend')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyBaoStatus', N'F', N'已完成', 2, NULL, N'Finish')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyBaoStatus', N'L', N'已鎖定', 3, NULL, N'Lock')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyStageStatus', N'0', N'答錯', 2, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyStageStatus', N'1', N'答對', 1, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyStageStatus', N'L', N'已鎖定', 3, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyType', N'A', N'隨意單題解答', 3, NULL, N'Any')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyType', N'B', N'批次全部解答', 2, NULL, N'Batch')
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'ReplyType', N'S', N'循序單題解答', 1, NULL, N'Step')
GO
ALTER TABLE [dbo].[Bao] ADD  CONSTRAINT [DF_Bao_MaxError]  DEFAULT ((0)) FOR [StageMaxError]
GO
ALTER TABLE [dbo].[Bao] ADD  CONSTRAINT [DF_Bao_LaunchStatus]  DEFAULT ('0') FOR [LaunchStatus]
GO
ALTER TABLE [dbo].[BaoAttend] ADD  CONSTRAINT [DF_BaoAttend_NowLevel]  DEFAULT ((1)) FOR [NowLevel]
GO
ALTER TABLE [dbo].[StageReplyStatus] ADD  CONSTRAINT [DF_UserAppStage_ErrorCount]  DEFAULT ((0)) FOR [ErrorCount]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Pwd]  DEFAULT ('') FOR [Pwd]
GO
ALTER TABLE [dbo].[UserApp] ADD  CONSTRAINT [DF_UserApp_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[UserCust] ADD  CONSTRAINT [DF_UserCust_Pwd]  DEFAULT ('') FOR [Pwd]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AES加密' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StageReplyLog', @level2type=N'COLUMN',@level2name=N'Reply'
GO
