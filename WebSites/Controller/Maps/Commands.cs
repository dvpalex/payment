using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using Ev = Controller.Lib.Views.Ev;

namespace Controller.Lib.Map.Commands {

	class EventMap : SuperPag.Framework.Web.WebController.CommandMap 
	{
		/*
		// Consulta de Pedidos
		[CommandHandler(typeof(Web.Commands.InformaParametrosParaConsultaDePedidosPrePago))]
		public static Type[] InformaParametrosParaConsultaDePedidosPrePago() {
			return EventsToRemove(
				typeof(Ev.ConsultaDePedidosPrePago.VoltaDaConsultaDePedidos)
				);
		}
		*/

        [CommandHandler(typeof(Controller.Lib.Commands.InsertPaymentAgentSetup))]
        public static Type[] InsertPaymentAgentSetup()
        {
            return EventsToRemove(
                typeof(Ev.PaymentAgentSetupEdit.Delete)
            );
        }

        
        [CommandHandler(typeof(Controller.Lib.Commands.InsertStorePaymentForm))]
        public static Type[] InsertStorePaymentForm()
        {
            return EventsToRemove(
                typeof(Ev.EditStorePaymentForm.Delete)
            );
        }
	}
}