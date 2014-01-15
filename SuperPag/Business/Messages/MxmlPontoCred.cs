using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MxmlPontoCred
    {
        private Guid _Id;

        public Guid Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _xml;

        public string Xml
        {
            get { return _xml; }
            set { _xml = value; }
        }
        private Guid _Userid;

        public Guid Userid
        {
            get { return _Userid; }
            set { _Userid = value; }
        }

        private DateTime _Data;

        public DateTime Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
        
    }
}
