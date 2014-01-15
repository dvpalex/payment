function fnTrapKD(btn, event)
{
	if (document.all)
	{
		if (event.keyCode == 13)
		{
			event.returnValue=false;
			event.cancel = true;
			btn.click();
		}
	}
	else if (document.getElementById)
	{
		if (event.which == 13)
		{
			event.returnValue=false;
			event.cancel = true;
			btn.click();
		}
	}
	else if(document.layers)
	{
		if(event.which == 13)
		{
			event.returnValue=false;
			event.cancel = true;
			btn.click();
		}
	}
}

function OpenPrintPopUpWindow(sUrl) {
	l = window.screenLeft + 20;	
	sWD = "toolbar=0,status=1,menubar=0,width=720,height=450,left=" + l + ",top=20,resizable=0,scrollbars=yes";
	h = window.open(sUrl, "", sWD, true);
	h.focus();
}

function printPreview()
{
	var idComando = 7;
	
	var objWebBrowser = '<OBJECT ID="webBrowser" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></OBJECT>';
	
	document.body.insertAdjacentHTML('beforeEnd', objWebBrowser); 
		
	webBrowser.ExecWB(idComando, 1);
	
	webBrowser.outerHTML = "";
}

function pageSetup()
{
	var idComando = 8;
	
	var objWebBrowser = '<OBJECT ID="webBrowser" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></OBJECT>';
	
	document.body.insertAdjacentHTML('beforeEnd', objWebBrowser); 
		
	webBrowser.ExecWB(idComando, 1);
	
	webBrowser.outerHTML = "";
}
