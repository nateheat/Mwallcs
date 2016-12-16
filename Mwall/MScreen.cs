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
    class MScreen
    {
        public static Random random = new Random();
        public const int MAX_CYCLE = 100;
        public List<MColumn>[] ArrCycleList = new List<MColumn>[MAX_CYCLE];
        private double width, height;
        private Canvas canv;
        private int cur; // the ticking cursor

        /// <summary>
        /// Default constructor 
        /// </summary>
        /// <param name="canvas">the canvas to be drawn on</param>
        public MScreen(Canvas canvas)
        {
            //width = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth);
            //height = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight);
            width = System.Windows.SystemParameters.PrimaryScreenWidth;
            height = System.Windows.SystemParameters.PrimaryScreenHeight;
            canv = canvas;
            for(int i = 0; i < MAX_CYCLE; i++) ArrCycleList[i] = new List<MColumn>();
            cur = 0;
        }

        public int GenerateCometColumns()
        {
            float fsize = MConfig.GetInstance().FSize;
            int colWidth = (int)Math.Ceiling(fsize) + 5;

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
            float fsize = MConfig.GetInstance().FSize;
            int colWidth = (int)Math.Ceiling(fsize) + 5;

            for(int i = 0; i < width/colWidth; i++)
            {
                string str = MColumnBase.PRETEXT[random.Next(MColumnBase.PRETEXT.Length)];
                int startingX = i * colWidth + random.Next(colWidth)/2 - colWidth / 4;
                int rndinterval = random.Next(9) + 1;
                MColumn mc = new MColumnVert(new Point(startingX, 0), fsize, height, canv, str);
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


    }
}
