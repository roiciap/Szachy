using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szachy.Game
{
    public interface Player
    {
        bool realPerson { get; }
        List<Pieces.Piece> pieces { get; set; }

        List<Point> getAttackingPoints();

        void makeMove();
        
        
    }
}
