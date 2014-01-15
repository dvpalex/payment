BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupDepId_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupDepId] DROP CONSTRAINT FK_PaymentAgentSetupDepId_PaymentAgentSetup
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptDepId_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptDepId] DROP CONSTRAINT FK_PaymentAttemptDepId_PaymentAttempt
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptDepIdReturnChk_PaymentAttemptDepIdReturn]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptDepIdReturnChk] DROP CONSTRAINT FK_PaymentAttemptDepIdReturnChk_PaymentAttemptDepIdReturn
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptDepIdReturn_PaymentAttemptDepIdReturnHdr]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptDepIdReturn] DROP CONSTRAINT FK_PaymentAttemptDepIdReturn_PaymentAttemptDepIdReturnHdr
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptVBVLog_PaymentAttemptVBV]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptVBVLog] DROP CONSTRAINT FK_PaymentAttemptVBVLog_PaymentAttemptVBV
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupDepId]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupDepId]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptDepId]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptDepId]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptDepIdReturn]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptDepIdReturn]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptDepIdReturnChk]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptDepIdReturnChk]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptDepIdReturnHdr]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptDepIdReturnHdr]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptVBVLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptVBVLog]
GO

CREATE TABLE [dbo].[PaymentAgentSetupDepId] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[bankNumber] [int] NULL ,
	[bankDigit] [int] NULL ,
	[agencyNumber] [int] NULL ,
	[agencyDigit] [int] NULL ,
	[accountNumber] [int] NULL ,
	[accountDigit] [int] NULL ,
	[cederName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[cederCNPJ] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[conventionType] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[calcType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[expirationDays] [int] NULL ,
	[idPattern] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO


CREATE TABLE dbo.PaymentAttemptDepIdReturnHdr
	(
	headerId int NOT NULL IDENTITY (1, 1),
	bankNumber int NOT NULL,
	sequencialReturnNumber int NOT NULL,
	recordFileDate datetime NOT NULL,
	companyName varchar(100) NOT NULL,
	agencyNumber int NULL,
	agencyDV char(1) NULL,
	assignorNumber int NULL,
	assignorDV char(1) NULL,
	companyCode int NOT NULL,
	nameOfCapturedFile varchar(200) NOT NULL,
	creationDateCapturedFile datetime NOT NULL,
	nameOfArquivedFile varchar(200) NOT NULL,
	processDate datetime NOT NULL,
	numberOfDetailsRecords int NOT NULL
	)  ON [PRIMARY]
GO

CREATE TABLE dbo.PaymentAttemptDepIdReturn
	(
	paymentAttemptDepIdReturnId int NOT NULL IDENTITY (1, 1),
	paymentAttemptId uniqueidentifier NULL,
	data_deposito datetime NULL,
	ag_acolhedora smallint NULL,
	digito_agencia tinyint NULL,
	remetente_deposito varchar(30) NULL,
	valor_deposito_dinheiro numeric(18, 2) NULL,
	valor_deposito_cheque numeric(18, 2) NULL,
	valor_total_deposito numeric(18, 2) NULL,
	digitoAgenciaRecebedora char(10) NULL,
	numero_documento int NULL,
	cod_canal_distribuicao tinyint NULL,
	num_sequencia_arquivo int NULL,
	headerDepIdentId int NOT NULL,
	creationDate datetime NOT NULL,
	status int NOT NULL
	)  ON [PRIMARY]
GO

CREATE TABLE dbo.PaymentAttemptDepIdReturnChk
	(
	checkId int NOT NULL IDENTITY (1, 1),
	paymentAttemptDepIdReturnId int NOT NULL,
	data_deposito_cheque datetime NULL,
	ag_acolhedora_cheque smallint NULL,
	dig_acolhedora_cheque tinyint NULL,
	cod_depositante_cheque varchar(20) NULL,
	vlr_deposito_total numeric(18, 2) NULL,
	vlr_cheque numeric(18, 2) NULL,
	cod_banco smallint NULL,
	cod_agencia_cheque smallint NULL,
	numero_cheque int NULL,
	sequencia_arquivo int NULL,
	creationDate datetime NOT NULL,
	status int NOT NULL
	)  ON [PRIMARY]
GO

CREATE TABLE dbo.PaymentAttemptDepId
	(
	paymentAttemptId uniqueidentifier NOT NULL,
	agentOrderReference int NOT NULL IDENTITY (1, 1),
	bankNumber int NOT NULL,
	idNumber varchar(30) NULL,
	paymentDate datetime NULL,
	dueDate datetime NULL,
	paymentAttemptDepIdReturnId int NULL,
	paymentStatus int NULL
	)  ON [PRIMARY]
GO

CREATE TABLE dbo.PaymentAttemptVBVLog
	(
	paymentAttemptId uniqueidentifier NOT NULL,
	interfaceType smallint NOT NULL,
	returnDate datetime NOT NULL,
	tidMaster varchar(30) NULL,
	tid varchar(30) NULL,
	lr numeric(4, 0) NULL,
	arp int NULL,
	ars varchar(128) NULL,
	vbvOrderId varchar(20) NULL,
	price int NULL,
	free varchar(128) NULL,
	pan varchar(30) NULL,
	bank int NULL,
	authent int NULL,
	cap varchar(50) NULL
	)  ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentAgentSetupDepId] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetupDepId] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptDepIdReturn] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptDepIdReturn] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptDepIdReturnId]
	)  ON [PRIMARY] 
