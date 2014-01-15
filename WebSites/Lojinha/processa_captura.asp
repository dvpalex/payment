<!--#INCLUDE FILE="grava_captura.inc"-->
<%
        ' Script simplificado para capturar apenas uma autorizacao
	' de cada vez

	REM -- pega dados de configuração
	NumEstabelecimento = Application("NumEstabelecimento")
	PrazoMaxBradescoDebito = Application("PrazoMaxBradescoDebito")
REM ============ OBJETOS DE CRIPTOGRAFIA ==============
	set cripto = server.createObject("CriptoAuto.criptoAuto")
	set http = server.createObject("netAuto.http")
	hr = cripto.Initialize(NumEstabelecimento, PrazoMaxBradescoDebito) 

	CapOK = Application("CapturaOK")
	CapFalha = Application("CapturaFalha")

	Dim CAP, CAPhash, CAPcript
	CAP = ""

	hr = cripto.addCaptureItem( request.Form("codigo_pedido"), Request.Form("valor"))

	hr = cripto.capture(captureString, captureSign)

	if hr < 0 then
		erro = server.urlEncode("Problemas na comunicacao com o Payment Gateway! Verifique se o Internet Information Server esta no ar" + CStr(hr))
		response.redirect(CapFalha + "?resultado=1&erro="+erro+"&")
	end if

	' Verificar se existe algum pedido capturado com sucesso
	hr = 0
	hr = cripto.getCaptureStatus(request.Form("codigo_pedido"))

	if hr = 1 then
		' -- contem order ids sem erro
		CAPResCoded = server.urlEncode(captureString)
		CAPHashCoded = server.urlEncode(captureSign)
		status_captura = hr
		grava_captura()
		resp = "resposta=0&Assinatura="+CAPHashCoded+"&CAPRes="+CAPResCoded+"&"
		response.redirect(CapOK + "?"+resp)
	else
		' -- somente erros
		erro = server.urlEncode("Payment gateway rejeitou a captura")

		CAPResCoded = server.urlEncode(captureString)
		CAPHashCoded = server.urlEncode(captureSign)
		status_captura = hr
		grava_captura()
		resp = "resposta=1&Erro="+erro+"&Assinatura="+CAPHashCoded+"&CAPRes="+CAPResCoded+"&"
		response.redirect(CapFalha + "?"+resp)
	end if
        set http = Nothing
	set cripto = Nothing
%>