		function DateBlur(a)
		{
			var xTexto= new String;
			var now = new Date();
			var month = Mask(String("00")+String((now.getMonth()+1)),"##");
			var date =  Mask(String("00")+String(now.getDate()),"##");
			var year = String(now.getFullYear());
			if (!a.isTextEdit) return;
			DateSetParameters(a);
			if (a.value == "" && (!a.allowDateNull || a.allowDateNull == "true")) return;
			if (a.value == "") 
			if (a.dateFormat == "DMY")
					a.value = DateDivide(String(date)+String(month)+String(year),"DMY",true);
			else
					a.value = DateDivide(String(month)+String(date)+String(year),"DMY",true);

			a.value = DateDivide(a.value,a.dateFormat,true);
			DateLimits(a);
			a.value = Mask(a.value,a.mask); 
		}

		function DateDivide(Texto,Formato,Bluring)
		{
			var iPosInicDia = new Number;
			var iPosInicMes = new Number;
			var iPosInicAno = new Number;
			var iPosTamanho = new Number;
			var Dia,Mes,Ano = new String;
			var now = new Date()
			if (Formato ==  "DMY")
			{
				iPosInicDia = 0;
				iPosInicMes = 2;
			}
			else
			{
				iPosInicDia = 2;
				iPosInicMes = 0;
			}    

			iPosInicAno = 4; 
			Dia = Texto.substr(iPosInicDia,2);
			Mes = Texto.substr(iPosInicMes,2);
			Ano = Texto.substr(iPosInicAno,4);

			if (Bluring)
			{
				if (Ano.length == 0 || Ano == 0)
					Ano = String(now.getFullYear());
	      
				if (Ano.length < 4)
				{
					if (Ano <= 79)
						Ano = "20"+Mask(String("00")+String(Ano),"##");
					else if (Ano <= 99)
						Ano =  "19"+Mask(String("00")+String(Ano),"##");     
					else 
						Ano = "1"+Mask(String("000")+String(Ano),"###");  
				} 
				if (Dia <= 0) 
						Dia = 1;
				Dia = Mask(String("00")+String(Dia),"##");
				Mes = Mask(String("00")+String(Mes),"##");
			}
		    
			if (Dia > 31)
				Dia = 31;
		  
			if (Mes && Mes.length == 2)
			{
				if (Mes == 0)
					Mes = Mask(String("00")+String((now.getMonth()+1)),"##");
				      
				if (Mes > 12)
					Mes = 12;
				
				if ((Mes == 2) && (Dia > 29))
					Dia = 29;
				else if ((Mes == 4 || Mes == 6 || Mes == 9 || Mes == 11) && Dia == 31)
					Dia = 30;      
			}

			if (Ano.length == 4)
				if ( ( ((Ano%4) != 0) || ((Ano%400) != 0)) && ((Mes == 2) && (Dia == 29)))
							Dia = 28;

			if (Formato == "DMY")
				return String(String(Dia)+String(Mes)+String(Ano));
			else
				return String(String(Mes)+String(Dia)+String(Ano));       
		}

		function DateKeyPress(a)
		{
			var xTexto = new String;
			var xValueNumbers = "0123456789";
			var xDia = new String;
			var xMes = new String;
			var xAno = new String;
			var xTextRange;
		      
			if (!a.isTextEdit) return;  
		   
			DateSetParameters(a);
		   
			xTexto = a.value;

			if ((xTexto.length+1) > MaskTotalChars(a.mask)) 
					a.value = "";
		    
			if (xValueNumbers.indexOf(String.fromCharCode(window.event.keyCode)) != -1)
			{
					xTexto =  String(a.value)+ String.fromCharCode(window.event.keyCode);  
					a.value = DateDivide(xTexto,a.dateFormat);
			}  
		  
			window.event.keyCode=0;
		  
		}

		function DateFocus(a)
		{ 
			if (!a.isTextEdit) return;
			DateSetParameters(a);
			a.value=UnMaskValue(a.value);
			a.select();
		}

		function DateSetParameters(a)
		{
			a.mask =  "##/##/####";
			if (!a.dateFormat || (a.dateFormat != "DMY" && a.dateFormat != "MDY"))
				a.dateFormat = "DMY";   

			if (!a.silent) a.silent = "false";
			if (!a.dateMax) a.dateMax = "20790606";
			if (!a.dateMin) a.dateMin = "19000101";   
		}

		function UnMaskValue(Texto,Masks)
		{
			var sTxtQ = "";
			var ipos;
			var xValueNumbers = new String;
			xValueNumbers = "";
			if (!Masks)
					xValueNumbers = "0123456789";
			else if (Masks.indexOf("#") == -1 && Masks.indexOf("A") == -1)
				xValueNumbers = "0123456789";
			else  
				{
					if (Masks.indexOf("#") != -1)
						xValueNumbers = xValueNumbers + "0123456789";
					if (Masks.indexOf("A") != -1)
						xValueNumbers = xValueNumbers + "ABCDEFGHIJKLMNOPQRSTUVXYWZ";  
					if (Masks.indexOf("!") != -1)
						xValueNumbers = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYWZ";  
				}   
			Texto = Texto.toUpperCase();
			for(ipos=0;ipos<Texto.length;ipos++)
			{
				if (xValueNumbers.indexOf(Texto.charAt(ipos)) != -1)
					sTxtQ=sTxtQ+Texto.charAt(ipos);
			}
			return sTxtQ;    
		}

		function MaskTotalChars(Texto)
		{
			var RawMask = new String;
			RawMask = MaskRawMask(Texto);
			return RawMask.length;
		}

		function DateDivide(Texto,Formato,Bluring)
		{
			var iPosInicDia = new Number;
			var iPosInicMes = new Number;
			var iPosInicAno = new Number;
			var iPosTamanho = new Number;
			var Dia,Mes,Ano = new String;
			var now = new Date()
		         
			if (Formato ==  "DMY")
				{
					iPosInicDia = 0;
					iPosInicMes = 2;
				}
			else
				{
					iPosInicDia = 2;
					iPosInicMes = 0;
				}    
		   
			iPosInicAno = 4; 
		   
			Dia = Texto.substr(iPosInicDia,2);
			Mes = Texto.substr(iPosInicMes,2);
			Ano = Texto.substr(iPosInicAno,4);
		   
			if (Bluring)
				{
					if (Ano.length == 0 || Ano == 0)
						Ano = String(now.getFullYear());
		      
					if (Ano.length < 4)
					{
		        
						if (Ano <= 79)
							Ano = "20"+Mask(String("00")+String(Ano),"##");
						else if (Ano <= 99)
							Ano =  "19"+Mask(String("00")+String(Ano),"##");     
						else 
							Ano = "1"+Mask(String("000")+String(Ano),"###");  
					} 
		       
					if (Dia <= 0) 
							Dia = 1;
		  
					Dia = Mask(String("00")+String(Dia),"##");
					Mes = Mask(String("00")+String(Mes),"##");
				}
		    
			if (Dia > 31)
				Dia = 31;
		  
			if (Mes && Mes.length == 2)
				{
					if (Mes == 0)
							Mes = Mask(String("00")+String((now.getMonth()+1)),"##");
		           
					if (Mes > 12)
							Mes = 12;
		      
					if ((Mes == 2) && (Dia > 29))
							Dia = 29;
					else if ((Mes == 4 || Mes == 6 || Mes == 9 || Mes == 11) && Dia == 31)
							Dia = 30;      
				}
			if (Ano.length == 4)
				if ( ( ((Ano%4) != 0) || ((Ano%400) != 0)) && ((Mes == 2) && (Dia == 29)))
							Dia = 28;
			if (Formato == "DMY")
				return String(String(Dia)+String(Mes)+String(Ano));
			else
				return String(String(Mes)+String(Dia)+String(Ano));       
		   
		}

		function DateLimitsMask(Texto,Formato)
		{
			if (Formato == "DMY")
				return String(Texto.substr(6,2))+String(Texto.substr(4,2))+ String(Texto.substr(0,4));
			else
				return String(Texto.substr(4,2))+String(Texto.substr(6,2))+ String(Texto.substr(0,4));
		}

		function Mask(Texto,sMascara)
		{
			var sRetorno = "";
			var ipos;
			var iPosMascara;
			var sTextoQuente = "";
			var xValueNumbers = "0123456789";
			var xValueChars = "ABCDEFGHIJKLMNOPQRSTUVXYWZ";
			var xValueValidChars = "";
			if (sMascara.indexOf("#") != -1 || sMascara.indexOf("0") != -1)
					xValueValidChars = xValueNumbers;
			if (sMascara.indexOf("A") != -1)
					xValueValidChars = xValueValidChars + xValueChars;
			if (sMascara.indexOf("!") != -1)
					xValueValidChars = xValueNumbers + xValueChars;
			Texto = Texto.toUpperCase();
			for(ipos=0;ipos<Texto.length;ipos++)
			{
				if (xValueValidChars.indexOf(Texto.charAt(ipos)) != -1)
					sTextoQuente=sTextoQuente+Texto.charAt(ipos);
			}
		  
			iPosMascara = sMascara.length;
			ipos = sTextoQuente.length;
			for(;;)
			{  
						iPosMascara-=1;
						if (iPosMascara < 0) break;
						if (sMascara.charAt(iPosMascara) == "0" || sMascara.charAt(iPosMascara) == "#" || sMascara.charAt(iPosMascara) == "A" || sMascara.charAt(iPosMascara) == "!")
							{
								for (;;)
								{  
										ipos-=1;
										if (ipos<0) break;
										if ( (sMascara.charAt(iPosMascara) == "0" || sMascara.charAt(iPosMascara) == "#") && xValueNumbers.indexOf(sTextoQuente.charAt(ipos)) != -1)
											{
												sRetorno = sTextoQuente.charAt(ipos) + sRetorno;
												break;
											}
										if (sMascara.charAt(iPosMascara) == "A" && xValueChars.indexOf(sTextoQuente.charAt(ipos)) != -1)
											{
												sRetorno = sTextoQuente.charAt(ipos) + sRetorno;
												break;
											}
										if (sMascara.charAt(iPosMascara) == "!")
											{
												sRetorno = sTextoQuente.charAt(ipos) + sRetorno;
												break;
											}
								}
							}
						else
							sRetorno = sMascara.charAt(iPosMascara) + sRetorno;
						if (ipos <= 0) break;      
			}       

			return sRetorno;    
		}

		function DateLimits(a)
		{
			var xTextoCompare = new String;
			xTextoCompare = a.value;
			if (a.dateFormat ==  "DMY")
				xTextoCompare = xTextoCompare.substr(4,4)+xTextoCompare.substr(2,2)+xTextoCompare.substr(0,2);
			else
				xTextoCompare = xTextoCompare.substr(4,4)+xTextoCompare.substr(0,2)+xTextoCompare.substr(2,2);

			DateSetParameters(a);
			if(a.dateMin)
			{
					if (xTextoCompare < a.dateMin)
						{
							if (a.silent == "false")
								alert("Data Incorreta!")
							a.value = DateDivide(DateLimitsMask(a.dateMin,a.dateFormat),a.dateFormat,true);
						}   
			}
			            
			if(a.dateMax)
			{
			    
				if (xTextoCompare > a.dateMax)
						{
							if (a.silent && a.silent == "false")
									alert("Data Incorreta!")
							a.value = DateDivide(DateLimitsMask(a.dateMax,a.dateFormat),a.dateFormat,true);
						}
				}
			
		}

		function DateKeyPress(a)
		{
			var xTexto = new String;
			var xValueNumbers = "0123456789";
			var xDia = new String;
			var xMes = new String;
			var xAno = new String;
			var xTextRange;
			    
			if (!a.isTextEdit) return;  
			  
			DateSetParameters(a);
			  
			xTexto = a.value;

			if ((xTexto.length+1) > MaskTotalChars(a.mask)) 
					a.value = "";
			  
			if (xValueNumbers.indexOf(String.fromCharCode(window.event.keyCode)) != -1)
			{
					xTexto =  String(a.value)+ String.fromCharCode(window.event.keyCode);  
					a.value = DateDivide(xTexto,a.dateFormat);
			}  
			
			window.event.keyCode=0;
			
		}

		function DateDivide(Texto,Formato,Bluring)
		{
			var iPosInicDia = new Number;
			var iPosInicMes = new Number;
			var iPosInicAno = new Number;
			var iPosTamanho = new Number;
			var Dia,Mes,Ano = new String;
			var now = new Date()
			if (Formato ==  "DMY")
				{
					iPosInicDia = 0;
					iPosInicMes = 2;
				}
			else
				{
					iPosInicDia = 2;
					iPosInicMes = 0;
				}    
			iPosInicAno = 4; 
			Dia = Texto.substr(iPosInicDia,2);
			Mes = Texto.substr(iPosInicMes,2);
			Ano = Texto.substr(iPosInicAno,4);
			if (Bluring)
				{
					if (Ano.length == 0 || Ano == 0)
						Ano = String(now.getFullYear());
					if (Ano.length < 4)
					{
						if (Ano <= 79)
							Ano = "20"+Mask(String("00")+String(Ano),"##");
						else if (Ano <= 99)
							Ano =  "19"+Mask(String("00")+String(Ano),"##");     
						else 
							Ano = "1"+Mask(String("000")+String(Ano),"###");  
					} 
			      
					if (Dia <= 0) 
							Dia = 1;
					Dia = Mask(String("00")+String(Dia),"##");
					Mes = Mask(String("00")+String(Mes),"##");
				}
			  
			if (Dia > 31)
				Dia = 31;
			if (Mes && Mes.length == 2)
				{
					if (Mes == 0)
							Mes = Mask(String("00")+String((now.getMonth()+1)),"##");
					if (Mes > 12)
							Mes = 12;
					if ((Mes == 2) && (Dia > 29))
							Dia = 29;
					else if ((Mes == 4 || Mes == 6 || Mes == 9 || Mes == 11) && Dia == 31)
							Dia = 30;      
				}
			  
			if (Ano.length == 4)
				if ( ( ((Ano%4) != 0) || ((Ano%400) != 0)) && ((Mes == 2) && (Dia == 29)))
							Dia = 28;

			if (Formato == "DMY")
				return String(String(Dia)+String(Mes)+String(Ano));
			else
				return String(String(Mes)+String(Dia)+String(Ano));       
		}

		function MaskRawMask(Texto)
		{
			var ipos = new Number;
			var RawMask = new String;
			RawMask = "";
			for(ipos=0;ipos<=Texto.length;ipos++)
			{
				if(Texto.charAt(ipos) == "#" || Texto.charAt(ipos) == "!" || Texto.charAt(ipos) == "A")
					RawMask = RawMask+Texto.charAt(ipos);
			}
			return RawMask;		
		}

		function DatePropertyChange(a)
		{
			if (window.event.propertyName == "value")
				a.unMaskedValue = UnMaskValue(String(a.value),a.mask)
		}
