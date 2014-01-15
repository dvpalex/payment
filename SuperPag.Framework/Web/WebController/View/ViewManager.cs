using System;

namespace SuperPag.Framework.Web.WebController
{
	//TODO: Comentário
	public class ViewInfo
	{
		//TODO: Comentário
		string _name;
		string _url;
        string _title;

        //TODO: Comentário
        public string Title
        {
            get
            {
                return _title;
            }
        }

		//TODO: Comentário
		public string Name
		{
			get 
			{
				return _name;
			}
		}

		//TODO: Comentário
		public string Url
		{
			get 
			{
				return _url;
			}
		}

		//TODO: Comentário
		public ViewInfo(string title, string name, string url)
		{
            _title = title;
			_name = name;
			_url = url;
		}
	}

	//TODO: Comentário
	public class ViewManager
	{
		
	}
}
