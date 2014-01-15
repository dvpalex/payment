using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using System.IO;

namespace SuperPag.Framework.Helper
{
	public class BasicFunctions 
	{
		public static string GetHexFromByteArray(byte[] byteArray) 
		{
			string hexText = null;

			for (int i = 0; i < byteArray.Length; i++) 
			{
				hexText += byteArray[i].ToString("X2");
			}

			return hexText;
		}

		/// <summary>
		/// Verifica se uma string possui um número
		/// </summary>
		/// <param name="Text">Texto para ser verificado</param>
		/// <returns>True - numérico / False - não numérico</returns>
		public static bool IsNumeric(object Object) 
		{
			//Verifica se não é null ou em branco
			if (Object != null && Object.ToString() != string.Empty) 
			{
				string Text = Object.ToString();

				//Verifica cada caracter se é numérico
				for (int i = 0; i < Text.Length; i++) {
					if (! char.IsNumber(Text, i) ) {
						return false;
					} 
				}
				return true;
			}

			return false;
		}

		public static bool IsNumeric(object Object, Type typeCheck) 
		{
			//Verifica se não é null ou em branco
			if (Object != null && Object.ToString() != string.Empty) 
			{
				string Text = Object.ToString();

				if ( typeCheck == typeof(decimal) ) 
				{
					try 
					{
						decimal value = Convert.ToDecimal( Text );
					}
					catch 
					{
						return false;
					}
				}
				else if ( typeCheck == typeof(double) ) 
				{
					try 
					{
						double value = Convert.ToDouble( Text );
					}
					catch 
					{
						return false;
					}
				}
				else if ( typeCheck == typeof(long) ) 
				{
					try 
					{
						long value = Convert.ToInt64( Text );
					}
					catch 
					{
						return false;
					}
				}
				return true;
			}

			return false;
		}

		public static string ConvertXmlToString(System.Xml.XmlDocument doc) 
		{
			if(doc == null) return "";

			System.IO.StringWriter stringWriter = new System.IO.StringWriter();
			System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(stringWriter);

			xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
			xmlTextWriter.Indentation = 4;
		
			doc.WriteTo(xmlTextWriter);

			return stringWriter.ToString();
		}

		public static string EncryptToBase64String( string stringToEncrypt ) 
		{
			if ( stringToEncrypt == null || stringToEncrypt == "") return "";

			byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(stringToEncrypt);

			return Convert.ToBase64String(inputByteArray);
		}

		public static string DecryptFromBase64String( string stringToDecrypt ) 
		{
			if ( stringToDecrypt == null || stringToDecrypt == "") return "";

			byte[] inputByteArray = Convert.FromBase64String(stringToDecrypt);
			System.Text.Encoding encoding = System.Text.Encoding.UTF8;
			return encoding.GetString( inputByteArray );
		}

		public static string Mid( string var, int length ) 
		{
			return var.Length > length ? var.Substring( 0, length) : var;
		}
	}

	#region Funções para verificação de datas, dias úteis, etc...
	public class DateFunctions {
		private enum DirectionWorkDate {
			Next = 1,
			Previous = 2
		}

		public static DateTime GetNextWorkDate( int Year, int Month, int WorkDay, DateTime[] Holidays, bool AcceptSaturday ) {
			DateTime initialDate = new DateTime( Year, Month, 1 );
			return GetNextWorkDate( initialDate, WorkDay, Holidays, AcceptSaturday  );
		}
		
		public static DateTime GetPreviousWorkDate( int Year, int Month, int WorkDay, DateTime[] Holidays ) {
			DateTime initialDate = new DateTime( Year, Month, 1 );
			return GetPreviousWorkDate( initialDate, WorkDay, Holidays );
		}

		public static DateTime GetNextWorkDate( DateTime dateSource, int WorkDay, DateTime[] Holidays ) {
			return GetNextWorkDate( dateSource, WorkDay, Holidays, DirectionWorkDate.Next, false);
		}

		public static DateTime GetNextWorkDate( DateTime dateSource, int WorkDay, DateTime[] Holidays, bool AcceptSaturday ) 
		{
			return GetNextWorkDate( dateSource, WorkDay, Holidays, DirectionWorkDate.Next, AcceptSaturday);
		}

