using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Matrix = System.Windows.Media.Matrix;
using Point = System.Windows.Point;
using StoryBoard = System.Windows.Media.Animation.Storyboard;

namespace netoaster
{
    public enum ToasterPosition
    {
        PrimaryScreenBottomRight,
        PrimaryScreenTopRight,
        PrimaryScreenBottomLeft,
        PrimaryScreenTopLeft,
        ApplicationBottomRight,
        ApplicationTopRight,
        ApplicationBottomLeft,
        ApplicationTopLeft,
    }

    
    public enum ToasterAnimation
    {
        FadeIn,
        SlideInFromRight,
        SlideInFromLeft,
        SlideInFromTop,
        SlideInFromBottom,
        GrowFromRight,
        GrowFromLeft,
        GrowFromTop,
        GrowFromBottom,
    }
    class ToastSupport
    {
        public static StoryBoard GetAnimation(ToasterAnimation animation, ref Grid toaster)
        {
            Storyboard story = new Storyboard();
            SplineDoubleKeyFrame frame = new SplineDoubleKeyFrame();

                    DoubleAnimation fadein = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
                    fadein.BeginTime = TimeSpan.FromSeconds(0);
                    Storyboard.SetTargetProperty(fadein, new PropertyPath("(UIElement.Opacity)"));
                    story.Children.Add(fadein);
                    DoubleAnimation fadeout = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                    fadeout.BeginTime = TimeSpan.FromSeconds(7);
                    Storyboard.SetTargetProperty(fadeout, new PropertyPath("(UIElement.Opacity)"));
                    story.Children.Add(fadeout);
            return story;
        }

        public static Dictionary<string, double> GetTopandLeft(ToasterPosition positionSelection, Window windowRef, double margin)
        {
            var retDict = new Dictionary<string, double>();
            Rectangle workingArea;
            Point bottomcorner;
            Point topcorner;

            workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            
            var currentAppWindow = Application.Current.MainWindow;
            //Get the Currently running applications screen.
            var screen = System.Windows.Forms.Screen.FromHandle(
                new System.Windows.Interop.WindowInteropHelper(currentAppWindow).Handle);
            
            var workingPosition = positionSelection;
            //Application being maximized causes some wonky crap sometimes. 
            //May as well use Primary Screen. 
            if (currentAppWindow.WindowState == WindowState.Maximized)
            {
                switch (positionSelection)
                {
                    case ToasterPosition.ApplicationBottomRight:
                        workingPosition = ToasterPosition.PrimaryScreenBottomRight;
                        break;
                    case ToasterPosition.ApplicationTopRight:
                        workingPosition = ToasterPosition.PrimaryScreenTopRight;
                        break;
                }
            }
            var transform = getTransform(windowRef);
            
            retDict.Add("Left", 0);
            retDict.Add("Top", 0);
            switch (workingPosition)
            {
               
                case ToasterPosition.PrimaryScreenBottomRight:
                    workingArea = screen.WorkingArea;
                    bottomcorner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));
                    retDict["Left"] = bottomcorner.X - windowRef.ActualWidth - margin;
                    retDict["Top"] = bottomcorner.Y - windowRef.ActualHeight - margin;
                    break;
              
                case ToasterPosition.PrimaryScreenTopRight:
                    workingArea = screen.WorkingArea;
                    bottomcorner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));
                    topcorner = transform.Transform(new Point(workingArea.Right, workingArea.Top));
                    retDict["Left"] = bottomcorner.X - windowRef.ActualWidth - margin;
                    retDict["Top"] = topcorner.Y + margin;
                    break;
             
                default:
                    //ToasterPosition.PrimaryScreenBottomRight
                    workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                    bottomcorner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));
                    topcorner = transform.Transform(new Point(workingArea.Right, workingArea.Top));
                    retDict["Left"] = bottomcorner.X - windowRef.ActualWidth - 100;
                    retDict["Top"] = bottomcorner.Y - windowRef.ActualHeight;
                    break;
            }
            return retDict;

        }

        private static Matrix getTransform(Window windowRef)
        {
            var presentationSource = PresentationSource.FromVisual(windowRef);
            if (presentationSource != null)
            {
                if (presentationSource.CompositionTarget != null)
                {
                    return presentationSource.CompositionTarget.TransformFromDevice;
                }
            }
            return new Matrix();
        }
    }
}
