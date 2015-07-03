using System.Windows;
using netoaster;

namespace toasterdemoapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            selectbox.SelectedIndex = 0;
            aniselectbox.SelectedIndex = 0;
        }

        public ToasterPosition CurrentPopupPosition { get; set; }
        public ToasterAnimation CurrentPopupAnimation { get; set; }

        private void showsuccess(object sender, RoutedEventArgs e)
        {
            SuccessToaster.Toast(emailtext1.Text,autorname1.Text, themetext1.Text,System.Convert.ToInt16(combo.Text), (ToasterPosition)selectbox.SelectedItem, animation: (ToasterAnimation)aniselectbox.SelectedItem, margin: 20.0);
            
        }
        public int pop;

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ErrorToaster.Toast(emailtext1.Text, autorname1.Text, themetext1.Text, emailtext2.Text, autorname2.Text, themetext2.Text,emailtext3.Text, autorname3.Text, themetext3.Text, System.Convert.ToInt16(combo.Text),System.Convert.ToInt16(combo.Text), (ToasterPosition)selectbox.SelectedItem, animation: (ToasterAnimation)aniselectbox.SelectedItem, margin: 20.0);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
          WarningToaster.Toast(emailtext1.Text, autorname1.Text, themetext1.Text, emailtext2.Text, autorname2.Text, themetext2.Text, System.Convert.ToInt16(combo.Text), (ToasterPosition)selectbox.SelectedItem, animation: (ToasterAnimation)aniselectbox.SelectedItem, margin: 20.0);
        }

    }
}
