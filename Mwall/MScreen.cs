using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mwall
{
    /// <summary>
    /// It represents the display screen of the monitor, currently only supports the main screen of the PC.
    /// MScreen also hold the references of the MColumn which need to be updated (or draw) on specific "tick".
    /// </summary>
    public class MScreen
    {
        private static List<MScreen> lms = new List<MScreen>();
        public static bool ReGen()
        {
            foreach (MScreen scr in lms)
            {
                scr.CleanColumns();
                scr.GenerateColumns();
            }
            return true;
        }

        public static Random random = new Random();
        public const int MAX_CYCLE = 100;
        internal List<MColumn>[] ArrCycleList = new List<MColumn>[MAX_CYCLE];
        private double width, height;
        private Canvas canv;
        private int cur; // the ticking cursor

        /// <summary>
        /// Default constructor 
        /// </summary>
        /// <param name="canvas">the canvas to be drawn on</param>
        public MScreen(Canvas canvas)
        {
            width = System.Windows.SystemParameters.PrimaryScreenWidth;
            height = System.Windows.SystemParameters.PrimaryScreenHeight;
            canv = canvas;
            for(int i = 0; i < ArrCycleList.Length; i++) ArrCycleList[i] = new List<MColumn>();
            cur = 0;
            if (!lms.Contains(this)) lms.Add(this);
        }

        public bool GenerateColumns()
        {
            MConfig mcf = MConfig.GetInstance();
            switch (mcf.Style)
            {
                case MWStyle.Comet:
                    this.GenerateCometColumns();
                    break;
                case MWStyle.Straight:
                    this.GenerateStraightColumns();
                    break;
                default:
                    break;
            }
            return true;

        }

        public int GenerateCometColumns()
        {
            MConfig conf = MConfig.GetInstance();
            float fsize = conf.FSize;
            int colWidth = (int)Math.Ceiling(fsize) + conf.ColumnGap;

            for(int i = 0; i < width/colWidth; i++)
            {
                string str = MColumnBase.PRETEXT[random.Next(MColumnBase.PRETEXT.Length)];
                int startingX = i * colWidth + random.Next(colWidth)/2 - colWidth / 4;
                int verticalGap = random.Next(10);
                int rndinterval = random.Next(8) + 1;
                int rndlen = random.Next(12) + 5;
                MColumn mc = new MColumnComet(new Point(startingX, 0), fsize, height, canv, str, rndlen );
                for(int j = 1; j < MAX_CYCLE; j++) 
                    if (0 == j % rndinterval) ArrCycleList[j].Add(mc);
            } 
            

            return 0;
        }

        public int GenerateStraightColumns()
        {
            MConfig conf = MConfig.GetInstance();
            float fsize = conf.FSize;
            int colWidth = (int)Math.Ceiling(fsize) + conf.ColumnGap;

            for(int i = 0; i < width/colWidth; i++)
            {
                string str = MColumnBase.PRETEXT[random.Next(MColumnBase.PRETEXT.Length)];
                int startingX = i * colWidth + random.Next(colWidth)/2 - colWidth / 4;
                int rndinterval = random.Next(9) + 1;
                MColumn mc = new MColumnStraight(new Point(startingX, 0), fsize, height, canv, str);
                for(int j = 1; j < MAX_CYCLE; j++) 
                    if (0 == j % rndinterval) ArrCycleList[j].Add(mc);
            } 
            return 0;
        }

        public int Draw()
        {
            cur = (cur >= MAX_CYCLE - 1)? 0 : cur + 1;
            foreach(MColumn mc in ArrCycleList[cur]) { mc.Draw(); }
            return 0;
        }

        public bool CleanColumns()
        {
            canv.Children.Clear();
            for (int i = 0; i < ArrCycleList.Length; i++)
                ArrCycleList[i] = new List<MColumn>();

            return true;
        }


    }
}
