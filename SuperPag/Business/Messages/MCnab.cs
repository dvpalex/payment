using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Summary description for Cnab
/// </summary>
public class MCnab
{
    public MCnab(string Descricao, int Inicio, int Fim, int Tamanho, int Layout, string Versao, string Valor, DateTime Ano, int TypeId, string Picture)
    {
        this.Descricao = Descricao;
        this.Inicio = Inicio;
        this.Fim = Fim;
        this.Tamanho = Tamanho;
        this.Layout = Layout;
        this.Versao = Versao;
        this.Valor = Valor;
        this.Ano = Ano;
        this.TypeId = TypeId;
        this.Picture = Picture;
    }

    private string _Descricao;
    private int _Inicio;
    private int _Fim;
    private int _Tamanho;
    private int _Layout;
    private string _Versao;
    private string _Valor;
    private DateTime _Ano;
    private int _TypeId;
    private string _Picture;

    public string Descricao
    {
        get { return _Descricao; }
        set { _Descricao = value; }
    }
    public int Inicio
    {
        get { return _Inicio; }
        set { _Inicio = value; }
    }
    public int Fim
    {
        get { return _Fim; }
        set { _Fim = value; }
    }
    public int Tamanho
    {
        get { return _Tamanho; }
        set { _Tamanho = value; }
    }
    public int Layout
    {
        get { return _Layout; }
        set { _Layout = value; }
    }
    public string Versao
    {
        get { return _Versao; }
        set { _Versao = value; }
    }
    public string Valor
    {
        get { return _Valor; }
        set { _Valor = value; }
    }
    public DateTime Ano
    {
        get { return _Ano; }
        set { _Ano = value; }
    }
    public int TypeId
    {
        get { return _TypeId; }
        set { _TypeId = value; }
    }
    public string Picture
    {
        get { return _Picture; }
        set { _Picture = value; }
    }

    /// <summary>
    /// Retorna a linha do cnab
    /// </summary>
    /// <param name="List">Lista da classe cnab.</param>
    /// <returns></returns>
    public static string GetLine(IList<MCnab> List)
    {
        StringBuilder ObjStringBuilder = new StringBuilder(DimencionaString(Convert.ToInt32(List[0].Layout)).ToString());

        foreach (MCnab ObjMCnab in List)
        {
            ObjStringBuilder.Insert((ObjMCnab.Inicio - 1), FormatReg(ObjMCnab.Tamanho, ObjMCnab.Valor, ObjMCnab.Picture));
        }

        string str = ObjStringBuilder.ToString().Substring(0, Convert.ToInt32(List[0].Layout));
        
        return str;
    }

    private static string FormatReg(int TamReg, string Registro, string Picture)
    {
        string str = null;

        for (int i = 0; i < (TamReg - Registro.Length); i++)
        {
            if (Picture == "X")
            {
                str += ' ';
            }
            else
            {
                str += "0";
            }
        }

        if (Picture == "X")
        {
            str = Registro + str;
        }
        else
        {
            str = str + Registro;
        }
        return str;
    }

    private static StringBuilder DimencionaString(int CNAB)
    {
        StringBuilder NovaLinha = new StringBuilder();

        for (int i = 0; i < CNAB; i++)
        {
            NovaLinha.Append(" ");
        }
        return NovaLinha;
    }
}
