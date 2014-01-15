
function _MaskAPI()
{
	this.version = "0.3";
	this.instances = 0;
	this.objects = {};
}

MaskAPI = new _MaskAPI();

function DecimalValidate( source, arguments ) 
{	
	op = arguments.Value;
	exp = new RegExp("^\\s*([-\\+])?(((\\d+)\\" + "." + ")*)(\\d+)"
                    + ((10 > 0) ? "(\\" + "," + "(\\d{1," + "10" + "}))?" : "")
                    + "\\s*$");
    m = op.match(exp);
    if (m == null)
        { arguments.IsValid=false; return }
    var intermed = m[2] + m[5] ;
    cleanInput = m[1] + intermed.replace(new RegExp("(\\" + 3 + ")", "g"), "") + ((4 > 0) ? "." + m[7] : 0);
    num = parseFloat(cleanInput);
    if(isNaN(num))
    { 
		arguments.IsValid=false;
		return;
	} 
	else 
	{
		arguments.IsValid=true;
		return;
	} 
}