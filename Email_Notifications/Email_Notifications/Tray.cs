using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using netoaster;
using MimeKit;

namespace Email_Notifications
{
    class Tray
    {
        Client clientDB = new Client();
        System.Windows.Threading.DispatcherTimer timer;
        List<Imap> myCurrentImap = new List<Imap>();
        TaskbarIcon tbiField = new TaskbarIcon();
        String[] stringsContextMenuField = { "Смена почты", "Время показа уведомлений", "Частота запросов", "Не беспокоить", "Выход" };
        List<String> address = new List<string>(),
                name = new List<string>(),
                theme = new List<string>();

        public Tray()
        {
            tbiField.Icon = Properties.Resources.Icon;
            tbiField.ToolTipText = "Программа работает";
            tbiField.ContextMenu = new System.Windows.Controls.ContextMenu();
            //List<System.Windows.Controls.MenuItem> myItems = new List<System.Windows.Controls.MenuItem>();
            for (int i = 0; i < stringsContextMenuField.Length; i++)
            {
                System.Windows.Controls.MenuItem item = new System.Windows.Controls.MenuItem();
                item.Header = stringsContextMenuField[i];
                if (i == 0) item.Click += PostChange;
                if (i == 1) item.Click += ShowISecondnterval;
                if (i == 2) item.Click += ShowInterval;
                if (i == 3) item.Click += stopTimer;
                if (i == 4) item.Click += Exit;
                tbiField.ContextMenu.Items.Add(item);
            }
            timerRun();

        }

        private void timerRun()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, GlobalSettings.ServerCheckTimeInMinutes, 0);

            var actAcc = clientDB.ActiveAccount();
            myCurrentImap = new List<Imap>();
            foreach (string s in actAcc.Keys)
            {
                myCurrentImap.Add(new Imap(Cryptography.Decrypt(s), Cryptography.Decrypt(actAcc[s])));
            }
            try
            {
                foreach (Imap im in myCurrentImap)
                {
                    im.Connection();
                    clientDB.UpdateAccountMessages(Cryptography.Encrypt(im.currentEmail), im.Connections.Inbox.Count);
                }
            }
            catch
            {
                timer.Stop();
            }

            timer.Start();
        }

        //Эвент таймера, тут надо показывать зулины сообщения
        private void timerTick(object sender, EventArgs e)
        {
            Dictionary<string, int> currentCount = new Dictionary<string, int>();

            try
            {
                var actAcc = clientDB.ActiveAccount();
                myCurrentImap = new List<Imap>();
                foreach (string s in actAcc.Keys)
                {
                    myCurrentImap.Add(new Imap(Cryptography.Decrypt(s), Cryptography.Decrypt(actAcc[s])));
                }
                foreach (Imap im in myCurrentImap)
                {
                    im.Disconnection();
                    im.Connection();
                    currentCount.Add(Cryptography.Encrypt(im.currentEmail), im.Connections.Inbox.Count);
                }
                var tempCount = clientDB.GetAccountMessages();
                foreach (string str in currentCount.Keys)
                {
                    Imap tempImap = new Imap();
                    foreach (Imap im in myCurrentImap)
                    {
                        if (im.currentEmail == Cryptography.Decrypt(str))
                        {
                            tempImap = im;
                            break;
                        }
                    }
                    if (tempCount[str] == -1)
                    {
                        clientDB.UpdateAccountMessages(str, tempImap.Connections.Inbox.Count);
                    }
                    else
                    {
                        while (currentCount[str] > tempCount[str])
                        {
                            MimeMessage m = tempImap.DownloadMessage(tempCount[str]);
                            tempCount[str]++;
                            clientDB.UpdateAccountMessages(str, tempCount[str]);

                            string from = m.From[0].ToString(),
                            addresFrom = from.Split(' ').Last();
                            address.Add(addresFrom.Substring(1, addresFrom.Length - 1));
                            name.Add(m.From[0].Name);
                            theme.Add(m.Subject);
                        }
                        if (name.Count > 0)
                        {
                            ShowToaster(address, name, theme, tempImap.currentEmail);
                            address.Clear();
                            name.Clear();
                            theme.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //MessageBox.Show("Не удалось загрузить письмо. Уведомления остановлены");
                timer.Stop();
            }
        }

        private void ShowToaster(List<string> _addres, List<string> _name, List<string> _theme, string email)
        {
            int countOfElement = _addres.Count;
            if (countOfElement == 1)
            {
                SuccessToaster.Toast(_addres[0], _name[0], _theme[0], GlobalSettings.NotificationLiveTimeInSeconds, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);
                SuccessToaster.email = email;
            }
            else if (countOfElement == 2)
            {
                WarningToaster.Toast(_addres[countOfElement - 1], _name[countOfElement - 1], _theme[countOfElement - 1], _addres[countOfElement - 2], _name[countOfElement - 2], _theme[countOfElement - 2], GlobalSettings.NotificationLiveTimeInSeconds, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);
                WarningToaster.email = email;
            }
            else if (countOfElement>=3)
            {
                ErrorToaster.Toast(_addres[countOfElement - 1], _name[countOfElement - 1], _theme[countOfElement - 1], _addres[countOfElement - 2], _name[countOfElement - 2], _theme[countOfElement - 2], _addres[countOfElement - 3], _name[countOfElement - 3], _theme[countOfElement - 3], GlobalSettings.NotificationLiveTimeInSeconds, countOfElement, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);
                ErrorToaster.email = email;
            }                      
        }

        private void OpenBrowser(object sender, RoutedEventArgs e)
        {
            try
            {
                //switch (Settings.GetInstance().Adress.Split('@')[1])
                //{
                //    case "yandex.ru":
                //        System.Diagnostics.Process.Start("https://mail.yandex.ru/");
                //        break;
                //    case "gmail.com":
                //        System.Diagnostics.Process.Start("https://mail.google.com");
                //        break;
                //    case "mail.ru":
                //        System.Diagnostics.Process.Start("https://e.mail.ru");
                //        break;
                //    default:
                //        System.Diagnostics.Process.Start("https://" + Settings.GetInstance().Adress.Split('@')[1]);
                //        break;
                //}
            }
            catch
            {
                MessageBox.Show("Не удается запустить браузер");
            }

        }

        private void stopTimer(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                GlobalSettings.NotificationsEnabled = false;
                timer.Stop();
                tbiField.ToolTipText = "Программа остановлена";
                (sender as System.Windows.Controls.MenuItem).Header = "Включить оповещения";
            }
            else
            {
                GlobalSettings.NotificationsEnabled = true;
                tbiField.ToolTipText = "Программа работает";
                (sender as System.Windows.Controls.MenuItem).Header = "Не беспокоить";
                timer.Interval = new TimeSpan(0, GlobalSettings.ServerCheckTimeInMinutes, 0);
                timer.Start();
            }
        }

        private void ShowISecondnterval(object sender, RoutedEventArgs e)
        {
            SecondsInterval mysInterval = new SecondsInterval();
            mysInterval.Show();
        }

        private void ShowInterval(object sender, RoutedEventArgs e)
        {
            Interval myInterval = new Interval();
            myInterval.Show();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            GlobalSettings settin = GlobalSettings.GetInstance();
            GlobalSettings.SaveSettings(settin);
            Email_Notifications.App.Current.Shutdown();
        }

        private void PostChange(object sender, RoutedEventArgs e)
        {
            MailAddressSelection fl = new MailAddressSelection();
            fl.Show();
        }
    }
}
