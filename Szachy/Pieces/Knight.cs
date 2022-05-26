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
    internal class Knight : Piece
    {
        public PlayerColour colour { get; private set; }

        public Game.Player Player { get; private set; }

        public PiecesType PieceType { get; private set; }

        public Point position { get; set; }
        public Game.Board board { get; private set; }

        public int Value { get => 3; }

        public Knight(PlayerColour ownerColour, int x, int y, Board chessBoard)
        {
            this.colour = ownerColour;
            this.board = chessBoard;
            position = new Point(x, y);
            PieceType = PiecesType.KNIGHT;

        }
        public bool checkMove(int x, int y)
        {
            Piece dir = board.getPiece(x, y);
            if (dir.colour == this.colour && dir.PieceType != PiecesType.NONE) return false;
            if (x >= 0 && y >= 0 && x < 8 && y < 8)
            {
                if (x - 2 == position.X || x + 2 == position.X)
                {
                    if (y - 1 == position.Y || y + 1 == position.Y)
                    {
                        return true;
                    }
                }
                if (y - 2 == position.Y || y + 2 == position.Y)
                {
                    if (x - 1 == position.X || x + 1 == position.X)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        public List<Point> GetPossibleMoves()
        {
            List<Point> possibleMoves = new List<Point>();

            int x=position.X;
            int y=position.Y;

            if (checkMove(x + 2, y + 1)) possibleMoves.Add(new Point(x + 2, y + 1));
            if (checkMove(x + 2, y - 1)) possibleMoves.Add(new Point(x + 2, y - 1));
            if (checkMove(x - 2, y + 1)) possibleMoves.Add(new Point(x - 2, y + 1));
            if (checkMove(x - 2, y - 1)) possibleMoves.Add(new Point(x - 2, y - 1));
            if (checkMove(x - 1, y + 2)) possibleMoves.Add(new Point(x - 1, y + 2));
            if (checkMove(x + 1, y + 2)) possibleMoves.Add(new Point(x + 1, y + 2));
            if (checkMove(x - 1, y - 2)) possibleMoves.Add(new Point(x - 1, y - 2));
            if (checkMove(x + 1, y - 2)) possibleMoves.Add(new Point(x + 1, y - 2));
            return possibleMoves;
        }
        public List<MovementAndValue> GetAllMovementsWithValues()
        {
            var possibleMoves = GetPossibleMoves();
            var result = new List<MovementAndValue>();
            foreach (var move in possibleMoves)
            {
                result.Add(new MovementAndValue()
                {
                    from = position,
                    to = move,
                    value = board.getPiece(move.X, move.Y).Value
                });
            }
            return result;
        }
        public int move(Point where)
        {
            if (checkMove(where.X, where.Y))
            {
                int result = 0;
                result = board.moved(position, where);
                return result;

            }
            return -1;
        }
    }
}
