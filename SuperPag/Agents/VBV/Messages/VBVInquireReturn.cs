﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.312
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.42.
// 

namespace SuperPag.Agents.VBV.Messages
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class VBVInquireReturn
    {

        private string tidField;

        private int lrField;

        private int arpField;

        private string arsField;

        private string freeField;

        private string orderidField;

        private string panField;

        private int bankField;

        private int priceField;

        private int authentField;

        /// <remarks/>
        public string tid
        {
            get
            {
                return this.tidField;
            }
            set
            {
                this.tidField = value;
            }
        }

        /// <remarks/>
        public int lr
        {
            get
            {
                return this.lrField;
            }
            set
            {
                this.lrField = value;
            }
        }

        /// <remarks/>
        public int arp
        {
            get
            {
                return this.arpField;
            }
            set
            {
                this.arpField = value;
            }
        }

        /// <remarks/>
        public string ars
        {
            get
            {
                return this.arsField;
            }
            set
            {
                this.arsField = value;
            }
        }

        /// <remarks/>
        public string free
        {
            get
            {
                return this.freeField;
            }
            set
            {
                this.freeField = value;
            }
        }

        /// <remarks/>
        public string orderid
        {
            get
            {
                return this.orderidField;
            }
            set
            {
                this.orderidField = value;
            }
        }

        /// <remarks/>
        public string pan
        {
            get
            {
                return this.panField;
            }
            set
            {
                this.panField = value;
            }
        }

        /// <remarks/>
        public int bank
        {
            get
            {
                return this.bankField;
            }
            set
            {
                this.bankField = value;
            }
        }

        /// <remarks/>
        public int price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public int authent
        {
            get
            {
                return this.authentField;
            }
            set
            {
                this.authentField = value;
            }
        }
    }
}