GO
ALTER TABLE [dbo].[PaymentAttemptDepIdReturnChk] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptDepIdReturnChk] PRIMARY KEY  CLUSTERED 
	(
		[checkId]
	)  ON [PRIMARY] 
GO
ALTER TABLE [dbo].[PaymentAttemptDepIdReturnHdr] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptDepIdReturnHdr] PRIMARY KEY  CLUSTERED 
	(
		[headerId]
	)  ON [PRIMARY] 
GO
ALTER TABLE [dbo].[PaymentAttemptDepIdReturnChk] ADD 
	CONSTRAINT [DF_PaymentAttemptDepIdReturnChk_status] DEFAULT (0) FOR [status]
GO


ALTER TABLE [dbo].[PaymentAgentSetupDepId] ADD 
	CONSTRAINT [FK_PaymentAgentSetupDepId_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptDepIdReturnChk] ADD 
	CONSTRAINT [FK_PaymentAttemptDepIdReturnChk_PaymentAttemptDepIdReturn] FOREIGN KEY 
	(
		[paymentAttemptDepIdReturnId]
	) REFERENCES [dbo].[PaymentAttemptDepIdReturn] (
		[paymentAttemptDepIdReturnId]
	)
GO
ALTER TABLE [dbo].[PaymentAttemptVBVLog] ADD 
	CONSTRAINT [FK_PaymentAttemptVBVLog_PaymentAttemptVBV] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttemptVBV] (
		[paymentAttemptId]
	)
GO
ALTER TABLE [dbo].[PaymentAttemptDepId] ADD 
	CONSTRAINT [FK_PaymentAttemptDepId_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO
ALTER TABLE [dbo].[PaymentAttemptDepIdReturn] ADD 
	CONSTRAINT [FK_PaymentAttemptDepIdReturn_PaymentAttemptDepIdReturnHdr] FOREIGN KEY 
	(
		[headerDepIdentId]
	) REFERENCES [dbo].[PaymentAttemptDepIdReturnHdr] (
		[headerId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupDepId] ADD 
	CONSTRAINT [DF_PaymentAgentSetupDepId_expirationDays] DEFAULT (5) FOR [expirationDays]
GO


UPDATE _DBVERSION SET dbversion = 2
GO

IF @@ERROR <> 0
BEGIN
	PRINT(@@ERROR)
	ROLLBACK
END
ELSE
	PRINT('OK')
	COMMIT