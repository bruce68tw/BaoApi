/****** Object:  Database [Bao]    Script Date: 2022/3/11 上午 09:27:22 ******/
/*
CREATE DATABASE [Bao]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Bao', FILENAME = N'C:\Users\bruce\Bao.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Bao_log', FILENAME = N'C:\Users\bruce\Bao_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Bao].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Bao] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Bao] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Bao] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Bao] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Bao] SET ARITHABORT OFF 
GO
ALTER DATABASE [Bao] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Bao] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Bao] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Bao] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Bao] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Bao] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Bao] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Bao] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Bao] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Bao] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Bao] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Bao] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Bao] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Bao] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Bao] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Bao] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Bao] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Bao] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Bao] SET  MULTI_USER 
GO
ALTER DATABASE [Bao] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Bao] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Bao] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Bao] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Bao] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Bao] SET QUERY_STORE = OFF
GO
*/
/****** Object:  Table [dbo].[Bao]    Script Date: 2022/3/11 上午 09:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bao](
	[Id] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[IsBatch] [bit] NOT NULL,
	[IsMove] [bit] NOT NULL,
	[IsMoney] [bit] NOT NULL,
	[GiftName] [nvarchar](100) NOT NULL,
	[Note] [nvarchar](500) NULL,
	[StageCount] [tinyint] NOT NULL,
	[LaunchStatus] [char](1) NOT NULL,
	[Status] [bit] NOT NULL,
	[Creator] [varchar](10) NOT NULL,
	[Revised] [datetime] NOT NULL,
 CONSTRAINT [PK_Bao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BaoAttend]    Script Date: 2022/3/11 上午 09:27:22 ******/
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
/****** Object:  Table [dbo].[BaoReply]    Script Date: 2022/3/11 上午 09:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoReply](
	[Id] [varchar](10) NOT NULL,
	[BaoId] [varchar](10) NOT NULL,
	[UserId] [varchar](10) NOT NULL,
	[Reply] [nvarchar](500) NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_BaoReply] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BaoStage]    Script Date: 2022/3/11 上午 09:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoStage](
	[Id] [varchar](10) NOT NULL,
	[BaoId] [varchar](10) NOT NULL,
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
/****** Object:  Table [dbo].[Cms]    Script Date: 2022/3/11 上午 09:27:22 ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 2022/3/11 上午 09:27:22 ******/
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
/****** Object:  Table [dbo].[UserApp]    Script Date: 2022/3/11 上午 09:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserApp](
	[Id] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NULL,
	[Phone] [varchar](15) NOT NULL,
	[Email] [varchar](100) NOT NULL,
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
/****** Object:  Table [dbo].[UserCust]    Script Date: 2022/3/11 上午 09:27:22 ******/
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
/****** Object:  Table [dbo].[XpCode]    Script Date: 2022/3/11 上午 09:27:22 ******/
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
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsBatch], [IsMove], [IsMoney], [GiftName], [Note], [StageCount], [LaunchStatus], [Status], [Creator], [Revised]) VALUES (N'D5V46GGVKA', N'動物園尋寶', CAST(N'2022-02-10T13:00:00.000' AS DateTime), CAST(N'2022-03-30T13:00:00.000' AS DateTime), 1, 1, 0, N'熊讚布偶', N'馬上玩. 在遊戲大廳看到[馬上玩] 及[每日挑戰] 按下立即進入遊戲！ 開新局. ○開新局時，系統不定時會提供免費或半價優惠的道具，看到時要記得領取啊！', 3, N'Y', 1, N'C001', CAST(N'2022-02-18T06:53:16.173' AS DateTime))
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsBatch], [IsMove], [IsMoney], [GiftName], [Note], [StageCount], [LaunchStatus], [Status], [Creator], [Revised]) VALUES (N'D5XA7DPSUA', N'文湖線尋寶', CAST(N'2022-02-09T13:00:00.000' AS DateTime), CAST(N'2022-03-30T13:00:00.000' AS DateTime), 0, 1, 1, N'3000元', N'小泰山救美(部件1) ... 公主可不容易，必須由小朋友為小泰山指出正確路徑才行！小朋友，請利用部件組合出正確國字，幫助小泰山救出公主', 3, N'Y', 1, N'C001', CAST(N'2022-02-18T06:53:16.173' AS DateTime))
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsBatch], [IsMove], [IsMoney], [GiftName], [Note], [StageCount], [LaunchStatus], [Status], [Creator], [Revised]) VALUES (N'D6EEN7TNGA', N'尋寶1', CAST(N'2022-01-21T13:00:00.000' AS DateTime), CAST(N'2022-03-30T13:00:00.000' AS DateTime), 0, 0, 0, N'精美紀念品', N'基本玩法/ 超級獎號/ 猜大小/ 猜單雙. 「BINGO BINGO賓果賓果」基本玩法. 「BINGO BINGO賓果賓果」是一種每五分鐘開獎一次的電腦彩券遊戲。', 2, N'Y', 1, N'C002', CAST(N'2022-02-18T06:53:16.160' AS DateTime))
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsBatch], [IsMove], [IsMoney], [GiftName], [Note], [StageCount], [LaunchStatus], [Status], [Creator], [Revised]) VALUES (N'D6EENPNYVA', N'尋寶2', CAST(N'2022-01-22T13:00:00.000' AS DateTime), CAST(N'2022-03-30T13:00:00.000' AS DateTime), 0, 0, 0, N'精美紀念品', N'你想找的網路人氣推薦遊戲說明商品就在蝦皮購物！買遊戲說明立即上蝦皮台灣商品專區享超低折扣優惠與運費補助，搭配賣家評價安心網購超簡單！', 2, N'Y', 1, N'C002', CAST(N'2022-02-18T06:53:16.160' AS DateTime))
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsBatch], [IsMove], [IsMoney], [GiftName], [Note], [StageCount], [LaunchStatus], [Status], [Creator], [Revised]) VALUES (N'D6EEP3YG1A', N'尋寶3', CAST(N'2022-01-23T13:00:00.000' AS DateTime), CAST(N'2022-03-30T13:00:00.000' AS DateTime), 0, 0, 1, N'1000元', N'UNO / (1971) 遊玩人數： 2-10人遊玩時間： 30分鐘遊戲量級： 輕遊戲機制： 組合收集想必大家對UNO ... 玩法說明. 1.從發牌者左邊的玩家開始，出牌順序為順時鐘方向。', 2, N'Y', 1, N'C002', CAST(N'2022-02-10T23:27:22.000' AS DateTime))
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsBatch], [IsMove], [IsMoney], [GiftName], [Note], [StageCount], [LaunchStatus], [Status], [Creator], [Revised]) VALUES (N'D6EEPMM36A', N'尋寶4', CAST(N'2022-01-24T13:00:00.000' AS DateTime), CAST(N'2022-03-30T13:00:00.000' AS DateTime), 0, 0, 0, N'精美紀念品', N'', 2, N'Y', 1, N'C002', CAST(N'2022-02-10T23:26:37.000' AS DateTime))
INSERT [dbo].[Bao] ([Id], [Name], [StartTime], [EndTime], [IsBatch], [IsMove], [IsMoney], [GiftName], [Note], [StageCount], [LaunchStatus], [Status], [Creator], [Revised]) VALUES (N'D6EEQPGMVA', N'尋寶5', CAST(N'2022-01-25T13:00:00.000' AS DateTime), CAST(N'2022-03-30T13:00:00.000' AS DateTime), 0, 0, 0, N'精美紀念品', N'加油 !!', 2, N'Y', 1, N'C002', CAST(N'2022-02-10T23:25:44.000' AS DateTime))
GO
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A001', N'D5V46GGVKA', N'1', 1, CAST(N'2022-02-03T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A001', N'D5XA7DPSUA', N'1', 1, CAST(N'2022-02-08T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A002', N'D5V46GGVKA', N'1', 1, CAST(N'2022-02-04T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A002', N'D5XA7DPSUA', N'1', 1, CAST(N'2022-02-09T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A003', N'D5V46GGVKA', N'1', 1, CAST(N'2022-02-05T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A003', N'D5XA7DPSUA', N'1', 1, CAST(N'2022-02-10T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A004', N'D5V46GGVKA', N'1', 1, CAST(N'2022-02-06T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A004', N'D5XA7DPSUA', N'1', 1, CAST(N'2022-02-11T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A005', N'D5V46GGVKA', N'1', 1, CAST(N'2022-02-07T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'A005', N'D5XA7DPSUA', N'1', 1, CAST(N'2022-02-12T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'D90L80DBYA', N'D5V46GGVKA', N'1', 1, CAST(N'2022-02-01T12:00:00.000' AS DateTime))
INSERT [dbo].[BaoAttend] ([UserId], [BaoId], [AttendStatus], [NowLevel], [Created]) VALUES (N'D90L80DBYA', N'D5XA7DPSUA', N'1', 1, CAST(N'2022-02-02T12:00:00.000' AS DateTime))
GO
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5VJR9D85A', N'D5V46GGVKA', N'panda.jpg', N'牠的食物', N'竹子', N'm8TrdH3SD96eric3tb_1Xw', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5VJT4ZGMA', N'D5V46GGVKA', N'map.png', N'東南方向', N'小浣熊', N'5o1o3JVdMjoyjKfOizv25g', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5XA7DQ7UA', N'D5XA7DPSUA', N'car.jpg', N'站名', N'大安', N'fpcw2DJb1cyUbx0SnqYxTw', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5XA7DQPEA', N'D5XA7DPSUA', N'food.png', N'餐廳名稱', N'貓大爺', N'ZNZRfpkUV_c1Yv1ohcw33g', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5XB16D7HA', N'D5V46GGVKA', N'zoo.jpg', N'海報', N'大象', N'4z0QHK4vkFbVQmrqUGQ0MA', 2)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D5XBJERMBA', N'D5XA7DPSUA', N'night.jpg', N'活動名稱', N'夜店', N'pac41t5DOnRKSLIMesHwBA', 2)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R5MS5T9A', N'D6EEQPGMVA', N'map.png', N'地圖', N'地圖', N'ikTfbRadCedVtEBJerw6ww', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R5MS69EA', N'D6EEQPGMVA', N'panda.jpg', N'貓熊', N'貓熊', N'EAXD9Zwuakxk7BNGBcNOTQ', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R62LTEKA', N'D6EEPMM36A', N'map.png', N'地圖', N'地圖', N'ikTfbRadCedVtEBJerw6ww', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R62LTWYA', N'D6EEPMM36A', N'panda.jpg', N'貓熊', N'貓熊', N'EAXD9Zwuakxk7BNGBcNOTQ', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R63RFWMA', N'D6EEP3YG1A', N'map.png', N'地圖', N'地圖', N'ikTfbRadCedVtEBJerw6ww', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R63RGC8A', N'D6EEP3YG1A', N'panda.jpg', N'貓熊', N'貓熊', N'EAXD9Zwuakxk7BNGBcNOTQ', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R64W8SLA', N'D6EENPNYVA', N'map.png', N'地圖', N'地圖', N'ikTfbRadCedVtEBJerw6ww', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R64W96YA', N'D6EENPNYVA', N'panda.jpg', N'貓熊', N'貓熊', N'EAXD9Zwuakxk7BNGBcNOTQ', 1)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R65KP8YA', N'D6EEN7TNGA', N'map.png', N'地圖', N'地圖', N'ikTfbRadCedVtEBJerw6ww', 0)
INSERT [dbo].[BaoStage] ([Id], [BaoId], [FileName], [AppHint], [CustHint], [Answer], [Sort]) VALUES (N'D9R65KPBCA', N'D6EEN7TNGA', N'panda.jpg', N'貓熊', N'貓熊', N'EAXD9Zwuakxk7BNGBcNOTQ', 1)
GO
INSERT [dbo].[Cms] ([Id], [CmsType], [Title], [Text], [Html], [Note], [FileName], [StartTime], [EndTime], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D655G2BSJA', N'Msg', N'系統維護公告', N'因系統升級，本系統將於2022/02/20 00:00 ~ 01:00 進行停機維護。', NULL, NULL, NULL, CAST(N'2022-02-10T00:00:00.000' AS DateTime), CAST(N'2022-02-22T00:00:00.000' AS DateTime), 1, N'U001', CAST(N'2021-12-08T19:27:48.000' AS DateTime), NULL, NULL)
INSERT [dbo].[Cms] ([Id], [CmsType], [Title], [Text], [Html], [Note], [FileName], [StartTime], [EndTime], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D65626P4TA', N'Msg', N'系統功能異動', N'即日起，個人資料維護Email欄位改為唯讀。', NULL, NULL, NULL, CAST(N'2021-12-20T00:00:00.000' AS DateTime), CAST(N'2021-12-31T00:00:00.000' AS DateTime), 1, N'U001', CAST(N'2021-12-08T19:40:59.000' AS DateTime), NULL, NULL)
INSERT [dbo].[Cms] ([Id], [CmsType], [Title], [Text], [Html], [Note], [FileName], [StartTime], [EndTime], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D65KUEVM4A', N'Card', N'新年賀卡', NULL, N'<p>恭喜新年好!!</p><p><img src="/image/CmsCard/D65KU9XSXA.png"><br></p>', NULL, NULL, CAST(N'2021-12-30T00:00:00.000' AS DateTime), CAST(N'2021-12-31T00:00:00.000' AS DateTime), 1, N'U001', CAST(N'2021-12-09T00:47:43.000' AS DateTime), NULL, NULL)
INSERT [dbo].[Cms] ([Id], [CmsType], [Title], [Text], [Html], [Note], [FileName], [StartTime], [EndTime], [Status], [Creator], [Created], [Reviser], [Revised]) VALUES (N'D65KWJ19RA', N'Card', N'春節賀卡', NULL, N'<p>春節快樂!!</p><p><img src="/image/CmsCard/D65KW4Y59A.jpg"><br></p>', NULL, N'賀卡.png', CAST(N'2022-02-11T00:00:00.000' AS DateTime), CAST(N'2022-02-28T00:00:00.000' AS DateTime), 1, N'U001', CAST(N'2021-12-09T00:49:05.000' AS DateTime), N'U001', CAST(N'2022-02-11T22:29:48.000' AS DateTime))
GO
INSERT [dbo].[User] ([Id], [Name], [Account], [Pwd], [Status], [IsAdmin]) VALUES (N'D656GB3BFA', N'Peter', N'pp', N'xIP2zoUcns2fuDX_dVFzfA', 1, 0)
INSERT [dbo].[User] ([Id], [Name], [Account], [Pwd], [Status], [IsAdmin]) VALUES (N'U001', N'Alex', N'aa', N'QSS8CpM1wn8IbyS6IHpJEg', 1, 1)
GO
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'A001', N'A001', N'A001', N'A001', N'A001', NULL, 1, CAST(N'2022-01-01T00:00:00.000' AS DateTime), CAST(N'2022-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'A002', N'A002', N'A002', N'A002', N'A002', NULL, 1, CAST(N'2022-01-01T00:00:00.000' AS DateTime), CAST(N'2022-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'A003', N'A003', N'A003', N'A003', N'A003', NULL, 1, CAST(N'2022-01-01T00:00:00.000' AS DateTime), CAST(N'2022-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'A004', N'A004', N'A004', N'A004', N'A004', NULL, 1, CAST(N'2022-01-01T00:00:00.000' AS DateTime), CAST(N'2022-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'A005', N'A005', N'A005', N'A005', N'A005', NULL, 1, CAST(N'2022-01-01T00:00:00.000' AS DateTime), CAST(N'2022-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'D6ASCJ59RA', N'尋寶1a', N'0912345678', N'aa@bb.c1', N'Taipei', NULL, 0, CAST(N'2021-12-11T18:19:29.000' AS DateTime), CAST(N'2022-01-29T12:54:26.510' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'D7K52P38FA', N'尋寶1a', N'0912345678', N'aa@bb.c2', N'Taipei', NULL, 0, CAST(N'2022-01-03T01:07:22.000' AS DateTime), CAST(N'2022-01-29T12:54:26.510' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'D8WD7TUL5A', N'尋寶abc', N'0912345678', N'aa@bb.c3', N'Taipei', NULL, 0, CAST(N'2022-01-26T20:05:29.000' AS DateTime), CAST(N'2022-01-29T12:54:26.510' AS DateTime))
INSERT [dbo].[UserApp] ([Id], [Name], [Phone], [Email], [Address], [AuthCode], [Status], [Created], [Revised]) VALUES (N'D90L80DBYA', N'尋寶獵人', N'0912345678', N'abc123@gmail.com', N'台北市農田路1號', N'80296', 1, CAST(N'2022-01-29T01:10:33.223' AS DateTime), CAST(N'2022-01-30T17:43:09.307' AS DateTime))
GO
INSERT [dbo].[UserCust] ([Id], [Name], [Account], [Pwd], [Phone], [Email], [Address], [IsCorp], [Status], [Created]) VALUES (N'C001', N'Alex基金會', N'aa', N'QSS8CpM1wn8IbyS6IHpJEg', N'091234567', N'aa@bb.cc', N'Taipei', 1, 1, CAST(N'2021-01-01T12:00:00.000' AS DateTime))
INSERT [dbo].[UserCust] ([Id], [Name], [Account], [Pwd], [Phone], [Email], [Address], [IsCorp], [Status], [Created]) VALUES (N'C002', N'Bibby先生', N'bb', N'Ia0L2Da5DQj0z2QLTCmOfA', N'091234568', N'a2@bb.cc', N'Taipei', 0, 1, CAST(N'2021-12-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'AttendStatus', N'1', N'已參加', 1, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'AttendStatus', N'F', N'已完成', 2, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'LaunchStatus', N'0', N'未上架', 1, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'LaunchStatus', N'1', N'準備上架', 1, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'LaunchStatus', N'X', N'下架', 3, NULL, NULL)
INSERT [dbo].[XpCode] ([Type], [Value], [Name], [Sort], [Ext], [Note]) VALUES (N'LaunchStatus', N'Y', N'已上架', 2, NULL, NULL)
GO
/****** Object:  Index [Bao_StartTime]    Script Date: 2022/3/11 上午 09:27:22 ******/
CREATE NONCLUSTERED INDEX [Bao_StartTime] ON [dbo].[Bao]
(
	[StartTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserApp_Email]    Script Date: 2022/3/11 上午 09:27:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserApp_Email] ON [dbo].[UserApp]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bao] ADD  CONSTRAINT [DF_Bao_LaunchStatus]  DEFAULT ('0') FOR [LaunchStatus]
GO
ALTER TABLE [dbo].[BaoAttend] ADD  CONSTRAINT [DF_BaoAttend_NowLevel]  DEFAULT ((1)) FOR [NowLevel]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Pwd]  DEFAULT ('') FOR [Pwd]
GO
ALTER TABLE [dbo].[UserApp] ADD  CONSTRAINT [DF_UserApp_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[UserCust] ADD  CONSTRAINT [DF_UserCust_Pwd]  DEFAULT ('') FOR [Pwd]
GO
--ALTER DATABASE [Bao] SET  READ_WRITE 
--GO
