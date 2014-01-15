function FormataData(campo,teclapres) {
            var tecla = teclapres.keyCode;
            vr = document.all[campo].value;
            vr = vr.replace( ".", "" );
            vr = vr.replace( "/", "" );
            vr = vr.replace( "/", "" );
            tam = vr.length + 1;
            if ( tecla != 9 && tecla != 8 ){
                        if ( tam > 2 && tam < 5 )
                                   document.all[campo].value = vr.substr( 0, tam - 2  ) + '/' + vr.substr( tam - 2, tam );
                        if ( tam >= 5 && tam <= 10 )
                                   document.all[campo].value = vr.substr( 0, 2 ) + '/' + vr.substr( 2, 2 ) + '/' + vr.substr( 4, 4 ); 
            }
}

 function FormataValor(campo,tammax,teclapres) {
            var tecla = teclapres.keyCode;
            vr = document.all[campo].value;
            vr = vr.replace( "/", "" );
            vr = vr.replace( "/", "" );
            vr = vr.replace( ",", "" );
            vr = vr.replace( ".", "" );
            vr = vr.replace( ".", "" );
            vr = vr.replace( ".", "" );
            vr = vr.replace( ".", "" );
            tam = vr.length;
            if (tam < tammax && tecla != 8){ tam = vr.length + 1 ; }
            if (tecla == 8 ){  tam = tam - 1 ; }
            if ( tecla == 8 || tecla >= 48 && tecla <= 57 || tecla >= 96 && tecla <= 105 ){
                        if ( tam <= 2 ){ 
                                    document.all[campo].value = vr ; }
                        if ( (tam > 2) && (tam <= 5) ){
                                    document.all[campo].value = vr.substr( 0, tam - 2 ) + ',' + vr.substr( tam - 2, tam ) ; }
                        if ( (tam >= 6) && (tam <= 8) ){
                                    document.all[campo].value = vr.substr( 0, tam - 5 ) + '' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
                        if ( (tam >= 9) && (tam <= 11) ){
                                    document.all[campo].value = vr.substr( 0, tam - 8 ) + '' + vr.substr( tam - 8, 3 ) + '' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
                        if ( (tam >= 12) && (tam <= 14) ){
                                    document.all[campo].value = vr.substr( 0, tam - 11 ) + '' + vr.substr( tam - 11, 3 ) + '' + vr.substr( tam - 8, 3 ) + '' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
                        if ( (tam >= 15) && (tam <= 17) ){
                                    document.all[campo].value = vr.substr( 0, tam - 14 ) + '' + vr.substr( tam - 14, 3 ) + '' + vr.substr( tam - 11, 3 ) + '' + vr.substr( tam - 8, 3 ) + '' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ;}
            }
/* original 
            if ( tecla == 8 || tecla >= 48 && tecla <= 57 || tecla >= 96 && tecla <= 105 ){
                        if ( tam <= 2 ){ 
                                    document.all[campo].value = vr ; }
                        if ( (tam > 2) && (tam <= 5) ){
                                    document.all[campo].value = vr.substr( 0, tam - 2 ) + ',' + vr.substr( tam - 2, tam ) ; }
                        if ( (tam >= 6) && (tam <= 8) ){
                                    document.all[campo].value = vr.substr( 0, tam - 5 ) + '.' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
                        if ( (tam >= 9) && (tam <= 11) ){
                                    document.all[campo].value = vr.substr( 0, tam - 8 ) + '.' + vr.substr( tam - 8, 3 ) + '.' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
                        if ( (tam >= 12) && (tam <= 14) ){
                                    document.all[campo].value = vr.substr( 0, tam - 11 ) + '.' + vr.substr( tam - 11, 3 ) + '.' + vr.substr( tam - 8, 3 ) + '.' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ; }
                        if ( (tam >= 15) && (tam <= 17) ){
                                    document.all[campo].value = vr.substr( 0, tam - 14 ) + '.' + vr.substr( tam - 14, 3 ) + '.' + vr.substr( tam - 11, 3 ) + '.' + vr.substr( tam - 8, 3 ) + '.' + vr.substr( tam - 5, 3 ) + ',' + vr.substr( tam - 2, tam ) ;}
            }
*/
}