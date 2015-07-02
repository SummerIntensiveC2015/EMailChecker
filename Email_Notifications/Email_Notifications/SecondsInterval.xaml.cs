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
    /// Логика взаимодействия для SecondsInterval.xaml
    /// </summary>
    public partial class SecondsInterval : Window
    {
        public SecondsInterval()
        {
            InitializeComponent();
            texBotInterval.Text = Settings.GetInstance().NotificationLiveTimeInSeconds.ToString();
            this.ShowInTaskbar = false;
            this.WindowStyle = System.Windows.WindowStyle.None;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int interval = int.Parse(texBotInterval.Text);
                if (interval > 0)
                {
                    Settings.GetInstance().ServerCheckTimeInMinutes = interval;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Введите целое число > 0");
                }

            }
            catch
            {
                MessageBox.Show("Введите целое число > 0");
            }
        }
    }
}
