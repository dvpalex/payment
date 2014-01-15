using System;
using System.Collections;
using System.Xml;
using System.Security.Principal;

namespace SuperPag.Framework.Web.WebController 
{
	//TODO: Comentário
	public class StepRecorder 
	{
		static XmlDocument _xmlLog;
		static XmlDocument _xmlDiskLog;

		const int STEPS_TO_FLUSH = 20;
		const int FLUSHS_TO_SAVE = 20;

		static int iCountSteps = 0;		
		static int iCountFlush = 0;
		static long iCountWrite = 0;
		XmlNode _ndUser;

		static StepRecorder() 
		{
			//Cria um novo xml de log
			_xmlLog = CreateXml(false);
			_xmlDiskLog = CreateXml(true);
		}

		string _userAlias;
		string UserAlias 
		{ 
			get { return _userAlias; } 
		} 

		private bool _log = true;

		public StepRecorder(IPrincipal principal) 
		{
			if( principal == null || principal.Identity == null)
			{
				_log = false;
				return;
			}

			string userAlias = principal.Identity.Name;
			string userId = "";
			string userInstitution = "";

			_userAlias = userAlias;

			lock(typeof(StepRecorder)) 
			{
				if(_xmlLog == null) throw new NullReferenceException("O XML de log é nulo");

				_ndUser = _xmlLog.SelectSingleNode("/Log/users/user[@alias='" + userAlias + "']");
				
				if(_ndUser == null) 
				{
					_ndUser = CreateUserEntry(userAlias, userId, userInstitution, _xmlLog);
				}
			}
		}

		private static XmlNode CreateUserEntry(
			string userAlias, 
			string userId, 
			string userInstitution,
			XmlDocument xml) {
			XmlNode ndUser = xml.CreateElement("user");

			XmlNode ndUsers = xml.SelectSingleNode("/Log/users");

			if(ndUsers == null) throw new NullReferenceException("Não foi possível encontrar 'Log/Users' ");

			ndUsers.AppendChild(ndUser);

			XmlAttribute atAlias = xml.CreateAttribute("alias");
			XmlAttribute atUserId = xml.CreateAttribute("userid");

			XmlAttribute atUserInstitution = xml.CreateAttribute("institution");

			atAlias.Value = userAlias;
			atUserId.Value = userId;
			atUserInstitution.Value = userInstitution;

			ndUser.Attributes.Append(atAlias);
			ndUser.Attributes.Append(atUserId);
			ndUser.Attributes.Append(atUserInstitution);

			return ndUser;
		}

		public void RecordError(string message) {
			lock(typeof(StepRecorder)) {
				if(! _log ) { return; }
				if(_xmlLog == null) throw new NullReferenceException("O XML de log é nulo");

				XmlNode ndError = _xmlLog.CreateElement("error");				
				
				XmlAttribute atTime = _xmlLog.CreateAttribute("time");
				atTime.Value = DateTime.Now.ToString("yyyyMMddHHmmss");

				if(message != null)
				{
					ndError.InnerText = message;
				} 
				else
				{
					ndError.InnerText = "erro desconhecido";
				}

				if(_ndUser.ChildNodes.Count > 0) {
					_ndUser.ChildNodes[_ndUser.ChildNodes.Count - 1].AppendChild(ndError);
				} 
				else {
					_ndUser.AppendChild(ndError);
				}
				

				FlushXml();
			}
		}

		public void RecordLogin() {
			lock(typeof(StepRecorder)) {
				if(! _log ) { return; }
				if(_xmlLog == null) throw new NullReferenceException("O XML de log é nulo");

				XmlNode ndError = _xmlLog.CreateElement("Login");				
				
				XmlAttribute atTime = _xmlLog.CreateAttribute("time");
				atTime.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
				_ndUser.AppendChild(ndError);

				FlushXml();
			}
		}

		public void RecordCommand( 
			BaseCommand commandToRecord) {
			lock(typeof(StepRecorder)) {
				if(! _log ) { return; }
				iCountSteps++;

				if(_xmlLog == null) throw new NullReferenceException("O XML de log é nulo");

				XmlNode ndCommand = _xmlLog.CreateElement("command");				
				
				XmlAttribute atTime = _xmlLog.CreateAttribute("time");
				atTime.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
				
				XmlAttribute atName = _xmlLog.CreateAttribute("name");				
				if(commandToRecord == null) {
					atName.Value = "desconhecido";
				} 
				else {
					atName.Value = commandToRecord.GetType().Name;
				}

				ndCommand.Attributes.Append(atName);
				ndCommand.Attributes.Append(atTime);

				if(_ndUser == null) throw new NullReferenceException("Erro ao encontrar arquivo de log do usuário!");

				_ndUser.AppendChild(ndCommand);

				if(iCountSteps >= STEPS_TO_FLUSH) {
					FlushXml();

					// verifica se é para renovar o XML
					if(iCountFlush >= FLUSHS_TO_SAVE) {
						SaveXmlToDisk();
						iCountFlush = 0;
					}
					iCountSteps = 0;
				}
			}
		}

		public static XmlDocument GetXml() {
			lock(typeof(StepRecorder)) {
				FlushXml();
				XmlDocument xml = new XmlDocument();

				if(_xmlDiskLog == null) throw new NullReferenceException("O XML de log é nulo");
				if(_xmlDiskLog.FirstChild == null) throw new NullReferenceException("O XML de log está em branco");

				xml.AppendChild(xml.ImportNode(_xmlDiskLog.FirstChild.Clone(), true));
				return xml;
			}
		}

