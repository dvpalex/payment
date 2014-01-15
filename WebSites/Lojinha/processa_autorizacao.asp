<!--#INCLUDE FILE="grava_autorizacao.inc"-->
<%
' no final possui as variaveis
'		authorization_string
'		authorization_code
' 		status
' e 		codigo_pedido

	REM -- pega dados de configuração
	NumEstabelecimento = Application("NumEstabelecimento")
	PrazoMaxBradescoDebito = Application("PrazoMaxBradescoDebito")
REM ============ OBJETOS DE CRIPTOGRAFIA ==============
	set cripto = server.createObject("CriptoAuto.criptoAuto")
	set http = server.createObject("netAuto.http")
	hr = cripto.Initialize(NumEstabelecimento, PrazoMaxBradescoDebito) 

	AutorizOK = Application("AutorizacaoOK")
	AutorizFalha = Application("AutorizacaoFalha")

REM =========================== CONFERE POST DO CLIENTE =====================================

	hr = cripto.clientAuthentication(Request.Form, CStr(session("numOrder")), CStr(session("valor")))
	if (hr <> 0) then
		grava_autorizacao()
		response.redirect(AutorizFalha + "?status="+Cstr(hr)+"&")
	end if
	hr = cripto.GetPaymentAuthorization()
	if (hr < 0) then
		grava_autorizacao()
		response.redirect(AutorizFalha + "?status="+Cstr(hr-1000)+"&")
	elseif (hr>0) then
		grava_autorizacao()
		response.redirect(AutorizFalha + "?status="+Cstr(hr)+"&")
	else
		result = 0	'autorizacao ok
		auth_str = server.urlEncode(cripto.authorizationResponse)
		' Avisa a loja sobre o resultado da autorização

		if cripto.cctype = "BradescoVISA" then
			prazo = "0"
		else
			prazo = cripto.ccprazo
		end if
		grava_autorizacao()
		response.redirect(AutorizOK + "?status=0&codigo_pedido="+CStr(session("numOrder")))
	end if
        set http = Nothing
	set cripto = Nothing
%>