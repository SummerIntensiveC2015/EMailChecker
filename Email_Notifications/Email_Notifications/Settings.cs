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
    public class Settings
    {
        private static string _PopServer;
        public string PopServer
        {
            get { return _PopServer; }
            set { _PopServer = value; }
        }
        private static int _PopPort;
        public int PopPort
        {
            get { return _PopPort; }
            set { _PopPort = value; }
        }

        private static string _ImapServer;
        public string ImapServer
        {
            get { return _ImapServer; }
            set { _ImapServer = value; }
        }
        private static int _ImapPort;
        public int ImapPort
        {
            get { return _ImapPort; }
            set { _ImapPort = value; }
        }

        private static bool _SSL;
        public bool SSL
        {
            get { return _SSL; }
            set { _SSL = value; }
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
        
        //значение прошлой проверки сервера на количество сообщений
        //если меньше - вывод оповещения
        public static int _MessageCount;
        public int MessageCount
        {
            get { return _MessageCount; }
            set { _MessageCount = value; }
        }

        //-1 до ручного закрытия уведомления
        private static int _NotificationLiveTimeInSeconds;
        public int NotificationLiveTimeInSeconds
        {
            get { return _NotificationLiveTimeInSeconds; }
            set { _NotificationLiveTimeInSeconds = value; }
        }
        private static int _ServerCheckTimeInMinutes;
        public int ServerCheckTimeInMinutes
        {
            get { return _ServerCheckTimeInMinutes; }
            set { _ServerCheckTimeInMinutes = value; }
        }
        private static bool _NotificationsEnabled;
        public bool NotificationsEnabled
        {
            get { return _NotificationsEnabled; }
            set { _NotificationsEnabled = value; }
        }



        private Settings()
        {
            _instance = new Settings(1);
        }
        private Settings(int i)
        {
        }
        private static Settings _instance = new Settings();
        public static Settings GetInstance()
        {
            return _instance;
        }


        public static void SaveSettings(Settings settin)
        {
            using(StreamWriter serStrm=new StreamWriter("settings.xml",false,Encoding.Default))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Settings));
                xmlSer.Serialize(serStrm, (object)settin);
            }
        }

        public static void LoadSettings()
        {
            using (StreamReader serStrm = new StreamReader("settings.xml", Encoding.Default))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Settings));
                _instance = (Settings)xmlSer.Deserialize(serStrm);
            }
        }
    }
}

