using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mwall
{
    class MScreen
    {
        public const int MAX_CYCLE = 50;
        public List<MColumn>[] ArrCycleList = new List<MColumn>[MAX_CYCLE];
        private int width, height;
        private Canvas canv;
        private int cur;

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
                string str = MColumn.PRETEXT[MColumn.rd.Next(MColumn.PRETEXT.Length)];
                int startingX = i * SUPPOSED_COLUMN_WIDTH + MColumn.rd.Next(SUPPOSED_COLUMN_WIDTH) - SUPPOSED_COLUMN_WIDTH *2 / 3;
                int verticalGap = MColumn.rd.Next(10);
                int rndinterval = MColumn.rd.Next(8) + 1;
                int rndlen = MColumn.rd.Next(12) + 5;
                MColumn mc = new MColumn(str, new Point(startingX, 0), 20, 20 + verticalGap, height, rndlen, rndinterval, canv);
                for(int j = 1; j < MAX_CYCLE; j++) 
                    if (0 == j % rndinterval) ArrCycleList[j].Add(mc);

                //ArrCycleList[rndinterval].Add(new MColumn(str, new Point(startingX, 0), 20, 20 + verticalGap, height, rndlen, rndinterval, BackCanv));
                //columnList.Add(new MColumn(str, new Point(startingX, 0), 20, 20 + verticalGap, height, rndlen, rndinterval, BackCanv));
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
