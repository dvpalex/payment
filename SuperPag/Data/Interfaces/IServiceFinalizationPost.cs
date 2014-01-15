using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DServiceFinalizationPost))]
    public interface IServiceFinalizationPost
    {
        [MethodType(MethodTypes.Query)]
        DServiceFinalizationPost[] List();

        [MethodType(MethodTypes.Query)]
        DServiceFinalizationPost[] List(int[] postStatus);
        
        [MethodType(MethodTypes.Query)]
        DServiceFinalizationPost Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Insert)]
        void Insert(DServiceFinalizationPost dServiceFinalizationPost);

        [MethodType(MethodTypes.Update)]
        void Update(DServiceFinalizationPost dServiceFinalizationPost);

        [MethodType(MethodTypes.Delete)]
        void Delete(Guid paymentAttemptId);
    }
}
