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
using System;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.42.
// 

namespace SuperPag.Agents.VBV3.Messages
{
    [Serializable]
    [XmlRoot("INQUIRE_RESPONSE")]
    public class VBV3InquireReturn
    {
        [XmlElement("TID")]     public string tid;
        [XmlElement("LR")]      public int lr;
        [XmlElement("ARP")]     public int arp;
        [XmlElement("ARS")]     public string ars;
        [XmlElement("FREE")]    public string free;
        [XmlElement("ORDERID")] public string orderid;
        [XmlElement("PAN")]     public string pan;
        [XmlElement("BANK")]    public int bank;
        [XmlElement("PRICE")]   public int price;
        [XmlElement("AUTHENT")] public int authent;
    }
}