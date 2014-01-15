var __OldSubmit

if(typeof(__doPostBack) != 'undefined')
{
	__OldSubmit = __doPostBack;
	__doPostBack = _NewSubmit;
}
			
function _NewSubmit(eventTarget, eventArgument) {
	__OnSubmit();
	__OldSubmit(eventTarget, eventArgument);
}

function __OnSubmit()
{	
	if( typeof(Page_IsValid) != 'undefined' )
	{
		if ( Page_IsValid != true ) { return; }
	}	 
	if(__clickedElement != null)
	{
		if(__clickedElement != undefined)
		{				
			switch(__clickedElement.tagName.toString().toLowerCase())
			{
				case 'a':
					__clickedElement.disabled = true;
					__clickedElement.href = '#';
					break;
				case 'input':
					switch(__clickedElement.type.toString().toLowerCase())
					{
						case 'image':
							__clickedElement.style.filter = 'progid:DXImageTransform.Microsoft.BasicImage(Rotation=0,Mirror=0,Invert=0,XRay=0,Grayscale=1,Opacity=0.80)';
							__clickedElement.onclick = __returnFalse;
							break;
						case 'submit':			
							//__clickedElement.style.textDecorationLineThrough = true;
							__clickedElement.style.color = 'DarkGray'
							__clickedElement.onclick = __returnFalse;
							break;
					}
			}
		}			
	}
}

function __returnFalse()
{
	return false;
}

var __clickedElement;

function __MarkClickedButton()
{
	__clickedElement = event.srcElement;				
}

