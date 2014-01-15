function CallPopUp(iPagina, sParameters, sRoot, sHwGuid, sFirstVal, sSecondVal, sThirdVal) {

	if(sFirstVal != ""){
	obj = document.all(sFirstVal);
	ValidatorValidate(obj);
	if(obj.isvalid == false){return false;}}
	
	if(sSecondVal != ""){
	obj = document.all(sSecondVal);
	ValidatorValidate(obj);
	if(obj.isvalid == false){return false;}}
	
	if(sThirdVal != ""){
	obj = document.all(sThirdVal);
	ValidatorValidate(obj);
	if(obj.isvalid == false){return false;}}
	
	
	var sc = screen.width
	var W, H
	var agent = navigator.userAgent.toLowerCase();
	var app = navigator.appName.toLowerCase();

	if ( sc <= 800 ) sQP+="&sp=1";
	W = (sc <= 800 && agent.indexOf("mac")==-1) ? 180 : 230;
	H = ( agent.indexOf("windows") >0 && agent.indexOf("aol") > 0) ? screen.availHeight-window.screenTop-22:screen.availHeight//*AOL
	
	sWD = "scrollbars=yes,scrolling=yes,toolbar=0,status=1,menubar=0,width=" + W + ",height=" + H + ",left=" + (sc-W) + ",top=0,resizable=1";
		
	if(sParameters == "" || sParameters == undefined){
		hwWindow = window.open(sRoot + "popup.aspx?guid=" + sHwGuid + "&pagina=" + iPagina + "&", "Painel", sWD, true);
		hwWindow.focus();
	} else {
		hwWindow = window.open(sRoot + "popup.aspx?guid=" + sHwGuid + "&pagina=" + iPagina + sParameters, "Painel", sWD, true);
		hwWindow.focus();
	}	
}

function GetTextBoxValue(pid)
{
	return escape(document.getElementById(pid).value);
}

function GetComboBoxValue(pid)
{
	obj = document.getElementById(pid);
	return escape(obj[obj.selectedIndex].value);
}

function SetTextBoxValue(pid, value)
{	
	document.getElementById(pid).value = value;
}

function SetLabelValue(pid, value)
{
	document.getElementById(pid).innerText = value;
}

function ReturnDataGridValue(sFirst, sSecond, sThird)
{
	eval("window.parent.opener.SetDataGridValue_" + window.parent.sHwGuid + "(sFirst, sSecond, sThird)");
	eval("window.parent.opener.DataGridValueSelected_" + window.parent.sHwGuid + "()");
	window.top.close();
}

