using System;

namespace SuperPag.Framework.Web.WebController
{
	//TODO: Coment�rio
	public class ViewInfo
	{
		//TODO: Coment�rio
		string _name;
		string _url;
        string _title;

        //TODO: Coment�rio
        public string Title
        {
            get
            {
                return _title;
            }
        }

		//TODO: Coment�rio
		public string Name
		{
			get 
			{
				return _name;
			}
		}

		//TODO: Coment�rio
		public string Url
		{
			get 
			{
				return _url;
			}
		}

		//TODO: Coment�rio
		public ViewInfo(string title, string name, string url)
		{
            _title = title;
			_name = name;
			_url = url;
		}
	}

	//TODO: Coment�rio
	public class ViewManager
	{
		
	}
}
