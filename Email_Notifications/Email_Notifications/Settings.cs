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


        public static void SaveSettings(Settings settin)
        {
            using (StreamWriter serStrm = new StreamWriter("settings.xml", false, Encoding.Default))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Settings));
                xmlSer.Serialize(serStrm, (object)settin);
            }
            /////////////////////////////////////////////////
            
            // Create an XmlDocument object.
            XmlDocument xmlDoc = new XmlDocument();

            // Load an XML file into the XmlDocument object.
            try
            {
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load("settings.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // Create a new CspParameters object to specify
            // a key container.
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "XML_ENC_RSA_KEY";

            // Create a new RSA key and save it in the container.  This key will encrypt
            // a symmetric key, which will then be encryped in the XML document.
            RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

            try
            {
                // Encrypt the "Password" element.
                Encrypt(xmlDoc, "Password", "EncryptedElement1", rsaKey, "rsaKey");
                Encrypt(xmlDoc, "Adress", "EncryptedElement2", rsaKey, "rsaKey");


                // Save the XML document.
                xmlDoc.Save("settings.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                // Clear the RSA key.
                rsaKey.Clear();
            }
        }

        public static void LoadSettings()
        {
            // Create an XmlDocument object.
            XmlDocument xmlDoc = new XmlDocument();

            // Load an XML file into the XmlDocument object.
            try
            {
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load("settings.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            // Create a new CspParameters object to specify
            // a key container.
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "XML_ENC_RSA_KEY";

            // Create a new RSA key and save it in the container.  This key will encrypt
            // a symmetric key, which will then be encryped in the XML document.
            RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

            try
            {
                Decrypt(xmlDoc, rsaKey, "rsaKey");
                Decrypt(xmlDoc, rsaKey, "rsaKey");
                xmlDoc.Save("settings.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // Clear the RSA key.
                rsaKey.Clear();
            }


            //////////////////////////////////
            using (StreamReader serStrm = new StreamReader("settings.xml", Encoding.Default))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Settings));
                _instance = (Settings)xmlSer.Deserialize(serStrm);
            }
            /////////////////////////////////////////////////

            // Create an XmlDocument object.
            xmlDoc = new XmlDocument();

            // Load an XML file into the XmlDocument object.
            try
            {
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load("settings.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            // Create a new CspParameters object to specify
            // a key container.
            cspParams = new CspParameters();
            cspParams.KeyContainerName = "XML_ENC_RSA_KEY";

            // Create a new RSA key and save it in the container.  This key will encrypt
            // a symmetric key, which will then be encryped in the XML document.
            rsaKey = new RSACryptoServiceProvider(cspParams);

            try
            {
                // Encrypt the "Password" element.
                Encrypt(xmlDoc, "Password", "EncryptedElement1", rsaKey, "rsaKey");
                Encrypt(xmlDoc, "Adress", "EncryptedElement2", rsaKey, "rsaKey");


                // Save the XML document.
                xmlDoc.Save("settings.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // Clear the RSA key.
                rsaKey.Clear();
            }
        }

        #region Cryptography methods
        //Note: copypaste from msdn
        private static void Encrypt(XmlDocument Doc, string ElementToEncrypt, string EncryptionElementID, RSA Alg, string KeyName)
        {
            // Check the arguments.
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (ElementToEncrypt == null)
                throw new ArgumentNullException("ElementToEncrypt");
            if (EncryptionElementID == null)
                throw new ArgumentNullException("EncryptionElementID");
            if (Alg == null)
                throw new ArgumentNullException("Alg");
            if (KeyName == null)
                throw new ArgumentNullException("KeyName");

            ////////////////////////////////////////////////
            // Find the specified element in the XmlDocument
            // object and create a new XmlElemnt object.
            ////////////////////////////////////////////////
            XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementToEncrypt)[0] as XmlElement;

            // Throw an XmlException if the element was not found.
            if (elementToEncrypt == null)
            {
                throw new XmlException("The specified element was not found");

            }
            RijndaelManaged sessionKey = null;

            try
            {
                //////////////////////////////////////////////////
                // Create a new instance of the EncryptedXml class
                // and use it to encrypt the XmlElement with the
                // a new random symmetric key.
                //////////////////////////////////////////////////

                // Create a 256 bit Rijndael key.
                sessionKey = new RijndaelManaged();
                sessionKey.KeySize = 256;

                EncryptedXml eXml = new EncryptedXml();

                byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, sessionKey, false);
                ////////////////////////////////////////////////
                // Construct an EncryptedData object and populate
                // it with the desired encryption information.
                ////////////////////////////////////////////////

                EncryptedData edElement = new EncryptedData();
                edElement.Type = EncryptedXml.XmlEncElementUrl;
                edElement.Id = EncryptionElementID;
                // Create an EncryptionMethod element so that the
                // receiver knows which algorithm to use for decryption.

                edElement.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncAES256Url);
                // Encrypt the session key and add it to an EncryptedKey element.
                EncryptedKey ek = new EncryptedKey();

                byte[] encryptedKey = EncryptedXml.EncryptKey(sessionKey.Key, Alg, false);

                ek.CipherData = new CipherData(encryptedKey);

                ek.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncRSA15Url);

                // Create a new DataReference element
                // for the KeyInfo element.  This optional
                // element specifies which EncryptedData
                // uses this key.  An XML document can have
                // multiple EncryptedData elements that use
                // different keys.
                DataReference dRef = new DataReference();

                // Specify the EncryptedData URI.
                dRef.Uri = "#" + EncryptionElementID;

                // Add the DataReference to the EncryptedKey.
                ek.AddReference(dRef);
                // Add the encrypted key to the
                // EncryptedData object.

                edElement.KeyInfo.AddClause(new KeyInfoEncryptedKey(ek));
                // Set the KeyInfo element to specify the
                // name of the RSA key.


                // Create a new KeyInfoName element.
                KeyInfoName kin = new KeyInfoName();

                // Specify a name for the key.
                kin.Value = KeyName;

                // Add the KeyInfoName element to the
                // EncryptedKey object.
                ek.KeyInfo.AddClause(kin);
                // Add the encrypted element data to the
                // EncryptedData object.
                edElement.CipherData.CipherValue = encryptedElement;
                ////////////////////////////////////////////////////
                // Replace the element from the original XmlDocument
                // object with the EncryptedData element.
                ////////////////////////////////////////////////////
                EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
            }
            catch (Exception ex)
            {
                // re-throw the exception.
                throw ex;
            }
            finally
            {
                if (sessionKey != null)
                {
                    sessionKey.Clear();
                }

            }

        }
        private static void Decrypt(XmlDocument Doc, RSA Alg, string KeyName)
        {
            // Check the arguments.  
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (Alg == null)
                throw new ArgumentNullException("Alg");
            if (KeyName == null)
                throw new ArgumentNullException("KeyName");

            // Create a new EncryptedXml object.
            EncryptedXml exml = new EncryptedXml(Doc);

            // Add a key-name mapping.
            // This method can only decrypt documents
            // that present the specified key name.
            exml.AddKeyNameMapping(KeyName, Alg);

            // Decrypt the element.
            exml.DecryptDocument();
        }
        #endregion
    }
}

