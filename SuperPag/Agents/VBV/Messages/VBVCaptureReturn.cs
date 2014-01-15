using System.Xml.Serialization;

namespace SuperPag.Agents.VBV.Messages
{
    public class VBVCaptureReturn
    {
        public string tid;
        public decimal lr;
        public string ars;
        public string cap;
        public string free;
    }
}