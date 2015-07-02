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
     * Settings settin = Settings.GetInstance();
            settin.Adress = "почта";
            settin.Password = "пароль";
            settin.PopServer = "pop." + settin.Adress.Split('@')[1];
            settin.ImapServer = settin.PopServer.Replace("pop", "imap");
            settin.PopPort = 995;
            settin.ImapPort = 993;
            settin.SSL = true;
            Settings.SaveSettings(settin);
            Imap myCon = new Imap();
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
        private Settings _CurrentSettings;

        public Imap()
        {
            _CurrentSettings = Settings.GetInstance();
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
                _Connection.Connect(_CurrentSettings.ImapServer, _CurrentSettings.ImapPort, _CurrentSettings.SSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);

            if (!_Connection.IsAuthenticated)
                _Connection.Authenticate(_CurrentSettings.Adress, _CurrentSettings.Password);
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
