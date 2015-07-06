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
using System.Windows.Shapes;

namespace Email_Notifications
{
    /// <summary>
    /// Логика взаимодействия для MailAddressSelection.xaml
    /// </summary>
    public partial class MailAddressSelection : Window
    {
        Client ClientBD = new Client();

        public MailAddressSelection()
        {
            InitializeComponent();
            String[] stringsContextMenuField = { "Добавить", "Редактировать", "Удалить" };
            ListVeiwEmailAddress.ContextMenu = new System.Windows.Controls.ContextMenu();
            for (int i = 0; i < stringsContextMenuField.Length; i++)
            {
                System.Windows.Controls.MenuItem item = new System.Windows.Controls.MenuItem();
                item.Header = stringsContextMenuField[i];
                if (i == 0) item.Click += AddAddress;
                if (i == 1) item.Click += EditAddress;
                if (i == 2) item.Click += DeleteAddress;
                ListVeiwEmailAddress.ContextMenu.Items.Add(item);
            }
        }

        private void AddAddress(object sender, RoutedEventArgs e)
        {
            try
            {
                Login log = new Login();
                log.ShowDialog();
                List<string> All = ClientBD.DisplayAllUsers();
                List<string> Active = ClientBD.DisplayActiveUsers();
                ListViewClear();
                foreach (string str in All)
                {
                    CheckBox cb = new CheckBox();
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    TextBlock tb = new TextBlock();
                    tb.Text = Cryptography.Decrypt(str);
                    sp.Children.Add(tb);
                    cb.Content = sp;
                    if (Active.Contains(str))
                        cb.IsChecked = true;
                    else
                        cb.IsChecked = false;

                    cb.Checked += CheckBox_Checked;
                    cb.Unchecked += CheckBox_Unchecked;

                    ListVeiwEmailAddress.Items.Add(cb);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void EditAddress(object sender, RoutedEventArgs e)
        {
            try
            {
                var users = ClientBD.AllAccount();
                string tempStr = Cryptography.Encrypt((((ListVeiwEmailAddress.SelectedItem as CheckBox).Content as StackPanel).Children[0] as TextBlock).Text);
                Login log = new Login(Cryptography.Decrypt(tempStr), Cryptography.Decrypt(users[tempStr]));
                log.ShowDialog();
                List<string> All = ClientBD.DisplayAllUsers();
                List<string> Active = ClientBD.DisplayActiveUsers();
                ListViewClear();
                foreach (string str in All)
                {
                    CheckBox cb = new CheckBox();
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    TextBlock tb = new TextBlock();
                    tb.Text = Cryptography.Decrypt(str);
                    sp.Children.Add(tb);
                    cb.Content = sp;
                    if (Active.Contains(str))
                        cb.IsChecked = true;
                    else
                        cb.IsChecked = false;

                    cb.Checked += CheckBox_Checked;
                    cb.Unchecked += CheckBox_Unchecked;

                    ListVeiwEmailAddress.Items.Add(cb);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DeleteAddress(object sender, RoutedEventArgs e)
        {
            try
            {
                string tempStr = Cryptography.Encrypt((((ListVeiwEmailAddress.SelectedItem as CheckBox).Content as StackPanel).Children[0] as TextBlock).Text);
                ClientBD.DeleteEmail(tempStr);
                List<string> All = ClientBD.DisplayAllUsers();
                List<string> Active = ClientBD.DisplayActiveUsers();
                ListViewClear();
                foreach (string str in All)
                {
                    CheckBox cb = new CheckBox();
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    TextBlock tb = new TextBlock();
                    tb.Text = Cryptography.Decrypt(str);
                    sp.Children.Add(tb);
                    cb.Content = sp;
                    if (Active.Contains(str))
                        cb.IsChecked = true;
                    else
                        cb.IsChecked = false;

                    cb.Checked += CheckBox_Checked;
                    cb.Unchecked += CheckBox_Unchecked;

                    ListVeiwEmailAddress.Items.Add(cb);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ListViewClear()
        {
            ListVeiwEmailAddress.Items.Clear();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                setMailAddressSelectionPosition();
                List<string> All = ClientBD.DisplayAllUsers();
                List<string> Active = ClientBD.DisplayActiveUsers();
                foreach (string str in All)
                {
                    CheckBox cb = new CheckBox();
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    TextBlock tb = new TextBlock();
                    tb.Text = Cryptography.Decrypt(str);
                    sp.Children.Add(tb);
                    cb.Content = sp;
                    if (Active.Contains(str))
                        cb.IsChecked = true;
                    else
                        cb.IsChecked = false;

                    cb.Checked += CheckBox_Checked;
                    cb.Unchecked += CheckBox_Unchecked;

                    ListVeiwEmailAddress.Items.Add(cb);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void setMailAddressSelectionPosition()
        {
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = (screenHeight - this.Height) / 2;
            this.Left = (screenWidth - this.Width) / 2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientBD.UpdateActiveUser(Cryptography.Encrypt((((sender as CheckBox).Content as StackPanel).Children[0] as TextBlock).Text), "1");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientBD.UpdateActiveUser(Cryptography.Encrypt((((sender as CheckBox).Content as StackPanel).Children[0] as TextBlock).Text), "0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
