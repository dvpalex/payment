using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MPages
    {

        public MPages ( int PageId, 
                        string PageName,
                        string PagePath,
                        int PageParentId,
                        int PageOrder
                      )
        {
            this.PageId = PageId;
            this.PageName = PageName;
            this.PagePath = PagePath;
            this.PageParentId = PageParentId;
            this.PageOrder = PageOrder;
        }
        

        private int _PageId;

        public int PageId
        {
            get { return _PageId; }
            set { _PageId = value; }
        }
        private string _PageName;

        public string PageName
        {
            get { return _PageName; }
            set { _PageName = value; }
        }
        private string _PagePath;

        public string PagePath
        {
            get { return _PagePath; }
            set { _PagePath = value; }
        }
        private int _PageParentId;

        public int PageParentId
        {
            get { return _PageParentId; }
            set { _PageParentId = value; }
        }
        private int _PageOrder;

        public int PageOrder
        {
            get { return _PageOrder; }
            set { _PageOrder = value; }
        }
    }
}
