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
    internal class Bishop : Piece
    {
        public PlayerColour colour { get; private set; }

        public Game.Player Player { get; private set; }///mozliwe ze do usuniecia wszedzie

        public PiecesType PieceType { get; private set; }

        public Point position { get; set; }
        public Game.Board board { get; private set; }



        public Bishop(PlayerColour ownerColour, int x, int y, Board chessBoard)
        {
            this.colour = ownerColour;
            this.board = chessBoard;
            position = new Point(x, y);
            PieceType = PiecesType.BISHOP;

        }
        public bool checkMove(int x, int y)
        {
            Piece dir = board.getPiece(x, y);
            if (dir.colour == colour && dir.PieceType != PiecesType.NONE) return false;
            if (x < 0 || x > 7 || y < 0 || y > 7) return false;
            if (x == position.X && y == position.Y) return false;
            float movement = ((float)position.X - x) / ((float)position.Y - y) ;
            if (movement==1 || movement == -1)
            {
                int movementX = position.X - x;
                int movementY = position.Y - y;
                int itX = 1, itY = 1;
                if(movementX > 0)
                {
                    itX = -1;
                }
                if(movementY > 0)
                {
                    itY = -1;
                }


                for(int i = 1; position.X+i*itX!=x; i ++)
                {
                    if(board.getPiece(position.X+i*itX,position.Y+i*itY).PieceType != PiecesType.NONE)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
            
        }

        public List<Point> GetPossibleMoves()
        {
            List<Point> possibleMoves = new List<Point>();
            int x=position.X;
            int y=position.Y;
            
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
