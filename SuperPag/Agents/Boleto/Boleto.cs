using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

namespace SuperPag.Agents.Boleto
{
    /// <summary>
    /// Summary description for Boleto
    /// </summary>
    public class Boleto
    {
        public Boleto() { }

        public string DigitoCodigoBarra(string iNumero)
        {
            int iAcumula = 0;
            int resto = 0;
            int iIndice = 2;
            for (int i = iNumero.ToString().Length - 1; i >= 0; i--)
            {
                iAcumula = iAcumula + ((Convert.ToInt32(iNumero.ToString().Substring(i, 1))) * (iIndice));
                iIndice++;
                if (iIndice > 9)
                    iIndice = 2;
            }

            Math.DivRem(iAcumula, 11, out resto);
            decimal iDigito = 11 - resto;

            if (iDigito < 0)
                iDigito *= -1;

            if (iDigito == 0 || iDigito == 1 || iDigito > 9)
                iDigito = 1;

            return (iDigito.ToString());
        }

        public string DigitoLinhaDigitavel(string iNumero)
        {
            int iAcumula = 0;
            int iIndice = ((iNumero.ToString().Length % 2) == 0 ? 1 : 2);
            int iResult = 0;
            int iDigito = 0;

            for (int i = 0; i < iNumero.ToString().Length; i++)
            {
                iResult = (Convert.ToInt32((iNumero.ToString().Substring(i, 1))) * iIndice);
                if (iResult >= 10)
                {
                    iResult = Convert.ToInt16(iResult.ToString().Substring(0, 1)) + Convert.ToInt16(iResult.ToString().Substring(1, 1));
                }
                iAcumula = iAcumula + iResult;

                if (iIndice == 1)
                    iIndice = 2;
                else
                    iIndice = 1;
            }


            if (iAcumula < 10)
            {
                iDigito = 10 - iAcumula;
            }
            else
            {
                float iResto = iAcumula;
                while (((((int)(iResto / 10)) * 10) - iAcumula) < 0)
                {
                    iResto++;
                }
                iDigito = (int)iResto - iAcumula;
            }
            iDigito = (iDigito == 10 ? 0 : iDigito);
            return (iDigito.ToString());
        }

        public string FatorVencimento(DateTime dVencimento)
        {
            TimeSpan iFator;
            DateTime dDataBase = new DateTime(1997, 10, 07);
            iFator = dVencimento.Subtract(dDataBase);
            return (iFator.Days.ToString());
        }

        public string LinhaDigitavel(string sCodigoBarra)
        {
            string sLinhaCampo1;
            string sLinhaCampo2;
            string sLinhaCampo3;
            string sLinhaCampo4;
            string sLinhaCampo5;

            sLinhaCampo2 = sCodigoBarra.Substring(24, 10);
            sLinhaCampo3 = sCodigoBarra.Substring(34, 10);
            sLinhaCampo4 = sCodigoBarra.Substring(4, 1) + " ";
            sLinhaCampo5 = sCodigoBarra.Substring(5, 4) + sCodigoBarra.Substring(9, 10);
            if (sLinhaCampo5 == "00000000000000")
            {
                sLinhaCampo5 = "000";
            }

            sLinhaCampo1 = sCodigoBarra.Substring(0, 4) + sCodigoBarra.Substring(19, 1) + "."
                + sCodigoBarra.Substring(20, 4) + this.DigitoLinhaDigitavel(sCodigoBarra.Substring(0, 4) + sCodigoBarra.Substring(19, 5)) + " ";
            sLinhaCampo2 = sLinhaCampo2.Substring(0, 5) + "." + sLinhaCampo2.Substring(5, 5) + this.DigitoLinhaDigitavel(sLinhaCampo2) + " ";
            sLinhaCampo3 = sLinhaCampo3.Substring(0, 5) + "." + sLinhaCampo3.Substring(5, 5) + this.DigitoLinhaDigitavel(sLinhaCampo3) + " ";

            return (sLinhaCampo1 + sLinhaCampo2 + sLinhaCampo3 + sLinhaCampo4 + sLinhaCampo5);
        }

