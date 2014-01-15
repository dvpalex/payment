<%
	REM -- pega dados de configuração
	NumEstabelecimento = Application("NumEstabelecimento")
	PrazoMaxBradescoDebito = Application("PrazoMaxBradescoDebito")
REM ============ OBJETOS DE CRIPTOGRAFIA ==============
	set cripto = server.createObject("CriptoAuto.criptoAuto")
	hr = cripto.Initialize(NumEstabelecimento, PrazoMaxBradescoDebito) 

	hr = cripto.makeOrderInformationForClient(request("valor"), request("numOrder"), postParaCliente)

	REM -- Armazena dados de pagamento
	session("valor") = request("valor")
	session("numOrder") = request("numOrder")
	session("numShopper") = request("numShopper")
%>
