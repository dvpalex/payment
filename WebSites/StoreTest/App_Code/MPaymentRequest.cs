using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;

namespace StoreTest
{
	public enum Sexo
	{
		[XmlEnum("")]
		None,
		[XmlEnum("M")]
		Masculino,
		[XmlEnum("F")]
		Feminino
	}


	public enum EstadoCivil
	{
		[XmlEnum("")]
		None,
		[XmlEnum("C")]
		Casado,
		[XmlEnum("D")]
		Divorciado,
		[XmlEnum("S")]
		Solteiro,
		[XmlEnum("V")]
		Viuvo,
		[XmlEnum("O")]
		Outro
	}


	#region SmartPagDataTypes
	[Serializable]
	public class SmartPagDateTime : IXmlSerializable
	{
		private DateTime _value = DateTime.MinValue;

		public static implicit operator SmartPagDateTime (DateTime dateTimeValue)
		{
			SmartPagDateTime smartPagDateTime = new SmartPagDateTime();
			smartPagDateTime._value = dateTimeValue;
			return smartPagDateTime;
		}

		public static implicit operator DateTime (SmartPagDateTime smartPagDateTime)
		{
			if (smartPagDateTime != null)
				return smartPagDateTime._value;
			else
				return DateTime.MinValue;
		}

		public XmlSchema GetSchema() { return null; }

		public void ReadXml(XmlReader reader)
		{
			string text = reader.ReadElementString();
			text = string.Format("{0}-{1}-{2}", text.Substring(0, 4), text.Substring(4, 2), text.Substring(6, 2));
			this._value = DateTime.Parse(text);
		}

		public void WriteXml(XmlWriter writer)
		{
			if (_value != DateTime.MinValue) writer.WriteString(_value.ToString("yyyyMMdd"));
		}
	}

	[Serializable]
	public class SmartPagInt : IXmlSerializable
	{
		private NumberFormatInfo _numberFI;
		private int _value = int.MinValue;

		public SmartPagInt()
		{
			_numberFI = new NumberFormatInfo();
			_numberFI.NumberDecimalDigits = 0;
			_numberFI.NumberGroupSeparator = "";
			_numberFI.NumberNegativePattern = 1;
			_numberFI.NumberDecimalSeparator = " ";
		}

		public static implicit operator SmartPagInt(int intValue)
		{
			SmartPagInt smartPagInt = new SmartPagInt();
			smartPagInt._value = intValue;
			return smartPagInt;
		}

		public static implicit operator int (SmartPagInt smartPagInt)
		{
			if (smartPagInt != null)
				return smartPagInt._value;
			else
				return int.MinValue;
		}

		public XmlSchema GetSchema() { return null; }

		public void ReadXml(XmlReader reader)
		{
			string text = reader.ReadElementString();
			this._value = int.Parse(text);
		}

		public void WriteXml(XmlWriter writer)
		{
			if (_value != int.MinValue) writer.WriteString(_value.ToString("N", _numberFI).Replace(" ", ""));
		}
	}

	[Serializable]
	public class SmartPagDouble : IXmlSerializable
	{
		private double _value = double.MinValue;
		private NumberFormatInfo _numberFI;

		public SmartPagDouble()
		{
			_numberFI = new NumberFormatInfo();
			_numberFI.CurrencyDecimalDigits = 2;
			_numberFI.CurrencyGroupSeparator = "";
			_numberFI.CurrencyNegativePattern = 1;
			_numberFI.CurrencyDecimalSeparator = " ";
			_numberFI.CurrencySymbol = "";
		}

		public static implicit operator SmartPagDouble (double doubleValue)
		{
			SmartPagDouble smartPagDouble = new SmartPagDouble();
			smartPagDouble._value = doubleValue;
			return smartPagDouble;
		}

		public static implicit operator double (SmartPagDouble smartPagDouble)
		{
			if (smartPagDouble != null)
				return smartPagDouble._value;
			else
				return double.MinValue;
		}

		public XmlSchema GetSchema() { return null; }

		public void ReadXml(XmlReader reader)
		{
			string text = reader.ReadElementString();
			int len = text.Length;
			text = string.Format("{0}{1}{2}", text.Substring(0, len - 2), NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator, text.Substring(len - 2));
			this._value = double.Parse(text);
		}

		public void WriteXml(XmlWriter writer)
		{
			string text = _value.ToString("C", _numberFI).Replace(" ", "");
			if (_value != double.MinValue) writer.WriteString(text);
		}
	}
	#endregion

