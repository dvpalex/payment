using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DWorkflowOrderStatus))]
    public interface IWorkflowOrderStatus
    {

        [MethodType(MethodTypes.Query)]
        DWorkflowOrderStatus[] List();

        [MethodType(MethodTypes.Query)]
        [DataRelation(typeof(DOrder), DOrder.Fields.consumerId, typeof(DConsumer), DConsumer.Fields.consumerId, Join.Inner)]
        [Where(0, "startTimeFrom", DWorkflowOrderStatus.Fields.creationDate, Filter.GreaterOrEqual, Link.And)]
        [Where(1, "startTimeTo", DWorkflowOrderStatus.Fields.creationDate, Filter.LessOrEqual, Link.And)]
        [Where(2, Block.Begin, Link.And)]
        [Where(3, "COD", typeof(DConsumer), "CNPJ", Filter.Equal, Link.Or)]
        [Where(4, "COD", typeof(DConsumer), "CPF", Filter.Equal, Link.Or)]
        [Where(5, Block.End, Link.And)]
        [OrderBy(0, DWorkflowOrderStatus.Fields.creationDate, SortOrder.ASC)]
        DWorkflowOrderStatus_Order[] List(string COD, DateTime startTimeFrom, DateTime startTimeTo);

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DWorkflowOrderStatus.Fields.creationDate, SortOrder.DESC)]
        DWorkflowOrderStatus[] ListSortedByDate(long orderId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DWorkflowOrderStatus dWorkflowOrderStatus);
    }
}
