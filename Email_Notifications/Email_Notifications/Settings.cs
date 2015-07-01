using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Email_Notifications
{
    /// <summary>
    /// Используется для хранения настроек и прочих объектов, для которых важно сохранять значение 
    /// при закрытии программы.
    /// </summary>
    public class PopSettings
    {
        private static string _Server;
        public string Server
        {
            get { return _Server; }
            set { _Server = value; }
        }
        private static string _Adress;
        public string Adress
        {
            get { return _Adress; }
            set { _Adress = value; }
        }
        private static string _Password;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private static int _Port;
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        private static bool _SSL;
        public bool SSL
        {
            get { return _SSL; }
            set { _SSL = value; }
        }

        //значение прошлой проверки сервера на количество сообщений
        //если меньше - вывод оповещения
        public static int _MessageCount;
        public int MessageCount
        {
            get { return _MessageCount; }
            set { _MessageCount = value; }
        }


        private PopSettings()
        {
            _instance = new PopSettings(1);
        }
        private PopSettings(int i)
        {
        }
        private static PopSettings _instance = new PopSettings();
        public static PopSettings GetInstance()
        {
            return _instance;
        }


        public static void SaveSettings(PopSettings settin)
        {
            using(StreamWriter serStrm=new StreamWriter("settings.xml",false,Encoding.Default))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(PopSettings));
                xmlSer.Serialize(serStrm, (object)settin);
            }
        }

        public static void LoadSettings()
        {
            using (StreamReader serStrm = new StreamReader("settings.xml", Encoding.Default))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(PopSettings));
                _instance = (PopSettings)xmlSer.Deserialize(serStrm);
            }
        }
    }
}

