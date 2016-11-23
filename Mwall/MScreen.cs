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
        public const int MAX_CYCLE = 100;
        public List<MColumn>[] ArrCycleList = new List<MColumn>[MAX_CYCLE];
        private int width, height;
        private Canvas canv;
        private int cur; // the ticking cursor

        /// <summary>
        /// Default constructor 
        /// </summary>
        /// <param name="canvas">the canvas to be drawn on</param>
        public MScreen(Canvas canvas)
        {
            width = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth);
            height = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight);
            canv = canvas;
            for(int i = 0; i < MAX_CYCLE; i++) ArrCycleList[i] = new List<MColumn>();
            cur = 0;
        }

        public int GenerateDistributedColumn()
        {
            int SUPPOSED_COLUMN_WIDTH = 25;

            for(int i = 0; i < width/SUPPOSED_COLUMN_WIDTH; i++)
            {
                string str = MColumnCls.PRETEXT[MColumnCls.rd.Next(MColumnCls.PRETEXT.Length)];
                int startingX = i * SUPPOSED_COLUMN_WIDTH + MColumnCls.rd.Next(SUPPOSED_COLUMN_WIDTH) - SUPPOSED_COLUMN_WIDTH *2 / 3;
                int verticalGap = MColumnCls.rd.Next(10);
                int rndinterval = MColumnCls.rd.Next(8) + 1;
                int rndlen = MColumnCls.rd.Next(12) + 5;
                MColumn mc = new MColumnCls(new Point(startingX, 0), 20, height, canv, str, rndlen );
                for(int j = 1; j < MAX_CYCLE; j++) 
                    if (0 == j % rndinterval) ArrCycleList[j].Add(mc);
            } 
            

            return 0;
        }

        public int GenerateVerticalColumn()
        {
            int SUPPOSED_COLUMN_WIDTH = 25;

            for(int i = 0; i < width/SUPPOSED_COLUMN_WIDTH; i++)
            {
                string str = MColumnBase.PRETEXT[MColumnBase.rd.Next(MColumnBase.PRETEXT.Length)];
                int startingX = i * SUPPOSED_COLUMN_WIDTH + MColumnCls.rd.Next(SUPPOSED_COLUMN_WIDTH) - SUPPOSED_COLUMN_WIDTH *2 / 3;
                int rndinterval = MColumnCls.rd.Next(4) + 1;
                MColumn mc = new MColumnVert(new Point(startingX, 0), 20, height, canv, str);
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
