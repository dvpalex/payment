using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DConsumer))]
    [Serializable()]
    public class MConsumer : Message
    {

        public MConsumer() { }

        private long _consumerId;
        [Mapping(DConsumer.Fields.consumerId)]
        public long ConsumerId
        {
            get { return _consumerId; }
            set { _consumerId = value; }
        }


        private string _cPF;
        [Mapping(DConsumer.Fields.CPF)]
        public string CPF
        {
            get { return _cPF; }
            set { _cPF = value; }
        }


        private string _rG;
        [Mapping(DConsumer.Fields.RG)]
        public string RG
        {
            get { return _rG; }
            set { _rG = value; }
        }


        private string _cNPJ;
        [Mapping(DConsumer.Fields.CNPJ)]
        public string CNPJ
        {
            get { return _cNPJ; }
            set { _cNPJ = value; }
        }


        private string _iE;
        [Mapping(DConsumer.Fields.IE)]
        public string IE
        {
            get { return _iE; }
            set { _iE = value; }
        }


        private string _name;
        [Mapping(DConsumer.Fields.name)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private DateTime _birthDate;
        [Mapping(DConsumer.Fields.birthDate)]
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }


        private string _ger;
        [Mapping(DConsumer.Fields.ger)]
        public string Ger
        {
            get { return _ger; }
            set { _ger = value; }
        }


        private string _civilState;
        [Mapping(DConsumer.Fields.civilState)]
        public string CivilState
        {
            get { return _civilState; }
            set { _civilState = value; }
        }


        private string _occupation;
        [Mapping(DConsumer.Fields.occupation)]
        public string Occupation
        {
            get { return _occupation; }
            set { _occupation = value; }
        }


        private string _phone;
        [Mapping(DConsumer.Fields.phone)]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }


        private string _commercialPhone;
        [Mapping(DConsumer.Fields.commercialPhone)]
        public string CommercialPhone
        {
            get { return _commercialPhone; }
            set { _commercialPhone = value; }
        }


        private string _celularPhone;
        [Mapping(DConsumer.Fields.celularPhone)]
        public string CelularPhone
        {
            get { return _celularPhone; }
            set { _celularPhone = value; }
        }


        private string _fax;
        [Mapping(DConsumer.Fields.fax)]
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }


        private string _responsibleName;
        [Mapping(DConsumer.Fields.responsibleName)]
        public string ResponsibleName
        {
            get { return _responsibleName; }
            set { _responsibleName = value; }
        }


        private string _responsibleCPF;
        [Mapping(DConsumer.Fields.responsibleCPF)]
        public string ResponsibleCPF
        {
            get { return _responsibleCPF; }
            set { _responsibleCPF = value; }
        }


        private string _email;
        [Mapping(DConsumer.Fields.email)]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private MCConsumerAddress _consumerAddresses;
        public MCConsumerAddress ConsumerAddresses
        {
            get { return _consumerAddresses; }
            set { _consumerAddresses = value; }
        }
    }

    [Serializable]
    [CollectionOf(typeof(MConsumer))]
	public class MCConsumer : MessageCollection
	{
	}
}