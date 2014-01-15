using System.Xml.Serialization;
using System;

namespace SuperPag.Agents.VisaMoset3.Messages
{
    [Serializable]
    [XmlRoot("MESSAGE_RESPONSE")]
    public class VBV3AuthorizeReturn
    {
        [XmlElement("TID")]     public string tid;
        [XmlElement("LR")]      public string lr;
        [XmlElement("ARS")]     public string ars;
        [XmlElement("ARP")]     public string arp;
        [XmlElement("FREE")]    public string free;
        [XmlElement("ORDERID")] public string orderid;
        [XmlElement("BANK")]    public string bank;
        [XmlElement("PRICE")]   public string price;
    }
}