using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Szachy.Pieces
{
    public class MovementAndValue
    {
        public Point from { get; set; }
        public Point to { get; set; }
        public int value;
    }
    public  interface Piece
    {
        PlayerColour colour { get; }

        PiecesType PieceType { get; }

        Point position { get; set; }

        Game.Board board { get; }

        int Value { get; }

        List<Point> GetPossibleMoves();

        int move(Point where);

        bool checkMove(int x, int y);
         List<MovementAndValue> GetAllMovementsWithValues();
    }
}
