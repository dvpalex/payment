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