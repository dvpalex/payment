BEGIN TRANSACTION

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_BillingSchedule_Billing]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[BillingSchedule] DROP CONSTRAINT FK_BillingSchedule_Billing
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttempt_BillingSchedule]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttempt] DROP CONSTRAINT FK_PaymentAttempt_BillingSchedule
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ConsumerAddress_Consumer]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ConsumerAddress] DROP CONSTRAINT FK_ConsumerAddress_Consumer
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Order_Consumer]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Order] DROP CONSTRAINT FK_Order_Consumer
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HandshakeConfigurationHtml_HandshakeConfiguration]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HandshakeConfigurationHtml] DROP CONSTRAINT FK_HandshakeConfigurationHtml_HandshakeConfiguration
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HandshakeConfigurationXml_HandshakeConfiguration]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HandshakeConfigurationXml] DROP CONSTRAINT FK_HandshakeConfigurationXml_HandshakeConfiguration
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HandshakeSessionLog_HandshakeSession]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HandshakeSessionLog] DROP CONSTRAINT FK_HandshakeSessionLog_HandshakeSession
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Billing_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Billing] DROP CONSTRAINT FK_Billing_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HandshakeSession_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HandshakeSession] DROP CONSTRAINT FK_HandshakeSession_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_OrderCreditCard_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[OrderCreditCard] DROP CONSTRAINT FK_OrderCreditCard_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_OrderInstallment_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[OrderInstallment] DROP CONSTRAINT FK_OrderInstallment_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_OrderItem_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT FK_OrderItem_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttempt_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttempt] DROP CONSTRAINT FK_PaymentAttempt_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Recurrence_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Recurrence] DROP CONSTRAINT FK_Recurrence_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Schedule_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Schedule] DROP CONSTRAINT FK_Schedule_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_WorkflowOrderStatus_Order]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[WorkflowOrderStatus] DROP CONSTRAINT FK_WorkflowOrderStatus_Order
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetup_PaymentAgent]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetup] DROP CONSTRAINT FK_PaymentAgentSetup_PaymentAgent
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentForm_PaymentAgent]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentForm] DROP CONSTRAINT FK_PaymentForm_PaymentAgent
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupABN_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupABN] DROP CONSTRAINT FK_PaymentAgentSetupABN_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupBB_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupBB] DROP CONSTRAINT FK_PaymentAgentSetupBB_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupBoleto_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupBoleto] DROP CONSTRAINT FK_PaymentAgentSetupBoleto_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupBradesco_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupBradesco] DROP CONSTRAINT FK_PaymentAgentSetupBradesco_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupItauShopline_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupItauShopline] DROP CONSTRAINT FK_PaymentAgentSetupItauShopline_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupKomerci_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupKomerci] DROP CONSTRAINT FK_PaymentAgentSetupKomerci_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupMoset_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupMoset] DROP CONSTRAINT FK_PaymentAgentSetupMoset_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupPaymentClientVirtual_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupPaymentClientVirtual] DROP CONSTRAINT FK_PaymentAgentSetupPaymentClientVirtual_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAgentSetupVBV_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAgentSetupVBV] DROP CONSTRAINT FK_PaymentAgentSetupVBV_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttempt_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttempt] DROP CONSTRAINT FK_PaymentAttempt_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_StorePaymentForm_PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[StorePaymentForm] DROP CONSTRAINT FK_StorePaymentForm_PaymentAgentSetup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptABN_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptABN] DROP CONSTRAINT FK_PaymentAttemptABN_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptBB_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptBB] DROP CONSTRAINT FK_PaymentAttemptBB_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptBoleto_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptBoleto] DROP CONSTRAINT FK_PaymentAttemptBoleto_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptBradesco_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptBradesco] DROP CONSTRAINT FK_PaymentAttemptBradesco_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptItauShopline_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptItauShopline] DROP CONSTRAINT FK_PaymentAttemptItauShopline_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptKomerci_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptKomerci] DROP CONSTRAINT FK_PaymentAttemptKomerci_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptMoset_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptMoset] DROP CONSTRAINT FK_PaymentAttemptMoset_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptPaymentClientVirtual_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptPaymentClientVirtual] DROP CONSTRAINT FK_PaymentAttemptPaymentClientVirtual_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptVBV_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptVBV] DROP CONSTRAINT FK_PaymentAttemptVBV_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ServiceFinalizationPost_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ServiceFinalizationPost] DROP CONSTRAINT FK_ServiceFinalizationPost_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ServicePaymentPost_PaymentAttempt]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ServicePaymentPost] DROP CONSTRAINT FK_ServicePaymentPost_PaymentAttempt
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttemptBoletoReturn_PaymentAttemptBoletoReturnHeader]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttemptBoletoReturn] DROP CONSTRAINT FK_PaymentAttemptBoletoReturn_PaymentAttemptBoletoReturnHeader
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Billing_PaymentForm]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Billing] DROP CONSTRAINT FK_Billing_PaymentForm
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_BillingSchedule_PaymentForm]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[BillingSchedule] DROP CONSTRAINT FK_BillingSchedule_PaymentForm
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_OrderInstallment_PaymentForm]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[OrderInstallment] DROP CONSTRAINT FK_OrderInstallment_PaymentForm
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentAttempt_PaymentForm]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentAttempt] DROP CONSTRAINT FK_PaymentAttempt_PaymentForm
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_StorePaymentForm_PaymentForm]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[StorePaymentForm] DROP CONSTRAINT FK_StorePaymentForm_PaymentForm
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentOptionInstallment_PaymentForm]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[StorePaymentInstallment] DROP CONSTRAINT FK_PaymentOptionInstallment_PaymentForm
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PaymentForm_PaymentFormGroup]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PaymentForm] DROP CONSTRAINT FK_PaymentForm_PaymentFormGroup
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Schedule_Recurrence]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Schedule] DROP CONSTRAINT FK_Schedule_Recurrence
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_UsersInRoles_Roles]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[UsersInRoles] DROP CONSTRAINT FK_UsersInRoles_Roles
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HandshakeSessionHtml_Store]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HandshakeSession] DROP CONSTRAINT FK_HandshakeSessionHtml_Store
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Order_Store]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Order] DROP CONSTRAINT FK_Order_Store
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ServicesConfiguration_Store]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ServicesConfiguration] DROP CONSTRAINT FK_ServicesConfiguration_Store
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_SPLegacyStore_Store]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[SPLegacyStore] DROP CONSTRAINT FK_SPLegacyStore_Store
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_StorePaymentForm_Store]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[StorePaymentForm] DROP CONSTRAINT FK_StorePaymentForm_Store
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_StorePaymentInstallment_Store]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[StorePaymentInstallment] DROP CONSTRAINT FK_StorePaymentInstallment_Store
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_UsersInStore_Store]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[UsersInStore] DROP CONSTRAINT FK_UsersInStore_Store
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ServiceEmailPaymentForm_StorePaymentForm]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ServiceEmailPaymentForm] DROP CONSTRAINT FK_ServiceEmailPaymentForm_StorePaymentForm
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Order_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Order] DROP CONSTRAINT FK_Order_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_UsersInRoles_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[UsersInRoles] DROP CONSTRAINT FK_UsersInRoles_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_UsersInStore_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[UsersInStore] DROP CONSTRAINT FK_UsersInStore_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ProcExtratoFinanceiro2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ProcExtratoFinanceiro2]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ProcPaidTransactions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ProcPaidTransactions]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ProcPaymentSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ProcPaymentSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ProcServicePaymentPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ProcServicePaymentPost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Billing]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Billing]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[BillingSchedule]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[BillingSchedule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Consumer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Consumer]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ConsumerAddress]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ConsumerAddress]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HandshakeConfiguration]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HandshakeConfiguration]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HandshakeConfigurationHtml]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HandshakeConfigurationHtml]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HandshakeConfigurationXml]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HandshakeConfigurationXml]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HandshakeSession]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HandshakeSession]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HandshakeSessionLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HandshakeSessionLog]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Order]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Order]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OrderCreditCard]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[OrderCreditCard]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OrderInstallment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[OrderInstallment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OrderItem]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[OrderItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgent]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgent]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupABN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupABN]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupBB]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupBB]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupBoleto]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupBoleto]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupBradesco]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupBradesco]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupItauShopline]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupItauShopline]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupKomerci]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupKomerci]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupMoset]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupMoset]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupPaymentClientVirtual]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupPaymentClientVirtual]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAgentSetupVBV]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAgentSetupVBV]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttempt]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttempt]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptABN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptABN]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptBB]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptBB]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptBoleto]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptBoleto]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptBoletoReturn]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptBoletoReturn]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptBoletoReturnHeader]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptBoletoReturnHeader]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptBradesco]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptBradesco]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptItauShopline]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptItauShopline]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptKomerci]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptKomerci]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptKomerciWS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptKomerciWS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptMoset]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptMoset]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptPaymentClientVirtual]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptPaymentClientVirtual]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptVBV]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptVBV]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentAttemptVBVLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentAttemptVBVLog]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentForm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentForm]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentFormGroup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentFormGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Recurrence]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Roles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Roles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SPLegacyDBImport]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SPLegacyDBImport]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SPLegacyPaymentForm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SPLegacyPaymentForm]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SPLegacyPaymentGroup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SPLegacyPaymentGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SPLegacyStore]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SPLegacyStore]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Schedule]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Schedule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ServiceEmailPaymentForm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ServiceEmailPaymentForm]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ServiceFinalizationPost]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ServiceFinalizationPost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ServicePaymentPost]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ServicePaymentPost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ServicesConfiguration]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ServicesConfiguration]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Store]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Store]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[StorePaymentForm]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[StorePaymentForm]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[StorePaymentInstallment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[StorePaymentInstallment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Users]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UsersInRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[UsersInRoles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UsersInStore]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[UsersInStore]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WorkflowOrderStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[WorkflowOrderStatus]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[_DBVERSION]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[_DBVERSION]
GO

CREATE TABLE [dbo].[Billing] (
	[billingId] [int] NOT NULL ,
	[billingGroup] [int] NOT NULL ,
	[orderId] [bigint] NOT NULL ,
	[isInfinityRecurrency] [bit] NOT NULL ,
	[recurrencyPaymentFormId] [int] NULL ,
	[recurrencyType] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[BillingSchedule] (
	[billingScheduleId] [int] NOT NULL ,
	[billingId] [int] NOT NULL ,
	[billingDate] [datetime] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[instalmentNumber] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Consumer] (
	[consumerId] [bigint] IDENTITY (1, 1) NOT NULL ,
	[CPF] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[RG] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[CNPJ] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[IE] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[birthDate] [datetime] NULL ,
	[ger] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[civilState] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[occupation] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[phone] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[commercialPhone] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[celularPhone] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[fax] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[responsibleName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[responsibleCPF] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[email] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ConsumerAddress] (
	[consumerAddressId] [bigint] IDENTITY (1, 1) NOT NULL ,
	[consumerId] [bigint] NOT NULL ,
	[addressType] [int] NOT NULL ,
	[logradouro] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[address] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[addressNumber] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[addressComplement] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[cep] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[district] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[city] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[state] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[country] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[HandshakeConfiguration] (
	[handshakeConfigurationId] [int] NOT NULL ,
	[storeId] [int] NOT NULL ,
	[handshakeType] [int] NOT NULL ,
	[autoPaymentConfirm] [bit] NOT NULL ,
	[acceptDuplicateOrder] [bit] NOT NULL ,
	[validateEmail] [bit] NOT NULL ,
	[sendEmailStoreKeeper] [bit] NOT NULL ,
	[sendEmailConsumer] [bit] NOT NULL ,
	[creationDate] [datetime] NOT NULL ,
	[lastUpdate] [datetime] NOT NULL ,
	[finalizationHtml] [bit] NOT NULL ,
	[paymentHtml] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[HandshakeConfigurationHtml] (
	[handshakeConfigurationId] [int] NOT NULL ,
	[urlHandshake] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlFinalization] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlFinalizationOffline] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlReturn] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlPaymentConfirmation] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlPaymentConfirmationOffline] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[requestEncoding] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[responseEncoding] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[HandshakeConfigurationXml] (
	[handshakeConfigurationId] [int] NOT NULL ,
	[urlHandshake] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlHandshakeError] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlFinalization] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlFinalizationOffline] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlReturn] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlPaymentConfirmation] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlPaymentConfirmationOffline] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[requestEncoding] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[responseEncoding] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[HandshakeSession] (
	[handshakeSessionId] [uniqueidentifier] NOT NULL ,
	[storeId] [int] NOT NULL ,
	[orderId] [bigint] NULL ,
	[handshakeType] [int] NOT NULL ,
	[createDate] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[HandshakeSessionLog] (
	[handshakeSessionId] [uniqueidentifier] NOT NULL ,
	[step] [int] NOT NULL ,
	[xmlData] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[url] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[createDate] [datetime] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Order] (
	[orderId] [bigint] IDENTITY (1, 1) NOT NULL ,
	[storeId] [int] NOT NULL ,
	[consumerId] [bigint] NULL ,
	[storeReferenceOrder] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[totalAmount] [numeric](18, 2) NOT NULL ,
	[finalAmount] [numeric](18, 2) NULL ,
	[installmentQuantity] [int] NULL ,
	[status] [int] NOT NULL ,
	[creationDate] [datetime] NOT NULL ,
	[lastUpdateDate] [datetime] NOT NULL ,
	[statusChangeUserId] [uniqueidentifier] NULL ,
	[statusChangeDate] [datetime] NULL ,
	[cancelDescription] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderCreditCard] (
	[orderId] [bigint] NOT NULL ,
	[securityNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cardHolderName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cardNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[dateExpiration] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderInstallment] (
	[orderId] [bigint] NOT NULL ,
	[installmentNumber] [int] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[installmentValue] [numeric](18, 2) NOT NULL ,
	[interestPercentage] [numeric](15, 5) NULL ,
	[status] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderItem] (
	[orderItemId] [bigint] IDENTITY (1, 1) NOT NULL ,
	[orderId] [bigint] NOT NULL ,
	[itemType] [int] NOT NULL ,
	[itemNumber] [int] NOT NULL ,
	[itemCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[itemDescription] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[itemQuantity] [int] NULL ,
	[itemValue] [numeric](18, 2) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgent] (
	[paymentAgentId] [int] NOT NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[webPage] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetup] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[paymentAgentId] [int] NULL ,
	[title] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupABN] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[codigoABN] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlAction] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlConsulta] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tipoFinanciamento] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tabelaFinanciamento] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[garantia] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupBB] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[agentOrderReferenceCounter] [int] NOT NULL ,
	[businessNumber] [int] NOT NULL ,
	[daysExpirationDate] [int] NOT NULL ,
	[urlBBPag] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[urlBBPagSonda] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupBoleto] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[bankNumber] [int] NOT NULL ,
	[bankDigit] [int] NOT NULL ,
	[agencyNumber] [int] NOT NULL ,
	[agencyDigit] [int] NOT NULL ,
	[accountNumber] [int] NOT NULL ,
	[accountDigit] [int] NOT NULL ,
	[cederCode] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cederName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cederCNPJ] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[wallet] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[conventionNumber] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[expirationDays] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupBradesco] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[bradescoUrl] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[businessNumber] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mngLogin] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mngPassword] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupItauShopline] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[criptoKey] [varchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[businessKey] [varchar] (26) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[urlItau] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[urlItauSonda] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupKomerci] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[businessNumber] [int] NOT NULL ,
	[instalmentType] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[urlKomerci] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[urlKomerciConfirm] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[urlKomerciAVS] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[checkAVS] [bit] NOT NULL ,
	[acceptedAVSReturn] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[AVSExceptionBINs] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupMoset] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[merchantId] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[autoCapture] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupPaymentClientVirtual] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[accessCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[secureHashSecret] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[merchantId] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[checkAVS] [bit] NOT NULL ,
	[acceptedAVSReturn] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[version] [varchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[captureUser] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[capturePassword] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[autoCapture] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAgentSetupVBV] (
	[paymentAgentSetupId] [int] NOT NULL ,
	[businessNumber] [int] NOT NULL ,
	[autoCapture] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttempt] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[paymentAgentSetupId] [int] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[price] [numeric](18, 2) NULL ,
	[orderId] [bigint] NOT NULL ,
	[startTime] [datetime] NOT NULL ,
	[lastUpdate] [datetime] NOT NULL ,
	[status] [int] NOT NULL ,
	[step] [int] NOT NULL ,
	[installmentNumber] [int] NULL ,
	[returnMessage] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[billingScheduleId] [int] NULL ,
	[isSimulation] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptABN] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[agentOrderReference] [int] IDENTITY (1, 1) NOT NULL ,
	[numControle] [numeric](18, 0) NULL ,
	[numProposta] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[statusProposta] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[qtdPrestacao] [int] NULL ,
	[prestacao] [numeric](18, 2) NULL ,
	[tabelaFinanciamento] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tipoPessoa] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[garantia] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[valorEntrada] [numeric](18, 2) NULL ,
	[dataVencimento] [datetime] NULL ,
	[codRet] [int] NULL ,
	[msgRet] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[abnStatus] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptBB] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[agentOrderReference] [int] IDENTITY (1, 1) NOT NULL ,
	[valor] [numeric](18, 2) NOT NULL ,
	[idConvenio] [int] NOT NULL ,
	[tipoPagamento] [tinyint] NOT NULL ,
	[dataPagamento] [datetime] NOT NULL ,
	[situacao] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[dataSonda] [datetime] NULL ,
	[qtdSonda] [smallint] NOT NULL ,
	[sondaOffline] [bit] NOT NULL ,
	[msgret] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[bbpagStatus] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptBoleto] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[agentOrderReference] [int] IDENTITY (1, 1) NOT NULL ,
	[documentNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[withdraw] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[withdrawDoc] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[address1] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[address2] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[address3] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[oct] [varchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[barCode] [varchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ourNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[instructions] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[paymentDate] [datetime] NOT NULL ,
	[expirationPaymentDate] [datetime] NOT NULL ,
	[paymentAttemptBoletoReturnId] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptBoletoReturn] (
	[paymentAttemptBoletoReturnId] [int] IDENTITY (1, 1) NOT NULL ,
	[paymentAttemptId] [uniqueidentifier] NULL ,
	[nossoNumero] [int] NULL ,
	[nossoNumeroDV] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[comando] [int] NULL ,
	[naturezaRecebimento] [int] NULL ,
	[dataLiquidacao] [datetime] NULL ,
	[valorTitulo] [numeric](18, 2) NULL ,
	[codigoBancoRecebedor] [int] NULL ,
	[prefixoAgenciaRecebedora] [int] NULL ,
	[digitoAgenciaRecebedora] [char] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[dataCredito] [datetime] NULL ,
	[valorTarifa] [numeric](18, 2) NULL ,
	[outrasDespesas] [numeric](18, 2) NULL ,
	[valorAbatimento] [numeric](18, 2) NULL ,
	[valorDescontoConcedido] [numeric](18, 2) NULL ,
	[valorRecebido] [numeric](18, 2) NULL ,
	[outrosRecebimentos] [numeric](18, 2) NULL ,
	[valorCreditoConta] [numeric](18, 2) NULL ,
	[jurosMora] [numeric](15, 5) NULL ,
	[indicativoCreditoDebito] [int] NULL ,
	[sequencialRegistro] [int] NULL ,
	[headerBoletoId] [int] NOT NULL ,
	[creationDate] [datetime] NOT NULL ,
	[status] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptBoletoReturnHeader] (
	[headerId] [int] IDENTITY (1, 1) NOT NULL ,
	[bankNumber] [int] NOT NULL ,
	[sequencialReturnNumber] [int] NOT NULL ,
	[recordFileDate] [datetime] NOT NULL ,
	[companyName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[agencyNumber] [int] NULL ,
	[agencyDV] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[assignorNumber] [int] NULL ,
	[assignorDV] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[companyCode] [int] NOT NULL ,
	[nameOfCapturedFile] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[creationDateCapturedFile] [datetime] NOT NULL ,
	[nameOfArquivedFile] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[processDate] [datetime] NOT NULL ,
	[numberOfDetailsRecords] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptBradesco] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[agentOrderReference] [int] IDENTITY (1, 1) NOT NULL ,
	[numOrder] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[merchantid] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tipoPagto] [int] NULL ,
	[prazo] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numparc] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[valparc] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[valtotal] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[cod] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ccname] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ccemail] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[cctype] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[assinatura] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[bradescoStatus] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptItauShopline] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[agentOrderReference] [int] IDENTITY (1, 1) NOT NULL ,
	[codEmp] [char] (26) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[valor] [char] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[chave] [char] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[dc] [varchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tipPag] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[sitPag] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[dtPag] [char] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[codAut] [char] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numId] [char] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[compVend] [char] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tipCart] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[dataSonda] [datetime] NULL ,
	[qtdSonda] [smallint] NOT NULL ,
	[sondaOffline] [bit] NOT NULL ,
	[msgret] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[itauStatus] [tinyint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptKomerci] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[agentOrderReference] [int] IDENTITY (1, 1) NOT NULL ,
	[transacao] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[bandeira] [char] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[codver] [char] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[data] [char] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[nr_cartao] [char] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[origem_bin] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numautor] [char] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numcv] [char] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numautent] [char] (27) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numsqn] [char] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[codret] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[msgret] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[avs] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[respavs] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[msgavs] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[komerciStatus] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptKomerciWS] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[transacao] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[autoCapture] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[data] [char] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[cardInformation] [varchar] (5000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numautor] [char] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numcv] [char] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numautent] [char] (27) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[numsqn] [char] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[codret] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[msgret] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[capcodret] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[capmsgret] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[avs] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[respavs] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[msgavs] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[komerciStatus] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptMoset] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[merchantId] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cardInformation] [varchar] (5000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[lr] [int] NULL ,
	[tid] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[free] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[capturedCod] [int] NULL ,
	[capturedTid] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[capturedArs] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[capturedCap] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[capturedValue] [numeric](18, 0) NULL ,
	[capturedCurrency] [int] NULL ,
	[message] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[mosetStatus] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptPaymentClientVirtual] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[agentOrderReference] [int] IDENTITY (1, 1) NOT NULL ,
	[purchaseAmount] [numeric](18, 2) NULL ,
	[cardInformation] [varchar] (5000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[signatureCreated] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[avs] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_Version] [varchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_AuthorizeId] [int] NULL ,
	[vpc_AVS_Street01] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_AVS_PostCode] [varchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_AVSResultCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_AcqAVSRespCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_AcqCSCRespCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_AcqResponseCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_BatchNo] [int] NULL ,
	[vpc_CSCResultCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_Card] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_Message] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_CaptureMessage] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_ReceiptNo] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_SecureHash] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_TransactionNo] [int] NULL ,
	[vpc_CapTransactionNo] [int] NULL ,
	[vpc_TxnResponseCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_CapTxnResponseCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_ShopTransactionNo] [varchar] (19) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_AuthorisedAmount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_CapturedAmount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vpc_TicketNumber] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[paymentClientVirtualiStatus] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptVBV] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[tidMaster] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tid] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[lr] [numeric](4, 0) NULL ,
	[arp] [int] NULL ,
	[ars] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vbvOrderId] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[price] [int] NULL ,
	[free] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[pan] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[bank] [int] NULL ,
	[authent] [int] NULL ,
	[cap] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[dataSonda] [datetime] NULL ,
	[qtdSonda] [smallint] NOT NULL ,
	[sondaOffline] [bit] NOT NULL ,
	[vbvStatus] [tinyint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentAttemptVBVLog] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[interfaceType] [smallint] NOT NULL ,
	[returnDate] [datetime] NOT NULL ,
	[tidMaster] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tid] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[lr] [numeric](4, 0) NULL ,
	[arp] [int] NULL ,
	[ars] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[vbvOrderId] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[price] [int] NULL ,
	[free] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[pan] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[bank] [int] NULL ,
	[authent] [int] NULL ,
	[cap] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentForm] (
	[paymentFormId] [int] NOT NULL ,
	[paymentFormGroupId] [int] NOT NULL ,
	[paymentAgentId] [int] NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaymentFormGroup] (
	[paymentFormGroupId] [int] NOT NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Recurrence] (
	[recurrenceId] [int] IDENTITY (1, 1) NOT NULL ,
	[orderId] [bigint] NOT NULL ,
	[interval] [int] NOT NULL ,
	[startDate] [datetime] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[paymentFormDetail] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[status] [tinyint] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Roles] (
	[RoleId] [uniqueidentifier] NOT NULL ,
	[RoleName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LoweredRoleName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SPLegacyDBImport] (
	[INT_NUMERO_PEDIDO] [numeric](20, 0) NOT NULL ,
	[orderId] [bigint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SPLegacyPaymentForm] (
	[storeId] [int] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[ucInstructions] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SPLegacyPaymentGroup] (
	[storeId] [int] NOT NULL ,
	[paymentFormGroupId] [int] NOT NULL ,
	[ucInstructions] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SPLegacyStore] (
	[storeId] [int] NOT NULL ,
	[ucTableTop] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Schedule] (
	[scheduleId] [int] IDENTITY (1, 1) NOT NULL ,
	[orderId] [bigint] NOT NULL ,
	[recurrenceId] [int] NULL ,
	[installmentNumber] [int] NULL ,
	[installmentType] [int] NULL ,
	[date] [datetime] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[paymentFormDetail] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[status] [tinyint] NOT NULL ,
	[paymentAttemptId] [uniqueidentifier] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[ServiceEmailPaymentForm] (
	[storeId] [int] NOT NULL ,
	[emailType] [tinyint] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[idioma] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[sendHtml] [bit] NOT NULL ,
	[encoding] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[bodyTemplate] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[subjectTemplate] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[itensTemplate] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[installmentTemplate] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[toField] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ccField] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[fromField] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[ServiceFinalizationPost] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[postStatus] [int] NOT NULL ,
	[postRetries] [int] NOT NULL ,
	[lastUpdate] [datetime] NOT NULL ,
	[emailSentDate] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ServicePaymentPost] (
	[paymentAttemptId] [uniqueidentifier] NOT NULL ,
	[installmentNumber] [int] NOT NULL ,
	[postStatus] [int] NOT NULL ,
	[postRetries] [int] NOT NULL ,
	[lastUpdate] [datetime] NOT NULL ,
	[emailSentDate] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ServicesConfiguration] (
	[storeId] [int] NOT NULL ,
	[offLineFinalizationRetries] [int] NOT NULL ,
	[offLinePaymentRetries] [int] NOT NULL ,
	[contingencyEmails] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[urlBoletoRetry] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Store] (
	[storeId] [int] NOT NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[urlSite] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[storeKey] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[password] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[mailSenderEmail] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[handshakeConfigurationId] [int] NOT NULL ,
	[creationDate] [datetime] NOT NULL ,
	[lastUpdate] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[StorePaymentForm] (
	[storeId] [int] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[paymentAgentSetupId] [int] NULL ,
	[showInCombo] [bit] NOT NULL ,
	[useTestValues] [bit] NOT NULL ,
	[isActive] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[StorePaymentInstallment] (
	[storeId] [int] NOT NULL ,
	[paymentFormId] [int] NOT NULL ,
	[installmentNumber] [tinyint] NOT NULL ,
	[description] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[minValue] [numeric](18, 2) NULL ,
	[maxValue] [numeric](18, 2) NULL ,
	[interestPercentage] [numeric](15, 5) NOT NULL ,
	[installmentType] [tinyint] NOT NULL ,
	[allowInParcialPayment] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Users] (
	[UserId] [uniqueidentifier] NOT NULL ,
	[Username] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LoweredUsername] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Password] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[PasswordFormat] [int] NOT NULL ,
	[Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LoweredEmail] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[PasswordQuestion] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PasswordAnswer] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[IsApproved] [bit] NOT NULL ,
	[IsLockedOut] [bit] NOT NULL ,
	[IsOnLine] [bit] NOT NULL ,
	[CreateDate] [datetime] NOT NULL ,
	[LastLoginDate] [datetime] NOT NULL ,
	[LastPasswordChangedDate] [datetime] NOT NULL ,
	[LastLockedOutDate] [datetime] NOT NULL ,
	[LastActivityDate] [datetime] NOT NULL ,
	[FailedPasswordAttemptCount] [int] NOT NULL ,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL ,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL ,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL ,
	[Comment] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[UsersInRoles] (
	[UserId] [uniqueidentifier] NOT NULL ,
	[RoleId] [uniqueidentifier] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[UsersInStore] (
	[UserId] [uniqueidentifier] NOT NULL ,
	[storeId] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[WorkflowOrderStatus] (
	[orderId] [bigint] NOT NULL ,
	[status] [int] NOT NULL ,
	[text] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[creationDate] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[_DBVERSION] (
	[dbversion] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Billing] WITH NOCHECK ADD 
	CONSTRAINT [PK_Billing] PRIMARY KEY  CLUSTERED 
	(
		[billingId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[BillingSchedule] WITH NOCHECK ADD 
	CONSTRAINT [PK_BillingSchedule] PRIMARY KEY  CLUSTERED 
	(
		[billingScheduleId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Consumer] WITH NOCHECK ADD 
	CONSTRAINT [PK_Consumer] PRIMARY KEY  CLUSTERED 
	(
		[consumerId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ConsumerAddress] WITH NOCHECK ADD 
	CONSTRAINT [PK_ConsumerAddress] PRIMARY KEY  CLUSTERED 
	(
		[consumerAddressId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[HandshakeConfiguration] WITH NOCHECK ADD 
	CONSTRAINT [PK_HandshakeConfiguration] PRIMARY KEY  CLUSTERED 
	(
		[handshakeConfigurationId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[HandshakeConfigurationHtml] WITH NOCHECK ADD 
	CONSTRAINT [PK_HandshakeConfigurationHtml] PRIMARY KEY  CLUSTERED 
	(
		[handshakeConfigurationId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[HandshakeConfigurationXml] WITH NOCHECK ADD 
	CONSTRAINT [PK_HandshakeConfigurationXml] PRIMARY KEY  CLUSTERED 
	(
		[handshakeConfigurationId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[HandshakeSession] WITH NOCHECK ADD 
	CONSTRAINT [PK_HandshakeSessionHtml] PRIMARY KEY  CLUSTERED 
	(
		[handshakeSessionId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Order] WITH NOCHECK ADD 
	CONSTRAINT [PK_Order] PRIMARY KEY  CLUSTERED 
	(
		[orderId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[OrderCreditCard] WITH NOCHECK ADD 
	CONSTRAINT [PK_OrderGateway] PRIMARY KEY  CLUSTERED 
	(
		[orderId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[OrderInstallment] WITH NOCHECK ADD 
	CONSTRAINT [PK_OrderInstallment] PRIMARY KEY  CLUSTERED 
	(
		[orderId],
		[installmentNumber]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[OrderItem] WITH NOCHECK ADD 
	CONSTRAINT [PK_OrderItem] PRIMARY KEY  CLUSTERED 
	(
		[orderItemId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgent] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgent] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetup] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetup] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupABN] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetupABN] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupBB] WITH NOCHECK ADD 
	CONSTRAINT [PK_StorePaymentAgentBB] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupBoleto] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentBoletoBB] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupBradesco] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetupBradesco] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupItauShopline] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetupItauShopline] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupKomerci] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetupKomerci] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupMoset] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetupMoset] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupPaymentClientVirtual] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetupPaymentClientVirtual] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAgentSetupVBV] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAgentSetupVBV] PRIMARY KEY  CLUSTERED 
	(
		[paymentAgentSetupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttempt] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttempt] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptABN] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptABN] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptBB] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptBB] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptBoleto] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptBoleto] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptBoletoReturn] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptBoletoReturn] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptBoletoReturnId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptBoletoReturnHeader] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptBoletoReturnHeader] PRIMARY KEY  CLUSTERED 
	(
		[headerId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptBradesco] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptBradesco] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptItauShopline] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptItauShopline] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptKomerci] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptKomerci] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptKomerciWS] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptKomerciWS] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptMoset] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptMoset] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptPaymentClientVirtual] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptPaymentClientVirtual] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentAttemptVBV] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentAttemptVBV] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentForm] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentForm] PRIMARY KEY  CLUSTERED 
	(
		[paymentFormId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PaymentFormGroup] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentFormGroup] PRIMARY KEY  CLUSTERED 
	(
		[paymentFormGroupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Recurrence] WITH NOCHECK ADD 
	CONSTRAINT [PK_Recurrence] PRIMARY KEY  CLUSTERED 
	(
		[recurrenceId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Roles] WITH NOCHECK ADD 
	CONSTRAINT [PK_Roles] PRIMARY KEY  CLUSTERED 
	(
		[RoleId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SPLegacyDBImport] WITH NOCHECK ADD 
	CONSTRAINT [PK_SPLegacyDBImport] PRIMARY KEY  CLUSTERED 
	(
		[INT_NUMERO_PEDIDO]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SPLegacyPaymentForm] WITH NOCHECK ADD 
	CONSTRAINT [PK_SPLegacyPaymentForm] PRIMARY KEY  CLUSTERED 
	(
		[storeId],
		[paymentFormId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SPLegacyPaymentGroup] WITH NOCHECK ADD 
	CONSTRAINT [PK_SPLegacyPaymentGroup] PRIMARY KEY  CLUSTERED 
	(
		[storeId],
		[paymentFormGroupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SPLegacyStore] WITH NOCHECK ADD 
	CONSTRAINT [PK_SPLegacyStore] PRIMARY KEY  CLUSTERED 
	(
		[storeId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Schedule] WITH NOCHECK ADD 
	CONSTRAINT [PK_Schedule] PRIMARY KEY  CLUSTERED 
	(
		[scheduleId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ServiceEmailPaymentForm] WITH NOCHECK ADD 
	CONSTRAINT [PK_ServiceEmailPaymentForm] PRIMARY KEY  CLUSTERED 
	(
		[storeId],
		[emailType],
		[paymentFormId],
		[idioma]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ServiceFinalizationPost] WITH NOCHECK ADD 
	CONSTRAINT [PK_ServiceFinalizationPost] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ServicePaymentPost] WITH NOCHECK ADD 
	CONSTRAINT [PK_ServicePaymentPost] PRIMARY KEY  CLUSTERED 
	(
		[paymentAttemptId],
		[installmentNumber]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ServicesConfiguration] WITH NOCHECK ADD 
	CONSTRAINT [PK_ServicesConfiguration] PRIMARY KEY  CLUSTERED 
	(
		[storeId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Store] WITH NOCHECK ADD 
	CONSTRAINT [PK_Store] PRIMARY KEY  CLUSTERED 
	(
		[storeId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[StorePaymentForm] WITH NOCHECK ADD 
	CONSTRAINT [PK_StorePaymentForm] PRIMARY KEY  CLUSTERED 
	(
		[storeId],
		[paymentFormId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[StorePaymentInstallment] WITH NOCHECK ADD 
	CONSTRAINT [PK_StorePaymentInstallment] PRIMARY KEY  CLUSTERED 
	(
		[storeId],
		[paymentFormId],
		[installmentNumber]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Users] WITH NOCHECK ADD 
	CONSTRAINT [PK_Users] PRIMARY KEY  CLUSTERED 
	(
		[UserId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[UsersInRoles] WITH NOCHECK ADD 
	CONSTRAINT [PK_UsersInRoles] PRIMARY KEY  CLUSTERED 
	(
		[UserId],
		[RoleId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[UsersInStore] WITH NOCHECK ADD 
	CONSTRAINT [PK_UsersInStore] PRIMARY KEY  CLUSTERED 
	(
		[UserId],
		[storeId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[HandshakeConfiguration] WITH NOCHECK ADD 
	CONSTRAINT [DF_HandshakeConfiguration_acceptDuplicateOrder] DEFAULT (0) FOR [acceptDuplicateOrder],
	CONSTRAINT [DF_HandshakeConfiguration_validateEmail] DEFAULT (0) FOR [validateEmail],
	CONSTRAINT [DF_HandshakeConfiguration_sEmailStoreKeeper] DEFAULT (0) FOR [sendEmailStoreKeeper],
	CONSTRAINT [DF_HandshakeConfiguration_sEmailConsumer] DEFAULT (0) FOR [sendEmailConsumer]
GO

ALTER TABLE [dbo].[PaymentAttempt] WITH NOCHECK ADD 
	CONSTRAINT [DF_PaymentAttempt_status] DEFAULT (0) FOR [status],
	CONSTRAINT [DF_PaymentAttempt_isSimulation] DEFAULT (0) FOR [isSimulation]
GO

ALTER TABLE [dbo].[PaymentAttemptVBV] WITH NOCHECK ADD 
	CONSTRAINT [DF_PaymentAttemptVBV_qtdSonda] DEFAULT (0) FOR [qtdSonda],
	CONSTRAINT [DF_PaymentAttemptVBV_sondaOffline] DEFAULT (0) FOR [sondaOffline]
GO

ALTER TABLE [dbo].[Recurrence] WITH NOCHECK ADD 
	CONSTRAINT [IX_Recurrence] UNIQUE  NONCLUSTERED 
	(
		[orderId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Roles] WITH NOCHECK ADD 
	CONSTRAINT [IX_Roles] UNIQUE  NONCLUSTERED 
	(
		[LoweredRoleName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ServiceEmailPaymentForm] WITH NOCHECK ADD 
	CONSTRAINT [DF_ServiceEmailPaymentForm_sendHtml] DEFAULT (0) FOR [sendHtml]
GO

ALTER TABLE [dbo].[Store] WITH NOCHECK ADD 
	CONSTRAINT [IX_Store] UNIQUE  NONCLUSTERED 
	(
		[storeKey]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[StorePaymentForm] WITH NOCHECK ADD 
	CONSTRAINT [DF_StorePaymentForm_showInCombo] DEFAULT (1) FOR [showInCombo],
	CONSTRAINT [DF_StorePaymentForm_useTestValues] DEFAULT (0) FOR [useTestValues]
GO

ALTER TABLE [dbo].[StorePaymentInstallment] WITH NOCHECK ADD 
	CONSTRAINT [DF_StorePaymentInstallment_installmentType] DEFAULT (1) FOR [installmentType],
	CONSTRAINT [DF_PaymentOptionInstallment_allowInParcialPayment] DEFAULT (0) FOR [allowInParcialPayment]
GO

ALTER TABLE [dbo].[Users] WITH NOCHECK ADD 
	CONSTRAINT [IX_Users_1] UNIQUE  NONCLUSTERED 
	(
		[LoweredUsername]
	)  ON [PRIMARY] ,
	CONSTRAINT [IX_Users_2] UNIQUE  NONCLUSTERED 
	(
		[LoweredEmail]
	)  ON [PRIMARY] 
GO

 CREATE  UNIQUE  INDEX [IX_HandshakeConfiguration] ON [dbo].[HandshakeConfiguration]([storeId], [handshakeType]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [IX_PaymentAttemptBB] ON [dbo].[PaymentAttemptBB]([agentOrderReference]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [IX_PaymentAttemptBoleto] ON [dbo].[PaymentAttemptBoleto]([agentOrderReference]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [IX_PaymentAttemptItauShopline] ON [dbo].[PaymentAttemptItauShopline]([agentOrderReference]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [IX_PaymentAttemptKomerci] ON [dbo].[PaymentAttemptKomerci]([agentOrderReference]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_ScheduleDate] ON [dbo].[Schedule]([date] DESC ) ON [PRIMARY]
GO

 CREATE  INDEX [IX_ServicePaymentPost] ON [dbo].[ServicePaymentPost]([postRetries]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Users] ON [dbo].[Users]([UserId]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Billing] ADD 
	CONSTRAINT [FK_Billing_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	),
	CONSTRAINT [FK_Billing_PaymentForm] FOREIGN KEY 
	(
		[recurrencyPaymentFormId]
	) REFERENCES [dbo].[PaymentForm] (
		[paymentFormId]
	)
GO

ALTER TABLE [dbo].[BillingSchedule] ADD 
	CONSTRAINT [FK_BillingSchedule_Billing] FOREIGN KEY 
	(
		[billingId]
	) REFERENCES [dbo].[Billing] (
		[billingId]
	),
	CONSTRAINT [FK_BillingSchedule_PaymentForm] FOREIGN KEY 
	(
		[paymentFormId]
	) REFERENCES [dbo].[PaymentForm] (
		[paymentFormId]
	)
GO

ALTER TABLE [dbo].[ConsumerAddress] ADD 
	CONSTRAINT [FK_ConsumerAddress_Consumer] FOREIGN KEY 
	(
		[consumerId]
	) REFERENCES [dbo].[Consumer] (
		[consumerId]
	)
GO

ALTER TABLE [dbo].[HandshakeConfigurationHtml] ADD 
	CONSTRAINT [FK_HandshakeConfigurationHtml_HandshakeConfiguration] FOREIGN KEY 
	(
		[handshakeConfigurationId]
	) REFERENCES [dbo].[HandshakeConfiguration] (
		[handshakeConfigurationId]
	)
GO

ALTER TABLE [dbo].[HandshakeConfigurationXml] ADD 
	CONSTRAINT [FK_HandshakeConfigurationXml_HandshakeConfiguration] FOREIGN KEY 
	(
		[handshakeConfigurationId]
	) REFERENCES [dbo].[HandshakeConfiguration] (
		[handshakeConfigurationId]
	)
GO

ALTER TABLE [dbo].[HandshakeSession] ADD 
	CONSTRAINT [FK_HandshakeSession_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	),
	CONSTRAINT [FK_HandshakeSessionHtml_Store] FOREIGN KEY 
	(
		[storeId]
	) REFERENCES [dbo].[Store] (
		[storeId]
	)
GO

ALTER TABLE [dbo].[HandshakeSessionLog] ADD 
	CONSTRAINT [FK_HandshakeSessionLog_HandshakeSession] FOREIGN KEY 
	(
		[handshakeSessionId]
	) REFERENCES [dbo].[HandshakeSession] (
		[handshakeSessionId]
	)
GO

ALTER TABLE [dbo].[Order] ADD 
	CONSTRAINT [FK_Order_Consumer] FOREIGN KEY 
	(
		[consumerId]
	) REFERENCES [dbo].[Consumer] (
		[consumerId]
	),
	CONSTRAINT [FK_Order_Store] FOREIGN KEY 
	(
		[storeId]
	) REFERENCES [dbo].[Store] (
		[storeId]
	),
	CONSTRAINT [FK_Order_Users] FOREIGN KEY 
	(
		[statusChangeUserId]
	) REFERENCES [dbo].[Users] (
		[UserId]
	)
GO

ALTER TABLE [dbo].[OrderCreditCard] ADD 
	CONSTRAINT [FK_OrderCreditCard_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	)
GO

ALTER TABLE [dbo].[OrderInstallment] ADD 
	CONSTRAINT [FK_OrderInstallment_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	),
	CONSTRAINT [FK_OrderInstallment_PaymentForm] FOREIGN KEY 
	(
		[paymentFormId]
	) REFERENCES [dbo].[PaymentForm] (
		[paymentFormId]
	)
GO

ALTER TABLE [dbo].[OrderItem] ADD 
	CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetup] ADD 
	CONSTRAINT [FK_PaymentAgentSetup_PaymentAgent] FOREIGN KEY 
	(
		[paymentAgentId]
	) REFERENCES [dbo].[PaymentAgent] (
		[paymentAgentId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupABN] ADD 
	CONSTRAINT [FK_PaymentAgentSetupABN_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupBB] ADD 
	CONSTRAINT [FK_PaymentAgentSetupBB_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupBoleto] ADD 
	CONSTRAINT [FK_PaymentAgentSetupBoleto_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupBradesco] ADD 
	CONSTRAINT [FK_PaymentAgentSetupBradesco_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupItauShopline] ADD 
	CONSTRAINT [FK_PaymentAgentSetupItauShopline_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupKomerci] ADD 
	CONSTRAINT [FK_PaymentAgentSetupKomerci_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupMoset] ADD 
	CONSTRAINT [FK_PaymentAgentSetupMoset_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupPaymentClientVirtual] ADD 
	CONSTRAINT [FK_PaymentAgentSetupPaymentClientVirtual_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAgentSetupVBV] ADD 
	CONSTRAINT [FK_PaymentAgentSetupVBV_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	)
GO

ALTER TABLE [dbo].[PaymentAttempt] ADD 
	CONSTRAINT [FK_PaymentAttempt_BillingSchedule] FOREIGN KEY 
	(
		[billingScheduleId]
	) REFERENCES [dbo].[BillingSchedule] (
		[billingScheduleId]
	),
	CONSTRAINT [FK_PaymentAttempt_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	),
	CONSTRAINT [FK_PaymentAttempt_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	),
	CONSTRAINT [FK_PaymentAttempt_PaymentForm] FOREIGN KEY 
	(
		[paymentFormId]
	) REFERENCES [dbo].[PaymentForm] (
		[paymentFormId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptABN] ADD 
	CONSTRAINT [FK_PaymentAttemptABN_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptBB] ADD 
	CONSTRAINT [FK_PaymentAttemptBB_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptBoleto] ADD 
	CONSTRAINT [FK_PaymentAttemptBoleto_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptBoletoReturn] ADD 
	CONSTRAINT [FK_PaymentAttemptBoletoReturn_PaymentAttemptBoletoReturnHeader] FOREIGN KEY 
	(
		[headerBoletoId]
	) REFERENCES [dbo].[PaymentAttemptBoletoReturnHeader] (
		[headerId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptBradesco] ADD 
	CONSTRAINT [FK_PaymentAttemptBradesco_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptItauShopline] ADD 
	CONSTRAINT [FK_PaymentAttemptItauShopline_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptKomerci] ADD 
	CONSTRAINT [FK_PaymentAttemptKomerci_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptMoset] ADD 
	CONSTRAINT [FK_PaymentAttemptMoset_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptPaymentClientVirtual] ADD 
	CONSTRAINT [FK_PaymentAttemptPaymentClientVirtual_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentAttemptVBV] ADD 
	CONSTRAINT [FK_PaymentAttemptVBV_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[PaymentForm] ADD 
	CONSTRAINT [FK_PaymentForm_PaymentAgent] FOREIGN KEY 
	(
		[paymentAgentId]
	) REFERENCES [dbo].[PaymentAgent] (
		[paymentAgentId]
	),
	CONSTRAINT [FK_PaymentForm_PaymentFormGroup] FOREIGN KEY 
	(
		[paymentFormGroupId]
	) REFERENCES [dbo].[PaymentFormGroup] (
		[paymentFormGroupId]
	)
GO

ALTER TABLE [dbo].[Recurrence] ADD 
	CONSTRAINT [FK_Recurrence_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	)
GO

ALTER TABLE [dbo].[SPLegacyStore] ADD 
	CONSTRAINT [FK_SPLegacyStore_Store] FOREIGN KEY 
	(
		[storeId]
	) REFERENCES [dbo].[Store] (
		[storeId]
	)
GO

ALTER TABLE [dbo].[Schedule] ADD 
	CONSTRAINT [FK_Schedule_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	),
	CONSTRAINT [FK_Schedule_Recurrence] FOREIGN KEY 
	(
		[recurrenceId]
	) REFERENCES [dbo].[Recurrence] (
		[recurrenceId]
	)
GO

ALTER TABLE [dbo].[ServiceEmailPaymentForm] ADD 
	CONSTRAINT [FK_ServiceEmailPaymentForm_StorePaymentForm] FOREIGN KEY 
	(
		[storeId],
		[paymentFormId]
	) REFERENCES [dbo].[StorePaymentForm] (
		[storeId],
		[paymentFormId]
	)
GO

ALTER TABLE [dbo].[ServiceFinalizationPost] ADD 
	CONSTRAINT [FK_ServiceFinalizationPost_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[ServicePaymentPost] ADD 
	CONSTRAINT [FK_ServicePaymentPost_PaymentAttempt] FOREIGN KEY 
	(
		[paymentAttemptId]
	) REFERENCES [dbo].[PaymentAttempt] (
		[paymentAttemptId]
	)
GO

ALTER TABLE [dbo].[ServicesConfiguration] ADD 
	CONSTRAINT [FK_ServicesConfiguration_Store] FOREIGN KEY 
	(
		[storeId]
	) REFERENCES [dbo].[Store] (
		[storeId]
	)
GO

ALTER TABLE [dbo].[StorePaymentForm] ADD 
	CONSTRAINT [FK_StorePaymentForm_PaymentAgentSetup] FOREIGN KEY 
	(
		[paymentAgentSetupId]
	) REFERENCES [dbo].[PaymentAgentSetup] (
		[paymentAgentSetupId]
	),
	CONSTRAINT [FK_StorePaymentForm_PaymentForm] FOREIGN KEY 
	(
		[paymentFormId]
	) REFERENCES [dbo].[PaymentForm] (
		[paymentFormId]
	),
	CONSTRAINT [FK_StorePaymentForm_Store] FOREIGN KEY 
	(
		[storeId]
	) REFERENCES [dbo].[Store] (
		[storeId]
	)
GO

ALTER TABLE [dbo].[StorePaymentInstallment] ADD 
	CONSTRAINT [FK_PaymentOptionInstallment_PaymentForm] FOREIGN KEY 
	(
		[paymentFormId]
	) REFERENCES [dbo].[PaymentForm] (
		[paymentFormId]
	),
	CONSTRAINT [FK_StorePaymentInstallment_Store] FOREIGN KEY 
	(
		[storeId]
	) REFERENCES [dbo].[Store] (
		[storeId]
	)
GO

ALTER TABLE [dbo].[UsersInRoles] ADD 
	CONSTRAINT [FK_UsersInRoles_Roles] FOREIGN KEY 
	(
		[RoleId]
	) REFERENCES [dbo].[Roles] (
		[RoleId]
	),
	CONSTRAINT [FK_UsersInRoles_Users] FOREIGN KEY 
	(
		[UserId]
	) REFERENCES [dbo].[Users] (
		[UserId]
	)
GO

ALTER TABLE [dbo].[UsersInStore] ADD 
	CONSTRAINT [FK_UsersInStore_Store] FOREIGN KEY 
	(
		[storeId]
	) REFERENCES [dbo].[Store] (
		[storeId]
	),
	CONSTRAINT [FK_UsersInStore_Users] FOREIGN KEY 
	(
		[UserId]
	) REFERENCES [dbo].[Users] (
		[UserId]
	)
GO

ALTER TABLE [dbo].[WorkflowOrderStatus] ADD 
	CONSTRAINT [FK_WorkflowOrderStatus_Order] FOREIGN KEY 
	(
		[orderId]
	) REFERENCES [dbo].[Order] (
		[orderId]
	)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [DBO].[ProcExtratoFinanceiro2]
(
@dataInicial datetime,
@dataFinal datetime,
@storeId int
)
as

begin

select
	substring(o.storereferenceorder, 0, 3) + '-' + substring(o.storereferenceorder, 3, len(o.storereferenceorder) - 2) as pedido,
	--vbv
	pav.arp,
	pav.tid,
	--komerci
	pak.numautor,
	pak.numcv,
	pak.numautent,
	--boleto
	pab.agentorderreference as 'nossoNumero',

	oi.installmentvalue as valorParcela,
	o.finalamount as valorPedido,

	--boleto
	pabr.valorRecebido,
	case
		when pa.price > pabr.valorRecebido then 'Inconsistente'
		when pa.price < pabr.valorRecebido then 'Pago Inconsistente'
		when pa.price = pabr.valorRecebido then 'Ok'
	end as statusConciliacao,

	pabr.valorRecebido - 1.5 as 'valorLiq',

--	substring(o.storereferenceorder, 0, 3) + '-' + substring(o.storereferenceorder, 3, len(o.storereferenceorder) - 2) as pedido,
	o.creationDate as dataEntradaPedido,
	case
		when pa.paymentformid = 9 then pab.expirationPaymentDate
		else pa.lastUpdate
	end as dataParcela,
	case
		when pa.status in (2,5) and pa.paymentformid = 9 then cast(day(pabr.dataLiquidacao) as varchar) + '/' + cast(month(pabr.dataLiquidacao) as varchar) + '/' + cast(year(pabr.dataLiquidacao) as varchar)
		when pa.status = 2 and pa.paymentformid <> 9 then cast(day(pa.lastUpdate) as varchar) + '/' + cast(month(pa.lastUpdate) as varchar) + '/' + cast(year(pa.lastUpdate) as varchar)
		else NULL
	end as dataPagamento,
	case
		when pa.status = 2 then 'Pago'
		when pa.status = 3 then 'Não Pago'
		when pa.status = 4 then 'Cancelado'
		when pa.status = 5 then 'Pendente'
		else 'Não Concluido'
	end as statusCobranca,
	pf.[name] as formaPagamento,
	o.installmentQuantity as qtdParcelas,

	case
		when sfp.postStatus = 2 then 'Enviado'
		when sfp.postStatus = 3 then 'Enviando'
		when sfp.postStatus = 4 then 'Enviado e Confirmado'
		else 'Não Enviado'
	end as statusPostFinalizacao,

	case
		when spp.postStatus = 2 then 'Enviado'
		when spp.postStatus = 3 then 'Enviando'
		when spp.postStatus = 4 then 'Enviado e Confirmado'
		else 'Não Enviado'
	end as statusPostPagamento
from [order] o
left join paymentattempt pa on o.orderid = pa.orderid
left join orderinstallment oi on oi.orderid = pa.orderid
left join paymentform pf on pf.paymentformid = pa.paymentformid
left join paymentformgroup pfg on pfg.paymentformgroupid = pf.paymentformgroupid
left join paymentattemptvbv pav on pa.paymentattemptid = pav.paymentattemptid
left join paymentattemptkomerci pak on pa.paymentattemptid = pak.paymentattemptid
left join paymentattemptitaushopline pais on pa.paymentattemptid = pais.paymentattemptid
left join paymentattemptboleto pab on pa.paymentattemptid = pab.paymentattemptid
left join paymentattemptboletoreturn pabr on pab.agentorderreference = pabr.nossoNumero
left join servicepaymentpost spp on spp.paymentattemptid = pa.paymentattemptid
left join servicefinalizationpost sfp on sfp.paymentattemptid = pa.paymentattemptid
where 
	pa.paymentattemptid in	(
				select pa.paymentattemptid
				from paymentattempt pa
				join [order] o on o.orderid = pa.orderid
				where
					pa.paymentformid not in (9)
					and pa.lastUpdate between @dataInicial and @dataFinal
					and pa.status = 2
					and o.storeid = @storeId
				union
				select distinct	pa.paymentattemptid
				from paymentattemptboletoreturn pabr
				left join paymentattemptboleto pab on pab.agentorderreference = pabr.nossoNumero
				left join paymentattempt pa on pa.paymentattemptid = pab.paymentattemptid
				left join [order] o on o.orderid = pa.orderid
				where
					o.storeid = @storeId
					and pabr.creationDate between @dataInicial and @dataFinal
					and pa.status in (2,5)
					and pabr.dataliquidacao > '2007-01-01'
				)
order by dataPagamento, o.storereferenceorder, oi.installmentnumber

end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [DBO].[ProcPaidTransactions]
(
@dataInicial datetime,
@dataFinal datetime
)
as 

begin

select s.[name] as 'Nome da Loja', count(*) as 'Número de Transações Pagas', min(o.creationDate) as 'Data Primeira Transação', max(o.creationDate) as 'Data da Última Transação'
from paymentattempt pa
join [order] o on o.orderid = pa.orderid
join store s on s.storeid = o.storeid
where
	pa.status = 2
	and o.orderid not in (select orderid from splegacydbimport)
	and (pa.lastUpdate >= @dataInicial and pa.lastUpdate < @dataFinal)
group by s.[name]
order by 'Número de Transações Pagas'

end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE   procedure dbo.ProcPaymentSummary
(
@storeId int,
@DataInicial DateTime, 
@DataFinal DateTime 
)
as

begin


select pf.paymentFormid, pf.[name] , pa.status, count(*) as qtde, sum(o.finalAmount) as total
from paymentAttempt pa with(nolock)
left join [order] o with(nolock) on o.orderId = pa.orderId
left join paymentForm pf with(nolock) on pf.paymentFormId = pa.paymentFormId
left join storePaymentForm spf with(nolock) on spf.paymentFormId = pf.paymentFormId
where o.storeId = @storeId
and spf.storeId = @storeId
and  o.creationDate between @DataInicial and @DataFinal
group by pa.status,  pf.paymentFormid, pf.[name]
order by pf.[name]
end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE procedure dbo.ProcServicePaymentPost
(
@storeId int
)
as

begin

	select top 60 spp.*
	from servicepaymentpost spp
	join paymentattempt pa on pa.paymentattemptid = spp.paymentattemptid
	join [order] o on o.orderid = pa.orderid
	where
		o.storeid = @storeId
		and spp.postStatus <> 4
		and spp.postRetries < 3

end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

INSERT INTO _DBVERSION VALUES(1)
GO

IF @@ERROR <> 0
BEGIN
	PRINT(@@ERROR)
	ROLLBACK
END
ELSE
	PRINT('OK')
	COMMIT