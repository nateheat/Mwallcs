﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Diagnostics;

namespace Mwall
{
    internal interface MColumn
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
        protected double scrHeight; // the screen's height in pixel
        protected float fontSize; 
        protected Canvas canv; // store the canvas where the labels to be drawn

        protected MColumnBase(Point StartPoint, float FontSize, double ScreenHeight, Canvas BackgroundCanvas, string TextSet)
        {
            textSet = TextSet;
            startPoint = StartPoint;
            fontSize = FontSize;
            scrHeight = ScreenHeight;
            canv = BackgroundCanvas;
        }

        public abstract int Draw();
    }

    class MColumnComet : MColumnBase
    {
        int verticalDist; // the space between two characters vertically

        double columnHeight; // the column's height is screen height + current displaying text's (labels') height

        int len; // how many characters to be shown simultaneously
        int cursor; // the cursor for which label is at the bottom of the column
        Label[] lbArr; // the label array
        double[] opacityArr; // control the opacity for each label in the column

        public MColumnComet(Point StartPoint, float FontSize, double ScreenHeight, Canvas BackgroundCanvas, string TextSet, int LenCount, int VerticalDistance = 0)
            : base(StartPoint, FontSize, ScreenHeight, BackgroundCanvas, TextSet)
        {
            len = LenCount;
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
            mlb.Margin = new Thickness(oldx, (oldy + len * verticalDist) % columnHeight, 0, 0);
            mlb.Opacity = opacityArr[len - 1];
            for(int j = 1; j < len; j++)
            {
                int i = (cursor + j) % len;
                Label clb = lbArr[i];
                clb.Opacity = opacityArr[j - 1];
            }
            return 0;
        }
    }


    class MColumnStraight : MColumnBase
    {
        enum State
        {
            Dropping, Fading, Silence
        }

        State state;

        double[] opacityArr; // controls the opacity of the column when it reaches bottom
        int counter;
        int silenceTurn;
        int fadingTurn;

        Label label;

        public MColumnStraight(Point StartPoint, float FontSize, double ScreenHeight, Canvas BackgroundCanvas, string TextSet, int FadingTurn = 8, int SilenceTurn = 4)
            : base(StartPoint, FontSize, ScreenHeight, BackgroundCanvas, TextSet)
        {
            fadingTurn = FadingTurn;
            silenceTurn = SilenceTurn;
            label = new Label();
            label.Content = textSet.Substring(rd.Next(textSet.Length),1) + Environment.NewLine;
            label.FontSize = fontSize;
            label.Foreground = new SolidColorBrush(Colors.ForestGreen);
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Margin = new Thickness(startPoint.X, startPoint.Y, 0, 0);
            BackgroundCanvas.Children.Add(label);
            state = State.Dropping;
            opacityArr = new double[fadingTurn];
            double opaDelta = 1f / opacityArr.Length;
            for (int i = 0; i < opacityArr.Length; i++)
                opacityArr[i] = (i + 1) * opaDelta;
            //counter = fadingTurn - 1;
            int initc = rd.Next((int)Math.Round(ScreenHeight/FontSize)) / 2;
            for (int i = 0; i < initc; i++)
            {
                if (label.ActualHeight > scrHeight) break;
                label.Content += textSet.Substring(rd.Next(textSet.Length), 1) + Environment.NewLine;
            }
            
            counter = initc;
            label.Opacity = .95;
        }

        public override int Draw()
        {
            if (State.Dropping == state)
            {
                if(counter < 0)
                {
                    label.Content = "";
                    counter = 0;
                    label.Opacity = .95;
                }
                if (label.ActualHeight < scrHeight)
                {
                    label.Content += textSet.Substring(rd.Next(textSet.Length), 1) + Environment.NewLine;
                }
                else
                {
                    state = State.Fading;
                    counter = opacityArr.Length - 1;
                }
            }
            else if (State.Fading == state)
            {
                if (--counter >= 0)
                {
                    label.Opacity = opacityArr[counter];
                }
                else
                {
                    counter = silenceTurn;
                    state = State.Silence;
                }
            }
            else if (State.Silence == state)
            {
                if(--counter < 0) {
                    //counter = opacityArr.Length - 1;
                    label.Content = textSet.Substring(rd.Next(textSet.Length), 1) + Environment.NewLine;
                    state = State.Dropping;
                    //counter = 0;
                }
            }
            else
            {
                // shall not reach here
                Debug.WriteLine("Unknown state in MColumnVert's Draw");
            }
            return 0;
        }
    }
}
