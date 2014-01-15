using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag
{
    public abstract class ControlBase : System.Web.UI.UserControl
    {
        private object controlInfo;
        private Guid paymentAttemptId;

        public object ControlInfo
        {
            get { return controlInfo; }
            set { controlInfo = value; }
        }

        public Guid PaymentAttemptId
        {
            get { return paymentAttemptId; }
            set { paymentAttemptId = value; }
        }
	
	
        public abstract void ShowControl();
    }
}
