using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;

namespace Email_Notifications
{
    /// <summary>
    /// Используется для хранения настроек и прочих объектов, для которых важно сохранять значение 
    /// при закрытии программы.
    /// </summary>
    public class Settings
    {
        public string PopServer;
        public int PopPort;
        public string ImapServer;
        public int ImapPort;
        public bool SSL;
        //значение прошлой проверки сервера на количество сообщений
        //если меньше - вывод оповещения
        public int MessageCount;

        public Settings(string username)
        {
            PopServer = "pop." + username.Split('@')[1];
            ImapServer = PopServer.Replace("pop", "imap");
            PopPort = 995;
            ImapPort = 993;
            SSL = true;
        }

    }
}

