using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Szachy.Game;

namespace Szachy.Pieces
{
    internal class PawnPointer : Piece
    {
        public PlayerColour colour { get; private set; }

        public Game.Player Player { get; private set; }

        public PiecesType PieceType { get; private set; }

        public Point position { get; set; }
        public Game.Board board { get; private set; }

        public int Value { get => 1; }

        public PawnPointer(PlayerColour ownerColour, int x, int y, Board chessBoard)
        {
            PieceType = PiecesType.NONE;
            position = new Point(x, y);
            board = chessBoard;
            colour = ownerColour;
        }
        public bool checkMove(int x, int y)
        {
            return false;
        }

        public List<Point> GetPossibleMoves()
        {
            return new List<Point>();
        }
        public List<MovementAndValue> GetAllMovementsWithValues()
        {
            var result = new List<MovementAndValue>();
            return result;
        }
        public int move(Point where)
        {
            return -1 ;
        }
    }
}
