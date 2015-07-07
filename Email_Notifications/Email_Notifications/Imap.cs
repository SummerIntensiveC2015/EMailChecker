using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;

namespace Email_Notifications
{
    /*
            Imap myCon = new Imap("почта", "пароль");
            myCon.Connection();
            int count = myCon.Connections.Inbox.Count;
            MimeMessage  m = myCon.DownloadMessage(count-1);
            //тема
            MessageBox.Show(m.Subject);
            //имя отправителя
            MessageBox.Show(m.From[0].Name);
     */
    class Imap
    {
        private Settings currentSettings;
        public string currentEmail;
        private string currentPassword;

        public Imap(string email, string password)
        {
            currentSettings = new Settings(email);
            currentEmail = email;
            currentPassword = password;
        }

        public Imap()
        {
            // TODO: Complete member initialization
        }

        private ImapClient _Connection = new ImapClient();
        public ImapClient Connections
        {
            get { return _Connection; }
            set { _Connection = value; }
        }
        public void Connection()
        {
            if (!_Connection.IsConnected)
                _Connection.Connect(currentSettings.ImapServer, currentSettings.ImapPort, currentSettings.SSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);
            if (!_Connection.IsAuthenticated)
                _Connection.Authenticate(currentEmail, currentPassword);
            if (!_Connection.Inbox.IsOpen)
                _Connection.Inbox.Open(FolderAccess.ReadOnly);
        }

        public List<MimeMessage> DownloadMessages(int startindex, int endindex)
        {
            Connection();
            List<MimeMessage> result = new List<MimeMessage>();
            for (int i = startindex; i <= endindex; i++)
            {
                result.Add(_Connection.Inbox.GetMessage(i));
            }
            return (result);
        }

        public MimeMessage DownloadMessage(int index)
        {
            Connection();
            MimeMessage Result = _Connection.Inbox.GetMessage(index);
            return (Result);
        }

        private bool isActivity()
        {
            return (_Connection.IsAuthenticated);
        }

        public void Disconnection()
        {
            if (isActivity())
            {
                _Connection.Disconnect(true);
            }
        }
    }
}
