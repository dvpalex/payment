﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.42.
// 

namespace SuperPag.Agents.KomerciWS.Messages
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class CONFIRMATION
    {

        private string cODRETField;

        private string mSGRETField;

        /// <remarks/>
        public string CODRET
        {
            get
            {
                return this.cODRETField;
            }
            set
            {
                this.cODRETField = value;
            }
        }

        /// <remarks/>
        public string MSGRET
        {
            get
            {
                return this.mSGRETField;
            }
            set
            {
                this.mSGRETField = value;
            }
        }
    }
}