<!--
//Region Global Variables
var skm_TopMenuStyleInfos=new Object();
var skm_SelectedMenuStyleInfos=new Object();
var skm_UnselectedMenuStyleInfos=new Object();
var skm_MenuFadeDelays=new Object();
var skm_clockValue=0;
var skm_ticker;
var skm_highlightTopMenus=new Object();
var skm_images=new Array();
var skm_OpenMenuItems = new Array();
//EndRegion
//Region Methods to hook up a menu to the global variables
function skm_registerMenu(menuID, selectedStyleInfo, unselectedStyleInfo, menuFadeDelay, highlightTopMenu, topMenuItemStyle){
	skm_SelectedMenuStyleInfos[menuID]=selectedStyleInfo;
	skm_UnselectedMenuStyleInfos[menuID]=unselectedStyleInfo;
	skm_MenuFadeDelays[menuID]=menuFadeDelay;
	skm_highlightTopMenus[menuID]=highlightTopMenu;
	skm_TopMenuStyleInfos[menuID] = topMenuItemStyle;
}
//Region The methods and contructor of the skm_styleInfo object.
function skm_applyStyleInfoToElement(element){
	element.style.backgroundColor=this.backgroundColor;
	element.style.borderColor=this.borderColor;
	element.style.borderStyle=this.borderStyle;
	element.style.borderWidth=this.borderWidth;
	element.style.color=this.color;
	if (this.fontFamily!='')
		element.style.fontFamily=this.fontFamily;
	element.style.fontSize=this.fontSize;
	element.style.fontStyle=this.fontStyle;
	element.style.fontWeight=this.fontWeight;	
	if (this.className!='')
	{
		element.style.className = this.className;
		element.className=this.className;
	} 
	else 
	{
		if(element.className == 'menuTopOverMNU')
		{
			if(element.id.indexOf('subMenu') > -1)			
			{
				element.className = 'itemSubMenuMNU';
			}	
			else
			{
				element.className = 'itemSubMenuPrincipalMNU';
			}
		}
	}
}
function skm_styleInfo(backgroundColor,borderColor,borderStyle,borderWidth,color,fontFamily,fontSize,fontStyle,fontWeight,className){
	this.backgroundColor=backgroundColor;
	this.borderColor=borderColor;
	this.borderStyle=borderStyle;
	this.borderWidth=borderWidth;
	this.color=color;
	this.fontFamily=fontFamily;
	this.fontSize=fontSize;
	this.fontStyle=fontStyle;
	this.fontWeight=fontWeight;
	this.className=className;
	this.applyToElement=skm_applyStyleInfoToElement;
}
//Region MouseEventHandlers
function skm_mousedOverMenu(menuID,elem,parent,displayedVertically,imageSource){
	skm_stopTick();
	skm_closeSubMenus(elem);
	var childID=elem.id+"-subMenu";  // Display child menu if needed
	if (document.getElementById(childID)!=null){  // make the child menu visible and specify that its position is specified in absolute coordinates
		document.getElementById(childID).style.display='block';
		document.getElementById(childID).style.position='absolute';
		skm_OpenMenuItems = skm_OpenMenuItems.concat(childID);
		if (displayedVertically){ // Set the child menu's left and top attributes according to the menu's offsets
			document.getElementById(childID).style.left=skm_getAscendingLefts(parent)+parent.offsetWidth;
			document.getElementById(childID).style.top=skm_getAscendingTops(elem);
			var visibleWidth=parseInt(window.outerWidth?window.outerWidth-9:document.body.clientWidth,10);
			if ((parseInt(document.getElementById(childID).offsetLeft,10)+parseInt(document.getElementById(childID).offsetWidth,10))>visibleWidth) {
				document.getElementById(childID).style.left=visibleWidth-parseInt(document.getElementById(childID).offsetWidth,10);
			}
		}else{  // Set the child menu's left and top attributes according to the menu's offsets
			document.getElementById(childID).style.left=skm_getAscendingLefts(elem);
			document.getElementById(childID).style.top=skm_getAscendingTops(parent)+parent.offsetHeight;
			if (document.getElementById(childID).offsetWidth<elem.offsetWidth)
				document.getElementById(childID).style.width=elem.offsetWidth;
		}
	}
	if (skm_SelectedMenuStyleInfos[menuID] != null) skm_SelectedMenuStyleInfos[menuID].applyToElement(elem);
	if (skm_highlightTopMenus[menuID]){
		var eId=elem.id+'';
		while (eId.indexOf('-subMenu')>=0){
			eId=eId.substring(0, eId.lastIndexOf('-subMenu'));
			//skm_SelectedMenuStyleInfos[menuID].applyToElement(document.getElementById(eId));			
			skm_TopMenuStyleInfos[menuID].applyToElement(document.getElementById(eId));
		}
	}	
	if (imageSource!=''){
		setimage(elem,imageSource)
	}
}
function skm_mousedOverClickToOpen(menuID,elem,parent,imageSource){
	skm_stopTick();
	if (skm_SelectedMenuStyleInfos[menuID] != null) skm_SelectedMenuStyleInfos[menuID].applyToElement(elem);
	if (skm_highlightTopMenus[menuID]){
		var eId=elem.id+'';
		while (eId.indexOf('-subMenu')>=0){
			eId=eId.substring(0, eId.lastIndexOf('-subMenu'));
			//skm_SelectedMenuStyleInfos[menuID].applyToElement(document.getElementById(eId));			
			skm_TopMenuStyleInfos[menuID].applyToElement(document.getElementById(eId));
		}
	}	
	if (imageSource!=''){
		setimage(elem,imageSource)
	}
}
function skm_mousedOverSpacer(menuID,elem,parent){
	skm_stopTick();
}
function skm_mousedOutMenu(menuID,elem,imageSource){
	skm_doTick(menuID);
	if (skm_UnselectedMenuStyleInfos[menuID] != null) skm_UnselectedMenuStyleInfos[menuID].applyToElement(elem);
	if (skm_highlightTopMenus[menuID]){
		var eId=elem.id+'';
		while (eId.indexOf('-subMenu')>=0){
			eId=eId.substring(0, eId.lastIndexOf('-subMenu'));
			skm_UnselectedMenuStyleInfos[menuID].applyToElement(document.getElementById(eId));
		}
	}
	if (imageSource!=''){
		setimage(elem,imageSource)
	}
}
function skm_mousedOutSpacer(menuID, elem){
	skm_doTick(menuID);
}
//Region Utility Functions
function skm_closeSubMenus(parent){
	if (skm_OpenMenuItems == "undefined") return;
	for (var i=skm_OpenMenuItems.length-1; i>-1;i--) {
		if (parent.id.indexOf(skm_OpenMenuItems[i]) != 0) {
			document.getElementById(skm_OpenMenuItems[i]).style.display = 'none';
			skm_shimSetVisibility(false, skm_OpenMenuItems[i]);
			skm_OpenMenuItems = new Array().concat(skm_OpenMenuItems.slice(0, i), skm_OpenMenuItems.slice(i+1));
  		} 
	}
}
function skm_shimSetVisibility(makevisible, tableid){
	var tblRef=document.getElementById(tableid);
	var IfrRef=document.getElementById('shim'+tableid);
	if (tblRef!=null && IfrRef!=null){
		if(makevisible){
			IfrRef.style.width=tblRef.offsetWidth;
			IfrRef.style.height=tblRef.offsetHeight;
			IfrRef.style.top=tblRef.style.top;
			IfrRef.style.left=tblRef.style.left;
			IfrRef.style.zIndex=tblRef.style.zIndex-1;
			IfrRef.style.display="block";
		}else{
			IfrRef.style.display="none";
		}
	}
}
function skm_IsSubMenu(id){
	if(typeof(skm_subMenuIDs)=='undefined') return false;
	for (var i=0;i<skm_subMenuIDs.length;i++)
	  if (id==skm_subMenuIDs[i]) return true;
	return false;
}
function skm_getAscendingLefts(elem){
	if (elem==null)
		return 0;
	else
	{
		if ((elem.style.position=='absolute' || elem.style.position=='relative') && !skm_IsSubMenu(elem.id)) return 0;
		return elem.offsetLeft+skm_getAscendingLefts(elem.offsetParent);
	}
}
function skm_getAscendingTops(elem){
	if (elem==null)
		return 0;
	else {
		if ((elem.style.position=='absolute' || elem.style.position=='relative') && !skm_IsSubMenu(elem.id)) return 0;
		return elem.offsetTop+skm_getAscendingTops(elem.offsetParent);
	}
}
//Region Fade Functions
function skm_doTick(menuID){
	if (skm_clockValue>=skm_MenuFadeDelays[menuID]){
		skm_stopTick();
		skm_closeSubMenus(document.getElementById(menuID));
	} else {
		skm_clockValue++;
		skm_ticker=setTimeout("skm_doTick('"+menuID+"');", 500);
	}
}
function skm_stopTick(){
	skm_clockValue=0;
	clearTimeout(skm_ticker);
}
function preloadimages(){
	for (i=0;i<preloadimages.arguments.length;i++){
		skm_images[i]=new Image();
		skm_images[i].src=preloadimages.arguments[i];
	}
}
function setimage(elem,imageSource){
	var i=elem.getElementsByTagName("img")[0];
	i.src=imageSource;
}
//-->