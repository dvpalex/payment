using System;
using System.Reflection;
using System.Web.UI;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{
    /// <summary>
    /// Summary description for EfinExportExcel.
    /// </summary>

    [ToolboxData("<{0}:ButtonExportExcel runat=server></{0}:ButtonExportExcel>")]
    public class ButtonExportExcel : System.Web.UI.WebControls.Button
    {
        //define o tipo de exportacao 
        public enum ExportTypeEnum
        {
            EXCEL,
            CSV,
            TEXT
        }

        public enum SeparatorTypeEnum
        {
            COMMA,
            SEPARATOR,
            TAB
        }

        public enum PrintHeaderEnum
        {
            TRUE,
            FALSE
        }

        public enum EncodeTypeEnum
        {
            UTF8,
            Unicode,
            ISO
        }

        private static MessageCollection _dadosExportacao;
        private const string C_HTTP_HEADER_CONTENT = "Content-Disposition";
        private const string C_HTTP_ATTACHMENT = "attachment;filename=";
        private const string C_HTTP_INLINE = "inline;filename=";
        private const string C_HTTP_CONTENT_TYPE_OCTET = "application/octet-stream";
        private const string C_HTTP_CONTENT_TYPE_EXCEL = "application/ms-excel";
        private const string C_HTTP_CONTENT_TYPE_TEXT = "application/text";
        private const string C_HTTP_CONTENT_LENGTH = "Content-Length";
        private const string C_QUERY_PARAM_CRITERIA = "Criteria";
        private const string C_ERROR_NO_RESULT = "Dados não encontrados";

        private ExportTypeEnum _ExportType;
        private SeparatorTypeEnum _SeparatorType;
        private PrintHeaderEnum _PrintHeader;
        private EncodeTypeEnum _EncodeType;
        private string _delimiter = "";

        private const char CHRTAB = (char)9;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MessageCollection DadosExportacao
        {
            get { return _dadosExportacao; }
            set { _dadosExportacao = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NomeArquivo
        {
            get
            {
                string _NomeArquivo = (string)base.ViewState["NomeArquivo"];

                if (_NomeArquivo == null)
                    return "ExportData.xls";

                return _NomeArquivo;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("NomeArquivo", "O nome para o arquivo deve ser válido.");
                base.ViewState["NomeArquivo"] = value;
            }
        }

        public ExportTypeEnum ExportType
        {
            get { return _ExportType; }
            set { _ExportType = value; }
        }

        public SeparatorTypeEnum SeparatorType
        {
            get { return _SeparatorType; }
            set { _SeparatorType = value; }
        }

        public PrintHeaderEnum PrintHeader
        {
            get { return _PrintHeader; }
            set { _PrintHeader = value; }
        }

        public EncodeTypeEnum EncodeType
        {
            get { return _EncodeType; }
            set { _EncodeType = value; }
        }


        protected override void OnClick(EventArgs e)
        {
            ExportaDados();
        }

        private string ConvertDataViewToString(MessageCollection mc, string Delimiter, string Separator)
        {
            StringBuilder ResultBuilder = new StringBuilder();
            ResultBuilder.Length = 0;

            if (_PrintHeader == PrintHeaderEnum.TRUE)
            {
                foreach (PropertyInfo pi in mc[0].GetType().GetProperties())
                {
                    ResultBuilder.Append(pi.Name);
                    ResultBuilder.Append(Separator);
                }

                ResultBuilder.Append(Environment.NewLine);
            
            }

            if (ResultBuilder.Length > Separator.Trim().Length)
                ResultBuilder.Length = ResultBuilder.Length - Separator.Trim().Length;

            int inicio = (_PrintHeader == PrintHeaderEnum.TRUE) ? 0 : 1;

            foreach (Message m in mc)
            {
                foreach (PropertyInfo pi in m.GetType().GetProperties())
                {
                    if (Delimiter.Trim().Length > 0)
                        ResultBuilder.Append(Delimiter);
                    if (_SeparatorType != SeparatorTypeEnum.SEPARATOR)
                    {
                        if(pi.GetValue(m, null) != null)
                            ResultBuilder.Append(pi.GetValue(m, null).ToString());
                    }
                    else
                        ResultBuilder.Append(pi.GetValue(m, null).ToString());
                    if (Delimiter.Trim().Length > 0)
                        ResultBuilder.Append(Delimiter);
                    ResultBuilder.Append(Separator);
                }
                ResultBuilder.Length = ResultBuilder.Length - 1;
                ResultBuilder.Append(Environment.NewLine);
            }
            return ResultBuilder.ToString();
        }

        public void ExportaDados()
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader(C_HTTP_HEADER_CONTENT, C_HTTP_ATTACHMENT + this.NomeArquivo);

            if (_ExportType == ExportTypeEnum.CSV)
                response.ContentType = C_HTTP_CONTENT_TYPE_OCTET;
            else if (_ExportType == ExportTypeEnum.TEXT)
                response.ContentType = C_HTTP_CONTENT_TYPE_TEXT;
            else
                response.ContentType = C_HTTP_CONTENT_TYPE_EXCEL;

            string _exportContent = string.Empty;
            if ((_dadosExportacao != null) && (_dadosExportacao.Count > 0))
            {
                if (_delimiter == null)
                    _delimiter = "";

                if (_SeparatorType == SeparatorTypeEnum.TAB)
                    _exportContent = ConvertDataViewToString(_dadosExportacao, _delimiter, CHRTAB.ToString());
                else if (_SeparatorType == SeparatorTypeEnum.SEPARATOR)
                    _exportContent = ConvertDataViewToString(_dadosExportacao, _delimiter, ";");
                else
                    _exportContent = ConvertDataViewToString(_dadosExportacao, _delimiter, ",");
            }
            if (_exportContent.Length <= 0)
                _exportContent = C_ERROR_NO_RESULT;

            
            


            if (_EncodeType == EncodeTypeEnum.UTF8)
            {
                System.Text.UTF8Encoding EncodingUTF8 = new System.Text.UTF8Encoding();
                response.AddHeader(C_HTTP_CONTENT_LENGTH, EncodingUTF8.GetByteCount(_exportContent).ToString());
                response.BinaryWrite(EncodingUTF8.GetBytes(_exportContent));
            }
            else if (_EncodeType == EncodeTypeEnum.ISO)
            {
                response.AddHeader(C_HTTP_CONTENT_LENGTH, System.Text.Encoding.GetEncoding("iso-8859-1").GetByteCount(_exportContent).ToString());
                response.BinaryWrite(System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(_exportContent));
            }
            else
            {
                System.Text.UnicodeEncoding EncodingUnicode = new System.Text.UnicodeEncoding();
                response.AddHeader(C_HTTP_CONTENT_LENGTH, EncodingUnicode.GetByteCount(_exportContent).ToString());
                response.BinaryWrite(EncodingUnicode.GetBytes(_exportContent));
            }


            response.Charset = "";

            //' Stop execution of the current page
            response.End();
        }
    }
}
