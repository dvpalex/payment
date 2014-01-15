using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Agents
{
    public interface IAgent
    {
        void Start(Guid paymentAttemptId);
        void Finish();
    }
}
