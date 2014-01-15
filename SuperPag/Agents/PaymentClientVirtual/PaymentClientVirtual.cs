using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SuperPag.Agents.PaymentClientVirtual
{
    public class VPCStringComparer : IComparer
    {
        public int Compare(System.Object a, System.Object b)
        {
            // Return if we are comparing the same object or one of the 
            // objects is null, since we don't need to go any further.
            if (a == b) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            // Ensure we have string to compare
            string sa = a as string;
            string sb = b as string;

            // Get the CompareInfo object to use for comparing
            System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
            if (sa != null && sb != null)
            {
                // Compare using an Ordinal Comparison.
                return myComparer.Compare(sa, sb, System.Globalization.CompareOptions.Ordinal);
            }
            throw new ArgumentException("a and b should be strings.");
        }
    }

    public class PaymentClientVirtual
    {
        public static string CreateMD5Signature(string RawData)
        {
            /*
             <summary>Creates a MD5 Signature</summary>
             <param name="RawData">The string used to create the MD5 signautre.</param>
             <returns>A string containing the MD5 signature.</returns>
             */

            System.Security.Cryptography.MD5 hasher = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
            byte[] HashValue = hasher.ComputeHash(Encoding.ASCII.GetBytes(RawData));

            string strHex = "";
            foreach (byte b in HashValue)
            {
                strHex += b.ToString("x2");
            }
            return strHex.ToUpper();
        }
        
        public static string getResponseDescription(string vResponseCode)
        {
            /* 
             <summary>Maps the vpc_TxnResponseCode to a relevant description</summary>
             <param name="vResponseCode">The vpc_TxnResponseCode returned by the transaction.</param>
             <returns>The corresponding description for the vpc_TxnResponseCode.</returns>
             */
            string result = "Unknown";

            if (vResponseCode.Length > 0)
            {
                switch (vResponseCode)
                {
                    case "0": result = "Transaction Successful"; break;
                    case "1": result = "Transaction Declined"; break;
                    case "2": result = "Bank Declined Transaction"; break;
                    case "3": result = "No Reply from Bank"; break;
                    case "4": result = "Expired Card"; break;
                    case "5": result = "Insufficient Funds"; break;
                    case "6": result = "Error Communicating with Bank"; break;
                    case "7": result = "Payment Server detected an error"; break;
                    case "8": result = "Transaction Type Not Supported"; break;
                    case "9": result = "Bank declined transaction (Do not contact Bank)"; break;
                    case "A": result = "Transaction Aborted"; break;
                    case "B": result = "Transaction Declined - Contact the Bank"; break;
                    case "C": result = "Transaction Cancelled"; break;
                    case "D": result = "Deferred transaction has been received and is awaiting processing"; break;
                    case "F": result = "3-D Secure Authentication failed"; break;
                    case "I": result = "Card Security Code verification failed"; break;
                    case "L": result = "Shopping Transaction Locked (Please try the transaction again later)"; break;
                    case "N": result = "Cardholder is not enrolled in Authentication scheme"; break;
                    case "P": result = "Transaction has been received by the Payment Adaptor and is being processed"; break;
                    case "R": result = "Transaction was not processed - Reached limit of retry attempts allowed"; break;
                    case "S": result = "Duplicate SessionID"; break;
                    case "T": result = "Address Verification Failed"; break;
                    case "U": result = "Card Security Code Failed"; break;
                    case "V": result = "Address Verification and Card Security Code Failed"; break;
                    default: result = "Unable to be determined"; break;
                }
            }
            return result;
        }
        
        public static string displayAVSResponse(string vAVSResultCode)
        {
            /*
             <summary>Maps the vpc_AVSResultCode to a relevant description</summary>
             <param name="vAVSResultCode">The vpc_AVSResultCode returned by the transaction.</param>
             <returns>The corresponding description for the vpc_AVSResultCode.</returns>
             */
            string result = "Unknown";

            if (vAVSResultCode.Length > 0)
            {
                if (vAVSResultCode.Equals("Unsupported"))
                {
                    result = "AVS not supported or there was no AVS data provided";
                }
                else
                {
                    switch (vAVSResultCode)
                    {
                        case "X": result = "Exact match - address and 9 digit ZIP/postal code"; break;
                        case "Y": result = "Exact match - address and 5 digit ZIP/postal code"; break;
                        case "S": result = "Service not supported or address not verified (international transaction)"; break;
                        case "G": result = "Issuer does not participate in AVS (international transaction)"; break;
                        case "A": result = "Address match only"; break;
                        case "W": result = "9 digit ZIP/postal code matched, Address not Matched"; break;
                        case "Z": result = "5 digit ZIP/postal code matched, Address not Matched"; break;
                        case "R": result = "Issuer system is unavailable"; break;
                        case "U": result = "Address unavailable or not verified"; break;
                        case "E": result = "Address and ZIP/postal code not provided"; break;
                        case "N": result = "Address and ZIP/postal code not matched"; break;
                        case "0": result = "AVS not requested"; break;
                        default: result = "Unable to be determined"; break;
                    }
                }
            }
            return result;
        }

        public static string displayCSCResponse(string vCSCResultCode)
        {
            /*
             <summary>Maps the vpc_CSCResultCode to a relevant description</summary>
             <param name="vCSCResultCode">The vpc_CSCResultCode returned by the transaction.</param>
             <returns>The corresponding description for the vpc_CSCResultCode.</returns>
             */
            string result = "Unknown";
            if (vCSCResultCode.Length > 0)
            {
                if (vCSCResultCode.Equals("Unsupported"))
                {
                    result = "CSC not supported or there was no CSC data provided";
                }
                else
                {

                    switch (vCSCResultCode)
                    {
                        case "M": result = "Exact code match"; break;
                        case "S": result = "Merchant has indicated that CSC is not present on the card (MOTO situation)"; break;
                        case "P": result = "Code not processed"; break;
                        case "U": result = "Card issuer is not registered and/or certified"; break;
                        case "N": result = "Code invalid or not matched"; break;
                        default: result = "Unable to be determined"; break;
                    }
                }
            }
            return result;
        }
        
        public static bool ValidaAVS(string respavs, string acceptedavs)
        {
            if (respavs.ToUpper() == "X" || respavs.ToUpper() == "Y")
                return true;

            if (String.IsNullOrEmpty(acceptedavs))
                return false;

            foreach (string regra in acceptedavs.Split(",".ToCharArray()))
                if (respavs.ToUpper() == regra.ToUpper())
                    return true;

            return false;
        }
    }
}
