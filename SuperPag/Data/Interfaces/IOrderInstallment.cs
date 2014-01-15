using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DOrderInstallment))]
    public interface IOrderInstallment
    {
        [MethodType(MethodTypes.Query)]
        DOrderInstallment[] List();

        [MethodType(MethodTypes.Query)]
        DOrderInstallment[] List(long orderId);
        
        [MethodType(MethodTypes.Query)]
        DOrderInstallment Locate(long orderId, int installmentNumber);

        [MethodType(MethodTypes.Insert)]
        void Insert(DOrderInstallment dOrderItem);

        [MethodType(MethodTypes.Update)]
        void Update(DOrderInstallment dOrderItem);

        [MethodType(MethodTypes.Update)]
        void Update(int set_status, long by_orderId);

        [MethodType(MethodTypes.Update)]
        void Update(int set_status, long by_orderId, int by_installmentNumber);

        [MethodType(MethodTypes.Delete)]
        void Delete(long orderId, int installmentNumber);
        
        [MethodType(MethodTypes.Delete)]
        void Delete(long orderId);
    }
}