	#region Elemento Item
	[Serializable]
	public class MItem
	{
		[XmlElement("codigo_item")]				public string Codigo;
		[XmlElement("descricao_item")]			public string Descricao;
		[XmlElement("quantidade_item")]			public SmartPagInt Quantidade;
		[XmlElement("valor_unitario_item")]		public SmartPagDouble ValorUnitario;
	}

	[Serializable]
	public class MItens
	{
		[XmlElement("quantidade_pedido")]		public int QuantidadePedido;
		[XmlElement("item")]					public MItem[] ArrayOfItem;
	}
	#endregion

	#region Elemento DadosCliente
	[Serializable]
	public class MPessoaFisica
	{
		[XmlElement("nome_pf")]				public string Nome;
		[XmlElement("cpf_pf")]				public string CPF;
		[XmlElement("sexo_pf")]				public Sexo Sexo;
		[XmlElement("estado_civil_pf")]		public EstadoCivil EstadoCivil;
		[XmlElement("profissao_pf")]		public string Profissao;
		[XmlElement("email_pf")]			public string Email;
		[XmlElement("telefone_pf")]			public string Telefone;
		[XmlElement("fax_pf")]				public string Fax;
		[XmlElement("data_nascimento_pf")]	public SmartPagDateTime DataNascimento;
	}

	[Serializable]
	public class MPessoaJuridica
	{
		[XmlElement("razao_social_pj")]			public string RazaoSocial;
		[XmlElement("CGC_pj")]					public string CGC;
		[XmlElement("email_pj")]				public string Email;
		[XmlElement("telefone_pj")]				public string Telefone;
		[XmlElement("fax_pj")]					public string Fax;
		[XmlElement("Inscricao_estadual_pj")]	public string InscricaoEstadual;
		[XmlElement("cpf_responsavel_pj")]		public string CPFResponsavel;
		[XmlElement("nome_responsavel_pj")]		public string NomeResponsavel;
	}

	[Serializable]
	public class MEnderecoCobranca
	{
		[XmlElement("logradouro_ec")]	public string Logradouro;
		[XmlElement("endereco_ec")]		public string Endereco;
		[XmlElement("numero_ec")]		public string Numero;
		[XmlElement("complemento_ec")]	public string Complemento;
		[XmlElement("bairro_ec")]		public string Bairro;
		[XmlElement("cidade_ec")]		public string Cidade;
		[XmlElement("cep_ec")]			public string CEP;
		[XmlElement("estado_ec")]		public string Uf;
		[XmlElement("pais_ec")]			public string Pais;
	}

	[Serializable]
	public class MEnderecoEntrega
	{
		[XmlElement("nome_responsavel_ee")]		public string NomeResponsavel;
		[XmlElement("telefone_responsavel_ee")] public string TelefoneResponsavel;
		[XmlElement("logradouro_ee")]			public string Logradouro;
		[XmlElement("endereco_ee")]				public string Endereco;
		[XmlElement("numero_ee")]				public string Numero;
		[XmlElement("complemento_ee")]			public string Complemento;
		[XmlElement("bairro_ee")]				public string Bairro;
		[XmlElement("cidade_ee")]				public string Cidade;
		[XmlElement("cep_ee")]					public string CEP;
		[XmlElement("estado_ee")]				public string Uf;
		[XmlElement("pais_ee")]					public string Pais;
	}

	[Serializable]
	public class MDadosCliente
	{
		[XmlElement("pessoa_fisica")]		public MPessoaFisica PessoaFisica;
		[XmlElement("pessoa_juridica")]		public MPessoaJuridica PessoaJuridica;
		[XmlElement("endereco_cobranca")]	public MEnderecoCobranca EnderecoCobranca;
		[XmlElement("endereco_entrega")]	public MEnderecoEntrega EnderecoEntrega;
	}
	#endregion

	#region Elemento SetupLoja
	[Serializable]
	public class Urls
	{
		[XmlElement("post_final")]		public string PostFinal;
		[XmlElement("link_retorno")]	public string LinkRetorno;
	}

	[Serializable]
	public class SetupLoja
	{
		[XmlElement("urls")]			public Urls Urls;
	}
	#endregion

