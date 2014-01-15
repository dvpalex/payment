using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using Itaucripto;

namespace SuperPag.Agents.ItauShopLine
{
    public class Crypto
    {
        private string _criptStr;
        private string _codEmp;
        private int _pedido;
        private short _tipPag;

        public Crypto()
        {
        }

        public Crypto(string convenio, string referenceOrder, string value, string obs, string key,
            string sederName, string clienteCode, string clienteNumber, string sederAdress, string sederDistricit,
            string sederPostalCode, string sederCity, string sederState, string expirationPaymentValue, string returnUrl)
        {
            Itaucripto.cripto oCripto = new Itaucripto.cripto();

            _criptStr = oCripto.geraDados(convenio, referenceOrder, value, obs, key, sederName,
                clienteCode, clienteNumber, sederAdress, sederDistricit, sederPostalCode, sederCity, sederState,
                expirationPaymentValue, returnUrl);
        }

        public string Decripto(string dados, string chave)
        {
            Itaucripto.cripto oCripto = new Itaucripto.cripto();
            
            string ret = oCripto.decripto(dados, chave);

            _codEmp = oCripto.retornaCodEmp();
            _pedido = oCripto.retornaPedido();
            _tipPag = oCripto.retornaTipPag();

            return ret;
        }

        public string GeraConsulta(string codEmp, string pedido, string formato, string chave)
        {
            Itaucripto.cripto oCripto = new Itaucripto.cripto();

            return oCripto.geraConsulta(codEmp, pedido, formato, chave);
        }
        
        public string CriptStr
        {
            get { return _criptStr; }
        }
        
        public string CodEmp
        {
            get { return _codEmp; }
        }
        
        public int Pedido
        {
            get { return _pedido; }
        }
        
        public short TipPag
        {
            get { return _tipPag; }
        }
    }
}
