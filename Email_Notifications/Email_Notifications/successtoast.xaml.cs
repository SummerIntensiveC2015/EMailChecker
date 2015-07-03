using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using netoaster;

public partial class SuccessToaster
{
    private SuccessToaster(string messageAutor,string messageTheme, ToasterPosition position, ToasterAnimation animation, double margin)
    {
        InitializeComponent();

        var msgAutor = (System.Windows.Documents.Run)SuccessToasterInstance.FindName("autorname");
        if (msgAutor != null) msgAutor.Text = messageAutor ?? string.Empty;
        var msgTheme = (System.Windows.Documents.Run)SuccessToasterInstance.FindName("themetext");
        if (msgTheme != null) msgTheme.Text = messageTheme ?? string.Empty;
        var msgName = (System.Windows.Documents.Run)SuccessToasterInstance.FindName("name");
        if (msgName != null) msgName.Text = messageAutor.Substring(0,1) ?? string.Empty;

        Storyboard story = ToastSupport.GetAnimation(animation, ref SuccessToasterInstance);
        story.Completed += (sender, args) => { this.Close(); };
        story.Begin(SuccessToasterInstance);

        Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() =>
        {
            var topLeftDict = ToastSupport.GetTopandLeft(position, this, margin);
            Top = topLeftDict["Top"];
            Left = topLeftDict["Left"];
        }));
    }

    public static void Toast(string messageAutor = "", string messageTheme="[Без темы]",
        ToasterPosition position = ToasterPosition.PrimaryScreenTopRight, ToasterAnimation animation = ToasterAnimation.SlideInFromRight,
        double margin = 10.0)
    {
        var err = new SuccessToaster(messageAutor, messageTheme, position, animation, margin);
        err.Show();
    }


    public static void Toast(string p, ToasterPosition toasterPosition, ToasterAnimation animation, double margin)
    {
        throw new NotImplementedException();
    }
}