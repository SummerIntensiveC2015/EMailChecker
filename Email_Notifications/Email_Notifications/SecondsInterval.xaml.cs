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
            setPosition();
        }

        private void setPosition()
        {
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Top = (screenHeight - this.Height);
            this.Left = (screenWidth - (int)(this.Width / 0.93));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int interval = int.Parse(texBotInterval.Text),
                    maxinterval = Settings.GetInstance().ServerCheckTimeInMinutes * 60;
                if (interval > 0 && interval < maxinterval)
                {
                    Settings.GetInstance().NotificationLiveTimeInSeconds = interval;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Введите целое число > 0 и < "+maxinterval);
                }

            }
            catch
            {
                MessageBox.Show("Введите корректное число");
            }
        }
    }
}
