using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MimeKit;
using MailKit;
using MailKit.Net.Pop3;
using MailKit.Security;

namespace Email_Notifications
{
    /*
            Settings settin = Settings.GetInstance();
            settin.Adress = "почта";
            settin.Password = "пароль";
            settin.PopServer = "pop." + settin.Adress.Split('@')[1];
            settin.ImapServer = settin.PopServer.Replace("pop", "imap");
            settin.PopPort = 995;
            settin.ImapPort = 993;
            settin.SSL = true;
            Settings.SaveSettings(settin);
            int count = Pop.MessageCount();
            MimeMessage  m = Pop.DownloadMessage(count-1);
            //тема
            MessageBox.Show(m.Subject);
            //имя отправителя
            MessageBox.Show(m.From[0].Name);
     */
    class Pop
    {
        public static int MessageCount()
        {
            Settings settin = Settings.GetInstance();
            int mc;
            using (var client = new Pop3Client())
            {
                client.Connect(settin.PopServer, settin.PopPort, settin.SSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);
                client.Authenticate(settin.Adress, settin.Password);

                mc = client.Count();

                client.Disconnect(true);
            }
            return mc;
        }
        public static MimeMessage DownloadMessage(int i)
        {
            Settings settin = Settings.GetInstance();
            MimeMessage message;
            using (var client = new Pop3Client())
            {
                client.Connect(settin.PopServer, settin.PopPort, settin.SSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);
                client.Authenticate(settin.Adress, settin.Password);

                message = client.GetMessage(i);

                client.Disconnect(true);
            }
            return message;
        }
        public static List<MimeMessage> DownloadMessages(int startIndex, int count)
        {
            Settings settin = Settings.GetInstance();
            List<MimeMessage> messages = new List<MimeMessage>();
            using (var client = new Pop3Client())
            {
                client.Connect(settin.PopServer, settin.PopPort, settin.SSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);
                client.Authenticate(settin.Adress, settin.Password);

                messages.AddRange(client.GetMessages(startIndex, count));

                client.Disconnect(true);
            }
            return messages;
        }
    }
}
