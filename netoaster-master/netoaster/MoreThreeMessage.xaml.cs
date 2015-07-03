using System;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using netoaster;
using System.Windows.Media;
using System.Diagnostics;

public partial class ErrorToaster
{
    public static string email;
    private ErrorToaster(string messageEmail1, string messageAutor1, string messageTheme1, string messageEmail2, string messageAutor2, string messageTheme2, string messageEmail3, string messageAutor3, string messageTheme3, int timeActive, int countNewMessage, ToasterPosition position, ToasterAnimation animation, double margin)
    {
        InitializeComponent();

        Brush[] brushes = new Brush[] {
            new SolidColorBrush(Color.FromArgb(0xFF, 0x2D, 0x1E, 0x4B)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x7B, 0x25, 0xFA)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x71, 0x9F, 0x3F))
        };
        Random rnd = new Random();
        avatar1.Fill = brushes[rnd.Next(brushes.Length)];
        avatar2.Fill = brushes[rnd.Next(brushes.Length)];
        avatar3.Fill = brushes[rnd.Next(brushes.Length)];
        var msgAutor1 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("autorname1");
        if (msgAutor1 != null) msgAutor1.Text = messageAutor1 ?? string.Empty;
        var msgTheme1 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("themetext1");
        if (messageTheme1.Length >= 28)
        {
            if (msgTheme1 != null) msgTheme1.Text = messageTheme1.Substring(0, 27) + "..." ?? string.Empty;
        }
        else
        {
            if (msgTheme1 != null) msgTheme1.Text = messageTheme1 ?? string.Empty;
        }
        var msgEmail1 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("emailtext1");
        if (msgEmail1 != null) msgEmail1.Text = messageEmail1 ?? string.Empty;
        var msgName1 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("autorletter1");
        if (msgName1 != null) msgName1.Text = messageEmail1.Substring(0, 1).ToUpper() ?? string.Empty;

        var msgAutor2 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("autorname2");
        if (msgAutor2 != null) msgAutor2.Text = messageAutor2 ?? string.Empty;
        var msgTheme2 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("themetext2");
        if (messageTheme2.Length >= 28)
        {
            if (msgTheme2 != null) msgTheme2.Text = messageTheme2.Substring(0, 27) + "..." ?? string.Empty;
        }
        else
        {
            if (msgTheme1 != null) msgTheme2.Text = messageTheme2 ?? string.Empty;
        }
        var msgEmail2 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("emailtext2");
        if (msgEmail2 != null) msgEmail2.Text = messageEmail2 ?? string.Empty;
        var msgName2 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("autorletter2");
        if (msgName2 != null) msgName2.Text = messageEmail2.Substring(0, 1).ToUpper() ?? string.Empty;

        var msgAutor3 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("autorname3");
        if (msgAutor3 != null) msgAutor3.Text = messageAutor3 ?? string.Empty;
        var msgTheme3 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("themetext3");
        if (messageTheme3.Length >= 28)
        {
            if (msgTheme3 != null) msgTheme3.Text = messageTheme3.Substring(0, 27) + "..." ?? string.Empty;
        }
        else
        {
            if (msgTheme3 != null) msgTheme3.Text = messageTheme3 ?? string.Empty;
        }
        var msgEmail3 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("emailtext3");
        if (msgEmail3 != null) msgEmail3.Text = messageEmail3 ?? string.Empty;
        var msgName3 = (System.Windows.Documents.Run)ErrorToasterInstance.FindName("autorletter3");
        if (msgName3 != null) msgName3.Text = messageEmail3.Substring(0, 1).ToUpper() ?? string.Empty;

        if (countNewMessage > 0)
        {
            fullinfo.Content = "Новых писем -" + " " + countNewMessage.ToString();
            fullinfo.Visibility = System.Windows.Visibility.Visible;
            rectang.Visibility = System.Windows.Visibility.Visible;
            this.Height = 252;
            bord.Height = 252;
        }
        else
        {
            fullinfo.Visibility = System.Windows.Visibility.Hidden;
            this.Height = 240;
            bord.Height = 240;
            rectang.Visibility = System.Windows.Visibility.Hidden;

        }

        Storyboard story = ToastSupport.GetAnimation(animation, ref ErrorToasterInstance, timeActive);
        story.Completed += (sender, args) => { this.Close(); };
        story.Begin(ErrorToasterInstance);

        Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() =>
        {
            var topLeftDict = ToastSupport.GetTopandLeft(position, this, margin);
            Top = topLeftDict["Top"];
            Left = topLeftDict["Left"];
        }));
    }

    public static void Toast(
       string messageEmail1, string messageAutor1 = "", string messageTheme1 = "", string messageEmail2 = "", string messageAutor2 = "", string messageTheme2 = "", string messageEmail3 = "", string messageAutor3 = "", string messageTheme3 = "", int timeActive = 3, int countNewMessage=0, ToasterPosition position = ToasterPosition.PrimaryScreenBottomRight, ToasterAnimation animation = ToasterAnimation.SlideInFromRight,
        double margin = 10.0)
    {
        var err = new ErrorToaster(messageEmail1,messageAutor1,messageTheme1,messageEmail2,messageAutor2,messageTheme2,messageEmail3,messageAutor3,messageTheme3,timeActive, countNewMessage, position, animation, margin);
        err.Show();
    }

    private void Storyboard_Completed(object sender, EventArgs e)
    {
        this.Close();
    }

    private void close_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.Close();
    }

    private void Border_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        Process.Start("https://" + email.Split('@')[1]);
    }
}