	#region ParametrosOpcionais
	[Serializable]
	public class ParametrosOpcionais
	{
		[XmlElement("param_op1_ped")]				public string ParamOp1Ped;
		[XmlElement("param_op2_ped")]				public string ParamOp2Ped;
		[XmlElement("param_op3_ped")]				public string ParamOp3Ped;
		[XmlElement("post_xml")]					public string PostXML;
		[XmlElement("forma_pagto")]					public string FormaPagto;
		[XmlElement("bandeira")]					public string Bandeira;
		[XmlElement("pqtdparcelas")]				public SmartPagInt PQtdParcelas;
		[XmlElement("show_tela_finalizacao")]		public SmartPagInt ShowTelaFinalizacao;
		[XmlElement("frame50")]						public SmartPagInt Frame50;
		[XmlElement("instrucao_finalizacao")]		public string InstrucaoFinalizacao;
		[XmlElement("Instrucao_boleto")]			public string InstrucaoBoleto;
		[XmlElement("urlbotao1")]					public string UrlBotao1;
		[XmlElement("urlbotao2")]					public string UrlBotao2;
		[XmlElement("urlbotao3")]					public string UrlBotao3;
		[XmlElement("urlbotao4")]					public string UrlBotao4;
		[XmlElement("urlbotao5")]					public string UrlBotao5;
		[XmlElement("urlbotao6")]					public string UrlBotao6;
		[XmlElement("urlbotao7")]					public string UrlBotao7;
		[XmlElement("link_botao6")]					public string LinkBotao6;
		[XmlElement("envia_email_cliente")]			public SmartPagInt EnviaEmailClient;
		[XmlElement("show_valor_total")]			public SmartPagInt ShowValorTotal;
		[XmlElement("COB_QUANTIDADE")]				public SmartPagInt COBQuantidade;
		[XmlElement("COB_LIQ_1PAR")]				public SmartPagInt COBLiq1Par;
		[XmlElement("COB_RECORRENCIA")]				public SmartPagInt COBRecorrencia;
		[XmlElement("COB_DATA_BASE_AGENDAMENTO")]	public SmartPagDateTime COBDataBaseAgendamento;
		[XmlElement("data_boleto")]					public SmartPagDateTime DataBoleto;
		[XmlElement("idioma")]						public string Idioma;
	}
	#endregion

	#region Elemento DadosPagamento
	[Serializable]
	public class MDadosPagamento
	{
		[XmlElement("bandeira")]			public int Bandeira = int.MinValue;
		[XmlElement("id_trans")]			public string IDTransacao = null;
		[XmlElement("num_autorizacao")]		public string NumAutorizacao = null;
		[XmlElement("urlboleto1")]			public string UrlBoleto = null;
		[XmlElement("oct1")]				public string Oct = null;
	}
	#endregion

	#region ControleLoja
	[Serializable]
	public class ControleLoja
	{
		[XmlElement("profile_key")]					public string profile_key;
	}
	#endregion
	
	[Serializable]
	[XmlRoot("pedido")]
	public class MPaymentRequest
	{
		[XmlElement("numero_pedido")]		
		public string NumeroPedido;
		[XmlElement("forma_pagto")]				
		public string FormaPagamento;
		[XmlElement("valor_total_pedido")]		
		public SmartPagDouble ValorTotalPedido;
		[XmlElement("valor_frete_pedido")]		
		public SmartPagDouble ValorFretePedido;
//		[XmlElement("codigo_meio_pagamento")]	public int CodigoMeioPagamento = int.MinValue;
		[XmlElement("itens")]					
		public MItens Itens;
		[XmlElement("dados_cliente")]			
		public MDadosCliente DadosCliente;
		[XmlElement("setup_loja")]				
		public SetupLoja SetupLoja;
		[XmlElement("parametros_opcionais")]	
		public ParametrosOpcionais ParametrosOpcionais;
//		[XmlElement("dados_pagamento")]			public MDadosPagamento DadosPagamento;
		[XmlElement("controle_loja")]			
		public ControleLoja ControleLoja;
	}


	[Serializable]
	[XmlRoot("postpagto")]
	public class MPaymentConfirmation
	{
		[XmlElement("pedido")]	
			public string Pedido;
		[XmlElement("num_parcela")]			public SmartPagInt Parcela;
		[XmlElement("forma_pagto")]			public string FormaPagamento;
		[XmlElement("cod_controle")]		public string Controle;
		[XmlElement("val_parcial_pedido")]	public SmartPagDouble ValorParcialPedido;
		[XmlElement("val_final_pedido")]	public SmartPagDouble ValorFinalPedido;
		[XmlElement("sfrete")]				public string Frete;
	}

}
