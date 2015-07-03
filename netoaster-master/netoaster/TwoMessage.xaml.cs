using System;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using netoaster;
using System.Windows.Media;
using System.Windows.Forms;
using System.Diagnostics;

public partial class WarningToaster
{
    public static string email;
    private WarningToaster(string messageEmail1, string messageAutor1, string messageTheme1, string messageEmail2, string messageAutor2, string messageTheme2, int timeActive , ToasterPosition position, ToasterAnimation animation, double margin)
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

    var msgAutor1 = (System.Windows.Documents.Run)WarningToasterInstance.FindName("autorname1");
    if (msgAutor1 != null) msgAutor1.Text = messageAutor1 ?? string.Empty;
    var msgTheme1 = (System.Windows.Documents.Run)WarningToasterInstance.FindName("themetext1");
    if (messageTheme1.Length >= 28)
    {
        if (msgTheme1 != null) msgTheme1.Text = messageTheme1.Substring(0, 27) + "..." ?? string.Empty;
    }
    else
    {
        if (msgTheme1 != null) msgTheme1.Text = messageTheme1 ?? string.Empty;
    }
    var msgEmail1 = (System.Windows.Documents.Run)WarningToasterInstance.FindName("emailtext1");
    if (msgEmail1 != null) msgEmail1.Text = messageEmail1 ?? string.Empty;
    var msgName1 = (System.Windows.Documents.Run)WarningToasterInstance.FindName("autorletter1");
    if (msgName1 != null) msgName1.Text = messageEmail1.Substring(0, 1).ToUpper() ?? string.Empty;

    var msgAutor2 = (System.Windows.Documents.Run)WarningToasterInstance.FindName("autorname2");
    if (msgAutor2 != null) msgAutor2.Text = messageAutor2 ?? string.Empty;
    var msgTheme2 = (System.Windows.Documents.Run)WarningToasterInstance.FindName("themetext2");
    if (messageTheme2.Length >= 28)
    {
        if (msgTheme2 != null) msgTheme2.Text = messageTheme2.Substring(0, 27) + "..." ?? string.Empty;
    }
    else
    {
        if (msgTheme1 != null) msgTheme2.Text = messageTheme2 ?? string.Empty;
    }
    var msgEmail2 = (System.Windows.Documents.Run)WarningToasterInstance.FindName("emailtext2");
    if (msgEmail2 != null) msgEmail2.Text = messageEmail2 ?? string.Empty;
    var msgName2 = (System.Windows.Documents.Run)WarningToasterInstance.FindName("autorletter2");
    if (msgName2 != null) msgName2.Text = messageEmail2.Substring(0, 1).ToUpper() ?? string.Empty;

    Storyboard story = ToastSupport.GetAnimation(animation, ref WarningToasterInstance, timeActive);
    story.Completed += (sender, args) => { this.Close(); };
    story.Begin(WarningToasterInstance);

    Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() =>
    {
        var topLeftDict = ToastSupport.GetTopandLeft(position, this, margin);
        Top = topLeftDict["Top"];
        Left = topLeftDict["Left"];
    }));
  }

    public static void Toast(string messageEmail1 = "A", string messageAutor1 = "", string messageTheme1 = "[Без темы]", string messageEmail2 = "A", string messageAutor2 = "", string messageTheme2 = "[Без темы]", int timeActive = 3, 
            ToasterPosition position = ToasterPosition.PrimaryScreenBottomRight,ToasterAnimation animation = ToasterAnimation.SlideInFromRight,
            double margin = 10.0)
    {
        var err = new WarningToaster(messageEmail1, messageAutor1, messageTheme1, messageEmail2, messageAutor2, messageTheme2, timeActive, position, animation, margin);
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
        Process.Start("https:" + email.Split('@')[1]);
    }




    //public static void Toast(string p1, string p2, string p3, string p4, string p5, string p6, short p7, ToasterPosition toasterPosition, ToasterAnimation animation, double margin)
    //{
    //    throw new NotImplementedException();
    //}
}