using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;

namespace Email_Notifications
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class FirstLogin : Window
    {
        private static bool firstLogin = true;
        Imap myCon;
        
        public FirstLogin()
        {
            try
            {
                InitializeComponent();
                this.Hide();
                setLoginPosition();
                Auth();
                if (!firstLogin)
                {
                    textBoxLogin.Text = Settings._Adress;
                    textBoxPassword.Password = Settings._Password;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Auth()
        {
            try
            {
                Settings.LoadSettings();
            }
            catch
            {
            }
            int tmpcount = -1;
            if (Settings._Adress != null)
            {
                myCon = new Imap();
                try
                {
                    myCon.Connection();
                    tmpcount = myCon.Connections.Inbox.Count;
                    if (firstLogin)
                    {
                        Tray myTray = new Tray();
                        firstLogin = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    this.Show();
                }

            }
            else
            {
                this.Show();
            }
        }

        private void setLoginPosition()
        {
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = (screenHeight - this.Height);
            this.Left = (screenWidth - (int)(this.Width / 0.93)); 
        }
        private void hideForm()
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            
            bool CorrectLogin = (textBoxLogin.Text.Length>5 && textBoxLogin.Text.Length<50 && textBoxLogin.Text.Contains('@')),
                CorrectPassword = (textBoxPassword.Password.Length > 3 && textBoxPassword.Password.Length < 50);
            int count = -1;
            if (CorrectLogin && CorrectPassword)
            {
                try
                {
                    Settings.setSettings(textBoxLogin.Text, textBoxPassword.Password);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                myCon = new Imap();
                try
                {
                    myCon.Connection();
                    count = myCon.Connections.Inbox.Count;

                    try
                    {
                        Settings settin = Settings.GetInstance();
                        Settings.SaveSettings(settin);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось сохранить настройки. Проверьте доступность диска и запустите программу от имени администратора.");
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Подключение не удалось. Проверьте правильность данных и наличие интернет соединения.");
                    //MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Ваши данные некорректны");
            }
            if (count != -1)
            {
                Hide();
                if (firstLogin)
                {
                    Tray myTray = new Tray();
                    firstLogin = false;
                }
            }
        }
    }
}
