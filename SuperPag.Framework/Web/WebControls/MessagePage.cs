using System;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Resources;
using SuperPag.Framework.Web.WebControls;
using System.IO;
using System.Text;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	/// Summary description for MessagePage.
	/// </summary>
	public class MessagePage : SuperPag.Framework.Web.WebController.BaseView
	{
		//TODO: Comentário
		public event MessageEventHandler.MessagePageDataBind AfterMessageBind;
		public event MessageEventHandler.MessagePageDataBind BeforeMessageBind;
		public event MessageEventHandler.MessagePageDataUnBind BeforeMessageUnBind;

		MsgTextBox navField = new MsgTextBox();

		//TODO: Comentário
		static string _navScript;
		static string _popUpScript;

		private bool _isEmpty = true;

		//TODO: Comentario
		protected bool IsEmpty
		{
			get
			{
				return _isEmpty;
			}
		}

		//TODO: Comentário
		static MessagePage()
		{
			System.Reflection.Assembly asm;
			asm = System.Reflection.Assembly.GetExecutingAssembly();

			//string scriptPath = SuperPag.Framework.Helper.ResourcesHelper.FindResourcePath(asm, "NavScript.js");

            //System.IO.Stream navScript = 
            //    asm.GetManifestResourceStream(scriptPath);

			string[] x = asm.GetManifestResourceNames();

			//System.IO.StreamReader reader = new System.IO.StreamReader(navScript);

            //System.Text.StringBuilder sbuilder = new System.Text.StringBuilder();
            //sbuilder.AppendLine("<script language='javascript'>");
            //sbuilder.AppendLine(reader.ReadToEnd());
            //sbuilder.AppendLine("</script>");

		//	_navScript = sbuilder.ToString();


			//scriptPath = SuperPag.Framework.Helper.ResourcesHelper.FindResourcePath(asm, "PopUpScript.js");

            //System.IO.Stream popScript = 
            //    asm.GetManifestResourceStream(scriptPath);

            //reader = new System.IO.StreamReader(popScript);

            //sbuilder = new System.Text.StringBuilder();
            //sbuilder.AppendLine( "<script language='javascript'>");
            //sbuilder.AppendLine(reader.ReadToEnd());
            //sbuilder.AppendLine("</script>");

            //_popUpScript = sbuilder.ToString();               
		}

		//TODO: Comentário
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			System.Reflection.Assembly asm;
			asm = System.Reflection.Assembly.GetExecutingAssembly();

            //string scriptPath = ResourcesHelper.FindResourcePath(asm, "NavScript.js");

            //System.IO.Stream navScript = 
            //    asm.GetManifestResourceStream(scriptPath);

            //System.IO.StreamReader reader = new System.IO.StreamReader(navScript);

            //System.Text.StringBuilder sbuilder = new System.Text.StringBuilder();
            //sbuilder.AppendLine( "<script language='javascript'>" );
            //sbuilder.AppendLine( reader.ReadToEnd() );
            //sbuilder.AppendLine( "</script>" );

            //_navScript = sbuilder.ToString();

            //this.Page.RegisterStartupScript("NavigationScript", _navScript);
            //this.Page.RegisterStartupScript("PostScript", "<script language=\"javascript\" src=\"" + base.ResolveUrl("~") + "JScript/PostScript.js\"></script>");
            //this.Page.RegisterOnSubmitStatement("OnSubmitEvent", "javascript:__OnSubmit();");

            //if (this.Form != null)//Verifico se existe um form na página
            //    this.Form.Attributes.Add("onclick", "javascript:__MarkClickedButton();");
		}

		protected override void Render(HtmlTextWriter writer) 
		{
			//Cria um stringWriter e renderiza o conteúdo da página nele
			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
			base.Render(htmlTextWriter);

			//Obtem uma string em uppercase do conteúdo da stringBuilder
			string pageContent = stringWriter.ToString();
			
			writer.Write(pageContent);
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

            if (this.Session == null)
                System.Web.Security.FormsAuthentication.RedirectToLoginPage("");

            //Adiciona o comentáario definido no mapeamento da view na label da MasterPage
            if ( base.Master != null && base.Master.FindControl("lblPageTitle") != null )
                ((Label)base.Master.FindControl("lblPageTitle")).Text = (string)this.Context.Items["PageTitle"];

			if(! Page.IsPostBack)
			{
			//	Translate();

				if( HttpContext.Current.Items["__ERROR"] != null && 
					HttpContext.Current.Items["__ERROR"] is Exception)
				{					
					foreach(Control ctr in Page.Controls)
					{
						if(ctr is HtmlForm)
						{
							Exception ex = (Exception)HttpContext.Current.Items["__ERROR"];

							string divErro = 
								" <div onclick='ShowError()' style='LEFT: 730px; POSITION: absolute; TOP: 90px;'>" +
								" <FONT face='Verdana' size='1'> * </font></div>" + 
								" <input type='hidden' id='__hddErro' name='__hddErro' value='" +
								HttpUtility.HtmlEncode(ex.Message) + System.Environment.NewLine +
								HttpUtility.HtmlEncode(ex.ToString()) + "'>";
							LiteralControl lcErro = new LiteralControl(divErro);

							this.RegisterStartupScript("Error",
								"<script> function ShowError() { alert(document.all('__hddErro').value); } </script>");

							ctr.Controls.Add(lcErro);
						}
					}	
				}
			}
		}

		private string  GetAspxFolder()
		{
			string executionPath = Request.CurrentExecutionFilePath;

			FolderSearch folderSearch = new FolderSearch(executionPath);

			string aspxFolder = "";
			bool start = false;
			string folder = "";
			while(folderSearch.GetNextFolder(out folder))
			{
				if(folder.ToLower() == "views") start = true;
				else if (start)
				{
					aspxFolder += "." + folder;
				}
			}

			return aspxFolder;
		}

		private void Translate()
		{
			string aspxName = this.GetType().BaseType.FullName;
			aspxName = aspxName.Substring(aspxName.LastIndexOf("."));

			string resxName = WebController.ModuleConfigurationHelper.GetModuleAcronym(this.GetType().BaseType.Assembly) + ".Resx" + GetAspxFolder() + aspxName;

			ResourceManager resx = new ResourceManager(	resxName, this.GetType().BaseType.Assembly);

			FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

			try
			{
				foreach (FieldInfo f in fields)
				{
					if(f.FieldType == typeof(Label) || f.FieldType == typeof(MsgLabel))
					{
						object c = f.GetValue(this);
						if(c != null && ((Label)c).ID != null) 
						{
							string text = resx.GetString(((Label)c).ID + ":Text");
							if(text != null)
							{
								((Label)c).Text = text;
							}
						}
					}
					else if(f.FieldType == typeof(EventButton))
					{
						object c = f.GetValue(this);
						if(c != null && ((EventButton)c).ID != null) 
						{
							string text = resx.GetString(((EventButton)c).ID + ":Text");
							if(text != null)
							{
								((EventButton)c).Text = text;
							}							
						}
					} 
					else if(f.FieldType == typeof(LinkButton))
					{
						object c = f.GetValue(this);
						if(c != null && ((LinkButton)c).ID != null) 
						{
							string text = resx.GetString(((LinkButton)c).ID + ":Text");
							if(text != null)
							{
								((LinkButton)c).Text = text;
							}
						}	
					}
					else if(f.FieldType == typeof(MessageColumn))
					{
						object c = (MessageColumn)f.GetValue(this);
						if(c != null && ((MessageColumn)c).id != null) 
						{						
							string headerText = resx.GetString(((MessageColumn)c).id + ":HeaderText");
							if(headerText != null)
							{
								((MessageColumn)c).HeaderText = headerText;
							}
						}
					}
					else if(f.FieldType == typeof(EventColumn))
					{
						object c = f.GetValue(this);
						if(c != null && ((EventColumn)c).id != null) 
						{
							string headerText = resx.GetString(((EventColumn)c).id + ":HeaderText");
							if(headerText != null){((EventColumn)c).HeaderText = headerText;}

							string toolTip = resx.GetString(((EventColumn)c).id + ":ToolTip");
							if(toolTip != null){((EventColumn)c).ToolTip = toolTip;}

							string label = resx.GetString(((EventColumn)c).id + ":Label");
							if(label != null){((EventColumn)c).Label = label;}
						}
					}
				}
			} 
			catch (MissingManifestResourceException)
			{
			}
		}


		//Bind das messages no page load
		protected override void OnLoad(EventArgs e)
		{
			navField.ID = "__NavField";
			navField.Hidden = true;

			foreach(Control ctr in Page.Controls)
			{
				if(ctr is HtmlForm)
				{
					ctr.Controls.Add(navField);
				}
			}	
			
			base.OnLoad (e);

			this.Page.RegisterClientScriptBlock("MaskObject", "<script src='" + base.ResolveUrl ( "~" ) + "JScript/MaskObject.js'></script>");
			//Valida valores com os formatos HH:MM, HH:MM:SS e MM/YYYY
			this.Page.RegisterClientScriptBlock("ValidDateTime", "<script src='" + base.ResolveUrl ( "~" ) + "JScript/validDateAndTime.js'></script>");

			//Scripts com funções em geral
			if ( !this.Page.IsClientScriptBlockRegistered("script_Helper") )
				this.Page.RegisterClientScriptBlock("script_Helper", "<script src='" + base.ResolveUrl ( "~" ) + "JScript/Helper.js'></script>");

			//****Registro os scripts de mascaras

			//Coloco o script com os tamanhos dos pixels de cada fonte
			if ( !this.Page.IsClientScriptBlockRegistered("script_mask_PixelKeys") )
				this.Page.RegisterClientScriptBlock("script_mask_PixelKeys", 
					"<script language=\"JavaScript\" src='" + base.ResolveUrl ( "~" ) + "JScript/mapPixelKeys.js'></script>");

			if ( !this.Page.IsClientScriptBlockRegistered("script_mask_datetime") )
				this.Page.RegisterClientScriptBlock("script_mask_datetime", 
					"<script language=\"JavaScript\" src='" + base.ResolveUrl ( "~" ) + "JScript/maskdatetime.js'></script>");

			if ( !this.Page.IsClientScriptBlockRegistered("script_mask_currency") )
				this.Page.RegisterClientScriptBlock("script_mask_currency", 
					"<script language=\"JavaScript\" src='" + base.ResolveUrl ( "~" ) + "JScript/maskcurrency.js'></script>");

			if ( !this.Page.IsClientScriptBlockRegistered("script_mask_geral") )
				this.Page.RegisterClientScriptBlock("script_mask_geral", 
					"<script language=\"JavaScript\" src='" + base.ResolveUrl ( "~" ) + "JScript/maskgeral.js'></script>");

			if ( !this.Page.IsClientScriptBlockRegistered("script_mask_numeric") )
				this.Page.RegisterClientScriptBlock("script_mask_numeric",
					"<script language=\"JavaScript\" src='" + base.ResolveUrl ( "~" ) + "JScript/masknumeric.js'></script>");
			//********

			if(! Page.IsPostBack)
			{
				MessageBind();
			}
		}

		//TODO: Comentário
		protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
		{	
			if(sourceControl is Button && ((Button)sourceControl).CausesValidation)
			{
				Page.Validate();
				if(Page.IsValid)
				{
					MessageUnBind();
				}
			}

			else if(sourceControl is LinkButton && ((LinkButton)sourceControl).CausesValidation)
			{
				Page.Validate();
				if(Page.IsValid)
				{
					//
					MessageUnBind();
				}
			}
			base.RaisePostBackEvent (sourceControl, eventArgument);
		}

		//TODO: Comentário
		public void RaiseEvent(Type eventType)
		{
			RaiseEvent(eventType, new Hashtable());			
		}

		//TODO: Comentário
		public void RaiseEvent(Type eventType, Hashtable parameters)
		{
			if(eventType.Assembly != null &&
				eventType.Assembly.GetName() != null &&
				eventType.Assembly.GetName().Name != null)
			{
				string assemblyName =
					eventType.Assembly.GetName().Name;
				System.Runtime.Remoting.ObjectHandle  instanceHandle =
					Activator.CreateInstance(assemblyName, eventType.FullName);

				if(instanceHandle == null)
				{
					throw new NullReferenceException(" Erro ao instanciar evento ");
				}
				object instance = instanceHandle.Unwrap();

				if(!(instance is Web.WebController.BaseEvent))
				{
					throw new InvalidCastException(" Invalid BaseEvent type : " + eventType.FullName);
				}

    			parameters["__SCROLL"] = navField.Text;

				((Web.WebController.BaseEvent)instance).Execute(parameters);

                //TRATAMENTO DE ERRO DE BANCO DE DADOS
				//TODO:  corrigir
				if(HttpContext.Current.Items["__DELETE_ERROR"] is SuperPag.Framework.Data.Components.Data.DeleteConstraintException )
				{
                    StringBuilder msg = new StringBuilder();
                    msg.AppendLine("Este registro não pode ser apagado por estar sendo utilizado no sistema.");
                    ShowAlert("__DELETE_ERROR", msg.ToString() );
                }
                //TODO:  corrigir
                if (HttpContext.Current.Items["__DELETE_ERROR"] is SuperPag.Framework.Data.Components.Data.DuplicatedKeyException)
                {
                    StringBuilder msg = new StringBuilder();
//                  msg.AppendLine("O Campo " + ((SuperPag.Framework.Data.Components.Data.DuplicatedKeyException)HttpContext.Current.Items["__DELETE_ERROR"]).EntityName +  " já está cadastrado!");
                    msg.AppendLine("O registro não pode ser criado por conflitar com outro já cadastrado");
                    ShowAlert("__DELETE_ERROR", msg.ToString());
                } 

			} 
			else
			{
				throw new Exception(" Assembly não encontrado ");
			}			
		}

		//subo o evento
		public void OnAfterMessageBind()
		{
			if(AfterMessageBind != null)
			{
				AfterMessageBind(this);
			}
		}

		//subo o evento
		public void OnBeforeMessageBind()
		{
			if(BeforeMessageBind != null)
			{
				BeforeMessageBind(this);
			}
		}

		//TODO: Comentário
		protected void MessageBind()
		{
			OnAfterMessageBind();
			
			string[] eventsToRemove = new string[0];
			if(Context.Items["__REMOVEEVENTS"] != null)
			{
				eventsToRemove = Context.Items["__REMOVEEVENTS"].ToString().Split('|');
			}

			MessageBinder binder = new MessageBinder();
			binder.Bind(this);

			OnBeforeMessageBind();
		}

		internal void RegisterPopUpScript()
		{
			if(! Page.IsClientScriptBlockRegistered("PopUpScript"))
			{
				Page.RegisterClientScriptBlock("PopUpScript", _popUpScript);
			}
		}

		public virtual void OnButtonScriptRegistered(
			string guid,
			string postProcessingScript,
			string postProcessingScriptBody) 
		{
			postProcessingScript = postProcessingScript.Insert(
				postProcessingScript.IndexOf("{") + 1, postProcessingScriptBody );

			Page.RegisterClientScriptBlock(guid + "_End", postProcessingScript);
		}

		//TODO: Comentário
		protected void MessageUnBind()
		{
			bool isvalid = true;

			if(BeforeMessageUnBind != null)
			{
				BeforeMessageUnBind( this, ref isvalid );
			}

			if ( isvalid ) 
			{
				MessageUnBinder unbinder = new MessageUnBinder();
				unbinder.UnBind(this);
			}
		}

		private class FolderSearch
		{
			string _sPath = "";
			int _iPosSearch = 0;

			public FolderSearch(string sPath)
			{_sPath = sPath;}

			public bool GetNextFolder(out string sFolder)
			{
				sFolder = "";

				int iStartPos;
				iStartPos = _sPath.IndexOf("/", _iPosSearch);
                			
				if(iStartPos < 0) return false;

				int iEndPos;
				iEndPos = _sPath.IndexOf("/", iStartPos + 1);

				if(iEndPos < 0) return false;

				_iPosSearch = iEndPos;
				sFolder = _sPath.Substring(iStartPos + 1, iEndPos - iStartPos - 1);

				return true;
			}
		}
	}
	
}
