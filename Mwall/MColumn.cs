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
    class MColumn
    {
        string textSet; // the possible char set that appear in the column
        Point startPoint; // the starting point coordination on the screen
        Single fontSize; 
        int verticalDist; // the space between two characters vertically

        int scrHeight; // the screen's height in pixel
        int columnHeight; // the column's height is screen height + current displaying text's (labels') height

        //int tickCount; // the update frequency of this column
        int intervalCount; // the counter for frequency control
        int len; // how many characters to be shown simultaneously
        int cursor; // the cursor for which label is at the bottom of the column
        Canvas canv; // store the canvas where the labels to be drawn
        public static Random rd = new Random();
        Label[] lbArr; // the label array
        double[] opacityArr; // control the opacity for each label in the column
        //static int lcmHeight = 1;

        public static string[] PRETEXT = new string[] { "01", "0123456789",
            "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ",  
            "abcdefghijklmnopqrstuvwxyz0123456789", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
            "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
            "abcdefghijklmnopqrstuvwxyz0123456789{}[]\\|<>?/~", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789{}[]\\|<>?/~",
            "!@#$%^&*()-=_+", "{}[]\\|<>?/~", "0123456789!@#$%^&*()-=_+{}[]\\|<>?/~"};
 

        public MColumn(string textStr, Point StartPoint, Single FontSize, int VerticalDistance, int screenHeight, int LenCount, int interval, Canvas BackgroundCanvas)
        {
            textSet = textStr;
            startPoint = StartPoint;
            fontSize = FontSize;
            len = LenCount;
            verticalDist = VerticalDistance;

            scrHeight = screenHeight;
            columnHeight = screenHeight + verticalDist * len;
            //lcmHeight = GetLCM(lcmHeight, columnHeight);

            intervalCount = interval;
            canv = BackgroundCanvas;
            cursor = len - 1;
            //tickCount = 0;

            opacityArr = new double[len];
            double opaDelta = 1f / len;
            for (int i = 0; i < len; i++)
                opacityArr[i] = (i + 1) * opaDelta;

            lbArr = new Label[len];
            for (int i = 0; i < len; i++)
            {
                Label l = new Label();
                //l.ReleaseMouseCapture();
                l.Content = textSet.Substring(rd.Next(textSet.Length),1);
                l.FontSize = fontSize;
                l.Foreground = new SolidColorBrush(Colors.ForestGreen);
                BlurEffect b = new BlurEffect();
                b.Radius = 2;
                l.Effect = b;
                l.Opacity = opacityArr[i];
                //l.Margin = new Thickness(startPoint.X, (startPoint.Y + i * verticalDist) % scrHeight, 0, 0);
                l.Margin = new Thickness(startPoint.X, (startPoint.Y + i * verticalDist) % columnHeight, 0, 0);
                BackgroundCanvas.Children.Add(l);
                lbArr[i] = l;
            }
        }

        public int Draw()
        {
            //tickCount++;
            //if (++tickCount < intervalCount) return 0;
            //tickCount = 0;
            //if (0 != tickCount % intervalCount) return 0;

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
}
