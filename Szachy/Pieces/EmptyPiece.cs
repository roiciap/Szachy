using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szachy.Pieces
{
    public class EmptyPiece : Piece
    {
        public PlayerColour colour { get; private set; }

        public Game.Player Player { get; private set; }

        public PiecesType PieceType { get; private set; }

        public Point position { get; set; }
        public Game.Board board { get; private set; }

        public int Value { get => 0; }


        public EmptyPiece(int x, int y,Game.Board chessBoard)
        {
            colour = PlayerColour.NONE;
            board = chessBoard;
            position = new Point(x, y);
            PieceType = PiecesType.NONE;

        }

        public EmptyPiece()
        {
            PieceType = PiecesType.NONE;
            colour = PlayerColour.NONE;
            position = new Point(-1, -1);
        }

        
        public bool checkMove(int x, int y)
        {
            return false;
        }

        public List<Point> GetPossibleMoves()
        {
            return new List<Point>();
        }

        public int move(Point where)
        {
            return -1;   
        }

        public List<MovementAndValue> GetAllMovementsWithValues()
        {
            return new List<MovementAndValue>();
        }
    }
}