		public XmlDocument GetUserXml() {
			lock(typeof(StepRecorder)) {
				if(! _log ) { return null; }

				FlushXml();
				
				if(_xmlDiskLog == null) throw new NullReferenceException("O XML de log é nulo");
				
				XmlNode ndUser = 
					_xmlDiskLog.SelectSingleNode("/Log/users/user[@alias='" + UserAlias + "']");
				
				if(ndUser == null) throw new NullReferenceException("Entrada do usuário não encontrada no XML de log");

				XmlDocument xml = new XmlDocument();
				xml.AppendChild(xml.ImportNode(ndUser, true));

				return xml;
			}
		}

		private static void FlushXml() {
			lock(typeof(StepRecorder)) {
				iCountFlush++;
			
				if(_xmlLog == null) throw new NullReferenceException("O XML de log é nulo");

				//Para cada usuario
				XmlNodeList lstUsers = _xmlLog.SelectNodes("/Log/users/user");

				if(lstUsers == null) throw new NullReferenceException("Entrada de usuários não encontrada no XML de log");
				
				foreach(XmlNode ndUser in lstUsers) {
					string userAlias = ndUser.Attributes["alias"].Value;
					string userId = ndUser.Attributes["userid"].Value;
					string userInstitution = ndUser.Attributes["institution"].Value;

					if(_xmlDiskLog == null) throw new NullReferenceException("O XML de log é nulo");

					XmlNode ndUserDisk = 
						_xmlDiskLog.SelectSingleNode("/Log/users/user[@alias='" + userAlias + "']");

					if(ndUserDisk == null) {
						ndUserDisk = CreateUserEntry(userAlias, userId, userInstitution, _xmlDiskLog);
						
						//Para cada comando
						for(int i = 0; i < ndUser.ChildNodes.Count; i++) {
							XmlNode ndCommand =
								_xmlDiskLog.ImportNode(ndUser.ChildNodes[i], true);
						
							ndUserDisk.AppendChild(ndCommand);
						}
					}
					else {
						//Para cada comando execeto o primeiro
						for(int i = 1; i < ndUser.ChildNodes.Count; i++) {
							XmlNode ndCommand =
								_xmlDiskLog.ImportNode(ndUser.ChildNodes[i], true);
						
							ndUserDisk.AppendChild(ndCommand);
						}
					}

					while(ndUser.ChildNodes.Count > 1) {
						ndUser.RemoveChild(ndUser.ChildNodes[0]);
					}
				}				
			}
		}

		private static void SaveXmlToDisk() {
			lock(typeof(StepRecorder)) {

				//TODO: acertar config
				string pathXml = ""; //Configuration.ConfigurationReader.GetStepLogFolder();

				if(! pathXml.EndsWith("\\") ) { pathXml += "\\"; }

				if(! System.IO.Directory.Exists(pathXml)) {
					throw new ArgumentException("O diretório de log não existe");
				}

				if(_xmlDiskLog == null) throw new NullReferenceException("O XML de log é nulo");

				//TODO: Criar configurador
				//Limpa os arquivos com mais de 5 dias
				System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(pathXml);
				System.IO.FileInfo[] files = dir.GetFiles();
				foreach(System.IO.FileInfo file in files) {
					if(file.CreationTime > DateTime.Now.AddDays(5)) {
						file.Delete();
					}
				}

				//TODO: Compactar				
				pathXml += "log_" + Guid.NewGuid().ToString() + ".xml"; 

				_xmlDiskLog.Save(pathXml);

				_xmlDiskLog = CreateXml(true);
				
				iCountWrite++;
			}
		}

		private static XmlDocument CreateXml(bool appendData) {
			lock(typeof(StepRecorder)) {
				XmlDocument xml = new XmlDocument();
				xml.AppendChild(xml.CreateElement("Log"));
				xml.FirstChild.AppendChild(xml.CreateElement("users"));

				if(appendData) {
					XmlAttribute atTime = xml.CreateAttribute("date");
					atTime.Value = DateTime.Now.ToString("yyyyMMdd");
					xml.FirstChild.Attributes.Append(atTime);
				}

				return xml;
			}
		}

		public void RecordEvent(
			BaseEvent eventToRecord) {			
			lock(typeof(StepRecorder)) {
				if(! _log ) { return; }

				XmlNode ndEvent = _xmlLog.CreateElement("Event");
				
				XmlAttribute atTime = _xmlLog.CreateAttribute("time");
				atTime.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
				
				XmlAttribute atName = _xmlLog.CreateAttribute("name");				
				atName.Value = eventToRecord.GetType().Name;

				ndEvent.Attributes.Append(atName);
				ndEvent.Attributes.Append(atTime);

				if(_ndUser == null) return ;
				if(_ndUser.ChildNodes == null) return ;
				if(ndEvent == null) return ;
				if(_ndUser.ChildNodes.Item(_ndUser.ChildNodes.Count - 1) == null) return;

				_ndUser.ChildNodes.Item(_ndUser.ChildNodes.Count - 1).AppendChild(ndEvent);
			}
		}


	}
}
