using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Mwall
{
    interface MColumn
    {
       
        int Draw();

    }

    abstract class MColumnBase : MColumn
    {
        public static string[] PRETEXT = new string[] { "01", "0123456789",
            "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ",  
            "abcdefghijklmnopqrstuvwxyz0123456789", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
            "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
            "abcdefghijklmnopqrstuvwxyz0123456789{}[]\\|<>?/~", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789{}[]\\|<>?/~",
            "!@#$%^&*()-=_+", "{}[]\\|<>?/~", "0123456789!@#$%^&*()-=_+{}[]\\|<>?/~"};
        public static Random rd = new Random();

        protected string textSet; // the possible char set that appear in the column
        protected Point startPoint; // the starting point coordination on the screen
        protected int scrHeight; // the screen's height in pixel
        protected Single fontSize; 
        protected Canvas canv; // store the canvas where the labels to be drawn

        protected MColumnBase(Point StartPoint, Single FontSize, int ScreenHeight, Canvas BackgroundCanvas, string TextSet)
        {
            textSet = TextSet;
            startPoint = StartPoint;
            fontSize = FontSize;
            scrHeight = ScreenHeight;
            canv = BackgroundCanvas;
        }

        public abstract int Draw();
    }

    class MColumnCls : MColumnBase
    {
        //string textSet; // the possible char set that appear in the column
        //Point startPoint; // the starting point coordination on the screen
        //Single fontSize; 
        int verticalDist; // the space between two characters vertically

        //int scrHeight; // the screen's height in pixel
        int columnHeight; // the column's height is screen height + current displaying text's (labels') height

        ////int tickCount; // the update frequency of this column
        ////int intervalCount; // the counter for frequency control
        int len; // how many characters to be shown simultaneously
        int cursor; // the cursor for which label is at the bottom of the column
        //Canvas canv; // store the canvas where the labels to be drawn
        //public static Random rd = new Random();
        Label[] lbArr; // the label array
        double[] opacityArr; // control the opacity for each label in the column
        //static int lcmHeight = 1;

        //public static string[] PRETEXT = new string[] { "01", "0123456789",
        //    "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ",  
        //    "abcdefghijklmnopqrstuvwxyz0123456789", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
        //    "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
        //    "abcdefghijklmnopqrstuvwxyz0123456789{}[]\\|<>?/~", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789{}[]\\|<>?/~",
        //    "!@#$%^&*()-=_+", "{}[]\\|<>?/~", "0123456789!@#$%^&*()-=_+{}[]\\|<>?/~"};
 

        public MColumnCls(Point StartPoint, Single FontSize, int ScreenHeight, Canvas BackgroundCanvas, string TextSet, int LenCount, int VerticalDistance = 0)
            : base(StartPoint, FontSize, ScreenHeight, BackgroundCanvas, TextSet)
        {
            len = LenCount;
            //if(0 == VerticalDistance)
            verticalDist = (0 == VerticalDistance) ? (int)Math.Ceiling(FontSize): VerticalDistance;
            columnHeight = ScreenHeight + VerticalDistance * len;
            cursor = len - 1;

            opacityArr = new double[len];
            double opaDelta = 1f / len;
            for (int i = 0; i < len; i++)
                opacityArr[i] = (i + 1) * opaDelta;

            lbArr = new Label[len];
            for (int i = 0; i < len; i++)
            {
                Label l = new Label();
                l.Content = textSet.Substring(rd.Next(textSet.Length),1);
                l.FontSize = fontSize;
                l.Foreground = new SolidColorBrush(Colors.ForestGreen);
                BlurEffect b = new BlurEffect();
                b.Radius = 2;
                l.Effect = b;
                l.Opacity = opacityArr[i];
                l.Margin = new Thickness(startPoint.X, (startPoint.Y + i * verticalDist) % columnHeight, 0, 0);
                BackgroundCanvas.Children.Add(l);
                lbArr[i] = l;
            }
        }

        public override int Draw()
        {
            cursor = (cursor + 1) % len;
            Label mlb = lbArr[cursor];
            mlb.Content = textSet.Substring(rd.Next(textSet.Length),1);
            double oldx = mlb.Margin.Left;
            double oldy = mlb.Margin.Top;
            //mlb.Margin = new Thickness(startPoint.X, (startPoint.Y + (tickCount / intervalCount + len - 1) * verticalDist) % scrHeight, 0, 0);
            //mlb.Margin = new Thickness(oldx, (oldy + len * verticalDist) % scrHeight, 0, 0);
            mlb.Margin = new Thickness(oldx, (oldy + len * verticalDist) % columnHeight, 0, 0);
            mlb.Opacity = opacityArr[len - 1];
            for(int j = 1; j < len; j++)
            {
                int i = (cursor + j) % len;
                Label clb = lbArr[i];
                clb.Opacity = opacityArr[j - 1];
                //if(3 == j) clb.Content = textSet.Substring(rd.Next(textSet.Length),1);
                //clb.Margin = new Thickness(startPoint.X, startPoint.Y + (tickCount / intervalCount + j) * verticalDist, 0, 0);

            }
            return 0;
        }
    }


    class MColumnVert : MColumnBase
    {
        Label label;
        int curHeight; // the label's bottom height

        public MColumnVert(Point StartPoint, Single FontSize, int ScreenHeight, Canvas BackgroundCanvas, string TextSet)
            : base(StartPoint, FontSize, ScreenHeight, BackgroundCanvas, TextSet)
        {
            label = new Label();
            label.Content = textSet.Substring(rd.Next(textSet.Length),1) + "\n";
            label.FontSize = fontSize;
            label.Foreground = new SolidColorBrush(Colors.ForestGreen);
            //label.Width = fontSize;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Margin = new Thickness(startPoint.X, startPoint.Y, 0, 0);
            BackgroundCanvas.Children.Add(label);
        }

        public override int Draw()
        {
            label.Content += textSet.Substring(rd.Next(textSet.Length),1) + "\n";
            return 0;
        }
    }
}
