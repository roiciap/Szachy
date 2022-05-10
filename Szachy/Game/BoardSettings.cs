using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szachy.Game
{
    public class BoardSettings
    {
        public bool[,] moved ;
        public bool[,] castle;

        public BoardSettings()
        {
            castle=new bool[ 2, 2 ];

            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    castle[i,j] = true;
                }
            }
        }
        public BoardSettings( bool[,] canCastle)
        {
            moved = new bool[8, 2];
            castle = new bool[2, 2];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    castle[i, j] = canCastle[i,j];
                }
            }
        }



        public bool checkCastle(int x, PlayerColour colour)
        {
            return castle[x, (int)colour];
        }




    }
}
