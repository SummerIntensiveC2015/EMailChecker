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
    /// Логика взаимодействия для Interval.xaml
    /// </summary>
    public partial class Interval : Window
    {
        public Interval()
        {
            InitializeComponent();
            texBotInterval.Text = GlobalSettings.ServerCheckTimeInMinutes.ToString();
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
            this.Close();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int interval = int.Parse(texBotInterval.Text);
                if (interval > 0)
                {
                    GlobalSettings.ServerCheckTimeInMinutes = interval;
                    this.Close();
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
