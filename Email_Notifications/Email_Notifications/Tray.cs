using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using netoaster;
using MimeKit;
using System.ComponentModel;

namespace Email_Notifications
{
    class Tray
    {
        Client clientDB = new Client();
        System.Windows.Threading.DispatcherTimer timerField;
        List<Imap> myCurrentImap = new List<Imap>();
        TaskbarIcon tbiField = new TaskbarIcon();
        String[] stringsContextMenuField = { "Настройки", "Не беспокоить", "Выход" };


        Dictionary<string, List<MimeMessage>> MessagesToShow = new Dictionary<string, List<MimeMessage>>();

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
                if (i == 1) item.Click += stopTimer;
                if (i == 2) item.Click += Exit;
                tbiField.ContextMenu.Items.Add(item);
            }

            timerRun();
        }

        private void timerRun()
        {
            timerField = new System.Windows.Threading.DispatcherTimer();
            timerField.Tick += new EventHandler(timerTick);
            SetServerCheckInterval();

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
                timerField.Stop();
            }

            timerField.Start();
        }

        //Эвент таймера, тут надо показывать зулины сообщения
        private void timerTick(object sender, EventArgs e)
        {
            try
            {
                SetServerCheckInterval();
                //MessageBox.Show(MessagesToShow.Count.ToString());
                foreach (string tempStr in MessagesToShow.Keys)
                {
                    List<String> address = new List<string>(),
                        name = new List<string>(),
                        theme = new List<string>(),
                        text = new List<string>();
                   // MessageBox.Show(Cryptography.Decrypt(tempStr) + "  " + MessagesToShow[tempStr].Count.ToString());
                    int count = 0;
                    foreach (MimeMessage m in MessagesToShow[tempStr])
                    {
                        string from = m.From[0].ToString(),
                        addresFrom = from.Split(' ').Last();
                        address.Add(addresFrom.Substring(1, addresFrom.Length - 1));
                        name.Add(m.From[0].Name);
                        if (m.Subject != null)
                            theme.Add(m.Subject);
                        else
                            theme.Add("<Без темы>");
                        text.Add(m.TextBody);
                        count++;
                        //MessageBox.Show(Cryptography.Decrypt(tempStr) + " " + m.Subject);
                    }
                    MessagesToShow[tempStr].Clear();
                    //MessagesToShow[tempStr].Clear();
                    //MessageBox.Show(Cryptography.Decrypt(tempStr) + " " + count);
                    if (count > 0)
                    {
                        ShowToaster(text, address, name, theme, Cryptography.Decrypt(tempStr));
                        address.Clear();
                        name.Clear();
                        theme.Clear();
                        text.Clear();
                    }
                    //MessagesToShow.Remove(tempStr);
                }               

                var actAccs = clientDB.ActiveAccount();
                var actUsers = clientDB.DisplayActiveUsers();
                var tempCount = clientDB.GetAccountMessages();

                int UsersCount = actUsers.Count;

                Task[] TaskArr = new Task[UsersCount];
                MessagesToShow = new Dictionary<string, List<MimeMessage>>();
                //MessageBox.Show("Количество задач: " +TaskArr.Length.ToString());
                for (int i = 0; i < UsersCount; i++)
                {
                    int j = i;
                    TaskArr[i] = new Task(() =>
                        {
                            try
                            {
                                Imap NewImap = new Imap(Cryptography.Decrypt(actUsers[j]), Cryptography.Decrypt(actAccs[actUsers[j]]));
                                NewImap.Connection();
                                //MessageBox.Show(NewImap.currentEmail + " Стратортавала задача");
                                if (tempCount[actUsers[j]] != -1)
                                {
                                    int NewMessagesCount = NewImap.Connections.Inbox.Count;
                                    List<MimeMessage> NewList = new List<MimeMessage>();
                                    while (NewMessagesCount > tempCount[actUsers[j]])
                                    {
                                        NewList.Add(NewImap.DownloadMessage(tempCount[actUsers[j]]));
                                        tempCount[actUsers[j]]++;
                                        clientDB.UpdateAccountMessages(actUsers[j], tempCount[actUsers[j]]);
                                    }
                                    if (!MessagesToShow.ContainsKey(actUsers[j]))
                                    {
                                        MessagesToShow.Add(actUsers[j], NewList);
                                    }
                                    else
                                    {
                                        MessagesToShow[actUsers[j]].AddRange(NewList);
                                    }                                    
                                }
                                else
                                {
                                    clientDB.UpdateAccountMessages(Cryptography.Encrypt(NewImap.currentEmail), NewImap.Connections.Inbox.Count);
                                }
                                NewImap.Disconnection();
                            }
                            catch //(Exception ex)
                            {
                                //MessageBox.Show(ex.ToString());
                            }
                        });
                }
                foreach (Task task in TaskArr)
                {
                    task.Start();
                }
                //Task.WaitAll(TaskArr);
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Не удалось загрузить письмо. Уведомления остановлены");
                stopTimer((object)tbiField.ContextMenu.Items[1], new RoutedEventArgs());
            }
        }

        private void ShowToaster(List<string> _text, List<string> _addres, List<string> _name, List<string> _theme, string email)
        {
            //try
            //{
                int countOfElement = _addres.Count;
                if (countOfElement == 1)
                {
                    Message m = new Message(email, _addres[0], _theme[0], _text[0], _name[0]);
                    SuccessToaster.Toast(m, _addres[0], _name[0], _theme[0], GlobalSettings.NotificationLiveTimeInSeconds, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);

                }
                else if (countOfElement == 2)
                {
                    Message m1 = new Message(email, _addres[countOfElement - 1], _theme[countOfElement - 1], _text[countOfElement - 1], _name[countOfElement - 1]),
                       m2 = new Message(email, _addres[countOfElement - 2], _theme[countOfElement - 2], _text[countOfElement - 2], _name[countOfElement - 2]);
                    WarningToaster.Toast(m1, m2, _addres[countOfElement - 1], _name[countOfElement - 1], _theme[countOfElement - 1], _addres[countOfElement - 2], _name[countOfElement - 2], _theme[countOfElement - 2], GlobalSettings.NotificationLiveTimeInSeconds, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);

                }
                else if (countOfElement >= 3)
                {
                    Message m1 = new Message(email, _addres[countOfElement - 1], _theme[countOfElement - 1], _text[countOfElement - 1], _name[countOfElement - 1]),
                        m2 = new Message(email, _addres[countOfElement - 2], _theme[countOfElement - 2], _text[countOfElement - 2], _name[countOfElement - 2]),
                        m3 = new Message(email, _addres[countOfElement - 3], _theme[countOfElement - 3], _text[countOfElement - 3], _name[countOfElement - 3]);
                    ErrorToaster.Toast(m1, m2, m2, _addres[countOfElement - 1], _name[countOfElement - 1], _theme[countOfElement - 1], _addres[countOfElement - 2], _name[countOfElement - 2], _theme[countOfElement - 2], _addres[countOfElement - 3], _name[countOfElement - 3], _theme[countOfElement - 3], GlobalSettings.NotificationLiveTimeInSeconds, countOfElement, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        public void SetServerCheckInterval()
        {
            timerField.Interval = new TimeSpan(0, GlobalSettings.ServerCheckTimeInMinutes, 0);
            //timerField.Interval = new TimeSpan(0, 0, 10);
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
                if (timerField.IsEnabled)
                {
                    GlobalSettings.NotificationsEnabled = false;
                    timerField.Stop();
                    tbiField.ToolTipText = "Программа остановлена";
                    (sender as System.Windows.Controls.MenuItem).Header = "Включить оповещения";
                }
                else
                {
                    GlobalSettings.NotificationsEnabled = true;
                    tbiField.ToolTipText = "Программа работает";
                    (sender as System.Windows.Controls.MenuItem).Header = "Не беспокоить";
                    SetServerCheckInterval();
                    timerField.Start();
                }
        }

        private void PostChange(object sender, RoutedEventArgs e)
        {
            SetSettings mySetSettings = new SetSettings();
            mySetSettings.Show();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalSettings settin = GlobalSettings.GetInstance();
                GlobalSettings.SaveSettings(settin);
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить настройки. Проверьте доступность диска и запустите программу от имени администратора.");
            }
            Email_Notifications.App.Current.Shutdown();
        }
    }
}
