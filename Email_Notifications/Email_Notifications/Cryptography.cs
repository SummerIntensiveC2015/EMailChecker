using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;
using System.Security.Cryptography;
using System.IO;

namespace Email_Notifications
{
    public static class Cryptography
    {
        private static byte[] Key = GetKey();
        private static byte[] IV = GetKey();
        private static string salt = "bneoqpnbv[p'm]w-ojq0438gmit[JG8--[0HN0[jtjz9jq0[jgg";


        public static string Encrypt(string ElementToEncrypt)
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
                if (i != encrypted.Length - 1)
                    tempStr.Append(encrypted[i].ToString() + " ");
                else
                    tempStr.Append(encrypted[i].ToString());
            }
            return tempStr.ToString();
        }


        public static string Decrypt(string ElementToDecrypt)
        {
            if (ElementToDecrypt == null || ElementToDecrypt.Length <= 0)
                throw new ArgumentNullException("ElementToDecrypt");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string decrypted = null;
            string[] cipherTextStr = ElementToDecrypt.Split(new char[] { ' ' });
            int len = cipherTextStr.Length;
            byte[] cipherText = new byte[len];
            for (int i = 0; i < len; i++)
            {
                cipherText[i] = byte.Parse(cipherTextStr[i]);
            }
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                byte[] newIV = new byte[16];
                for (int i = 0; i < 16; i++)
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
            return decrypted;
        }


        private static byte[] GetKey()
        {
            StringBuilder tempStr = new StringBuilder();
            for (int i = 0; i < 1; i++)
            {
                tempStr.Append(salt);
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
    }
}
