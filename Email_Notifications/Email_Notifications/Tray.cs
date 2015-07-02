using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Email_Notifications
{
    class Tray
    {
        System.Windows.Threading.DispatcherTimer timer;
        int count;
        Imap myCurrentImap;
        TaskbarIcon tbiField = new TaskbarIcon();
        String[] stringsContextMenuField = {"Ваша почта", "Время показа уведомлений", "Частота запросов", "Не беспокоить", "Отключить"};
        Interval myInterval = new Interval();
        SecondsInterval mysInterval = new SecondsInterval();

        public Tray()
        {
            //tbi.Icon = Resources.Error;
            tbiField.ToolTipText = "Программа для оповещения о письмах";
            tbiField.ContextMenu = new System.Windows.Controls.ContextMenu();
            //List<System.Windows.Controls.MenuItem> myItems = new List<System.Windows.Controls.MenuItem>();
            for (int i = 0; i < stringsContextMenuField.Length; i++)
            {
                System.Windows.Controls.MenuItem item = new System.Windows.Controls.MenuItem();
                item.Header = stringsContextMenuField[i];
                if (i == 0) item.Click += OpenBrowser;
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
            myCurrentImap = new Imap();
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, Settings.GetInstance().ServerCheckTimeInMinutes, 0);
            //timer.Interval = new TimeSpan(0, 0, 20);
            try
            {
                myCurrentImap.Connection();
                count = myCurrentImap.Connections.Inbox.Count;
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
            
            int currentCount = 0;
            List<MimeKit.MimeMessage> messages = new List<MimeKit.MimeMessage>();
            try
            {
                myCurrentImap.Disconnection();
                myCurrentImap.Connection();
                currentCount = myCurrentImap.Connections.Inbox.Count;
                while (currentCount > count)
                {
                    
                    messages.Add(myCurrentImap.DownloadMessage(count));
                    count++;
                }
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к почтовому серверу. Уведомления остановлены");
                messages = null;
                timer.Stop();
            }
            foreach (MimeKit.MimeMessage m in messages)
            {
                //тема
                MessageBox.Show(m.Subject);
                //имя отправителя
                MessageBox.Show(m.From[0].Name);
                //адрес
                MessageBox.Show(m.From[0].ToString());
            }
            
            
        }

        private void OpenBrowser(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://" + Settings.GetInstance().Adress.Split('@')[1]);
            }
            catch
            {
                MessageBox.Show("Не удается запустить браузер");
            }
            
        }

        private void stopTimer(object sender, RoutedEventArgs e)
        {
            timer.Stop();

        }

        private void ShowISecondnterval(object sender, RoutedEventArgs e)
        {
            
            mysInterval.Show();
        }

        private void ShowInterval(object sender, RoutedEventArgs e)
        {
             myInterval.Show();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Email_Notifications.App.Current.Shutdown();
        }
    }
}
