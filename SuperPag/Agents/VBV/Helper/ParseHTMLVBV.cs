using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Agents.VBV.Messages;
using SuperPag.Helper.ParseHTML;

namespace SuperPag.Agents.VBV.Helper
{
    public class ParseHTMLVBV : ParseHTML
    {
        public VBVInquireReturn GetVBVInquireReturn()
        {
            VBVInquireReturn inquireReturn = new VBVInquireReturn();
            while (!this.Eof())
            {
                char ch = this.Parse();
                if (ch == 0)
                {
                    AttributeList tag = this.GetTag();
                    if (tag.Name == "input")
                    {
                        if (tag["name"] != null)
                        {
                            switch (tag["name"].Value.ToLower())
                            {
                                case "lr":
                                    int val;
                                    if (int.TryParse(tag["value"].Value, out val))
                                        inquireReturn.lr = val;
                                    break;
                                case "ars":
                                    inquireReturn.ars = tag["value"].Value;
                                    break;
                                case "tid":
                                    inquireReturn.tid = tag["value"].Value;
                                    break;
                                case "free":
                                    inquireReturn.free = tag["value"].Value;
                                    break;
                                case "authent":
                                    int val1;
                                    if (int.TryParse(tag["value"].Value, out val1))
                                        inquireReturn.authent = val1;
                                    break;
                                case "arp":
                                    int val2;
                                    if (int.TryParse(tag["value"].Value, out val2))
                                        inquireReturn.arp = val2;
                                    break;
                                case "bank":
                                    int val3;
                                    if (int.TryParse(tag["value"].Value, out val3))
                                        inquireReturn.bank = val3;
                                    break;
                                case "orderid":
                                    inquireReturn.orderid = tag["value"].Value;
                                    break;
                                case "pan":
                                    inquireReturn.pan = tag["value"].Value;
                                    break;
                                case "price":
                                    int val4;
                                    if (int.TryParse(tag["value"].Value, out val4))
                                        inquireReturn.price = val4;
                                    break;
                            }
                        }
                    }
                }
            }
            return inquireReturn;
        }
        public VBVCaptureReturn GetVBVCaptureReturn()
        {
            VBVCaptureReturn captureReturn = new VBVCaptureReturn();
            while (!this.Eof())
            {
                char ch = this.Parse();
                if (ch == 0)
                {
                    AttributeList tag = this.GetTag();
                    if (tag.Name == "input")
                    {
                        if (tag["name"] != null)
                        {
                            switch (tag["name"].Value.ToLower())
                            {
                                case "lr":
                                    int val;
                                    if (int.TryParse(tag["value"].Value, out val))
                                        captureReturn.lr = val;
                                    break;
                                case "ars":
                                    captureReturn.ars = tag["value"].Value;
                                    break;
                                case "tid":
                                    captureReturn.tid = tag["value"].Value;
                                    break;
                                case "free":
                                    captureReturn.free = tag["value"].Value;
                                    break;
                                case "cap":
                                    captureReturn.cap = tag["value"].Value;
                                    break;
                            }
                        }
                    }
                }
            }
            return captureReturn;
        }
    }
}