		public static DateTime GetPreviousWorkDate( DateTime dateSource, int WorkDay, DateTime[] Holidays ) 
		{
			return GetNextWorkDate( dateSource, WorkDay, Holidays, DirectionWorkDate.Previous, false);
		}

		private static DateTime GetNextWorkDate( DateTime dateSource, int WorkDay, DateTime[] Holidays, DirectionWorkDate Direction, bool AcceptSaturday ) {
			DateTime initialDate = dateSource;
			int cWorkDays = 0;
			
			if ( Holidays != null ) Array.Sort( Holidays ); //Faz o sort antes pois é pré-requisito do binarysearch.

			do {
				if (
					initialDate.DayOfWeek != DayOfWeek.Sunday &&
					( initialDate.DayOfWeek != DayOfWeek.Saturday || ( initialDate.DayOfWeek == DayOfWeek.Saturday && AcceptSaturday ) ) &&
					(Holidays == null || ( Holidays != null && Array.BinarySearch( Holidays, initialDate ) < 0 ) ) ) {
					cWorkDays++;
				}
				
				if ( Direction == DirectionWorkDate.Next )
					initialDate = initialDate.AddDays(1);
				else if ( Direction == DirectionWorkDate.Previous )
					initialDate = initialDate.AddDays(-1);
			}

			while(cWorkDays < WorkDay);

			if ( Direction == DirectionWorkDate.Next )
				return initialDate.AddDays(-1);
			else if ( Direction == DirectionWorkDate.Previous )
				return initialDate.AddDays(1);

			throw new Exception("Problemas para calcular o próximo dia útil válido."); 
		}

		/// <summary>
		/// Verifica se a data é válida
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <returns></returns>
		public static bool CheckIsDate(int year, int month, int day) {
			//Faz a validação de acordo com o mês
			switch (month) {
				case 1: case 3: case 5: case 7:
				case 8: case 10: case 12:
					return day > 0 && day <= 31;
				case 4: case 6: case 9: case 11:
					return day > 0 && day <= 30;
				case 2:
					//Verifica se é ano bissexto
					if (((int)year / 4) == year / 4.00) {
						return day > 0 && day <= 29;
					} else {
						return day > 0 && day <= 28;
					}
				default:
					return false;
			}
		}
	}
	#endregion

	#region Verificador de Documentos CPF e CNPJ
	public class Documentos {
		/// <summary>
		/// Validação de CPF
		/// </summary>
		/// <param name="CPF"></param>
		/// <returns></returns>
		public static bool CheckCPF( string CPF ) {
			//Valida digitos
			RegexOptions options = new RegexOptions();
			options |= RegexOptions.Singleline;
			if (Regex.IsMatch( CPF, "[^0-9]+", options)) {
				return false;
			}

			//Valida se não é 00000000000,99999999999
			for(int ii = 0; ii < 10; ii++) {
				if (Regex.IsMatch( CPF, "^[" + ii.ToString() + "]+", options)) {
					Match mc = Regex.Match( CPF, "^[" + ii.ToString() + "]+", options);
					if ( mc.Length >= 11)
						return false;
				}
			}
			
			//Calculo de validação
			int soma,resto,i; 
			int[] digitos; 
			if (CPF.Length == 0)return(true); 
			if (CPF.Length != 11) return(false); 

			digitos = new int[11]; 
			for(i = 0;i<11;i++) { 
				switch(CPF.Substring(i,1)) { 
					case "0": digitos[i] = 0; break; 
					case "1": digitos[i] = 1; break; 
					case "2": digitos[i] = 2; break; 
					case "3": digitos[i] = 3; break; 
					case "4": digitos[i] = 4; break; 
					case "5": digitos[i] = 5; break; 
					case "6": digitos[i] = 6; break; 
					case "7": digitos[i] = 7; break; 
					case "8": digitos[i] = 8; break; 
					case "9": digitos[i] = 9; break; 
				} 
			} 

			soma = 0; 
			for(i = 0;i<9;i++) 
				soma = soma + (digitos[i]*(10 - i)); 

			resto = 11 - (soma - ((soma/11)*11)); 
			if ((resto == 10) || (resto == 11)) resto = 0; 

			if (resto != digitos[9]) 
				return(false); 

			soma = 0; 
			for(i = 0;i<10;i++) 
				soma = soma + (digitos[i]*(11 - i)); 

			resto = 11 - (soma - ((soma / 11) * 11)); 
			if ((resto == 10) || (resto == 11)) resto = 0; 

			if (resto != digitos[10]) 
				return(false); 

			return true;
		}

