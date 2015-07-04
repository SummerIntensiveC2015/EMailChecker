using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Management;

namespace Email_Notifications
{
    /// <summary>
    /// Используется для хранения настроек и прочих объектов, для которых важно сохранять значение 
    /// при закрытии программы.
    /// </summary>
    public class Settings
    {
        public static byte[] Key = GetKey();
        public static byte[] IV = GetKey();

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
        public static string _Adress;
        public string Adress
        {
            get { return Encrypt(_Adress); }
            set { _Adress = Decrypt(value); }
        }
        public static string _Password;
        public string Password
        {
            get { return Encrypt(_Password); }
            set { _Password = Decrypt(value); }
        }

        //значение прошлой проверки сервера на количество сообщений
        //если меньше - вывод оповещения
        public static int _MessageCount;
        public int MessageCount
        {
            get { return _MessageCount; }
            set { _MessageCount = value; }
        }

        //-1 -- до ручного закрытия уведомления
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

        public static void setSettings(String username, String password)
        {
            _Adress = username;
            _Password = password;
            _instance.PopServer = "pop." + _Adress.Split('@')[1];
            _instance.ImapServer = _instance.PopServer.Replace("pop", "imap");
            _instance.PopPort = 995;
            _instance.ImapPort = 993;
            _instance.ServerCheckTimeInMinutes = 1;
            _instance.NotificationLiveTimeInSeconds = 15;
            _instance.SSL = true;
        }
        public static void SaveSettings(Settings settin)
        {
            using (StreamWriter serStrm = new StreamWriter("settings.xml", false, Encoding.Default))
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

        #region Cryptography methods
        private static string Encrypt(string ElementToEncrypt)
        {
            if (ElementToEncrypt == null || ElementToEncrypt.Length <= 0)
                throw new ArgumentNullException("ElementToEncrypt");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                byte[] newIV = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    newIV[i] = IV[i];
                }
                rijAlg.IV = newIV;
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(ElementToEncrypt);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            StringBuilder tempStr = new StringBuilder();
            for (int i = 0; i < encrypted.Length; i++)
            {
                tempStr.Append(encrypted[i].ToString()+" ");
            }
            return tempStr.ToString();
        }
        private static string Decrypt(string ElementToDecrypt)
        {
            if (ElementToDecrypt == null || ElementToDecrypt.Length <= 0)
                throw new ArgumentNullException("ElementToDecrypt");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string decrypted = null;
            string[] cipherTextStr = ElementToDecrypt.Split(new char[]{' '});
            byte[] cipherText=new byte[cipherTextStr.Length];
            int i = 0;
            foreach(string tempStr in cipherTextStr)
            {
                cipherText [i]= byte.Parse(tempStr);
                i++;
            }
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                byte[] newIV = new byte[16];
                for (i = 0; i < 16; i++)
                {
                    newIV[i] = IV[i];
                }
                rijAlg.IV = newIV;
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decrypted = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return ElementToDecrypt;
        }
        public static byte[] GetKey()
        {
            StringBuilder tempStr = new StringBuilder();
            for (int i = 0; i < 1; i++)
            {
                tempStr.Append("bneoqpnbv[p'm]w-ojq0438gmit[JG8--[0HN0[jtjz9jq0[jgg");
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Product, SerialNumber FROM Win32_BaseBoard");
            ManagementObjectCollection information = searcher.Get();
            foreach (ManagementObject obj in information)
            {
                foreach (PropertyData data in obj.Properties)
                    tempStr.Append((string)data.Value);
            }
            ManagementClass mc = new ManagementClass("Win32_Processor");
            information = mc.GetInstances();
            foreach (ManagementObject mo in information)
            {
                tempStr.Append(mo.Properties["ProcessorId"].Value.ToString());
            }
            //MessageBox.Show(tempStr.ToString());
            SHA256Managed sha256m = new SHA256Managed();
            byte[] hashData = sha256m.ComputeHash(Encoding.Default.GetBytes(tempStr.ToString()));
            //MessageBox.Show(hashData.Length.ToString());
            //tempStr = new StringBuilder();
            //for (int i = 0; i < hashData.Length; i++)
            //{
            //    tempStr.Append(hashData[i].ToString("x2"));
            //}
            //MessageBox.Show(tempStr.ToString());
            return hashData;
        }
        #endregion
    }
}

