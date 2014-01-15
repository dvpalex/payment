using System.Xml.Serialization;
using System;

namespace SuperPag.Agents.VBV3.Messages
{
    [Serializable]
    [XmlRoot("CAPTURE_RESPONSE")]
    public class VBV3CaptureReturn
    {
        [XmlElement("TID")]     public string tid;
        [XmlElement("LR")]      public decimal lr;
        [XmlElement("ARS")]     public string ars;
        [XmlElement("CAP")]     public string cap;
        [XmlElement("FREE")]    public string free;
    }
}