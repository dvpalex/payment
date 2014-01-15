function ValidaTecla(campo, event, tipo) {    
    var strValidos;
    var caractere;
    var BACKSPACE= 8; 
    var ENTER = 13 ;
    var key; 
    var tecla; 

    //definindo símbolos permitidos
    if (tipo=="ACEITA_NADA") 
     {  
        strValidos = ""
     }
     
    if (tipo=="SOMENTE_NUMEROS_PONTO") 
     {
        strValidos = "0123456789."
     }
     
    if (tipo=="SOMENTE_NUMEROS") 
     {
        strValidos = "0123456789"
     }
     
    if (tipo=="SOMENTE_VALORES")
     {
        strValidos = "0123456789,"
     }
     
    if (tipo=="SOMENTE_DATAS")
     {
        strValidos = "0123456789/"
     }
     
     if (tipo=="SOMENTE_FONE_CEP")
     {
        strValidos = "0123456789-"
     }
     
    if (tipo=="SOMENTE_HORAS")
     {
        strValidos = "0123456789:"
     }
     
    if (tipo=="SOMENTE_CPFCNPJ")
     {
        strValidos = "0123456789/-."
     }

     
    //Recuperando tecla digitada
    if(navigator.appName.indexOf("Netscape")!= -1) 
      {
        tecla= event.which; 
      }else
        {
          tecla= event.keyCode; 
        }   
    if(event.keyCode==8 && event.keyCode==0)
    {        
        return true;
    }
    return false;
}
function numero(event)
{
    if(event.keyCode>=48 && event.keyCode<=57)
    {
        return true;
    }
    return false;
}
