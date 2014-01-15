using System;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace SuperPag.Framework.Web.WebControls
{

	///	TODO: Comentário
	[ToolboxData("<{0}:PopUpButton runat=server></{0}:PopUpButton>")]
	public class PopUpButton : SuperPag.Framework.Web.WebControls.EventButton, IEventControl
	{
		#region Properties

		private ParameterInfo _firstParameter = new ParameterInfo();
		private ParameterInfo _secondParameter = new ParameterInfo();
		private ParameterInfo _thirdParameter = new ParameterInfo();

		private string _firstValidator = "";
		private string _secondValidator = "";
		private string _thirdValidator = "";

		private ReturnValueInfo _firstReturnValue = new ReturnValueInfo();
		private ReturnValueInfo _secondReturnValue = new ReturnValueInfo();
		private ReturnValueInfo _thirdReturnValue = new ReturnValueInfo();

		private int _screenWidth;
		private string _actionName;
	
		// FirstParameter
		[Bindable(true), 
		Category("PopUp Parameter Info"), 
		DefaultValue(""),
		PersistenceMode(PersistenceMode.InnerProperty),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ParameterInfo FirstParameter
		{get {return _firstParameter;}set{this._firstParameter = value;}}

		// SecondParameter
		[Bindable(true), 
		Category("PopUp Parameter Info"), 
		DefaultValue(""),
		PersistenceMode(PersistenceMode.InnerProperty),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ParameterInfo SecondParameter
		{get {return _secondParameter;}set{this._secondParameter = value;}}
		
		// ThirdParameter
		[Bindable(true), Category("PopUp Parameter Info"), 
		DefaultValue(""),PersistenceMode(PersistenceMode.InnerProperty),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ParameterInfo ThirdParameter
		{get {return _thirdParameter;}set{this._thirdParameter = value;}}

		// FirstReturnValue
		[Bindable(true), Category("PopUp Return Info"), 
		DefaultValue(""),PersistenceMode(PersistenceMode.InnerProperty),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ReturnValueInfo FirstReturnValue
		{get{return _firstReturnValue;}set{_firstReturnValue = value;}}

		// SecondReturnValue
		[Bindable(true), Category("PopUp Return Info"), 
		DefaultValue(""),PersistenceMode(PersistenceMode.InnerProperty),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ReturnValueInfo SecondReturnValue
		{get{return _secondReturnValue;}set{_secondReturnValue = value;}}
		
		// ThirdReturnValue
		[Bindable(true), Category("PopUp Return Info"), 
		DefaultValue(""),PersistenceMode(PersistenceMode.InnerProperty),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ReturnValueInfo ThirdReturnValue
		{get{return _thirdReturnValue;}set{_thirdReturnValue = value;}}


		//Configuracao Pop Up
		[Bindable(true), Category("PopUp Info"), DefaultValue("")]
		public int ScreenWidth
		{get{return _screenWidth;}set{_screenWidth = value;}}

		[Bindable(true), Category("PopUp Info"), DefaultValue(""), ]
		public string ActionName
		{get{return _actionName;}set{_actionName = value;}}

		[Bindable(true), Category("Validator"), DefaultValue(""), ]
		public string FirstValidator
		{get{return _firstValidator;}set{_firstValidator = value;}}

		[Bindable(true), Category("Validator"), DefaultValue(""), ]
		public string SecondValidator
		{get{return _secondValidator;}set{_secondValidator = value;}}
		
		[Bindable(true), Category("Validator"), DefaultValue(""), ]
		public string ThirdValidator
		{get{return _thirdValidator;}set{_thirdValidator = value;}}
				
		#endregion Properties

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			
			if(Page is MessagePage)
			{
				((MessagePage)Page).RegisterPopUpScript();
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

			string guid = Guid.NewGuid().ToString().Replace("-", "_");

			this.Attributes.Add(
				"onclick", 
				"CallPopUp('" + MakeActionUrl() + "'," + 
				MakeParameters() + " + '&popup=1'" + ",'" + Page.ResolveUrl("~/") + "','" + 
				guid + "','" + _firstValidator + "','" + 
				_secondValidator + "','" + 
				_secondValidator + "');return false;");			


			if(! Page.IsClientScriptBlockRegistered(guid) )
			{
				StringBuilder _sb = 
					new StringBuilder("<script language=\"javascript\">");

				_sb.Append("function SetDataGridValue_");
				_sb.Append(guid);
				_sb.Append("(sFirst, sSecond, sThird){");

				_sb.Append(GetReturnValue(_firstReturnValue, "sFirst"));
				_sb.Append(GetReturnValue(_secondReturnValue, "sSecond"));
				_sb.Append(GetReturnValue(_thirdReturnValue, "sThird"));

				_sb.Append("}</script>");

				Page.RegisterClientScriptBlock(guid, _sb.ToString());

				string postProcessingScript =
					"<script language=\"javascript\">function DataGridValueSelected_" +
					 guid +
					 "(){}</script>";
				string postProcessingScriptBody = ""; 

				((MessagePage)Page).OnButtonScriptRegistered(
					guid, 
					postProcessingScript, 
					postProcessingScriptBody);
			}

		}

		#region Metodos Privados

		private string GetReturnValue(ReturnValueInfo returnValue, string position)
		{
			if(returnValue != null)
			{
				switch(returnValue.ControlType)
				{
					case ReturnValueInfo.ReturnControlType.TextBox:
						return "SetTextBoxValue('" + returnValue.ControlName + "'," + position + ");";
					case  ReturnValueInfo.ReturnControlType.Label:
						return "SetLabelValue('" + returnValue.ControlName + "'," + position + ");";
					default:
						return "";
				}
			} 
			else 
			{
				return "";
			}
		}

		private string MakeParameters()
		{
			string parametersString = string.Empty;
			string tempParameters = string.Empty;
		
			tempParameters = GetParameterValue(_firstParameter);
			ConcatParameterString(ref parametersString, tempParameters);

			tempParameters = GetParameterValue(_secondParameter);
			ConcatParameterString(ref parametersString, tempParameters);

			tempParameters = GetParameterValue(_thirdParameter);
			ConcatParameterString(ref parametersString, tempParameters);

			if(parametersString == "") parametersString = "''";

			return parametersString;
		}

		private void ConcatParameterString(ref string string1, string string2)
		{
			if(string2 != "")
			{
				if(string1 == string.Empty)
				{
					string1 += string2;
				} 
				else
				{
					string1 += " + " + string2;
				}				
			}
		}

		private string GetParameterValue(ParameterInfo parameter)
		{
			if(parameter != null)
			{
				switch(parameter.ControlType)
				{
					case ParameterInfo.ParameterControlType.TextBox:
						return "'&" + parameter.ParameterName + "=' + GetTextBoxValue('" + parameter.ControlName + "')";
					case ParameterInfo.ParameterControlType.DropDownList:
						return "'&" + parameter.ParameterName + "=' + GetComboBoxValue('" + parameter.ControlName + "')";
					case ParameterInfo.ParameterControlType.ValorFixo:
						return "'&" + parameter.ParameterName + "=" + parameter.ControlName + "'";
					default:
						return "";
				}
			} 
			else 
			{
				return "";
			}
		}


		private string MakeActionUrl()
		{
			return Page.ResolveUrl("~/") + _actionName;
		}

		#endregion Metodos Privados		

	}

	#region ReturnValueInfo

	//TODO: Comentário
	[TypeConverter(typeof(ReturnValueInfoConverter))]
	public class ReturnValueInfo
	{
		public enum ReturnControlType
		{
			NotSet = 0,
			TextBox = 1,		
			Label = 2
		}

		private string _controlName;
		private ReturnControlType _controlType;

		[DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string ControlName
		{get {return _controlName;}set {_controlName = value;}}

		[DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public ReturnControlType ControlType 
		{get {return _controlType;}set {_controlType = value;}}

	}
	//TODO: Comentário
	internal class ReturnValueInfoConverter : ExpandableObjectConverter 
	{
		public override bool CanConvertFrom(
			ITypeDescriptorContext context, Type t) 
		{
			if (t == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertFrom(context, t);
		}


		public override object ConvertFrom(
			ITypeDescriptorContext context, 
			CultureInfo info,
			object value) 
		{
			if (value is string) 
			{
				try 
				{
					// parse no formato ControlName (ControlType)
					string s = (string) value;
					
					int space = s.IndexOf(' ');
					if(space != -1)
					{
						// parameter name
						string controlName = s.Substring(0, space);

						int paren = s.LastIndexOf('(');
						if (paren != -1 && s.LastIndexOf(')') 
							== s.Length - 1) 
						{
							//ControlType
							string controlType = s.Substring(paren + 1, s.Length - paren - 2);

							ReturnValueInfo r = new ReturnValueInfo();
							r.ControlName = controlName;
							r.ControlType = 
								(ReturnValueInfo.ReturnControlType)
								Enum.Parse(typeof(ReturnValueInfo.ReturnControlType), controlType);
							return r;
						}

					}
				}
				catch {}
				throw new ArgumentException("Can not convert");
        
			}
			return base.ConvertFrom(context, info, value);
		}
                                 
		public override object ConvertTo(
			ITypeDescriptorContext context, 
			CultureInfo culture, 
			object value,    
			Type destType) 
		{
			if (destType == typeof(string) && value is ReturnValueInfo) 
			{
				ReturnValueInfo r = (ReturnValueInfo)value;
				return r.ControlName + 
					" (" + r.ControlType.ToString() + ")";
			}
			return base.
				ConvertTo(context, culture, value, destType);
		}   
	}



	#endregion ReturnValueInfo

	#region ParameterInfo

	//TODO: Comentário
	[TypeConverter(typeof(ParameterInfoConverter))]
	public class ParameterInfo
	{
		public enum ParameterControlType
		{
			NotSet = 0,
			DropDownList = 1,
			TextBox = 2,		
			ValorFixo = 3
		}

		private string _controlName;
		private string _parameterName;
		private ParameterControlType _controlType;

		[DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string ParameterName 
		{get {return _parameterName;}set {_parameterName = value;}}

		[DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string ControlName
		{get {return _controlName;}set {_controlName = value;}}

		[DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public ParameterControlType ControlType 
		{get {return _controlType;}set {_controlType = value;}}

	}
	//TODO: Comentário
	internal class ParameterInfoConverter : ExpandableObjectConverter 
	{
		public override bool CanConvertFrom(
			ITypeDescriptorContext context, Type t) 
		{
			if (t == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertFrom(context, t);
		}


		public override object ConvertFrom(
			ITypeDescriptorContext context, 
			CultureInfo info,
			object value) 
		{
			if (value is string) 
			{
				try 
				{
					// parse no formato ParameterName=ControlName (ControlType)
					string s = (string) value;
					
					int eq = s.IndexOf('=');
					if(eq != -1)
					{
						// parameter name
						string parameterName = s.Substring(0, eq);

						int paren = s.LastIndexOf('(');
						if (paren != -1 && s.LastIndexOf(')') 
							== s.Length - 1) 
						{
							//control name
							string controlName = s.Substring(eq, paren - eq - 1);

							//ControlType
							string controlType = s.Substring(paren + 1, s.Length - paren - 2);

							ParameterInfo p = new ParameterInfo();
							p.ControlName = controlName;
							p.ParameterName = parameterName;
							p.ControlType = 
								(ParameterInfo.ParameterControlType)
								Enum.Parse(typeof(ParameterInfo.ParameterControlType), controlType);
							return p;
						}

					}
				}
				catch {}
				throw new ArgumentException("Can not convert");
        
			}
			return base.ConvertFrom(context, info, value);
		}
                                 
		public override object ConvertTo(
			ITypeDescriptorContext context, 
			CultureInfo culture, 
			object value,    
			Type destType) 
		{
			if (destType == typeof(string) && value is ParameterInfo) 
			{
				ParameterInfo p = (ParameterInfo)value;
				return p.ParameterName + "=" + p.ControlName + 
					" (" + p.ControlType.ToString() + ")";
			}
			return base.
				ConvertTo(context, culture, value, destType);
		}   
	}

	#endregion ParameterInfo

}