		/// <summary>
		/// Validação de CNPJ
		/// </summary>
		/// <param name="CNPJ"></param>
		/// <returns></returns>
		public static bool CheckCNPJ( string CNPJ ) {
			int[] digitos, soma, resultado; 
			int nrDig; 
			string ftmt; 
			bool[] CNPJOk; 
			
			if (CNPJ == "00000000000000")
				return false;

			ftmt="6543298765432"; 
			digitos = new int[14]; 
			soma = new int[2]; 
			soma[0] = 0; 
			soma[1] = 0; 
			resultado = new int[2]; 
			resultado[0] = 0; 
			resultado[1] = 0; 
			CNPJOk = new bool[2]; 
			CNPJOk[0] = false; 
			CNPJOk[1] = false; 
			for(nrDig=0;nrDig<14;nrDig++) { 
				digitos[nrDig]=int.Parse(CNPJ.Substring(nrDig,1)); 
				if (nrDig<=11) 
					soma[0]+= (digitos[nrDig]*int.Parse(ftmt.Substring(nrDig+1,1))); 
				if (nrDig<=12) 
					soma[1]+=(digitos[nrDig]*int.Parse(ftmt.Substring(nrDig,1))); 
			} 

			for(nrDig=0; nrDig<2;nrDig++) { 
				resultado[nrDig] = (soma[nrDig] % 11); 
				if ((resultado[nrDig]==0) || (resultado[nrDig]==1)) 
					CNPJOk[nrDig]=(digitos[12+nrDig]==0); 
				else 
					CNPJOk[nrDig]=(digitos[12+nrDig] == (11-resultado[nrDig])); 
			} 
			return(CNPJOk[0]&&CNPJOk[1]); 
		
		}
	}

	#endregion

	#region Classe Extenso

	public class Extenso {
		#region private Fields
		private String[] _u = {
								  "", "um", "dois", "três", "quatro", 
								  "cinco", "seis", "sete", "oito", "nove", 
								  "dez", "onze", "doze", "treze", "quatorze",
								  "quinze", "dezesseis", "dezessete", "dezoito", "dezenove" };
		private String[] _d = {
								  "", "", "vinte", "trinta", "quarenta",
								  "cinquenta", "sessenta", "setenta", "oitenta", "noventa" };
		private String[] _c = {
								  "", "cento", "duzentos", "trezentos", "quatrocentos",
								  "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos"};
		private String[] nome = { 
									"hum bilhão", " bilhões", "hum milhão", " milhões" };
		#endregion

