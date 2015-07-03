using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using netoaster;
using System.Diagnostics;

public partial class SuccessToaster
{
    public static string email;
    private SuccessToaster(string messageEmail, string messageAutor,string messageTheme, int timeActive, ToasterPosition position, ToasterAnimation animation, double margin)
    {
         

        InitializeComponent();
        Brush[] brushes = new Brush[] {
            new SolidColorBrush(Color.FromArgb(0xFF, 0x2D, 0x1E, 0x4B)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x7B, 0x25, 0xFA)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x71, 0x9F, 0x3F))
        };
        Random rnd = new Random();
        avatar.Fill = brushes[rnd.Next(brushes.Length)];
        
        var msgAutor = (System.Windows.Documents.Run)SuccessToasterInstance.FindName("autorname");
        if (msgAutor != null) msgAutor.Text = messageAutor ?? string.Empty;
        var msgTheme = (System.Windows.Documents.Run)SuccessToasterInstance.FindName("themetext");
        if (messageTheme.Length >= 28)
        {
            if (msgTheme != null) msgTheme.Text = messageTheme.Substring(0, 27) + "..." ?? string.Empty;
        }
        else
        {
            if (msgTheme != null) msgTheme.Text = messageTheme ?? string.Empty;
        }
        var msgEmail = (System.Windows.Documents.Run)SuccessToasterInstance.FindName("emailtext");
        if (msgEmail != null) msgEmail.Text = messageEmail ?? string.Empty;
        var msgName = (System.Windows.Documents.Run)SuccessToasterInstance.FindName("autorletter");
        if (msgName != null) msgName.Text = messageEmail.Substring(0, 1).ToUpper() ?? string.Empty;

        Storyboard story = ToastSupport.GetAnimation(animation, ref SuccessToasterInstance, timeActive);
        story.Completed += (sender, args) => { this.Close(); };
        story.Begin(SuccessToasterInstance);

        Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() =>
        {
            var topLeftDict = ToastSupport.GetTopandLeft(position, this, margin);
            Top = topLeftDict["Top"];
            Left = topLeftDict["Left"];
        }));
    }

    public static void Toast(string messageEmail="A", string messageAutor = "", string messageTheme = "[Без темы]", int timeActive=3,
        ToasterPosition position = ToasterPosition.PrimaryScreenTopRight, ToasterAnimation animation = ToasterAnimation.SlideInFromRight,
        double margin = 10.0)
    {
        var err = new SuccessToaster(messageEmail, messageAutor, messageTheme, timeActive, position, animation, margin);
        err.Show();
    }


    public static void Toast(string p, ToasterPosition toasterPosition, ToasterAnimation animation, double margin)
    {
        throw new NotImplementedException();
    }

    private void close_Click(object sender, RoutedEventArgs e)
    {
        window.Close();
    }

    private void Border_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
            Process.Start("https:"+email.Split('@')[1]);
    }
}