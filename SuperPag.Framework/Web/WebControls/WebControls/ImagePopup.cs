using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	/// Summary description for ImagePopup.
	/// </summary>
	[Designer(typeof(ImagePopupDesigner)),
	ToolboxData("<{0}:PopupImage runat=server></{0}:PopupImage>")]
	public class PopupImage : System.Web.UI.WebControls.Button
	{
		public PopupImage(){}

		#region Propriedades e Eventos

		const string SEC_CACHE_IMAGEPOPUP = "SEC_CACHE_IMAGEPOPUP";
		static SuperPag.Framework.Caching.CacheProxy cacheProxy;

		//Type
		public enum PopupImageType
		{
			Link = 1,
			Button = 2,
			Image = 3
		}

		[Bindable(true), Category("ImagePopup Event"), DefaultValue("")]
		public event PopupImageHandler.OnClickGetMessage ClickGetMessage;

		private ImageInfo currentImageInfo = null;
		private string _navigateUrl = "";
		private PopupImage.PopupImageType _popupType = PopupImage.PopupImageType.Button;

		[Bindable(true), Category("Popup Info"), DefaultValue("")]
		public string NavigateUrl
		{get{return _navigateUrl;}set{_navigateUrl = value;}}

		[Bindable(true), Category("Popup Type"), DefaultValue("")]
		public PopupImage.PopupImageType Type
		{get{return _popupType;}set{_popupType = value;}}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			
			ShowPopup( this.Page );
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick (e);

			OnClickGetMessage();
		}

		//TODO: Comentário
		public void OnClickGetMessage()
		{
			currentImageInfo = new ImageInfo();

			if(ClickGetMessage != null)
			{
				ClickGetMessage(this, currentImageInfo);
			}
		}

		public void ShowPopup( Page page ) 
		{
			if ( Ensure.IsNotNull( currentImageInfo ) &&  Ensure.ArrayIsNotNull( currentImageInfo.Image ) ) 
			{
				cacheProxy = SuperPag.Framework.Caching.CacheConfig.GetProxy();
				
				if ( ! cacheProxy.Exists(SEC_CACHE_IMAGEPOPUP ) )
					cacheProxy.Alloc( SEC_CACHE_IMAGEPOPUP );
				
				if ( cacheProxy.Exists( SEC_CACHE_IMAGEPOPUP, currentImageInfo.Guid ) )
					cacheProxy.RemoveItem( SEC_CACHE_IMAGEPOPUP, currentImageInfo.Guid ) ;
				
				//Expira em 10 minutos
				Caching.CachedValue.Wrap( cacheProxy, SEC_CACHE_IMAGEPOPUP, currentImageInfo.Guid, currentImageInfo, 10 ) ;

				if(page != null)
				{
					System.Drawing.Size size = SuperPag.Framework.Helper.ImageFunctions.ConvertArrayBytesToImage( currentImageInfo.Image ).Size;
					string paramWindow = "toolbar=0,status=0,menubar=0,width=" + size.Width.ToString() + ",height=" + size.Height.ToString() + ",resizable=0";
					
					string urlImagePopup = page.ResolveUrl( "~/" );
					
					page.RegisterClientScriptBlock("ImagePopUp", "<script>window.open('" + urlImagePopup + "imagepopup.aspx?id=" + currentImageInfo.Guid + "', 'imagepopup', '" + paramWindow + "');</script>");
				}
			}
		}
	}

	public sealed class PopupImageHandler 
	{
		public delegate void OnClickGetMessage(object sender, ImageInfo imageInfo);
	}

	[Serializable()]
	public class ImageInfo 
	{
		protected byte[] _Image = null;
		protected string _Guid = null;
		protected System.Drawing.SizeF _Size;

		/// <summary>
		///
		/// </summary>
		public byte[] Image
		{
			get 
			{
				return this._Image;
			}
			set 
			{
				this._Image = value;
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
		public System.Drawing.SizeF Size
		{
			get 
			{
				return this._Size;
			}
			set 
			{
				this._Size = value;
			}
		}
	}

	//TODO: Comentário
	public class ImagePopupDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			return designTimeHtml;
		}
	}

}
