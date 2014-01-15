<%
Function CriptografaDados(Data, DadoEncriptado)

    Set DynCrypto = server.CreateObject("DynCrypto.crypto")
    DadoEncriptado = DynCrypto.AsymEncrypt(Application("ChavePublica"), Data)
    Set DynCrypto = nothing

End Function

Function DescriptografaDados(DadoEncriptado, Chave, DadoDescriptado)

    Set DynCrypto = CreateObject("DynCrypto.Crypto")
    DadoDescriptado= DynCrypto.AsymDecrypt(Chave, DadoEncriptado)
    Set DynCrypto = nothing

End Function


' Credit Card check routine for ASP
' (c) 1998 by Click Online
' You may use these functions only if this header is not removed
' http://www.click-online.de
' info@click-online.de


Function trimtodigits(tstring)
'removes all chars except of 0-9
  s="" 
  ts=tstring
  for x=1 to len(ts)
    ch=mid(ts,x,1)
    if asc(ch)>=48 and asc(ch)<=57 then
      s=s & ch
    end if
  next
  trimtodigits=s
end function

Function checkcc(ccnumber,cctype)
  'checks credit card number for checksum,length and type
  'ccnumber= credit card number (all useless characters are
  '	being removed before check)
  '
  'cctype:
  '       "V" VISA
  '       "M" Mastercard/Eurocard
  '       "A" American Express
  '       "C" Diners Club / Carte Blanche
  '       "D" Discover
  '       "E" enRoute
  '       "J" JCB
  'returns:  checkcc=0 (Bit0)  : card valid
  '          checkcc=1 (Bit1) : wrong type
  '          checkcc=2 (Bit2) : wrong length
  '          checkcc=4 (Bit3) : wrong checksum (MOD10-Test)
  '          checkcc=8 (Bit4) : cardtype unknown
  '
  ctype=ucase(cctype)
  select case ctype
    case "VISA"
      cclength="13;16"
      ccprefix="4"
    case "MASTERCARD"
      cclength="16"
      ccprefix="51;52;53;54;55"
    case "AMERICAN"
      cclength="15"
      ccprefix="34;37"
    case "DINERS"
      cclength="14"
      ccprefix="300;301;302;303;304;305;36;38"
    case "D"
      cclength="16"
      ccprefix="6011"
    case "E"
      cclength="15"
      ccprefix="2014;2149"
    case "J"
      cclength="15;16"
      ccprefix="3;2131;1800"
    case else
      cclength=""
      ccprefix=""
  end select
  prefixes=split(ccprefix,";",-1)
  lengths=split(cclength,";",-1)
  number=trimtodigits(ccnumber)
  prefixvalid=false
  lengthvalid=false
  for each prefix in prefixes
    if instr(number,prefix)=1 then
      prefixvalid=true
    end if
  next  
  for each length in lengths
    if cstr(len(number))=length then
      lengthvalid=true
    end if
  next
  result=0
  if not prefixvalid then
    result=result+1
  end if  
  if not lengthvalid then
    result=result+2
  end if  
  qsum=0
  for x=1 to len(number)
    ch=mid(number,len(number)-x+1,1)
    'response.write ch
    if x mod 2=0 then
      sum=2*cint(ch)
      qsum=qsum+(sum mod 10)
      if sum>9 then 
        qsum=qsum+1
      end if
    else
      qsum=qsum+cint(ch)
    end if
  next
  'response.write qsum
  if qsum mod 10<>0 then
    result=result+4
  end if
  if cclength="" then
    result=result+8
  end if
  checkcc=result
End Function


%>