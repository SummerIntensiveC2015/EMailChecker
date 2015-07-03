using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using netoaster;

namespace Email_Notifications
{
    class Tray
    {
        System.Windows.Threading.DispatcherTimer timer;
        int count;
        Imap myCurrentImap;
        TaskbarIcon tbiField = new TaskbarIcon();
        String[] stringsContextMenuField = {"Ваша почта","Указать другую", "Время показа уведомлений", "Частота запросов", "Не беспокоить", "Отключить"};
        Interval myInterval = new Interval();
        SecondsInterval mysInterval = new SecondsInterval();
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
                if (i == 0) item.Click += OpenBrowser;
                if (i == 2) item.Click += ShowISecondnterval;
                if (i == 3) item.Click += ShowInterval;
                if (i == 4) item.Click += stopTimer;
                if (i == 5) item.Click += Exit;
                tbiField.ContextMenu.Items.Add(item);
            }
            timerRun();
            
        }

        private void timerRun()
        {
            myCurrentImap = new Imap();
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            //timer.Interval = new TimeSpan(0, Settings.GetInstance().ServerCheckTimeInMinutes, 0);
            timer.Interval = new TimeSpan(0, 0, 20);
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
            
            try
            {
                myCurrentImap.Disconnection();
                myCurrentImap.Connection();
                currentCount = myCurrentImap.Connections.Inbox.Count;
                //MessageBox.Show(currentCount.ToString());
                while (currentCount > count)
                {

                    MimeKit.MimeMessage m = new MimeKit.MimeMessage(myCurrentImap.DownloadMessage(count));
                    count++;
                    
                    string from = m.From[0].ToString(),
                    addresFrom = from.Split(' ').Last(); 
                    address.Add(addresFrom.Substring(1,addresFrom.Length-1));
                    name.Add(m.From[0].Name);
                    theme.Add(m.Subject);
                     
                    
                }
                if (name.Count > 0)
                {
                    ShowToaster(address, name, theme);
                    address.Clear();
                    name.Clear();
                    theme.Clear();
                }
                    
            }
            catch
            {
                
                MessageBox.Show("Не удалось загрузить письмо. Уведомления остановлены");

                timer.Stop();
                
            }

            
            
            
        }

        private void ShowToaster(List<string> _addres, List<string> _name, List<string> _theme)
        {
            int countOfElement = _addres.Count;
            if (countOfElement == 1)
            {
                SuccessToaster.Toast(_addres[0], _name[0], _theme[0], Settings.GetInstance().NotificationLiveTimeInSeconds, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);
                SuccessToaster.email = Settings.GetInstance().Adress;
            }
            else if (countOfElement == 2)
            {
                WarningToaster.Toast(_addres[countOfElement], _name[countOfElement], _theme[countOfElement], _addres[countOfElement - 1], _name[countOfElement - 1], _theme[countOfElement - 1], Settings.GetInstance().NotificationLiveTimeInSeconds, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);
                WarningToaster.email = Settings.GetInstance().Adress;
            }
            else if (countOfElement==3)
            {
                ErrorToaster.Toast(_addres[countOfElement], _name[countOfElement], _theme[countOfElement], _addres[countOfElement - 1], _name[countOfElement - 1], _theme[countOfElement - 1], _addres[countOfElement - 2], _name[countOfElement - 2], _theme[countOfElement - 2], Settings.GetInstance().NotificationLiveTimeInSeconds, countOfElement, ToasterPosition.PrimaryScreenBottomRight, animation: ToasterAnimation.FadeIn, margin: 20.0);
                ErrorToaster.email = Settings.GetInstance().Adress;
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
            if (timer.IsEnabled)
            {
                timer.Stop();
                tbiField.ToolTipText = "Программа остановлена";
            }
            else
            {
                tbiField.ToolTipText = "Программа работает";
                timer.Interval = new TimeSpan(0, Settings.GetInstance().ServerCheckTimeInMinutes, 0);
                timer.Start();
            }
            

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
