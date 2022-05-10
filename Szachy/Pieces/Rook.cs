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
    internal class Rook : Piece
    {
        public PlayerColour colour { get;private set; }

        public Game.Player Player { get;private set; }

        public PiecesType PieceType { get; private set; }

        public Point position { get; set; }
        public Game.Board board { get; private set; }


        public Rook(PlayerColour ownerColour, int x, int y, Board chessBoard)
        {
            this.colour = ownerColour;
            this.board = chessBoard;
            position = new Point(x, y);
            PieceType = PiecesType.ROOK;
        }
        public bool checkMove(int x, int y)
        {
            Piece dir = board.getPiece(x, y);
            if (dir.colour == colour && dir.PieceType != PiecesType.NONE) return false;
            if(x<0 || x>7 || y<0 || y>7) return false;
            if (x == position.X && y == position.Y) return false;
            int it = 1;
            if(x==position.X && y != position.Y)
            {
                if (y < position.Y)
                {
                    it = -1;
                }
                for(int i = position.Y + it; i != y; i += it)
                {
                    if (board.getPiece(x, i).PieceType != PiecesType.NONE) return false;
                }
                return true;
            }
            if(y==position.Y && x!= position.X)
            {
                if (x < position.X)
                {
                    it = -1;
                }
                for (int i = position.X + it; i != x; i += it)
                {
                    if (board.getPiece(i, y).PieceType != PiecesType.NONE) return false;
                }
                return true;
            }
            return false;
        }

        public List<Point> GetPossibleMoves()
        {
            List<Point> possibleMoves = new List<Point>();
            for(int i = 0; i < 8; i++)
            {
                if (position.X != i && checkMove(i, position.Y) ) possibleMoves.Add(new Point(i, position.Y));
                if (position.Y != i && checkMove(position.X, i)) possibleMoves.Add(new Point(position.X, i));
            }


            return possibleMoves;
        }

        public int move(Point where)
        {
            if (checkMove(where.X, where.Y))
            {
                int result=0;
                var thread = new Thread(() =>
                {
                    result = board.moved(position, where);
                });
                thread.Start();
                thread.Join();
                return result;

            }
            return -1;
        }
    }
}
