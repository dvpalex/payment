using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection; 

namespace SuperPag.Framework.Helper
{
    public class GenericSort : IComparer
	{
		String sortPropertyName;
		bool isCrescente;

		public GenericSort(String sortPropertyName, bool isCrescente) 
		{
			this.sortPropertyName = sortPropertyName;
			this.isCrescente = isCrescente;
		}

		public int Compare(object x, object y)
		{
            string[] childMessages = sortPropertyName.Split('.');

            int i = 0;
            while (childMessages.Length > 1 && i < childMessages.Length - 1)
            {
                x = x.GetType().GetProperty(childMessages[i]).GetValue(x,null);
                y = y.GetType().GetProperty(childMessages[i]).GetValue(y, null);
                i++;
            }

//			IComparable ic1 = (IComparable)x.GetType().GetProperty(sortPropertyName).GetValue(x, null);
//			IComparable ic2 = (IComparable)y.GetType().GetProperty(sortPropertyName).GetValue(y, null);

            IComparable ic1 = (IComparable)x.GetType().GetProperty(childMessages[i]).GetValue(x, null);
            IComparable ic2 = (IComparable)y.GetType().GetProperty(childMessages[i]).GetValue(y, null);

			//eval!!
			//IComparable ic1 = (IComparable)x.GetType().GetMethod(sortMethodName).Invoke(x, null);

			if(isCrescente)
				return ic1.CompareTo(ic2);
			else
				return ic2.CompareTo(ic1);
		}
	}
}
