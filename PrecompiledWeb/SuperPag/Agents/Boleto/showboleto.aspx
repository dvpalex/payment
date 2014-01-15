<%@ page language="C#" autoeventwireup="true" inherits="Agents_Boleto_showboleto, App_Web_zzxxzd_r" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server">
    <title>SuperPag - Boleto Bancário</title>
  	<link rel="stylesheet" rev="stylesheet" href="../../styles/BoletoBB.css" />
	</head>
	<body>
		<form id="form1" method="post" runat="server">
		   <div style="text-align:center">
		       <table style="width: 666px" cellpadding="0" cellspacing="0">
		           <tr>
		                <td class="cp" style="text-align: center">
		                    Instruções de Impressão
		                 </td>
		           </tr>
		           <tr>
		              <td class="ti">
		                Imprimir em impressora jato de tinta (ink jet) ou laser em qualidade normal. (Não use modo econômico). <br/>
		                Utilize folha A4 (210 x 297 mm) ou Carta (216 x 279 mm) - Corte na linha indicada<br/>
		              </td>
		          </tr>
		       </table>
		       <br/>
		       <table style="width: 666px" cellpadding="0" cellspacing="0">
		          <tr>
		            <td class="ct">
		               <img height="1" src="./images/6.gif" width="665" alt="" />
		            </td>
		          </tr>
		          <tr>
		            <td class="ct" style="text-align:right; font-weight:bold">Recibo do Sacado</td>
		          </tr>
		       </table>
		       <table style="width: 666px" cellpadding="0" cellspacing="0">
		        <tr>
		            <td class="cp" style="width: 151px; text-align: left"><img src="./images/<%=NumeroBanco%>.jpg" alt="" /></td>
                    <td style="width:4px; vertical-align: bottom"><img height="22" src="./images/3.gif" width="2" alt="" /></td>
                    <td class="cpt"  style="width: 63px; vertical-align:bottom; text-align: center"><div class="bc"><%=NumeroBanco + "-" + DigitoBanco%></div></td>
                    <td style="width:4px; vertical-align: bottom"><img height="22" src="./images/3.gif" width="2" alt="" /></td>
                    <td style="width:444px; text-align: right; vertical-align: bottom; table-layout: fixed" class="ld"><%=LinhaDigitavel%></td>
                </tr>
                <tr>
                    <td colspan="5"><img height="2" src="./images/2.gif" width="666" alt=""/></td>
                </tr>
               </table>
               <table style="width: 666px" cellpadding="0" cellspacing="0">
                 <tr>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 298px; height: 13px; vertical-align: top">Cedente</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 126px; height: 13px; vertical-align: top">Agência/Código do Cedente</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 34px; height: 13px; vertical-align: top">Espécie</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 53px; height: 13px; vertical-align: top">Quantidade</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 120px; height: 13px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">Nosso número</td>
                  </tr>
                  <tr>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 298px; height: 12px; vertical-align: top"><%=Cedente%></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 126px; height: 12px; vertical-align: top"><asp:Label ID="lblAgCodCedente" runat="server"/></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 34px; height: 12px; vertical-align: top">R$ </td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 53px; height: 12px; vertical-align: top"></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 120px; height: 12px; vertical-align: top; text-align: right; border-right: solid 1px black; padding-right: 1px"><%=NossoNumero%></td>
                  </tr>
                  <tr>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 298px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="298" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 126px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="126" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 34px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="34" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 53px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="53" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 120px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="120" alt=""/></td>
                  </tr>
                </table>
                <table style="width: 666px" cellpadding="0" cellspacing="0">
                  <tr>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="height: 13px; vertical-align: top" colspan="3">Número do documento</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 132px; height: 13px; vertical-align: top">CPF/CNPJ</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 134px; height: 13px; vertical-align: top">Vencimento</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 180px; height: 13px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">Valor documento</td>
                   </tr>
                   <tr>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="height: 12px; vertical-align: top" colspan="3"><%=NumeroDocumento %></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 132px; height: 12px; vertical-align: top"><%=CPF_CNPJ%></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 134px; height: 12px; vertical-align: top"><%=DataVencimento%></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 180px; height: 12px; vertical-align: top; text-align: right; border-right: solid 1px black; padding-right: 1px"><%=ValorDocumento%></td>
                   </tr>
                   <tr>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 64px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="64" alt=""/></td>
                    <td style="width: 64px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="64" alt=""/></td>
                    <td style="width: 64px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="64" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 132px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="132" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 134px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="134" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 180px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="180" alt=""/></td>
                   </tr>
                 </table>
                 <table style="width: 666px" cellpadding="0" cellspacing="0">
                   <tr>
                     <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                     <td class="ct" style="width: 113px; height: 13px; vertical-align: top">(-) Desconto / Abatimentos</td>
                     <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                     <td class="ct" style="width: 112px; height: 13px; vertical-align: top">(-) Outras deduções</td>
                     <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                     <td class="ct" style="width: 113px; height: 13px; vertical-align: top">(+) Mora / Multa</td>
                     <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                     <td class="ct" style="width: 113px; height: 13px; vertical-align: top">(+) Outros acréscimos</td>
                     <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                     <td class="ct" style="width: 180px; height: 13px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">(=) Valor cobrado</td>
                  </tr>
                  <tr>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 113px; height: 12px; vertical-align: top"></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 112px; height: 12px; vertical-align: top"></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 113px; height: 12px; vertical-align: top"></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 113px; height: 12px; vertical-align: top"></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 180px; height: 12px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">&nbsp;</td>
                  </tr>
                  <tr>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 113px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="113" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 112px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="112" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 113px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="113" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 113px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="113" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 180px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="180" alt=""/></td>
                  </tr>
                </table>
                <table style="width: 666px" cellpadding="0" cellspacing="0">
                  <tr>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 659px; height: 13px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">Sacado</td>
                  </tr>
                  <tr>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 659px; height: 12px; vertical-align: top; border-right: solid 1px black; padding-right: 1px"><%=Sacado%></td>
                  </tr>
                  <tr>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 659px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="659" alt=""/></td>
                  </tr>
                </table>
                <table style="width: 666px" cellpadding="0" cellspacing="0">
                  <tr>
                    <td class="ct" style="width: 7px; height: 12px;"></td>
                    <td class="ct" style="width: 500px; height: 12px;">Instruções</td>
                    <td class="ct" style="width: 14px; height: 12px;"></td>
                    <td class="ct" style="width: 145px; height: 12px;">Autenticação mecânica</td>
                  </tr>
                  <tr>
                    <td class="ct" style="width: 7px; height: 13px;"></td>
                    <td class="ct" style="width: 500px; height: 13px;"></td>
                    <td class="ct" style="width: 14px; height: 13px;"></td>
                    <td class="ct" style="width: 145px; height: 13px;"></td>
                  </tr>
                </table>
                <div style="width: 666px; height: 10px"></div>
                <table style="width: 666px">
                  <tr>
                    <td class="ct" style="width: 666px"></td>
                  </tr>
                  <tr>
                    <td class="ct" style="width: 666px; text-align: right">Corte na linha pontilhada</td>
                  </tr>
                  <tr>
                    <td class="ct" style="width: 666px"><img height="1" src="./images/6.gif" width="665" alt="" /></td>
                  </tr>
                </table>
                <br/>
                <br/>
                <table style="width: 666px" cellpadding="0" cellspacing="0">
                  <tr>
                    <td class="cp" style="width: 151px; text-align: left"><img src="./images/<%=NumeroBanco%>.jpg" alt=""/></td>
                    <td style="width: 4px; vertical-align: bottom"><img height="22" src="./images/3.gif" width="2" alt=""/></td>
                    <td class="cp" style="width: 61px; vertical-align: bottom; text-align:center;"><div class="bc"><%=NumeroBanco + "-" + DigitoBanco%></div></td>
                    <td style="width: 4px; vertical-align: bottom"><img height="22" src="./images/3.gif" width="2" alt=""/></td>
                    <td style="width: 446px; vertical-align: bottom; text-align: right; table-layout: fixed" class="ld"><%=LinhaDigitavel%></td>
                  </tr>
                  <tr>
                    <td colspan="5"><img height="2" src="./images/2.gif" width="666" alt=""/></td>
                  </tr>
                </table>
                <table style="width: 666px" cellpadding="0" cellspacing="0">
                  <tr>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 472px; height: 13px; vertical-align: top">Local de pagamento</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="ct" style="width: 180px; height: 13px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">Vencimento</td>
                  </tr>
                  <tr>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="15" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 472px; height: 12px; vertical-align: top"><asp:Label ID="lblLocalPagamento" runat="server" Text=""></asp:Label></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="15" src="./images/1.gif" width="1" alt=""/></td>
                    <td class="cp" style="width: 180px; height: 12px; vertical-align: top; border-right: solid 1px black; padding-right: 1px"><%=DataVencimento%></td>
                  </tr>
                  <tr>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 472px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="472" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 180px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="180" alt=""/></td>
                  </tr>
               </table>
               <table style="width: 666px" cellpadding="0" cellspacing="0">
                 <tr>
                   <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                   <td class="ct" style="width: 472px; height: 13px; vertical-align: top">Cedente</td>
                   <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                   <td class="ct" style="width: 180px; height: 13px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">Agência/Código cedente</td>
                 </tr>
                 <tr>
                   <td class="cp" style="width: 7px; height: 11px; vertical-align: top"><img height="12" src="images/1.gif" width="1" alt=""/></td>
                   <td class="cp" style="width: 472px; height: 11px; vertical-align: top"><%=Cedente%></td>
                   <td class="cp" style="width: 7px; height: 11px; vertical-align: top"><img height="12" src="images/1.gif" width="1" alt=""/></td>
                   <td class="cp" style="width: 180px; height: 11px; vertical-align: top; border-right: solid 1px black; padding-right: 1px"><asp:Label ID="lblAgCodCedente2" runat="server"/></td>
                 </tr>
                 <tr>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 472px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="472" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 180px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="180" alt=""/></td>
                 </tr>
               </table>
               <table style="width: 666px" cellpadding="0" cellspacing="0">
                 <tr>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="ct" style="width: 113px; height: 13px; vertical-align: top">Data do documento</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="ct" style="width: 163px; height: 13px; vertical-align: top">N° documento</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="ct" style="width: 62px; height: 13px; vertical-align: top">Espécie doc.</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="ct" style="width: 34px; height: 13px; vertical-align: top">Aceite</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="ct" style="width: 72px; height: 13px; vertical-align: top; white-space: nowrap;">Data processamento</td>
                    <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="ct" style="width: 180px; height: 13px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">Nosso número</td>
                 </tr>
                 <tr>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="cp" style="width: 113px; height: 12px; vertical-align: top; text-align: left"><%=DataDocumento%></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="cp" style="width: 163px; height: 12px; vertical-align: top"><%=NumeroDocumento %></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="cp" style="width: 62px; height: 12px; vertical-align: top; text-align: left">DM</td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="cp" style="width: 34px; height: 12px; vertical-align: top; text-align: left">N</td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="cp" style="width: 72px; height: 12px; vertical-align: top; text-align: left"></td>
                    <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                    <td class="cp" style="width: 180px; height: 12px; vertical-align: top; text-align: right; border-right: solid 1px black; padding-right: 1px"><%=NossoNumero%></td>
                </tr>
                <tr>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 113px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="113" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 163px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="163" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 62px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="62" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 34px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="34" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 72px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="72" alt=""/></td>
                    <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                    <td style="width: 180px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="180" alt=""/></td>
                 </tr>
               </table>
               <table style="width: 666px" cellpadding="0" cellspacing="0">
                <tr> 
                  <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="ct" style="height: 13px; vertical-align: top" colspan="3">Uso do banco</td>
                  <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="ct" style="width: 83px; height: 13px; vertical-align: top">Carteira</td>
                  <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="ct" style="width: 53px; height: 13px; vertical-align: top">Espécie</td>
                  <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="ct" style="width: 123px; height: 13px; vertical-align: top">Quantidade</td>
                  <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="ct" style="width: 72px; height: 13px; vertical-align: top">Valor</td>
                  <td class="ct" style="width: 7px; height: 13px; vertical-align: top"><img height="13" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="ct" style="width: 180px; height: 13px; vertical-align: top; border-right: solid 1px black; padding-right: 1px">(=)Valor documento</td>
                </tr>
                <tr>
                  <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="cp" style="height: 12px; vertical-align: top" colspan="3">&nbsp;</td>
                  <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="cp" style="width: 83px; height: 12px; vertical-align: top; text-align: left"><%=Carteira%></td>
                  <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="cp" style="width: 53px; height: 12px; vertical-align: top; text-align: left">R$</td>
                  <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="cp" style="width: 123px; height: 12px; vertical-align: top"></td>
                  <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="cp" style="width: 72px; height: 12px; vertical-align: top"><%=ValorDocumento%></td>
                  <td class="cp" style="width: 7px; height: 12px; vertical-align: top"><img height="12" src="./images/1.gif" width="1" alt="" /></td>
                  <td class="cp" style="width: 180px; height: 12px; vertical-align: top; text-align: right; border-right: solid 1px black; padding-right: 1px"><%=ValorDocumento%></td>
                </tr>
                <tr>
                  <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="8" alt=""/></td>
                  <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="38" alt=""/></td>
                  <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="37" alt=""/></td>
                  <td style="width: 31px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="37" alt=""/></td>
                  <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                  <td style="width: 83px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="83" alt=""/></td>
                  <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                  <td style="width: 53px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="53" alt=""/></td>
                  <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                  <td style="width: 123px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="123" alt=""/></td>
                  <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                  <td style="width: 72px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="72" alt=""/></td>
                  <td style="width: 7px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                  <td style="width: 180px; height: 1px; vertical-align: top"><img height="1" src="./images/2.gif" width="180" alt=""/></td>
                </tr>
               </table>
               <table style="width: 666px; text-align: left" cellpadding="0" cellspacing="0">
                <tr>
                  <td style="width: 10px; text-align: left">
                    <table cellspacing="0" cellpadding="0" style="text-align: left">
                        <tr>
                            <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                         </tr>
                         <tr> 
                            <td class="cp" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                         </tr>
                     </table>
                  </td>
                  <td style="vertical-align: top; width: 469px; text-align: left" rowspan="5">
                    <div class="ct">Instruções (Texto de responsabilidade do cedente)</div>
                    <div class="cp" style="text-align: left"><%=Instrucoes%><br /></div>
                  </td>
                  <td style="text-align: right; width: 187px">
                     <table cellspacing="0" cellpadding="0">
                        <tr>
                           <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                           <td class="ct" style="vertical-align: top; width: 179px; height: 13px; border-right: solid 1px black; padding-right: 1px">(-) Desconto / Abatimentos</td>
                        </tr>
                        <tr> 
                           <td class="cp" style="vertical-align: top; width: 7px; height: 12px"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                           <td class="cp" style="vertical-align: top; width: 179px; height: 12px; border-right: solid 1px black; padding-right: 1px">&nbsp;</td>
                        </tr>
                        <tr> 
                           <td style="vertical-align: top; width: 7px; height: 1px"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                           <td style="vertical-align: top; width: 179px; height: 1px; border-right: solid 1px black"><img height="1" src="./images/2.gif" width="179" alt=""/></td>
                        </tr>
                     </table>
                  </td>
                </tr>
                <tr>
                  <td style="text-align: left; width: 10px"> 
                    <table cellspacing="0" cellpadding="0" style="text-align: left">
                       <tr>
                           <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                       </tr>
                       <tr>
                           <td class="cp" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                       </tr>
                     </table>
                   </td>
                   <td style="text-align: right; width: 187px">
                     <table cellspacing="0" cellpadding="0">
                       <tr>
                         <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                         <td class="ct" style="vertical-align: top; width: 179px; height: 13px; border-right: solid 1px black; padding-right: 1px">(-) Outras deduções</td>
                       </tr>
                       <tr>
                         <td class="cp" style="vertical-align: top; width: 7px; height: 12px"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                         <td class="cp" style="vertical-align: top; width: 179px; height: 12px; border-right: solid 1px black; padding-right: 1px">&nbsp;</td>
                       </tr>
                       <tr>
                         <td style="vertical-align: top; width: 7px; height: 1px"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                         <td style="vertical-align: top; width: 179px; height: 1px; border-right: solid 1px black"><img height="1" src="./images/2.gif" width="179" alt=""/></td>
                       </tr>
                     </table>
                   </td>
                 </tr>
                 <tr>
                   <td style="text-align: left; width: 10px">
                      <table cellspacing="0" cellpadding="0" style="text-align: left">
                        <tr>
                         <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                        </tr>
                        <tr>
                         <td class="cp" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                        </tr>
                      </table>
                    </td>
                    <td style="text-align: right; width: 187px"> 
                     <table cellspacing="0" cellpadding="0">
                       <tr>
                         <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                         <td class="ct" style="vertical-align: top; width: 179px; height: 13px; border-right: solid 1px black; padding-right: 1px">(+) Mora / Multa</td>
                       </tr>
                       <tr>
                         <td class="cp" style="vertical-align: top; width: 7px; height: 12px"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                         <td class="cp" style="vertical-align: top; width: 179px; height: 12px; border-right: solid 1px black; padding-right: 1px">&nbsp;</td>
                       </tr>
                       <tr>
                         <td style="vertical-align: top; width: 7px; height: 1px"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                         <td style="vertical-align: top; width: 179px; height: 1px; border-right: solid 1px black"><img height="1" src="./images/2.gif" width="179" alt=""/></td>
                       </tr>
                     </table>
                     </td>
                   </tr>
                   <tr>
                    <td style="text-align: left; width: 10px">
                      <table cellspacing="0" cellpadding="0" style="text-align: left">
                        <tr>
                         <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                        </tr>
                        <tr>
                         <td class="cp" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                        </tr>
                      </table>
                    </td>
                    <td style="text-align: right; width: 187px"> 
                     <table cellspacing="0" cellpadding="0">
                       <tr>
                         <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                         <td class="ct" style="vertical-align: top; width: 179px; height: 13px; border-right: solid 1px black; padding-right: 1px">(+) Outros acréscimos</td>
                       </tr>
                       <tr>
                         <td class="cp" style="vertical-align: top; width: 7px; height: 12px"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                         <td class="cp" style="vertical-align: top; width: 179px; height: 12px; border-right: solid 1px black; padding-right: 1px">&nbsp;</td>
                       </tr>
                       <tr>
                         <td style="vertical-align: top; width: 7px; height: 1px"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                         <td style="vertical-align: top; width: 179px; height: 1px; border-right: solid 1px black"><img height="1" src="./images/2.gif" width="179" alt=""/></td>
                       </tr>
                     </table>
                     </td>
                   </tr>
                   <tr>
                    <td style="text-align: left; width: 10px">
                      <table cellspacing="0" cellpadding="0" style="text-align: left">
                        <tr>
                         <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                        </tr>
                        <tr>
                         <td class="cp" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                        </tr>
                      </table>
                    </td>
                    <td style="text-align: right; width: 187px"> 
                     <table cellspacing="0" cellpadding="0">
                       <tr>
                         <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                         <td class="ct" style="vertical-align: top; width: 179px; height: 13px; border-right: solid 1px black; padding-right: 1px">(=) Valor cobrado</td>
                       </tr>
                       <tr>
                         <td class="cp" style="vertical-align: top; width: 7px; height: 12px"><img height="12" src="./images/1.gif" width="1" alt=""/></td>
                         <td class="cp" style="vertical-align: top; width: 179px; height: 12px; border-right: solid 1px black; padding-right: 1px">&nbsp;</td>
                       </tr>
                       <tr>
                         <td style="vertical-align: top; width: 7px; height: 1px"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                         <td style="vertical-align: top; width: 179px; height: 1px; border-right: solid 1px black"><img height="1" src="./images/2.gif" width="179" alt=""/></td>
                       </tr>
                     </table>
                     </td>
                   </tr>                    
                  </table>
                  <table style="width: 666px" cellpadding="0" cellspacing="0">
                    <tr>
                      <td style="vertical-align: top; width: 666px; height: 1px"><img height="1" src="./images/2.gif" width="666" alt=""/></td>
                    </tr>
                  </table>
                  <table style="width: 666px; border-right: solid 1px black; padding-right: 1px" cellpadding="0" cellspacing="0">
                    <tr>
                      <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                      <td class="ct" style="vertical-align: top; width: 659px; height: 13px">Sacado</td>
                    </tr>
                    <tr>
                      <td class="ct" style="vertical-align: top; width: 7px; height: 12px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                      <td class="cp" style="vertical-align: top; width: 659px; height: 12px"><%=Sacado%></td>
                    </tr>
                  </table>
                  <table style="width: 666px; border-right: solid 1px black; padding-right: 1px" cellpadding="0" cellspacing="0">
                    <tr>
                      <td class="ct" style="vertical-align: top; width: 7px; height: 12px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                      <td class="cp" style="vertical-align: top; width: 659px; height: 12px"><%=Endereco%></td>
                    </tr>
                  </table>
                  <table style="width: 666px; border-right: solid 1px black; padding-right: 1px" cellpadding="0" cellspacing="0">
                    <tr>
                      <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                      <td class="cp" style="vertical-align: top; width: 470px; height: 13px"></td>
                      <td class="ct" style="vertical-align: top; width: 7px; height: 13px"><img height="13" src="./images/1.gif" width="1" alt=""/></td>
                      <td class="ct" style="vertical-align: top; width: 180px; height: 13px">Cód. baixa</td>
                    </tr>
                    <tr>
                      <td style="vertical-align: top; width: 7px; height: 1px"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                      <td style="vertical-align: top; width: 470px; height: 1px"><img height="1" src="./images/2.gif" width="470" alt=""/></td>
                      <td style="vertical-align: top; width: 7px; height: 1px"><img height="1" src="./images/2.gif" width="7" alt=""/></td>
                      <td style="vertical-align: top; width: 180px; height: 1px"><img height="1" src="./images/2.gif" width="180" alt=""/></td>
                    </tr>
                  </table>
                  <table style="width: 666px" cellpadding="0" cellspacing="0">
                    <tr>
                      <td class="ct" style="width: 7px; height: 12px"></td>
                      <td class="ct" style="width: 409px; height: 12px">Sacador/Avalista</td>
                      <td class="ct" style="width: 250px; height: 12px; text-align: right">Autenticação mecânica - <b>Ficha de Compensação</b></td>
                    </tr>
                  </table>
                  <table style="width: 666px" cellpadding="0" cellspacing="0">
                    <tr>
                     <td style="vertical-align: bottom; height: 50px; text-align: left"><%=CodigoBarras%></td>
                    </tr>
                  </table>
                  <table style="width: 666px" cellpadding="0" cellspacing="0">
                     <tr>
                       <td class="ct" style="width:666px"></td>
                     </tr>
                     <tr>
                       <td class="ct" style="width:666px; text-align:right">Corte na linha pontilhada</td>
                     </tr>
                     <tr>
                       <td class="ct" style="width:666px;"><img height="1" src="./images/6.gif" width="665" alt=""/></td>
                     </tr>
                 </table>
		   </div>
		</form>
	</body>
</html>