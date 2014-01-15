using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAgentSetupKomerci))]
    public interface IPaymentAgentSetupKomerci
    {
        [MethodType(MethodTypes.Query)]
        DPaymentAgentSetupKomerci[] List();

        [MethodType(MethodTypes.Query)]
        DPaymentAgentSetupKomerci Locate(int paymentAgentSetupId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAgentSetupKomerci dPaymentAgentSetupKomerci);

        [MethodType(MethodTypes.Update)]
        void Update(DPaymentAgentSetupKomerci dPaymentAgentSetupKomerci);

        [MethodType(MethodTypes.Delete)]
        void Delete(int paymentAgentSetupId);
    }
}
