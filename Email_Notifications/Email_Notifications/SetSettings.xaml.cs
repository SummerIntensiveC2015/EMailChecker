using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для SetSettings.xaml
    /// </summary>
    public partial class SetSettings : Window
    {
        int minvalue = 1,
            maxvalue = 100,
            minvalueSeconds = 1,
            maxvalueSeconds = 60;
        Client ClientDB = new Client();
        public SetSettings()
        {
            InitializeComponent();
            NUDTextBox.Text = GlobalSettings.ServerCheckTimeInMinutes.ToString();
            NUDTextBoxSeconds.Text = GlobalSettings.NotificationLiveTimeInSeconds.ToString();
            Refresh();
            SetSettings_setPosition();
        }

        public void Refresh()
        {
            DataRow[] dr = ClientDB.DisplayAllUsersActive();
            DataGridUsers.ItemsSource = ClientDB.ShowUser(dr);
            DataGridUsers.Items.Refresh();
        }

        private void SetSettings_setPosition()
        {
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = (screenHeight - this.Height) / 2;
            this.Left = (screenWidth - this.Width) / 2;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string tempStr = Cryptography.Encrypt((DataGridUsers.SelectedItem as DataRowView)[0].ToString());
                ClientDB.DeleteEmail(tempStr);
                Refresh();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Выберите адрес для удаления");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Login log = new Login(1);
                log.Hide();
                log.ShowDialog();
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                var users = ClientDB.AllAccount();
                string tempStr = Cryptography.Encrypt((DataGridUsers.SelectedItem as DataRowView)[0].ToString());
                Login log = new Login(Cryptography.Decrypt(tempStr), Cryptography.Decrypt(users[tempStr]), 1);
                log.ShowDialog();
                Refresh();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Выберите адрес для редактирования");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DataGridUsers_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (DataGridUsers.CurrentCell.Column == DataGridUsers.Columns[1])
                {
                    bool AccChecked = bool.Parse((DataGridUsers.CurrentCell.Item as DataRowView)[1].ToString());
                    if (AccChecked)
                        ClientDB.UpdateActiveUser(Cryptography.Encrypt((DataGridUsers.CurrentCell.Item as DataRowView)[0].ToString()), "0");
                    else
                        ClientDB.UpdateActiveUser(Cryptography.Encrypt((DataGridUsers.CurrentCell.Item as DataRowView)[0].ToString()), "1");
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonGlobalSettingsApply_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.ServerCheckTimeInMinutes = int.Parse(NUDTextBox.Text);
            GlobalSettings.NotificationLiveTimeInSeconds = int.Parse(NUDTextBoxSeconds.Text);
        }




        #region Minutes
        private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text);
            else number = 0;
            if (number < maxvalue)
                NUDTextBox.Text = Convert.ToString(number + 1);
        }

        private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text);
            else number = 0;
            if (number > minvalue)
                NUDTextBox.Text = Convert.ToString(number - 1);
        }

        private void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up)
            {
                NUDButtonUP.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { true });
            }


            if (e.Key == Key.Down)
            {
                NUDButtonDown.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { true });
            }
        }

        private void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { false });
        }

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (NUDTextBox.Text != "")
                if (!int.TryParse(NUDTextBox.Text, out number)) NUDTextBox.Text = GlobalSettings.ServerCheckTimeInMinutes.ToString();
            if (number > maxvalue) NUDTextBox.Text = maxvalue.ToString();
            if (number < minvalue) NUDTextBox.Text = minvalue.ToString();
            NUDTextBox.SelectionStart = NUDTextBox.Text.Length;
        }
        #endregion

        #region Seconds


        private void NUDButtonUPSeconds_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBoxSeconds.Text != "") number = Convert.ToInt32(NUDTextBoxSeconds.Text);
            else number = 0;
            if (number < maxvalueSeconds)
                NUDTextBoxSeconds.Text = Convert.ToString(number + 1);
        }

        private void NUDButtonDownSeconds_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBoxSeconds.Text != "") number = Convert.ToInt32(NUDTextBoxSeconds.Text);
            else number = 0;
            if (number > minvalueSeconds)
                NUDTextBoxSeconds.Text = Convert.ToString(number - 1);
        }

        private void NUDTextBoxSeconds_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up)
            {
                NUDButtonUPSeconds.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUPSeconds, new object[] { true });
            }


            if (e.Key == Key.Down)
            {
                NUDButtonDownSeconds.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDownSeconds, new object[] { true });
            }
        }

        private void NUDTextBoxSeconds_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUPSeconds, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDownSeconds, new object[] { false });
        }

        private void NUDTextBoxSeconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (NUDTextBoxSeconds.Text != "")
                if (!int.TryParse(NUDTextBoxSeconds.Text, out number)) NUDTextBoxSeconds.Text = GlobalSettings.NotificationLiveTimeInSeconds.ToString();
            if (number > maxvalueSeconds) NUDTextBoxSeconds.Text = maxvalueSeconds.ToString();
            if (number < minvalueSeconds) NUDTextBoxSeconds.Text = minvalueSeconds.ToString();
            NUDTextBoxSeconds.SelectionStart = NUDTextBoxSeconds.Text.Length;
        }

        #endregion
    }
}