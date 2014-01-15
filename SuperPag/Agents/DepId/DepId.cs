using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace SuperPag.Agents.DepId
{
    public class DepId
    {
        public static string GetIdFromPattern(string storeReferenceOrder, int agentOrderReference, string pattern, int installmentNumber)
        {
            //TODO: Implementar para funcionar com Parcelamento
            Regex regex = new Regex(@"^(\*|(-?\(\d,\d\))(;(-?\(\d,\d\)))*)$");
            if (!regex.IsMatch(pattern))
                throw new Exception("DepId Pattern not matched");

            if (pattern == "*")
                return agentOrderReference.ToString();

            string newId = "";
            string[] nbs = pattern.Split(";".ToCharArray());
            foreach (string nb in nbs)
            {
                string[] ss = nb.Replace("(", "").Replace(")", "").Split(",".ToCharArray());

                if ((int.Parse(ss[0]) + int.Parse(ss[1])) > storeReferenceOrder.Length)
                    throw new Exception(String.Format("DepId Pattern not matched with current order number {0}; Too high index.", storeReferenceOrder));

                if (!nb.StartsWith("-"))
                    newId += storeReferenceOrder.Substring(int.Parse(ss[0]), int.Parse(ss[1]));
                else
                    newId += storeReferenceOrder.Substring(storeReferenceOrder.Length - int.Parse(ss[0]) - int.Parse(ss[1]), int.Parse(ss[1]));
            }

            return newId;
        }
    }
}