        public static string MostraImagemCodigoBarras(string codigoBarras)
        {
            string imgCodigoBarras = "";
            string szCorBar;
            int iWidth;
            string[] szArray = new string[10];
            szArray[0] = "00110";
            szArray[1] = "10001";
            szArray[2] = "01001";
            szArray[3] = "11000";
            szArray[4] = "00101";
            szArray[5] = "10100";
            szArray[6] = "01100";
            szArray[7] = "00011";
            szArray[8] = "10010";
            szArray[9] = "01010";
            string szBinario = "";

            if (codigoBarras.Length % 2 != 0)
                codigoBarras = "0" + codigoBarras;

            char[] arrCodigoBarras = codigoBarras.ToCharArray();

            for (int i = 0; i < arrCodigoBarras.Length; i = i + 2)
            {
                string szAux1 = szArray[int.Parse(arrCodigoBarras[i].ToString())];
                string szAux2 = szArray[int.Parse(arrCodigoBarras[i + 1].ToString())];
                for (int j = 0; j < 5; j++)
                {
                    szBinario += szAux1.Substring(j, 1) + szAux2.Substring(j, 1);
                }
            }

            szBinario = "0000" + szBinario + "1000";

            char[] arrBinario = szBinario.ToCharArray();

            for (int i = 0; i < arrBinario.Length; i++)
            {
                byte iDig = (byte)arrBinario[i];

                if ((i % 2) == 0)
                    szCorBar = "p";
                else
                    szCorBar = "b";

                if (int.Parse(arrBinario[i].ToString()) == 0)
                    iWidth = 1;
                else
                    iWidth = 3;

                imgCodigoBarras += "<img src='./images/" + szCorBar + ".gif' width='" + iWidth + "' height='50'>";
            }

            return imgCodigoBarras;
        }
        public static string MostraImagemCodigoBarras(string codigoBarras, bool flag)
        {
            string imgCodigoBarras = "";
            string szCorBar;
            int iWidth;
            string[] szArray = new string[10];
            szArray[0] = "00110";
            szArray[1] = "10001";
            szArray[2] = "01001";
            szArray[3] = "11000";
            szArray[4] = "00101";
            szArray[5] = "10100";
            szArray[6] = "01100";
            szArray[7] = "00011";
            szArray[8] = "10010";
            szArray[9] = "01010";
            string szBinario = "";

            if (codigoBarras.Length % 2 != 0)
                codigoBarras = "0" + codigoBarras;

            char[] arrCodigoBarras = codigoBarras.ToCharArray();

            for (int i = 0; i < arrCodigoBarras.Length; i = i + 2)
            {
                string szAux1 = szArray[int.Parse(arrCodigoBarras[i].ToString())];
                string szAux2 = szArray[int.Parse(arrCodigoBarras[i + 1].ToString())];
                for (int j = 0; j < 5; j++)
                {
                    szBinario += szAux1.Substring(j, 1) + szAux2.Substring(j, 1);
                }
            }

            szBinario = "0000" + szBinario + "1000";

            char[] arrBinario = szBinario.ToCharArray();

            for (int i = 0; i < arrBinario.Length; i++)
            {
                byte iDig = (byte)arrBinario[i];

                if ((i % 2) == 0)
                    szCorBar = "p";
                else
                    szCorBar = "b";

                if (int.Parse(arrBinario[i].ToString()) == 0)
                    iWidth = 1;
                else
                    iWidth = 3;

                imgCodigoBarras += szCorBar + " " + iWidth + " ";
            }
            imgCodigoBarras = imgCodigoBarras.Remove(imgCodigoBarras.Length - 1, 1);
            return imgCodigoBarras;
        }

        public static void SaveBarcode(string CodigoBarra,string Path)
        {
            foreach (string str in Directory.GetFiles(Path))
            {
                try
                {
                    File.Delete(str);
                }
                catch { }
            }

            if (!File.Exists(Path + CodigoBarra + ".jpg"))
            {
                //Create a Barcode Professional object
                Neodynamic.WebControls.BarcodeProfessional.BarcodeProfessional bcp = new Neodynamic.WebControls.BarcodeProfessional.BarcodeProfessional();
                //Set the barcode symbology to Code 128
                bcp.Symbology = Neodynamic.WebControls.BarcodeProfessional.Symbology.Code128;
                //Set the value to encode
                bcp.Code = CodigoBarra;
                //Barcode dimensions settings
                bcp.BarHeight = 1.0f;
                bcp.BarWidth = 0.01f;
                bcp.DisplayCode = false;
                //Resolution
                float dpi = 300.0f;
                //Target size in inches
                System.Drawing.SizeF targetArea = new System.Drawing.SizeF(4.0551f, 0.5118f);
                //Get the barcode image fitting the target area
                System.Drawing.Image imgBarcode = bcp.GetBarcodeImage(dpi, targetArea);
                //Save it on disk in PNG format
                imgBarcode.Save(Path + CodigoBarra + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                imgBarcode.Dispose();
            }
        }
    }
}
