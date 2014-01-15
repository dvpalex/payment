function validMMYYYY( sender, args ) {
	var reg = /^(\d{1,2}\/\d{4})\s*$/gi;
	if( reg.test(args.Value) ) 
	{
		args.IsValid = false;
		
		var arrMonthYear = args.Value.split("/");
		
		if ( parseFloat( arrMonthYear[0] ) < 1 || parseFloat( arrMonthYear[0] ) > 12 ) return;
		
		if ( parseFloat( arrMonthYear[1] ) < 1900 ) return;
		
		args.IsValid = true;
		return;
	}
	else 
		args.IsValid = false;
}

function validHHMM( sender, args ) {
	var reg = /^\s*(\d{1,2})(:)(\d{1,2})\s*$/gi;
	if( reg.test(args.Value) ) 
	{
		args.IsValid = false;
		
		var arrHHMM = args.Value.split(":");
		
		if ( parseFloat( arrHHMM[0] ) < 0 || parseFloat( arrHHMM[0] ) > 23 ) return;
		
		if ( parseFloat( arrHHMM[1] ) < 0 || parseFloat( arrHHMM[1] ) > 59 ) return;
		
		args.IsValid = true;
		return;
	}
	else 
		args.IsValid = false;
}

function validHHMMSS( sender, args ) {
	var reg = /^(\d{1,2}:\d{1,2}:\d{1,2})\s*$/gi;
	if( reg.test(args.Value) ) 
	{
		args.IsValid = false;
		
		var arrHHMM = args.Value.split(":");
		
		if ( parseFloat( arrHHMM[0] ) < 0 || parseFloat( arrHHMM[0] ) > 23 ) return;
		
		if ( parseFloat( arrHHMM[1] ) < 0 || parseFloat( arrHHMM[1] ) > 59 ) return;
		
		if ( parseFloat( arrHHMM[2] ) < 0 || parseFloat( arrHHMM[2] ) > 59 ) return;
		
		args.IsValid = true;
		return;
	}
	else 
		args.IsValid = false;
}

