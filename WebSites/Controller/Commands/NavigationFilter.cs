using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;

namespace Controller.Lib.Commands
{
    public class NavigationFilter : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            this.AddMessage(new MNavigationSearch());

            return Map.Views.NavigationFilter;
        }
    }

    [Serializable]
    public class MNavigationSearch : Message
    {
        private string cpf;
        private DateTime initialDate;
        private DateTime finalDate;

        public DateTime FinalDate
        {
            get { return finalDate; }
            set { finalDate = value; }
        }
        public DateTime InitialDate
        {
            get { return initialDate; }
            set { initialDate = value; }
        }
        public string CPF
        {
            get { return cpf; }
            set { cpf = value; }
        }
	
    }
}