		#region public NumeroPorExtenso
		public string NumeroPorExtenso(decimal num) {
			if(num == 0) 
				return "zero";

			#region Calcula valores 
			Decimal mil_milhoes;
			Decimal milhoes;
			Decimal milhares;
			Decimal unidades;
			Decimal centavos;
			Decimal n = (long)num;
			Decimal frac = num - n;

			// Mil Milhões
			mil_milhoes = (n - n % 1000000000) / 1000000000;
			n -= mil_milhoes * 1000000000;
			// Milhões
			milhoes = (n - n % 1000000) / 1000000;
			n -= milhoes * 1000000;
			// Milhares
			milhares = (n - n % 1000) / 1000;
			n -= milhares * 1000;
			// Unidades
			unidades = n;
			// Arredondamento de centavos
			centavos = (long)(frac * 100);
			if ((long)(frac * 1000 % 10) > 5) 
				centavos++;
			#endregion

			#region Monta valor por extenso
			StringBuilder s = new StringBuilder();
			if (mil_milhoes > 0) {
				if (mil_milhoes == 1) {
					s.Append(nome[0]);
				} 
				else {
					s.Append(NumeroPorExtenso1a999(mil_milhoes));
					s.Append(nome[1]);
				}
				if ((unidades == 0) && ((milhares == 0) && (milhoes > 0))) {
					s.Append(" e ");
				} 
				else if ((unidades != 0) || ((milhares != 0) || (milhoes != 0))) {
					s.Append(", ");
				}
			}
			if (milhoes > 0) {
				if (milhoes == 1) {
					s.Append(nome[2]);
				} 
				else {
					s.Append(NumeroPorExtenso1a999(milhoes));
					s.Append(nome[3]);
				}
				if ((unidades == 0) && (milhares > 0)) {
					s.Append(" e ");
				} 
				else if ((unidades != 0) || (milhares != 0)) {
					s.Append(", ");
				}
			}
			if (milhares > 0) {
				if (milhares != 1) {
					s.Append(NumeroPorExtenso1a999(milhares));
				}
				s.Append("hum mil");
				if (unidades > 0) {
					if ((milhares > 100) && (unidades > 100)) {
						s.Append(", ");
					} 
					else if (((unidades % 100) != 0) || ((unidades % 100 == 0) && (milhares < 10))) {
						s.Append(" e ");
					} 
					else {
						s.Append(" ");
					}
				}
			}
			s.Append(NumeroPorExtenso1a999(unidades));
			if (num > 0) {
				s.Append(((long)num == 1L) ? " real" : " reais");
			}
			if (centavos != 0) {
				s.Append(" e ");
				s.Append(NumeroPorExtenso1a999(centavos));
				s.Append((centavos==1) ? " centavo" : " centavos");
			}
			#endregion

			return s.ToString();
		}
		#endregion

		#region private NumeroPorExtenso1a999
		private string NumeroPorExtenso1a999(decimal n) {
			#region Verifica valor
			if (n > 999) 
				return "Erro: número > 999";
			if(n < 0)
				return "Erro: número < 0";
			#endregion

			#region Monta valor por extenso
			if (n == 100) 
				return "cem";

			string extensoDoNumero = String.Empty;
			if (n > 99) {
				extensoDoNumero += _c[(int)(n / 100)];
				if (n % 100 > 0) 
					extensoDoNumero += " e ";
			}
			if (n % 100 < 20) 
				extensoDoNumero += _u[(int)n % 100];
			else {
				extensoDoNumero += _d[((int)n % 100) / 10];
				if ((n % 10 > 0) && (n > 10)) {
					extensoDoNumero += " e ";
					extensoDoNumero += _u[(int)n % 10];
				}
			}
			#endregion

			return extensoDoNumero;
		}
		#endregion
	}
	#endregion

	#region Classe FileFunctions 
	public class FileFunctions {
		public static byte[] GetBytes(Stream stream) {
			int lenght = (int)stream.Length;
			byte[] bytes = new byte[lenght];
			// Passa os dados da stream para o array de bytes
			stream.Read(bytes, 0, lenght);
			return bytes;
		}

		public static void SaveFile(string fileNameWithPath, byte[] bytes) {
			FileStream fs = null;
			if(File.Exists(fileNameWithPath)) {
				throw new ApplicationException(
					String.Format("Já existe um arquivo gravado com este nome! Nome do arquivo:{0}", fileNameWithPath));
			}

			try {
				// Cria um novo arquivo
				fs = new FileStream(fileNameWithPath, FileMode.CreateNew);
			}
			catch(UnauthorizedAccessException) {
				throw new ApplicationException(String.Format("Acesso negado para criação do arquivo! Nome do arquivo:{0}", fileNameWithPath));
			}
			catch(Exception ex) {
				throw new ApplicationException(String.Format("Erro na gravação do arquivo:{0}! Mensagem de erro: {1}", fileNameWithPath, ex.Message));
			}
			// Cria o binary writer para escrever 
			BinaryWriter bw = new BinaryWriter(fs);
			// Escreve os bytes no file stream
			bw.Write(bytes, 0,bytes.Length);
			bw.Close();
			fs.Close();
		}
	}
	#endregion

