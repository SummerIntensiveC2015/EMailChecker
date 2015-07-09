using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;

namespace Email_Notifications
{
    public class GlobalSettings
    {
        //-1 -- до ручного закрытия уведомления, например
        public static int NotificationLiveTimeInSeconds = 15;
        public int _NotificationLiveTimeInSeconds
        {
            get { return NotificationLiveTimeInSeconds; }
            set { NotificationLiveTimeInSeconds = value; }
        }
        public static int ServerCheckTimeInMinutes = 1;
        public int _ServerCheckTimeInMinutes
        {
            get { return ServerCheckTimeInMinutes; }
            set { ServerCheckTimeInMinutes = value; }
        }
        public static bool NotificationsEnabled;
        public bool _NotificationsEnabled
        {
            get { return NotificationsEnabled; }
            set { NotificationsEnabled = value; }
        }

        public GlobalSettings()
        {
            _instance=new GlobalSettings(1);
        }
        private GlobalSettings(int i)
        {
        }
        private static GlobalSettings _instance = new GlobalSettings();
        public static GlobalSettings GetInstance()
        {
            return _instance;
        }

        public static void SaveSettings(GlobalSettings settin)
        {
            using (StreamWriter serStrm = new StreamWriter("settings.xml", false, Encoding.Default))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(GlobalSettings));
                xmlSer.Serialize(serStrm, (object)settin);
            }
        }
        public static void LoadSettings()
        {
            using (StreamReader serStrm = new StreamReader("settings.xml", Encoding.Default))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(GlobalSettings));
                _instance = (GlobalSettings)xmlSer.Deserialize(serStrm);
            }
        }
    }
}
