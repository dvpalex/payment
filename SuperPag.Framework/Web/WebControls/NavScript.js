function memoryScroll() 
{ 
	document.all("__NavField").value = document.body.scrollTop;
	return true; 
} 

function startControls() 
{ 
	document.body.scrollTop = document.all("__NavField").value; 
	return true; 
}

document.body.onload = startControls; 
document.body.onscroll = memoryScroll; 

//Script para mudan�a do t�tulo do frame
if (top!= null && top.document != null) top.document.title = document.title;