	#region Funcoes para conveter imagem para byte array e vice-versa
	public class ImageFunctions {
		public static System.Drawing.Image ConvertArrayBytesToImage( byte[] array ) {
			return
				System.Drawing.Image.FromStream( new System.IO.MemoryStream( array ) );
		}

		public static System.Drawing.Image Resize( System.Drawing.Image image, int Width, int Height ) {
			return  
				image.GetThumbnailImage( Width, Height, null, new System.IntPtr() );
		}

		public static System.Drawing.Image ResizeArray( byte[] array, int Width, int Height ) {
			System.Drawing.Image image = ConvertArrayBytesToImage( array );
			return  Resize( image, Width, Height);
		}

		public static byte[] ConvertImageToByteArray( System.Drawing.Image image ) {
			System.IO.MemoryStream buffer = new System.IO.MemoryStream();
			image.Save( buffer, GetType(image) );

			return buffer.ToArray();
		}

		public static string GetContentType( System.Drawing.Image imgf ) {
			if( System.Drawing.Imaging.ImageFormat.Jpeg.Equals(imgf) ) {
				return "image/jpeg";
			}
			else if( System.Drawing.Imaging.ImageFormat.Gif.Equals(imgf) ) {
				return "image/gif";
			}
			else if( System.Drawing.Imaging.ImageFormat.Tiff.Equals(imgf) ) {
				return "image/tiff";
			}
			else if( System.Drawing.Imaging.ImageFormat.Png.Equals(imgf) ) {
				return "image/png";
			}
			else if( System.Drawing.Imaging.ImageFormat.Bmp.Equals(imgf) ) {
				return "image/bmp";
			}
			else if( System.Drawing.Imaging.ImageFormat.Emf.Equals(imgf) ) {
				return "image/emf";
			}
			else if( System.Drawing.Imaging.ImageFormat.Exif.Equals(imgf) ) {
				return "image/exif";
			}
			else if( System.Drawing.Imaging.ImageFormat.Icon.Equals(imgf) ) {
				return "image/icon";
			}
			else if( System.Drawing.Imaging.ImageFormat.Wmf.Equals(imgf) ) {
				return "image/wmf";
			}
			else {
				return null;
			}
		}

		public static System.Drawing.Imaging.ImageFormat GetType( System.Drawing.Image imgf ) {
			if( System.Drawing.Imaging.ImageFormat.Jpeg.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Jpeg;
			}
			else if( System.Drawing.Imaging.ImageFormat.Gif.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Gif;
			}
			else if( System.Drawing.Imaging.ImageFormat.Tiff.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Tiff;
			}
			else if( System.Drawing.Imaging.ImageFormat.Png.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Png;
			}
			else if( System.Drawing.Imaging.ImageFormat.Bmp.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Bmp;
			}
			else if( System.Drawing.Imaging.ImageFormat.Emf.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Emf;
			}
			else if( System.Drawing.Imaging.ImageFormat.Exif.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Exif;
			}
			else if( System.Drawing.Imaging.ImageFormat.Icon.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Icon;
			}
			else if( System.Drawing.Imaging.ImageFormat.Wmf.Equals(imgf) ) {
				return System.Drawing.Imaging.ImageFormat.Wmf;
			}
			else {
				return System.Drawing.Imaging.ImageFormat.Bmp;
			}
		}
	}
	#endregion

	#region Procura palavras em um string
	public class FindWord {
		private string _WordToSearch;
		private string _RegularExpression;
		private Regex _regex;

		public string WordToSearch {
			get { return _WordToSearch; }
		}

		public string RegularExpression {
			get { return _RegularExpression; }
		}

		public FindWord(string wordToSearch) {
			_WordToSearch = wordToSearch;
			_RegularExpression = String.Format(@"\b{0}\b", _WordToSearch);
			_regex = new Regex(_RegularExpression); 
		}

		public bool FoundInText(string text) {			
			return _regex.Match(text).Success;
		}
	}
	#endregion
}
