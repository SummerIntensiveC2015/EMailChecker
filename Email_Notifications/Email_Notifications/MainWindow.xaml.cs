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
    public partial class Login : Window
    {
        private static bool firstLogin = true;
        Imap myCon;
        Client clientDB = new Client();
        bool Edit = false;
        string OldEmail;

        public Login()
        {
            try
            {
                clientDB.CreateDB();
                //clientDB.DeleteAllEmail();
                InitializeComponent();
                this.Hide();
                setLoginPosition();
                Auth();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public Login(string email, string password)
        {
            try
            {
                OldEmail = email;
                Edit = true;
                InitializeComponent();
                this.Hide();
                setLoginPosition();
                if (!firstLogin)
                {
                    textBoxLogin.Text = email;
                    textBoxPassword.Password = password;
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
                GlobalSettings.LoadSettings();
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            var allUsers = clientDB.DisplayAllUsers();
            if (allUsers.Count != 0)
            {
                var clAllAcs = clientDB.AllAccount();
                myCon = new Imap(Cryptography.Decrypt(allUsers[0]), Cryptography.Decrypt(clAllAcs[allUsers[0]]));
                try
                {
                    myCon.Connection();
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
            try
            {
                bool CorrectLogin = (textBoxLogin.Text.Length > 5 && textBoxLogin.Text.Length < 50 && textBoxLogin.Text.Contains('@')),
                    CorrectPassword = (textBoxPassword.Password.Length > 3 && textBoxPassword.Password.Length < 50);
                int count = -1;
                if (CorrectLogin && CorrectPassword)
                {
                    try
                    {
                        myCon = new Imap(textBoxLogin.Text, textBoxPassword.Password);
                        myCon.Connection();
                        count = myCon.Connections.Inbox.Count;

                        try
                        {
                            GlobalSettings settin = GlobalSettings.GetInstance();
                            GlobalSettings.SaveSettings(settin);
                        }
                        catch
                        {
                            MessageBox.Show("Не удалось сохранить настройки. Проверьте доступность диска и запустите программу от имени администратора.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        MessageBox.Show("Подключение не удалось. Проверьте правильность данных и наличие интернет соединения.");
                    }
                }
                else
                {
                    MessageBox.Show("Ваши данные некорректны");
                }
                if (Edit)
                {
                    clientDB.UpdateEmail(Cryptography.Encrypt(OldEmail), Cryptography.Encrypt(textBoxLogin.Text), Cryptography.Encrypt(textBoxPassword.Password));
                    this.Close();
                }
                else
                {
                    bool NeedIt = true;
                    if (count != -1)
                    {
                        if (clientDB.DisplayAllUsers().Count != 0)
                        {
                            if (clientDB.DisplayAllUsers().Contains(Cryptography.Encrypt(textBoxLogin.Text)))
                                NeedIt = false;
                        }
                        if (NeedIt)
                        {
                            clientDB.InsertUser(Cryptography.Encrypt(textBoxLogin.Text), Cryptography.Encrypt(textBoxPassword.Password), true);
                            if (firstLogin)
                            {
                                this.Hide();
                                Tray myTray = new Tray();
                                firstLogin = false;
                            }
                            else
                                this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Такой адрес уже есть");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}