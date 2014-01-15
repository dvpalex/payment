using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	/// Summary description for DownloadFile.
	/// </summary>
	[Designer(typeof(MsgDownloadFileDesigner)),
	ToolboxData("<{0}:MsgDownloadFile runat=server></{0}:MsgDownloadFile>")]
	public class MsgDownloadFile : System.Web.UI.WebControls.Button
	{
		public MsgDownloadFile(){}

		#region Propriedades e Eventos

		const string SEC_CACHE_DOWNLOADFILE = "SEC_CACHE_DOWNLOADFILE";
		static SuperPag.Framework.Caching.CacheProxy cacheProxy;
		private DownloadInfo currentDownloadInfo = null;

		[Bindable(true), Category("MsgDownloadFile Event"), DefaultValue("")]
		public event MsgDownloadFileHandler.OnClickGetMessage ClickGetMessage;

		#endregion


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			
			DownloadFile( this.Page );
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick (e);

			OnClickGetMessage();
		}

		//TODO: Comentário
		public void OnClickGetMessage()
		{
			currentDownloadInfo = new DownloadInfo();

			if(ClickGetMessage != null)
			{
				ClickGetMessage(this, currentDownloadInfo);
			}
		}

		public void DownloadFile( Page page ) 
		{
			if ( Ensure.IsNotNull( currentDownloadInfo ) &&  Ensure.ArrayIsNotNull( currentDownloadInfo.Content ) ) 
			{
				cacheProxy = SuperPag.Framework.Caching.CacheConfig.GetProxy();
				
				if ( ! cacheProxy.Exists(SEC_CACHE_DOWNLOADFILE ) )
					cacheProxy.Alloc( SEC_CACHE_DOWNLOADFILE );
				
				if ( cacheProxy.Exists( SEC_CACHE_DOWNLOADFILE, currentDownloadInfo.Guid ) )
					cacheProxy.RemoveItem( SEC_CACHE_DOWNLOADFILE, currentDownloadInfo.Guid ) ;
				
				//Expira em 10 minutos
				Caching.CachedValue.Wrap( cacheProxy, SEC_CACHE_DOWNLOADFILE, currentDownloadInfo.Guid, currentDownloadInfo, 10 ) ;

				if(page != null)
				{
					string urlImagePopup = page.ResolveUrl( "~/" );					
					page.RegisterClientScriptBlock("DownloadFile", "<script> window.open('" + urlImagePopup + "download.aspx?id=" + currentDownloadInfo.Guid + "', 'downloadpopup');</script>");
				}
			}
		}
	}

	public sealed class MsgDownloadFileHandler 
	{
		public delegate void OnClickGetMessage(object sender, DownloadInfo downloadInfo);
	}

	//TODO: Comentário
	public class MsgDownloadFileDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			return designTimeHtml;
		}
	}

	[Serializable()]
	public class DownloadInfo 
	{
		protected byte[] _Content = null;
		protected string _Guid = null;
		protected string _ContentType = null;
		protected string _FileName = null;

		/// <summary>
		///
		/// </summary>
		public byte[] Content
		{
			get 
			{
				return this._Content;
			}
			set 
			{
				this._Content = value;
			}
		}
		/// <summary>
		///
		/// </summary>
		public string Guid
		{
			get 
			{
				return this._Guid;
			}
			set 
			{
				this._Guid = value;
			}
		}
		/// <summary>
		///
		/// </summary>
		public string ContentType
		{
			get 
			{
				return this._ContentType;
			}
			set 
			{
				this._ContentType = value;
			}
		}

		/// <summary>
		///
		/// </summary>
		public string FileName
		{
			get 
			{
				return this._FileName;
			}
			set 
			{
				this._FileName = value;
			}
		}
	}
}
