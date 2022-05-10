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
    internal class Pawn : Piece
    {
        public PlayerColour colour { get; private set; }

        public Game.Player Player { get; private set; }

        public PiecesType PieceType { get; private set; }

        public Point position { get; set; }
        public Game.Board board { get; private set; }



        public Pawn(PlayerColour ownerColour,int x, int y, Board chessBoard)
        {
            this.colour = ownerColour;
            this.board = chessBoard;
            position = new Point(x, y);
            PieceType = PiecesType.PAWN;
        }

        public bool checkMove(int x, int y)
        {
            if (board.getPiece(x, y).colour == colour) return false;
            if (x < 0 || x > 7 || y < 0 || y > 7) return false;
            if (x == position.X && y == position.Y) return false;

            int direction = 1;

            if(colour== PlayerColour.BLACK)
            {
                direction = -1;
            }

            if (x == position.X)
            {
                if(y == position.Y + direction)
                {
                    if(board.getPiece(x, y).colour == PlayerColour.NONE)
                    {
                        return true;
                    }
                }
                if ((position.Y == 6 && colour == PlayerColour.BLACK) || (position.Y == 1 && colour == PlayerColour.WHITE))
                {
                    if (y == position.Y + direction * 2)
                    {
                        if (board.getPiece(x, y).colour == PlayerColour.NONE
                            && board.getPiece(x, position.Y + direction).colour == PlayerColour.NONE)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if(x+1==position.X || x - 1 == position.X)
                {
                    if (position.Y + direction == y)
                    {
                        if (board.getPiece(x, y).colour != PlayerColour.NONE
                            && board.getPiece(x, y).colour != colour)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public List<Point> GetPossibleMoves()
        {
            List<Point> possibleMoves = new List<Point>();
            for(int i = 0; i < 8; i++)
            {
                for(int j=0; j < 8; j++)
                {
                    if(checkMove(i,j)) possibleMoves.Add(new Point(i,j));
                }
            }
            return possibleMoves;
        }

        public int move(Point where)
        {
            if (checkMove(where.X, where.Y))
            {
                int result = 0;
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
